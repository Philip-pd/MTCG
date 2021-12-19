using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace MTCG.SystemLogicClasses
{
    public class Player
    {
        
        private string Password;
        public string Name { get; }
        public int Elo { get; }
        public int Money { get; set; }
        public int[] Deck { get; } = new int[4]; //4-8 are for battle logic and will be done there
        public bool[] Collection { get; set; } = new bool[32]; //set to currently available cards

        public Player(string name, string pass,int elo,int money,int collection)
        {
            this.Name = name;
            this.Password = pass;
            this.Elo = elo; //1000 default
            this.Money = money; //25 default
            this.Deck[0] = -1; //to be set before you play
            CollectionDecr(collection); //integer
            
        }
        private void CollectionDecr(int number)
        {
            BitArray b = new BitArray(new int[] { number});
            
            b.CopyTo(this.Collection, 0);
        }


        public int GetCollectionInt()
        {
            BitArray ColBits = new BitArray(this.Collection);

            int[] array = new int[1];
            ColBits.CopyTo(array, 0);
            return array[0];

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
            for (int i = 0; i < 4; i++) //works //should check for doubles or don't but do that later if at all
            {
                try
                {
                    if (this.Collection[this.Deck[i]] == false)
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
