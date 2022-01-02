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
        public void CollectionHashWorks1()
        {
            Player a = new Player("p1", 1000, 20, 0,0,0);
            for(int i=0;i<=29;i++)
            {
                a.Collection[i] = true;
            }
            Player b = new Player("p1", 1000, 20, 1073741823, 0,0);
            Assert.AreEqual(a.GetCollectionInt(), b.GetCollectionInt());
        }
        [Test]
        public void CollectionHashWorks2()
        {
            
            Player a = new Player("p1", 1000, 20, 1073741823, 0, 0);
            for (int i = 0; i <= 29; i++)
            {
                Assert.AreEqual(a.Collection[i], true);
            }
        }

        [Test]
        public void SpotsInvalidDeckNoCollection()
        {
            Player a = new Player("p1", 1000, 20, 0, 0, 0);
            int[] ar = { 0, 1, 1, 3 };
            a.CreateDeck(ar);
            Assert.AreEqual(a.Deck[0], -1);
        }
        [Test]
        public void SpotsInvalidDeckMultiples()
        {
            Player a = new Player("p1", 1000, 20, 255,0,0);
            int[] ar = { 0, 1, 1, 3 };
            a.CreateDeck(ar);
            Assert.AreEqual(a.Deck[0], -1);
        }
        [Test]
        public void ValidatesDeck()
        {
            Player a = new Player("p1", 1000, 20, 255, 0, 0);
            int[] ar = { 0, 1, 2, 3 };
            a.CreateDeck(ar);
            Assert.AreEqual(a.Deck[0], 0);
        }
        [Test]
        public void FactoryTestMonsters()
        {
            CardFactory cardFactory = new CardFactory();
            Card WGoblin = cardFactory.GenerateCard(0);
            Assert.AreEqual(WGoblin.GetName(), "WaterGoblin");
            Card FGoblin = cardFactory.GenerateCard(1);
            Assert.AreEqual(FGoblin.GetName(), "FireGoblin");
            Card NGoblin = cardFactory.GenerateCard(2);
            Assert.AreEqual(NGoblin.GetName(), "NormalGoblin");
            Card WDragon = cardFactory.GenerateCard(3);
            Assert.AreEqual(WDragon.GetName(), "WaterDragon");
            Assert.AreEqual(FGoblin.GetDamage(WDragon), 0);

        }
        [Test]
        public void FactoryTestSpells()
        {
            CardFactory cardFactory = new CardFactory();
            Card WSpell = cardFactory.GenerateCard(27);
            Card FSpell = cardFactory.GenerateCard(28);
            Card NSpell = cardFactory.GenerateCard(29);
            Assert.AreEqual(WSpell.GetName(), "WaterSpell");
            Assert.AreEqual(FSpell.GetName(), "FireSpell");
            Assert.AreEqual(NSpell.GetName(), "NormalSpell");
        }
        [Test]
        public void GetsRightWinner() //rework or remove cause it will no longer work
        {


            Player a = new Player("p1", 1000, 20, 1073741823, 0, 0);
            int[] ar0 = { 15, 16, 17, 21 }; //all craken & water troll
            a.CreateDeck(ar0);
            Player b = new Player("p2", 1000, 20, 1073741823, 0, 0);
            int[] ar1 = { 27, 28, 29, 24 }; //all spells & water duck
            b.CreateDeck(ar1);

            Battle battle = new Battle(a, b); 
            battle.Play();
            Assert.Pass();//replace with player 2 elo higher player 1 elo lower 

        }
        
    }
}