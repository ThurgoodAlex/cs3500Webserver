using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
namespace Database;
/// <summary>
/// Author:  H. James de St. Germain, Alex Thurgood and Toby Armstrong
/// Date:    Spring 2020
/// Last updated by Alex Thurgood and Toby Armstrong on April 26th,2023
/// Course: CS 3500, University of Utah, School of Computing
/// Copyright: CS3500, Alex Thurgood and Toby Armstrong - This work may not be copied for use in academic coursework.
/// 
/// Code for connecting to and querying an SQL Database
/// </summary>

public class DatabaseCreation
{
    /// <summary>
    /// The information necessary for the program to connect to the Database
    /// </summary>
    public static readonly string connectionString;

    static DatabaseCreation()
    {
        var builder = new ConfigurationBuilder();

        builder.AddUserSecrets<DatabaseCreation>();
        IConfigurationRoot Configuration = builder.Build();
        var SelectedSecrets = Configuration.GetSection("DatabaseSecrets");

        connectionString = new SqlConnectionStringBuilder()
        {
            DataSource = SelectedSecrets["ServerURL"],
            InitialCatalog = SelectedSecrets["DBName"],
            UserID = SelectedSecrets["UserName"],
            Password = SelectedSecrets["DBPassword"],
            Encrypt = false
        }.ConnectionString;
    }

