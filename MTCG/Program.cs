﻿using System;
using MTCG.SystemLogicClasses;


namespace MTCG
{
    class Program
    {
        static void Main(string[] args)//Basically just for testing until we get REST
        {
            Player a = new Player("p1", "123",1000,25,255);
            Player b = new Player("p2", "129", 1000, 25, 255);
            int[] ar = { 0, 1, 2, 3 };
            a.CreateDeck(ar);
            b.CreateDeck(ar);
            a.PrintData();
            b.PrintData();
            //rest let them enter matchmaking. Matchmaking checks if decks are valid or returns error. if valid wait for other player then go to battle.
            Battle battle = new Battle(a, b); //Somehow restrict if invalid deck
            battle.Play();
            a.PrintData();
            b.PrintData();

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
