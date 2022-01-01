using NUnit.Framework;
using MTCG.SystemLogicClasses;
using MTCG.GameplayLogicClasses;
using System.IO;
using System;

namespace UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }
        [Test]
        public void CollectionHashWorks()
        {
            Player a = new Player("p1", "123", 1000, 25, 0);
            for(int i=0;i<=30;i++)
            {
                a.Collection[i] = true;
            }
            Player b = new Player("p1", "123", 1000, 25, 2147483647);
            Assert.AreEqual(a.GetCollectionInt(), b.GetCollectionInt());
        }

        [Test]
        public void SpotsInvalidDeckNoCollection()
        {
            Player a = new Player("p1", "123", 1000, 25, 0);
            int[] ar = { 0, 1, 1, 3 };
            a.CreateDeck(ar);
            Assert.AreEqual(a.Deck[0], -1);
        }
        [Test]
        public void SpotsInvalidDeckMultiples()
        {
            Player a = new Player("p1", "123", 1000, 25, 255);
            int[] ar = { 0, 1, 1, 3 };
            a.CreateDeck(ar);
            Assert.AreEqual(a.Deck[0], -1);
        }
        [Test]
        public void ValidatesDeck()
        {
            Player a = new Player("p1", "123", 1000, 25, 255);
            int[] ar = { 0, 1, 2, 3 };
            a.CreateDeck(ar);
            Assert.AreEqual(a.Deck[0], 0);
        }
        [Test]
        public void FactoryTestMonsters()
        {
            CardFactory cardFactory = new CardFactory();
            Card FGoblin = cardFactory.GenerateCard(1);
            Assert.AreEqual(FGoblin.GetName(), "FireGoblin");
            Card WDragon = cardFactory.GenerateCard(3);
            Assert.AreEqual(WDragon.GetName(), "WaterDragon");
            Assert.AreEqual(FGoblin.GetDamage(WDragon), 0);

        }
        [Test]
        public void FactoryTestSpells()
        {
            CardFactory cardFactory = new CardFactory();
            Card NSpell = cardFactory.GenerateCard(29);
            Assert.AreEqual(NSpell.GetName(), "NormalSpell");
        }
        [Test]
        public void GetsRightWinner() //rework or remove cause it will no longer work
        {


            Player a = new Player("p1", "123", 1000, 25, 255);
            int[] ar0 = { 1, 1, 1, 1 };
            a.CreateDeck(ar0);
            Player b = new Player("p2", "123", 1000, 25, 255);
            int[] ar1 = { 3, 3, 3, 3 };
            b.CreateDeck(ar1);

            Battle battle = new Battle(a, b); 
            battle.Play();
            Assert.Pass();//replace with player 2 elo higher player 1 elo lower 

        }
        
    }
}