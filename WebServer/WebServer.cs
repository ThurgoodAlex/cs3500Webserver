using Communications;
using System.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using WebServer;
using Database;


namespace StarterCode
{
    /// <summary>
    /// Author:  H. James de St. Germain, Alex Thurgood and Toby Armstrong
    /// Date:    Spring 2020
    /// Last updated by Alex Thurgood and Toby Armstrong on April 26th,2023
    /// Course: CS 3500, University of Utah, School of Computing
    /// Copyright: CS3500, Alex Thurgood and Toby Armstrong - This work may not be copied for use in academic coursework.
    /// 
    /// Code for a simple web server
    /// </summary>
    class WebServer
    {
        /// <summary>
        /// keep track of how many requests have come in.  Just used
        /// for display purposes.
        /// </summary>
        static private int counter = 1;

        static private Networking server = new Networking(NullLogger.Instance, OnClientConnect, OnDisconnect, onMessage, '\n');

        static private DatabaseCreation database;

        public static void Main(string[] args)
        {
            database = new DatabaseCreation();
            server.WaitForClients(11001, true);
            Console.ReadLine();
        }


        /// <summary>
        /// Basic connect handler - i.e., a browser has connected!
        /// Print an information message
        /// </summary>
        /// <param name="channel"> the Networking connection</param>

        internal static void OnClientConnect(Networking channel)
        {
            Console.WriteLine("Browser has connected!");
        }

        /// <summary>
        /// Create the HTTP response header, containing items such as
        /// the "HTTP/1.1 200 OK" line.
        /// 
        /// See: https://www.tutorialspoint.com/http/http_responses.htm
        /// 
        /// Warning, don't forget that there have to be new lines at the
        /// end of this message!
        /// </summary>
        /// <param name="length"> how big a message are we sending</param>
        /// <param name="type"> usually html, but could be css</param>
        /// <returns>returns a string with the response header</returns>
        private static string BuildHTTPResponseHeader(int length, string type = "text/html")
        {
            return $@"
HTTP/1.1 200 OK
Date: {DateTime.Now}
Server: Agario Leaderboard
Last-Modified: Wed, 22 Jul 2009 19:15:56 GMT
Content-Length: {length}
Content-Type: {type}
Connection: Closed
";
        }

        /// <summary>
        ///   Create a web page!  The body of the returned message is the web page
        ///   "code" itself. Usually this would start with the doctype tag followed by the HTML element.  Take a look at:
        ///   https://www.sitepoint.com/a-basic-html5-template/
        /// </summary>
        /// <returns> A string the represents a web page.</returns>
        private static string BuildHTTPBody()
        {
            return htmlPages.HomePage;
        }


        /// <summary>
        ///   <para>
        ///     When a request comes in (from a browser) this method will
        ///     be called by the Networking code.  Each line of the HTTP request
        ///     will come as a separate message.  The "line" we are interested in
        ///     is a PUT or GET request.  
        ///   </para>
        ///   <para>
        ///     The following messages are actionable:
        ///   </para>
        ///   <para>
        ///      get highscore - respond with a highscore page
        ///   </para>
        ///   <para>
        ///      get favicon - don't do anything (we don't support this)
        ///   </para>
        ///   <para>
        ///      get scores/name - along with a name, respond with a list of scores for the particular user
        ///   <para>
        ///      get scores/name/highmass/highrank/startime/endtime - insert the appropriate data
        ///      into the database.
        ///   </para>
        ///   </para>
        ///   <para>
        ///     create - contact the DB and create the required tables and seed them with some dummy data
        ///   </para>
        ///   <para>
        ///     get index (or "", or "/") - send a happy home page back
        ///   </para>
        ///   <para>
        ///     get css/styles.css?v=1.0  - send your sites css file data back
        ///   </para>
        ///   <para>
        ///     otherwise send a page not found error
        ///   </para>
        ///   <para>
        ///     Warning: when you send a response, the web browser is going to expect the message to
        ///     be line by line (new line separated) but we use new line as a special character in our
        ///     networking object.  Thus, you have to send _every line of your response_ as a new Send message.
        ///   </para>
        /// </summary>
        /// <param name="network_message_state"> provided by the Networking code, contains socket and message</param>
        internal static void onMessage(Networking channel, string message)
        {
            if (message.StartsWith("GET"))
            {
                string[] splitMessage = message.Split('/');
                if (splitMessage[1] == " HTTP" || splitMessage.Contains("index") || splitMessage.Contains("index HTTP") || splitMessage.Contains("index.html") || splitMessage.Contains("index.html HTTP"))
                {
                    string body = BuildHTTPBody();
                    channel.Send(BuildHTTPResponseHeader(body.Length));
                    channel.Send("");
                    channel.Send(body);
                }

                else
                if (splitMessage[1] == "highscores")
                {
                    string body = htmlPages.HighscoresPage;
                    body += database.QueryHighscores();
                    body += "</body> </html>";
                    channel.Send(BuildHTTPResponseHeader(body.Length));
                    channel.Send("");
                    channel.Send(body);

                }

                else if (splitMessage[1] == "scores")
                {
                    if (splitMessage.Length == 5)
                    {
                        string body = htmlPages.ScoresPage;
                        body += database.QueryPlayerScores(splitMessage[2]);
                        body += "</body> </html>";
                        channel.Send(BuildHTTPResponseHeader(body.Length));
                        channel.Send("");
                        channel.Send(body);
                    }
                    else if (splitMessage.Length == 8)
                    {
                        string body = htmlPages.ScoresPage;

                        //Parse insert values from strings to floats/ints
                        float.TryParse(splitMessage[3], out float maxMass);
                  
                        int.TryParse(splitMessage[4], out int startTime);
                        int.TryParse(splitMessage[5], out int endTime);

                        body += database.QueryAddValues(splitMessage[2], maxMass, startTime, endTime);
                        body += "</body> </html>";
                        channel.Send(BuildHTTPResponseHeader(body.Length));
                        channel.Send("");
                        channel.Send(body);
                    }
                   
                }

                else if (splitMessage[1] == "create")
                {
                    string body = htmlPages.CreatePage;
                    body += database.QueryCreate();
                    body += "</body> </html>";
                    channel.Send(BuildHTTPResponseHeader(body.Length));
                    channel.Send("");
                    channel.Send(body);
                }

                else if (splitMessage[1] == "fancy")
                {
                    string body = htmlPages.FancyPage;
                    body += database.QueryFancy();
                    body += "</body> </html>";
                    channel.Send(BuildHTTPResponseHeader(body.Length));
                    channel.Send("");
                    channel.Send(body);
                }
                else
                {
                    string body = htmlPages.PageNotFound;
                    channel.Send(BuildHTTPResponseHeader(body.Length));
                    channel.Send("");
                    channel.Send(body);
                }
            }
            else
            {
                return;
            }
        }


        /// <summary>
        ///    (1) Instruct the DB to seed itself (build tables, add data)
        ///    (2) Report to the web browser on the success
        /// </summary>
        /// <returns> the HTTP response header followed by some informative information</returns>
        //private static string CreateDBTablesPage()
        //{
        //    DatabaseCreation database = new DatabaseCreation();
        //    database.
        //}

        /// <summary>
        ///  when you disconnect from the server
        /// </summary>
        /// <param name="channel">The channel that is being disconnected</param>
        internal static void OnDisconnect(Networking channel)
        {
            Debug.WriteLine($"Goodbye {channel.RemoteAddressPort}");
        }

    }
}
