using System;
using System.Collections.Generic;
using System.Text;
using MTCG.GameplayLogicClasses;

namespace MTCG.SystemLogicClasses
{
    public class Battle
    {
        //a -> p1, b-> p2, c-> draw
        private List<Player> _playerList = new List<Player>();
        private readonly Random _random = new Random();
        private CardFactory factory = new CardFactory();

        public Battle(Player player1, Player player2)
        {
            Add(player1);
            Add(player2);
        }
        public void Add(Player player)
        {
            _playerList.Add(player); 
            Console.WriteLine(player.Name);
        }
        public string Play()
        {
            int[] p1_deck = new int[8];
            int[] p2_deck = new int[8];
            int p1_decksize = 4;
            int p2_decksize = 4;

            p1_deck = DeckSort(_playerList[0].Deck); //lock the two
            p2_deck = DeckSort(_playerList[1].Deck);


            for (int i = 0; i < 100; i++)
            {
                int p1_card = _random.Next(p1_decksize);
                int p2_card = _random.Next(p2_decksize);
                switch (Round(p1_deck[p1_card], p2_deck[p2_card]))
                {//Bewusst nicht ausgelagert da zwar übersichtlicher aber komplizierter und es nur 3 fälle geben kann
                    case 'a': //player 1 wins
                        p1_deck[7] = p2_deck[p2_card];
                        p2_deck[p2_card] = 0;
                        p1_decksize++;
                        p2_decksize--;
                        p1_deck = DeckSort(p1_deck);
                        p2_deck = DeckSort(p2_deck);
                        
                        break;
                    case 'b': //player 2 wins
                        p2_deck[7] = p1_deck[p1_card];
                        p1_deck[p1_card] = 0;
                        p2_decksize++;
                        p1_decksize--;
                        p1_deck = DeckSort(p1_deck);
                        p2_deck = DeckSort(p2_deck);
                        break;
                    case 'c': //draw
                        continue; //good cause it doesn't make deck amount repeat in log if it isn't changing
                    default:
                        throw new ArgumentException("Fatal Error in Round Logic. Invalid Argument Returned");
                }
                Console.WriteLine($"{_playerList[0].Name} {p1_decksize} | {_playerList[1].Name} {p2_decksize}");
                if (p1_decksize == 8)
                {
                    Console.WriteLine($"{_playerList[0].Name} was victorious");
                    _playerList[0].UpdateElo(_playerList[1].Elo, 'a');
                    _playerList[1].UpdateElo(_playerList[0].Elo, 'b');
                    return $"{_playerList[0].Name} was victorious";
                }
                if (p2_decksize == 8)
                {
                    Console.WriteLine($"{_playerList[1].Name} was victorious");
                    _playerList[0].UpdateElo(_playerList[1].Elo, 'b');
                    _playerList[1].UpdateElo(_playerList[0].Elo, 'a');
                    return $"{_playerList[1].Name} was victorious";
                }
            }
            Console.WriteLine("Game ended in a Draw");
            _playerList[0].UpdateElo(_playerList[1].Elo, 'c');
            _playerList[1].UpdateElo(_playerList[0].Elo, 'c');
            return "Game ended in a Draw";
        }
        private char Round(int A, int B) //can't mark defeated cards cause out of scope
        {
            Card cardA = factory.GenerateCard(A);
            Card cardB = factory.GenerateCard(B);
            int dmgA = cardA.GetDamage(cardB);
            int dmgB = cardB.GetDamage(cardA);
            Console.WriteLine($"{cardA.GetName()} {cardA.Damage} vs. {cardB.GetName()} {cardB.Damage}  --> {dmgA} vs. {dmgB}");
            if (dmgA==dmgB)
            {
                Console.WriteLine($"Draw!");
                return 'c';
            } else if (dmgA>dmgB)
            {
                Console.WriteLine($"Winner: {cardA.GetName()}");
                return 'a';
            } else
            {
                Console.WriteLine($"Winner: {cardB.GetName()}");
                return 'b';
            }

            
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