    /// <summary>
    /// Open a connection to the SQL server and query for highscores and displays it as a table.
    /// </summary>
    public string QueryHighscores()
    {
        Console.WriteLine("Getting Connection ...");
        string htmlTable = "<table> <tr> <td> GameID </td> <td> Name </td> <td> Highest Mass </td> </tr>";
        try
        {
            //create instance of database connection
            using SqlConnection con = new(connectionString);

            //
            // Open the SqlConnection.
            //
            con.Open();

            //
            // This code uses an SqlCommand based on the SqlConnection.
            //
            using SqlCommand command = new SqlCommand("SELECT GameTable.GameID, Name, HighestMass from Players join GameTable on Players.PlayerID = GameTable.PlayerID join HighestMass on GameTable.GameID = HighestMass.GameID", con);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int gameID = reader.GetInt32(0);
                string name = reader.GetString(1);
                double highestMass = reader.GetDouble(2);
                htmlTable += $"<tr> <td> {gameID} </td> <td> {name} </td> <td> {highestMass} </td> </tr>";
            }

            htmlTable += "</ htmlTable >";
            return htmlTable;
        }
        catch (SqlException exception)
        {
            Console.WriteLine($"Error in SQL connection: {exception.Message}");
            return "";
        }
    }


    /// <summary>
    /// This tries to create all of the necessary database tables and seed them with dummy data.
    /// If any of the required tables are already present, a SQL exception will be thrown and a message will be provided
    /// on the create page
    /// </summary>
    /// <exception cref="SqlException"> this gets thrown when there is already a table present but tries to build it again
    /// </exception>
    /// <returns> returns a string message saying whether the tables have been built or not on the webpage</returns>
    public string QueryCreate()
    {
        Console.WriteLine("Getting Connection ...");
        string htmlMessage = "<p>";
        try
        {
            //create instance of database connection
            using SqlConnection con = new(connectionString);

            //
            // Open the SqlConnection.
            //
            con.Open();

            //
            // This code uses an SqlCommand based on the SqlConnection.
            //
            try
            {
                using SqlCommand command = new SqlCommand("CREATE TABLE Players (PlayerID int NOT NULL PRIMARY KEY IDENTITY, Name varchar(50) NOT NULL); INSERT INTO Players (Name) VALUES('Alex'); INSERT INTO Players (Name) VALUES('Toby');INSERT INTO Players (Name) VALUES('Zoe'); INSERT INTO Players (Name) VALUES('Rylie')", con);

                using SqlDataReader reader = command.ExecuteReader();
                reader.Close();

                using SqlCommand command2 = new SqlCommand("CREATE TABLE HighestMass (HighestMass float NOT NULL, GameID int NOT NULL PRIMARY KEY IDENTITY); INSERT INTO HighestMass (HighestMass) VALUES(1000.0); INSERT INTO HighestMass (HighestMass) VALUES(4324.0); INSERT INTO HighestMass (HighestMass) VALUES(600.0); INSERT INTO HighestMass (HighestMass) VALUES(65000.0);", con);
                using SqlDataReader reader2 = command2.ExecuteReader();
                reader2.Close();

                using SqlCommand command3 = new SqlCommand("CREATE TABLE GameTable (GameID int NOT NULL PRIMARY KEY IDENTITY, PlayerID int NOT NULL); INSERT INTO GameTable (PlayerID) VALUES(1); INSERT INTO GameTable (PlayerID) VALUES(2); INSERT INTO GameTable (PlayerID) VALUES(7); INSERT INTO GameTable (PlayerID) VALUES(10);", con);
                using SqlDataReader reader3 = command3.ExecuteReader();
                reader3.Close();

                using SqlCommand command4 = new SqlCommand("CREATE TABLE GameTimePlayed (GameID int NOT NULL PRIMARY KEY IDENTITY, StartTime int NOT NULL, EndTime int NOT NULL); INSERT INTO GameTimePlayed(StartTime, EndTime) VALUES (300, 800); INSERT INTO GameTimePlayed(StartTime, EndTime) VALUES (1000, 2000); INSERT INTO GameTimePlayed(StartTime, EndTime) VALUES (8000, 10000); INSERT INTO GameTimePlayed(StartTime, EndTime) VALUES (10000, 50000);", con);
                using SqlDataReader reader4 = command4.ExecuteReader();
                reader4.Close();


                // SEE README
                //using SqlCommand command5 = new SqlCommand("CREATE TABLE HighestRank (HighestRank int NOT NULL, PlayerID int NOT NULL PRIMARY KEY IDENTITY); INSERT INTO HighestRank (HighestRank) VALUES (1); HighestRank (HighestRank) VALUES (100); HighestRank (HighestRank) VALUES (5); HighestRank (HighestRank) VALUES (7);", con);
                //using SqlDataReader reader5 = command5.ExecuteReader();
                //reader5.Close();

                // SEE README
                //using SqlCommand command6 = new SqlCommand("CREATE TABLE Color (Color vchar(50) NOT NULL, PlayerID int NOT NULL PRIMARY KEY IDENTITY); INSERT INTO Color (Color) VALUES ('red'); INSERT INTO Color (Color) VALUES ('blue'); INSERT INTO Color (Color) VALUES ('green'); INSERT INTO Color (Color) VALUES ('purple');", con);
                //using SqlDataReader reader6 = command6.ExecuteReader();
                //reader6.Close();


            }
            catch (SqlException exception)
            {
                htmlMessage += "Unsuccessful table creation, the database already contains the necessary tables </p>";
                return htmlMessage;
            }

            htmlMessage += "Successful table creation </p>";
            return htmlMessage;
        }
        catch (SqlException exception)
        {
            Console.WriteLine($"Error in SQL connection: {exception.Message}");
            return "";
        }
    }


    /// <summary>
    /// This gets all of the stats from the given players name and displays it in a table
    /// </summary>
    /// <param name="playerName"> the name of the player you want to visit</param>
    /// <exception cref="SqlException"> when an error in the SQL connection has happened</exception>
    /// <returns> the html table with all of the stats of the given players name and displays it on the webpage.</returns>
    public string QueryPlayerScores(string playerName)
    {
        Console.WriteLine("Getting Connection ...");
        string htmlTable = "<table> <tr> <td> GameID </td> <td> Name </td> <td> Highest Mass </td> </tr>";
        try
        {
            //create instance of database connection
            using SqlConnection con = new(connectionString);

            //
            // Open the SqlConnection.
            //
            con.Open();

            //
            // This code uses an SqlCommand based on the SqlConnection.
            //
            using SqlCommand command = new SqlCommand($"SELECT GameTable.GameID, Name, HighestMass from Players join GameTable on Players.PlayerID = GameTable.PlayerID join HighestMass on GameTable.GameID = HighestMass.GameID where Name = '{playerName}';", con);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int gameID = reader.GetInt32(0);
                string name = reader.GetString(1);
                double highestMass = reader.GetDouble(2);

                htmlTable += $"<tr> <td> {gameID} </td> <td> {name} </td> <td> {highestMass} </td> </tr>";
            }

            htmlTable += "</ htmlTable >";
            return htmlTable;
        }
        catch (SqlException exception)
        {
            Console.WriteLine($"Error in SQL connection: {exception.Message}");
            return "";
        }
    }



    /// <summary>
    /// displays two messages about which player played the latest game and the associated playerID and gameID and which
    /// player has played the most games with the associated playerID and number of games played.
    /// </summary>
    /// <exception cref="SqlException"> when an error in the SQL connection has happened </exception>
    /// <returns>two messages stating information about the latest game and the most amount of games played</returns>
    public string QueryFancy()
    {
        Console.WriteLine("Getting Connection ...");
        string htmlMessage = "<p>";
        string htmlMessage2 = "<p>";
        try
        {
            //create instance of database connection
            using SqlConnection con = new(connectionString);

            //
            // Open the SqlConnection.
            //
            con.Open();

            //
            // This code uses an SqlCommand based on the SqlConnection.
            //
            using SqlCommand command = new SqlCommand($"SELECT Top 1 * from GameTable order by (GameTable.GameID) desc;", con);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int gameID = reader.GetInt32(0);
                int playerID = reader.GetInt32(1);
                htmlMessage += $"The most recent game was played by player ID {playerID} and the game id of that game is {gameID} </p>";

            }
            reader.Close();


            using SqlCommand command2 = new SqlCommand($"SELECT TOP 1 PlayerID, COUNT(GameID)as GamesPlayed FROM GameTable GROUP BY PlayerID ORDER BY COUNT(GameID) DESC", con);
            using SqlDataReader reader2 = command2.ExecuteReader();


            while (reader2.Read())
            {
                int playerID = reader2.GetInt32(0);
                int gamesPlayed = reader2.GetInt32(1);

                htmlMessage2 += $"The most amount of games was played by player ID {playerID} and they played {gamesPlayed} games</p>";
            }


            return htmlMessage + htmlMessage2;
        }
        catch (SqlException exception)
        {
            Console.WriteLine($"Error in SQL connection: {exception.Message}");
            return "";
        }
    }


