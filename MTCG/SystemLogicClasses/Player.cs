using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.SystemLogicClasses
{
    public class Player
    {
        private string Name;
        private string Password;
        public int Elo { get; }
        public int Money { get; set; }
        public int[] Deck { get; } = new int[4]; //4-8 are for battle logic and will be done there
        public int[] Collection { get; set; } = new int[30]; //set to currently available cards

        public Player(string name, string pass)
        {
            this.Name = name;
            this.Password = pass;
            this.Elo = 1000;
            this.Money = 25;
            this.Deck[0] = -1;
      //      this.Collection[0] = 1;  ??? Plan?
        }
        public void PrintData()
        {
            Console.WriteLine("Data:  Name: " + this.Name +
                " | Password: " + this.Password + " | Elo:" + this.Elo +
                " | Money: " + this.Money + " | Deck:" + this.Deck[0] + " " + this.Deck[1] + " " + this.Deck[2] + " " + this.Deck[3]);

        }

        public void CreateDeck(int[] ar)
        {
            for (int i = 0; i < 4; i++)
            {
                this.Deck[i] = ar[i];
            }
            ValidateDeck();

        }

        private void ValidateDeck() //call whenever make new Deck
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (this.Collection[this.Deck[i]] == 0)
                    {
                        this.Deck[0] = -1;
                        return;
                    }
                }
                catch (System.NullReferenceException e)
                {
                    Console.WriteLine("Collection OoB: " + e);
                    this.Deck[0] = -1;
                    return;
                }
            }
        }
        public void UpdateElo(int p2_elo, char outcome)
        {
            //literally just copy paste chess elo
            return;
        }

        /*
         * 
         * Can:
         * 
         * Open Packs
         * Trade
         * Make Friends (only in DB)
         * Battle
         */

    }
}
