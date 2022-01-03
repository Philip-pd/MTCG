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
             *  Elo System: Build Base one now do complex later  Y
             *  
             * 
             * System Logic:
             * 
             *  DataBase Y
             *  
             *      Profile Y
             *  
             *      Ranking Y
             *      
             *          or
             *          Create user on login Y
             *       Packs 2
             *  
             *  Trading 2
             *  
             *  Rest Y
             *  
             *      MatchMaking
             *  
             *      Multithread Y
             * 
             */
        }
    }
}
