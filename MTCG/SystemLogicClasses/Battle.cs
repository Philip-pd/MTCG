using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.SystemLogicClasses
{
    public class Battle
    {
        private List<Player> _playerList = new List<Player>();
        private readonly Random _random = new Random();

        public Battle(Player player1, Player player2)
        {
            Add(player1);
            Add(player2);
        }
        public void Add(Player player)
        {
            _playerList.Add(player); //seems to be reference
        }
        public void Play()
        {
            int[] p1_deck = new int[8];
            int[] p2_deck = new int[8];
            int p1_decksize = 4;
            int p2_decksize = 4;

            p1_deck = DeckSort(_playerList[0].Deck);
            p2_deck = DeckSort(_playerList[1].Deck);


            for (int i = 0; i < 100; i++)
            {
                int p1_card = _random.Next(p1_decksize);
                int p2_card = _random.Next(p2_decksize);
                switch (Round(p1_deck[p1_card], p2_deck[p2_card]))
                {
                    case 'a'://Bewusst nicht ausgelagert da zwar übersichtlicher aber komplizierter und es nur 2 fälle geben kann
                        p1_deck[7] = p2_deck[p2_card];
                        p2_deck[p2_card] = 0;
                        p1_decksize++;
                        p1_deck = DeckSort(p1_deck);
                        p2_deck = DeckSort(p2_deck);
                        break;
                    case 'b':
                        p2_deck[7] = p1_deck[p1_card];
                        p1_deck[p1_card] = 0;
                        p2_decksize++;
                        p1_deck = DeckSort(p1_deck);
                        p2_deck = DeckSort(p2_deck);
                        break;
                    case 'c': //draw
                        continue;
                    default:
                        throw new ArgumentException("Fatal Error in Round Logic. Invalid Argument Returned");
                }
                if (p1_decksize == 8)
                {
                    //update Elo
                    return;
                }
                if (p2_decksize == 8)
                {
                    //update Elo
                    return;
                }
            }
            //Update Elo
            //player with 0 cards loose elo other one gain
            //Call Round 3 times
            //Card needs to be defeated somehow use bool to check
            //Update Elo after all rounds
            //maybe gen random here and send ints to round (1-4 send their value over if not empty deck & retry if empty) Empty after selection
            //Alternativeley randomize deck order and then just iterate over them. whatever will be easier
        }
        private char Round(int CardA, int CardB) //can't mark defeated cards cause out of scope
        {
            //Get random number x2
            //Select the cards
            //Generate those Cards
            //Send them over to Combat

            // return Combat(/*Card 1 Card 2*/);
            return 'b';
        }
        private bool Combat()
        {
            //kinda nonsense
            //Play out Battle logic
            //return 0 or 1 depending on who won
            return true;
        }
        private int[] DeckSort(int[] arr1) //Array has Var Length
        {
            int[] deck = new int[8];
            int spot = 0;
            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != 0)
                {
                    deck[spot] = arr1[i];
                    spot++;
                }

            }
            return deck;
        }
    }
}
