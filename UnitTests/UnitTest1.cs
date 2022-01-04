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
            Player a = new Player("p1", 1000, 20, 0,0,0,null);
            for(int i=0;i<=29;i++)
            {
                a.Collection[i] = true;
            }
            Player b = new Player("p1", 1000, 20, 1073741823, 0,0,null);
            Assert.AreEqual(a.GetCollectionInt(), b.GetCollectionInt());
        }
        [Test]
        public void CollectionHashWorks2()
        {
            
            Player a = new Player("p1", 1000, 20, 1073741823, 0, 0,null);
            for (int i = 0; i <= 29; i++)
            {
                Assert.AreEqual(a.Collection[i], true);
            }
        }
        [Test]
        public void ReturnsRightCollection()
        {
            Player a = new Player("p1", 1000, 20, 255, 0, 0,null);
            Assert.AreEqual(a.ReturnCollection(),"[true,true,true,true,true,true,true,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false]");
        }
        [Test]
        public void SpotsInvalidDeckNoCollection() //doesn't need to check if deck in constructor is valid cause it will always come from SQL which means it already got validated
        {
            Player a = new Player("p1", 1000, 20, 0, 0, 0,null);
            int[] ar = { 0, 1, 1, 3 };
            
            Assert.AreEqual(a.CreateDeck(ar), false);
        }
        [Test]
        public void SpotsInvalidDeckMultiples()
        {
            Player a = new Player("p1", 1000, 20, 255,0,0,null);
            int[] ar = { 0, 1, 1, 3 };
            Assert.AreEqual(a.CreateDeck(ar), false);
        }
        [Test]
        public void ValidatesDeck()
        {
            Player a = new Player("p1", 1000, 20, 255, 0, 0,null);
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


            int[] ar0 = { 15, 16, 17, 21 }; //all craken & water troll
            Player a = new Player("p1", 1000, 20, 1073741823, 0, 0,ar0);

            int[] ar1 = { 27, 28, 29, 24 }; //all spells & water duck
            Player b = new Player("p2", 1000, 20, 1073741823, 0, 0,ar1);

            Battle battle = new Battle(a, b); 
            battle.Play();
            Assert.Greater(a.Elo,b.Elo);

        }
        [Test]
        public void PlayerHandlerLoginWorks()
        {
            PlayerHandler handler = PlayerHandler.Instance;
            Player a = new Player("p1", 1000, 20, 1073741823, 0, 0,null);
            Player b = new Player("p2", 1000, 20, 1073741823, 0, 0, null);
            Assert.AreEqual(handler.PlayerLogin(a), true); //unique player logs in
            Assert.AreEqual(handler.PlayerLogin(a), false); //same player tries again
            Assert.AreEqual(handler.PlayerLogin(b), true); //another different player tries logging in

        }
        [Test]
        public void PlayerHandlerRetrieveWorks()
        {
            PlayerHandler handler = PlayerHandler.Instance;
            Player c = new Player("p3", 1000, 20, 1073741823, 0, 0, null); //need new player cause one from last test still persists cause singleton
            Assert.AreEqual(handler.PlayerLogin(c), true);
            Assert.AreEqual(handler.GetPlayerOnline("p3-Token"), c); 
        }
        [Test]
        public void PlayerHandlerLogOutWorks()
        {
            PlayerHandler handler = PlayerHandler.Instance;
            Player d = new Player("p4", 1000, 20, 1073741823, 0, 0, null);
            Player e = new Player("p5", 1000, 20, 1073741823, 0, 0, null);
            Assert.AreEqual(handler.PlayerLogin(d), true); //unique player logs in
            Assert.AreEqual(handler.PlayerLogin(e), true); //2nd unique player logs in
            Assert.AreEqual(handler.PlayerLogout("p4-Token"), true); //player logs out
            Assert.AreEqual(handler.GetPlayerOnline("p4-Token"), null); //player doesn't exist in online list anymore
            Assert.AreEqual(handler.GetPlayerOnline("p5-Token"), e); //p5 still exists
            Assert.AreEqual(handler.PlayerLogin(d), true); //same player tries again and succeeds
            

        }
        [Test]
        public void PackAddsToCollection()
        {

            Player a = new Player("p1", 1000, 20, 0, 0, 0, null); 
            BoosterPack pack = new BoosterPack();
            Assert.AreEqual(a.AddBoosterToCollection(pack.GetPack(0)), 5);
            Assert.AreEqual(a.GetCollectionInt(), 31);
        }
        [Test]
        public void PackRefundsPartialDuplicates()
        {

            Player a = new Player("p1", 1000, 20, 0, 0, 0, null); BoosterPack pack = new BoosterPack();
            Assert.AreEqual(a.AddBoosterToCollection(pack.GetPack(1)), 5);
            Assert.AreEqual(a.GetCollectionInt(), 992);
            Assert.AreEqual(a.AddBoosterToCollection(pack.GetPack(6)), 4);
            Assert.AreEqual(a.GetCollectionInt(), 34637792);
            Assert.AreEqual(a.Coins, 11);
        }
        [Test]
        public void PackRefundsDuplicates()
        {
            Player a = new Player("p1", 1000, 20, 1073741823, 0, 0, null);
            BoosterPack pack = new BoosterPack();
            Assert.AreEqual(a.AddBoosterToCollection(pack.GetPack(1)),0);

        }
        //test Make Trades
        //test Test Accept Trades
        //test can't accept trade if already in posession of card
        //test Cancel Trade
        //test sold card but already has so gets some gold instead 
        //test Enter MM
        //Test a few Elo Calculations
        //Test some Database Fetches
        //Check if I get blocked from adding same user to DB without server Crashing
        //test PlayerDAO whatever

    }
}