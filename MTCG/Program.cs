using System;
using MTCG.SystemLogicClasses;


namespace MTCG
{
    class Program
    {
        static void Main(string[] args)//Basically just for testing until we get REST
        {
            Player a = new Player("p1", "123");
            Player b = new Player("p2", "129");
            int[] ar = { 0, 1, 1, 3 };
            a.CreateDeck(ar);
            b.CreateDeck(ar);
            a.PrintData();
            b.PrintData();
            //rest let them enter matchmaking. Matchmaking checks if decks are valid or returns error. if valid wait for other player then go to battle.
            Battle battle = new Battle(a, b); //Somehow restrict if invalid deck
            battle.Play();
            a.PrintData();
            b.PrintData();

        }
    }
}
