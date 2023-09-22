using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    /// <summary>
    /// Author:   Alex Thurgood and Toby Armstrong
    /// Date: April 26th,2023
    /// Course: CS 3500, University of Utah, School of Computing
    /// Copyright: CS3500, Alex Thurgood and Toby Armstrong - This work may not be copied for use in academic coursework.
    /// 
    /// This class holds all of the different html bodies for the HTTP requests
    /// </summary>
    public class htmlPages
    {
        /// <summary>
        /// The html for the highscores page
        /// </summary>
        public const string HighscoresPage = @"<!DOCTYPE html>
<html>
    <head>
        <title>Highscores Page</title>
    </head>
<style>
table, th, td {
  border: 1px solid;
}

table {
  width: 100%;
}
h1 {
text-align: center;
}
body {
background-color: #9BB7D4;
</style>
    <body>
        <h1> Highscores </h1>
        <a href='http://localhost:11001'>Main Page</a>
";

        /// <summary>
        /// The html for the scores page
        /// </summary>
        public const string ScoresPage = @"<!DOCTYPE html>
<html>
    <head>
        <title>Player Stats</title>
    </head>
<style>
table, th, td {
  border: 1px solid;
}

table {
  width: 100%;
}
h1 {
text-align: center;
}
body {
background-color: #9BB7D4;
</style>
    <body>
        <h1> Player Stats </h1>
        <a href='http://localhost:11001'>Main Page</a>
";

        /// <summary>
        /// The html for the create page
        /// </summary>
        public const string CreatePage = @"<!DOCTYPE html>
<html>
    <head>
        <title>Create A Table</title>
    </head>
<style>
h1 {
text-align: center;
}
body {
background-color: #9BB7D4;
</style>
    <body>
        <h1> Create A Table </h1>
        <a href='http://localhost:11001'>Main Page</a>
";



        /// <summary>
        /// the html for the home page
        /// </summary>
        public const string HomePage = @"<!DOCTYPE html>
<html>
    <head>
        <title>Create A Table</title>
    </head>
<style>
h1 {
text-align: center;
}
body {
background-color: #9BB7D4;
</style>
    <body>
        <h1> Agario Leaderboard Links </h1>
        <ul>
            <li> <a href='http://localhost:11001/highscores/'>Highscores Page</a> </li>
            <li> <a href='http://localhost:11001/scores/'>Player Stats</a> </li>
            <li> <a href='http://localhost:11001/create/'>Create and Seed</a> </li>
            <li> <a href='http://localhost:11001/fancy/'>Fancy</a> </li>
        </ul>
        <h2> Info </h2>
<p>
This is the homepage that allows you to navigate to different links based on what information you want to retrieve. For example if you wanted to retrieve the highest mass from all the games, you would click on the 'Highscores' link. When you visit the 'Player Stats' Page, you will be directed to a blank website, to fix this, in the url, you have to put in the players name, for example it would be 'localhost:11001/scores/[name]/' of whatever player you want to visit. Make sure to do this or else the scores page will not show up properly. 
</p>
    </body>
</html>";

        /// <summary>
        /// The html for the fancy page
        /// </summary>
        public const string FancyPage = @"<!DOCTYPE html>
<html>
    <head>
        <title>Fancy Page</title>
    </head>
<style>
h1 {
text-align: center;
}
body {
background-color: #9BB7D4;
</style>
    <body>
        <h1> Fancy Page </h1>
        <a href='http://localhost:11001'>Main Page</a>
    </body>
</html>
";

        /// <summary>
        /// The html for the error page
        /// </summary>
        public const string PageNotFound = @"<!DOCTYPE html>
<html>
    <head>
        <title>PageNotFound</title>
    </head>
<style>
h1 {
text-align: center;
}
body {
background-color: #9BB7D4;
</style>
    <body>
        <h1> PageNotFound </h1>
        <a href='http://localhost:11001'>Main Page</a>
    </body>
</html>
";

    }
}
