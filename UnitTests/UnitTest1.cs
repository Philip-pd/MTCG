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
        public void ReturnsRightCollection()
        {
            Player a = new Player("p1", 1000, 20, 255, 0, 0);
            Assert.AreEqual(a.ReturnCollection(),"[true,true,true,true,true,true,true,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false]");
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
            Assert.Greater(a.Elo,b.Elo);

        }
        [Test]
        public void PlayerHandlerLoginWorks()
        {
            PlayerHandler handler = PlayerHandler.Instance;
            Player a = new Player("p1", 1000, 20, 1073741823, 0, 0);
            Player b = new Player("p2", 1000, 20, 1073741823, 0, 0);
            Assert.AreEqual(handler.PlayerLogin(a), true); //unique player logs in
            Assert.AreEqual(handler.PlayerLogin(a), false); //same player tries again
            Assert.AreEqual(handler.PlayerLogin(b), true); //another different player tries logging in

        }
        [Test]
        public void PlayerHandlerRetrieveWorks()
        {
            PlayerHandler handler = PlayerHandler.Instance;
            Player c = new Player("p3", 1000, 20, 1073741823, 0, 0); //need new player cause one from last test still persists cause singleton
            Assert.AreEqual(handler.PlayerLogin(c), true);
            Assert.AreEqual(handler.GetPlayerOnline("p3-Token"), c); 
        }
        //test open packs adds to collection
        //test Make Trades
        //test Test Accept Trades
        //test can't accept trade if already in posession of card
        //test can't cancel trade if got same card
        //test sold card but already has so gets some gold instead //be like need to do something here but its kinda broken shouldn't have went with 1 card only
        //test Enter MM
        //Test a few Elo Calculations
        //Test some Database Fetches
        //Check if I get blocked from adding same user to DB without server Crashing
        //test open packs returns money
        //test PlayerDAO whatever

    }
}