/// <summary>
/// Establishes connection to database and adds a row based on parameters.
/// </summary>
/// <param name="name"> the name to be added </param>
/// <param name="highMass"> the mass to be added </param>
/// <param name="startTime">the start time to be added </param>
/// <param name="endTime"> the end time to be added </param>
/// <returns></returns>
    public string QueryAddValues(string name, float highMass, int startTime, int endTime)
    {
        string htmlMessage = "<p>";
        try
        {
            using SqlConnection con = new SqlConnection(connectionString);

            con.Open();
            try
            {
                using SqlCommand command = new SqlCommand($"INSERT INTO Players (Name) VALUES('{name}')", con);
                using SqlDataReader reader = command.ExecuteReader();
                reader.Close();

                using SqlCommand commandPlayerID = new SqlCommand($"SELECT PlayerID FROM Players WHERE Name = '{name}';", con);
                using SqlDataReader readerPlayerID = commandPlayerID.ExecuteReader();
                readerPlayerID.Read();
                int playerID = readerPlayerID.GetInt32(0);
                readerPlayerID.Close();

                using SqlCommand command2 = new SqlCommand($"INSERT INTO GameTable (PlayerID) VALUES ('{playerID}')", con);
                using SqlDataReader reader2 = command2.ExecuteReader();
                reader2.Close();

                using SqlCommand command3 = new SqlCommand($"INSERT INTO HighestMass (HighestMass) VALUES ('{highMass}')", con);
                using SqlDataReader reader3 = command3.ExecuteReader();
                reader3.Close();

                using SqlCommand command4 = new SqlCommand($"INSERT INTO GameTimePlayed (StartTime, EndTime) VALUES ('{startTime}', '{endTime}')", con);
                using SqlDataReader reader4 = command4.ExecuteReader();
                reader4.Close();


                //SEE README
                //using SqlCommand command5 = new SqlCommand($"INSERT INTO HighestRank (HighestRank) VALUES ('{highestRank}')", con);
                //using SqlDataReader reader5 = command5.ExecuteReader();
                //reader5.Close();

                return htmlMessage += "The data was added successfully </p>";
            }
            catch (Exception exception)
            {
                return htmlMessage += "The data was not added successfully </p>";
            }

        }
        catch (SqlException exception)
        {
            Console.WriteLine($"Error in SQL connection:\n   - {exception.Message}");
            return "";
        }
    }

}
