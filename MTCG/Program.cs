using System;
using MTCG.SystemLogicClasses;


namespace MTCG
{
    class Program
    {
        static void Main(string[] args)//Basically just for testing until we get REST
        {
            Console.WriteLine("Starting server on port 8080"); //starts the server
            HTTPServer server = new HTTPServer(8080);
            server.Start();

            /*
             * Todo:
             * 
             * Gameplay Logic:
             * 
             *  Elo System: Build Base one now do complex later  3
             *  
             *  Dictionary for Weakness 4
             * 
             * System Logic:
             * 
             *  DataBase 1
             *  
             *      Profile
             *  
             *      Ranking
             *      
             *          Create User (in MM)
             *          or
             *          Create user on login and then use token of (sha256 of password + username + sha256 of password) as Identifier of User
             *          so the SQL don't break
             *       Packs 2
             *  
             *  Trading 2
             *  
             *  Rest 1.2
             *  
             *      MatchMaking
             *  
             *      Multithread
             * 
             */
        }
    }
}
