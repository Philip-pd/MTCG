using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Newtonsoft.Json;

namespace MTCG.SystemLogicClasses
{
    public class Player
    {
        
        public string Name { get; }
        [JsonIgnore]
        public string Token { get; }
        public int Elo { get; set; }
        public int Coins { get; set; }
        public int[] Deck { get; set; } = new int[4]; 
        [JsonIgnore]
        public bool[] Collection { get; set; } = new bool[32]; //set to currently available cards (only need 30 but no int with 30 bit) 
        public int Wins { get; set; }
        public int Losses { get; set; }
        private static readonly object playerlock = new object();

        public Player(string name,int elo,int coins,int collection,int wins,int losses,int[] deck)
        {
            this.Name = name;
            this.Token = name + "-Token"; 
            this.Elo = elo;
            this.Coins = coins;          
            this.Wins = wins;
            this.Losses = losses;
            CollectionDecr(collection); //integer
            if (deck == null)
            {
                this.Deck[0] = -1;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    this.Deck[i] = deck[i];
                }
            }
        }
        private void CollectionDecr(int number)
        {
            BitArray b = new BitArray(new int[] { number});
            lock(playerlock)
            { 
                b.CopyTo(this.Collection, 0);
            }
        }


        public int GetCollectionInt()
        {
            BitArray ColBits = new BitArray(this.Collection);

            int[] array = new int[1];
            lock (playerlock)
            {
                ColBits.CopyTo(array, 0);
            }
            return array[0];

        }

        public int AddBoosterToCollection(int[] newCards)
        {
            int duplicates = 0;
            lock (playerlock)
            {
                this.Coins -= 5;
                
                for(int i = 0; i<newCards.Length;i++)
                {
                    if (this.Collection[newCards[i]]==true)
                    {
                        duplicates++;
                        continue;
                    }
                    this.Collection[newCards[i]] = true;
                }
                this.Coins += duplicates; //refund for cards you already had
            }
            PlayerDAO dao = new PlayerDAOImpl();
            dao.UpdatePlayer(this); //instant update in Database
            return 5 - duplicates;
        }


        public string ReturnCollection() 
        {
            return JsonConvert.SerializeObject(this.Collection);
        }

        public bool CreateDeck(int[] ar) 
        {
            if(ValidateDeck(ar))
            {
                lock(playerlock)
                { 
                    for (int i = 0; i < 4; i++)
                    {
                        this.Deck[i] = ar[i];
                    }
                }
                PlayerDAO dao = new PlayerDAOImpl();
                dao.UpdatePlayerDeck(this);
                return true;
            }
            return false;

        }

        private bool ValidateDeck(int[] ar) //call whenever make new Deck
        {
            int[] prev = new int[4]; //only needs to check first 3 
            for (int i = 0; i < 4; i++) //works 
            {
                try
                {
                    prev[i] = ar[i];
                    if (!this.Collection[ar[i]]) //checks if you own the cards
                    {
                        return false;
                    }
                    for(int j=0;j<i;j++)
                    {
                        if (ar[i] == prev[j]) //checks if you only used each card once
                        {
                            return false;
                        }
                    }
                }
                catch (System.NullReferenceException e)
                {
                    Console.WriteLine("Collection OoB: " + e);
                    return false;
                }
            }
            return true;
        }
        public void UpdateElo(int p2_elo, char outcome)
        {
            lock(playerlock)
            {
                switch(outcome)
                {
                    case 'a':
                        this.Elo +=3;
                        this.Wins++;
                        break;
                    case 'b':
                        this.Elo -= 5;
                        this.Losses++;
                        break;
                    case 'c':
                        break;
                }
            }

            PlayerDAO dao= new PlayerDAOImpl();
            dao.UpdatePlayer(this);

            return;
        }
    }
}
