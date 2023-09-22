```
Author:     Tobias R. Armstrong and Alex Thurgood
Start Date: 21-April-2023
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  tobybobarm, ThurgoodAlex
Repo:       https://github.com/uofu-cs3500-spring23/assignment-nine---web-server---sql-ms_penguin
Commit Date: 26-April-2023 11:10 PM
Solution:   WebServer
Copyright:  CS 3500, Tobias R. Armstrong and Alex Thurgood - This work may not be copied for use in Academic Coursework.
```

#Database Table Summary#
 Overall we created six different tables. Our first table was called 'Players' which holds a name with an associated unique PlayerID. We then created a 'GameTable' table. This holds a unique GameID integer that acts as the primary key that gets assigned to each PlayerID when inputted into this table. Our third table is called 'GameTimePlayed' with the same unique primary key GameID column, an integer startTime and endTime columns as well. Even though these are ints, these act as the time in milliseconds from when each players game session started and ended. Our fourth table was 'HighestMass' that contained the highest mass for each associated unique GameID. Our fifth table was 'Color' and that included a color column that held string values and the associated unique GameIDS. Our last table created was 'HighestRank' and that included the highest rank achieved for each associated GameID value.


#Extent of work#
When a client connects, the server will display a welcome page with some basic information and links to new pages that correspond to the proper API endpoints. When the highscores link is clicked a page displaying a table with the gameID, player name, and highest mass achieved. The insert command will add the proper inputted values to the respective tables, but does not work with the highest rank because of time constraints while debugging). The client can contact the database for both insertion and creation of tables (create will populate tables with dummy data). When inserting and querying for a specific player’s stats, it is imperative that the client includes a ‘/’ at the end of their search (i.e. localhost:11001/scores/Toby/). The same goes when inserting data (must finish request with a ‘/’). This project has not been linked with the Agario solution as we did not have enough time because of other class commitments.


#Partnership Information#
We did most of the work using pair programming techniques. Some minor changes were added alone such as documentation and CSS/HTML additions.


#Branching#
We did not create any branches


#Testing#
To test the web server portion, we made sure every link worked properly and that the proper data and styling was applied.
When testing the database portion, we made sure to test each endpoint with different data to make sure that it was grabbing the right selections. To test our create endpoint, we ended up deleting the tables from SSMS and then calling the create endpoint to make sure they get added back to SSMS.


#Time Tracking (Personal Software Practice)#
ESTIMATED TIME: 15 HRS
 TRACKED TIME:
    April 21: 2 HRS
    April 24: 2 HRS
    April 25: 3 HRS
    April 26: 4 hrs

        Effective time spent: 5 HRS
        Debugging time spent: 3 HRS
        Learning time spent:  3 HRS

        Time spend as a team: 11 HRS
        Alex individual time:   HRS
        Toby Individual time:   HRS

        TOTAL TIME SPENT: 11 HRS

 Our time estimate was pretty accurate, would have been more accurate if we had more time. Overall, we have gained a better intuition of our programming skills and how long each assignment will take. This indicates that we have become better programmers and are more understanding of our capabilities and each assignment's requirements.