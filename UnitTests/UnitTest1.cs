using NUnit.Framework;
using MTCG.SystemLogicClasses;
using MTCG.GameplayLogicClasses;
using System.IO;
using System;
using System.Collections.Generic;

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
        public void FactoryTestMonsters_Goblins()
        {
            CardFactory cardFactory = new CardFactory();
            Card WGoblin = cardFactory.GenerateCard(0);
            Assert.AreEqual(WGoblin.GetName(), "WaterGoblin");
            Card FGoblin = cardFactory.GenerateCard(1);
            Assert.AreEqual(FGoblin.GetName(), "FireGoblin");
            Card NGoblin = cardFactory.GenerateCard(2);
            Assert.AreEqual(NGoblin.GetName(), "NormalGoblin");
            Card WDragon = cardFactory.GenerateCard(3);
            Assert.AreEqual(FGoblin.GetDamage(WDragon), 0);

        }
        [Test]
        public void FactoryTestMonsters_Dragons()
        {
            CardFactory cardFactory = new CardFactory();
            Card WDragon = cardFactory.GenerateCard(3);
            Assert.AreEqual(WDragon.GetName(), "WaterDragon");
            Card FDragon = cardFactory.GenerateCard(4);
            Assert.AreEqual(FDragon.GetName(), "FireDragon");
            Card NDragon = cardFactory.GenerateCard(5);
            Assert.AreEqual(NDragon.GetName(), "NormalDragon");
            Card FElf = cardFactory.GenerateCard(19);
            Assert.AreEqual(FDragon.GetDamage(FElf), 0);

        }
        [Test]
        public void FactoryTestMonsters_Wizzards()
        {
            CardFactory cardFactory = new CardFactory();
            Card WWizzard = cardFactory.GenerateCard(6);
            Assert.AreEqual(WWizzard.GetName(), "WaterWizzard");
            Card FWizzard = cardFactory.GenerateCard(7);
            Assert.AreEqual(FWizzard.GetName(), "FireWizzard");
            Card NWizzard = cardFactory.GenerateCard(8);
            Assert.AreEqual(NWizzard.GetName(), "NormalWizzard");
            Card FGoblin = cardFactory.GenerateCard(1);
            Assert.AreEqual(WWizzard.GetDamage(FGoblin), 44);

        }
        [Test]
        public void FactoryTestMonsters_Orcs()
        {
            CardFactory cardFactory = new CardFactory();
            Card WOrc = cardFactory.GenerateCard(9);
            Assert.AreEqual(WOrc.GetName(), "WaterOrc");
            Card FOrc = cardFactory.GenerateCard(10);
            Assert.AreEqual(FOrc.GetName(), "FireOrc");
            Card NOrc = cardFactory.GenerateCard(11);
            Assert.AreEqual(NOrc.GetName(), "NormalOrc");
            Card NWizzard = cardFactory.GenerateCard(8); ;
            Assert.AreEqual(WOrc.GetDamage(NWizzard), 0);

        }
        [Test]
        public void FactoryTestMonsters_Knights()
        {
            CardFactory cardFactory = new CardFactory();
            Card WKnight = cardFactory.GenerateCard(12);
            Assert.AreEqual(WKnight.GetName(), "WaterKnight");
            Card FKnight = cardFactory.GenerateCard(13);
            Assert.AreEqual(FKnight.GetName(), "FireKnight");
            Card NKnight = cardFactory.GenerateCard(14);
            Assert.AreEqual(NKnight.GetName(), "NormalKnight");
            Assert.AreEqual(WKnight.GetDamage(NKnight), 25);//knights have no special rules
        }
        [Test]
        public void FactoryTestMonsters_Kraken()
        {
            CardFactory cardFactory = new CardFactory();
            Card WKraken = cardFactory.GenerateCard(15);
            Assert.AreEqual(WKraken.GetName(), "WaterKraken");
            Card FKraken = cardFactory.GenerateCard(16);
            Assert.AreEqual(FKraken.GetName(), "FireKraken");
            Card NKraken = cardFactory.GenerateCard(17);
            Assert.AreEqual(NKraken.GetName(), "NormalKraken");
            Card WSpell = cardFactory.GenerateCard(27);
            Assert.AreEqual(WKraken.GetDamage(WSpell), 999);//Kraken VS Spell
            Assert.AreEqual(WKraken.GetDamage(FKraken), 0);//Kraken VS Fire
        }

        [Test]
        public void FactoryTestMonsters_Elf()
        {
            CardFactory cardFactory = new CardFactory();
            Card WElf = cardFactory.GenerateCard(18);
            Assert.AreEqual(WElf.GetName(), "WaterElf");
            Card FElf = cardFactory.GenerateCard(19);
            Assert.AreEqual(FElf.GetName(), "FireElf");
            Card NElf = cardFactory.GenerateCard(20);
            Assert.AreEqual(NElf.GetName(), "NormalElf");
            Assert.AreEqual(WElf.GetDamage(FElf), 24);//No Special cases
        }
        [Test]
        public void FactoryTestMonsters_Troll()
        {
            CardFactory cardFactory = new CardFactory();
            Card WTroll = cardFactory.GenerateCard(21);
            Assert.AreEqual(WTroll.GetName(), "WaterTroll");
            Card FTroll = cardFactory.GenerateCard(22);
            Assert.AreEqual(FTroll.GetName(), "FireTroll");
            Card NTroll = cardFactory.GenerateCard(23);
            Assert.AreEqual(NTroll.GetName(), "NormalTroll");
            Assert.AreEqual(WTroll.GetDamage(FTroll), 30);//No Special cases
        }

        [Test]
        public void FactoryTestMonsters_Duck()
        {
            CardFactory cardFactory = new CardFactory();
            Card WDuck = cardFactory.GenerateCard(24);
            Assert.AreEqual(WDuck.GetName(), "WaterDuck");
            Card FDuck = cardFactory.GenerateCard(25);
            Assert.AreEqual(FDuck.GetName(), "FireDuck");
            Card NDuck = cardFactory.GenerateCard(26);
            Assert.AreEqual(NDuck.GetName(), "NormalDuck");
            Card NKnight = cardFactory.GenerateCard(14);
            Assert.AreEqual(WDuck.GetDamage(NKnight), 50);
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
        public void TestSpellToSpell()
        {
            CardFactory cardFactory = new CardFactory();
            Card WSpell = cardFactory.GenerateCard(27);
            Card FSpell = cardFactory.GenerateCard(28);
            Assert.AreEqual(WSpell.GetDamage(FSpell), 40);
            Assert.AreEqual(FSpell.GetDamage(WSpell), 10);
        }
        [Test]
        public void TestSpellToMonster()
        {
            CardFactory cardFactory = new CardFactory();
            Card WSpell = cardFactory.GenerateCard(27);
            Card FGoblin = cardFactory.GenerateCard(1);
            Assert.AreEqual(WSpell.GetDamage(FGoblin), 40);
            Assert.AreEqual(FGoblin.GetDamage(WSpell), 10);
        }
        [Test]
        public void GetsRightWinner() 
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
        public void DrawPossible() //Should never happen like this but done like that to guarantee outcome
        {


            int[] ar0 = { 1, 1, 1, 1 }; //all same card (usually not possible cause fetch from db and gets checked before it is entered there)
            Player a = new Player("p1", 1000, 20, 1073741823, 0, 0, ar0);

            int[] ar1 = { 1, 1, 1, 1 }; //all same card
            Player b = new Player("p2", 1000, 20, 1073741823, 0, 0, ar1);

            Battle battle = new Battle(a, b);
            battle.Play();
            Assert.AreEqual(a.Elo, b.Elo);

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
        public void PlayerHandlerEnterMMWorks()
        {
            PlayerHandler handler = PlayerHandler.Instance;
            int[] ar0 = { 15, 16, 17, 21 };
            Player v = new Player("p6", 1000, 20, 1073741823, 0, 0, ar0); //need new player cause one from last test still persists cause singleton
            Assert.AreEqual(handler.PlayerLogin(v), true);
            Assert.AreEqual(handler.PlayerEnterMM("p6-Token"), "Player Successfully entered Queue. Waiting for Opponent...");
            Assert.AreEqual(handler.GetPlayerMatchmaking("p6-Token"),v);
            Assert.AreEqual(handler.PlayerLogout("p6-Token"), true);
            Assert.AreEqual(handler.RemovefromQueue("p6-Token"), true);
        }
        [Test]
        public void PlayerHandlerMMWorks()
        {
            PlayerHandler handler = PlayerHandler.Instance;
            int[] ar0 = { 1, 1, 1, 1 };
            Player foo = new Player("p7", 1000, 20, 1073741823, 0, 0, ar0); //need new player cause one from last test still persists cause singleton
            Player bar = new Player("p8", 1000, 20, 1073741823, 0, 0, ar0);
            Assert.AreEqual(handler.PlayerLogin(foo), true);
            Assert.AreEqual(handler.PlayerLogin(bar), true);
            Assert.AreEqual(handler.PlayerEnterMM("p7-Token"), "Player Successfully entered Queue. Waiting for Opponent...");
            Assert.AreEqual(handler.PlayerEnterMM("p8-Token"), "Game ended in a Draw");
        }
        [Test]
        public void PlayerHandlerMMBlockWorks()
        {
            PlayerHandler handler = PlayerHandler.Instance;
            int[] ar0 = { -1, 1, 1, 1 };
            Player invalid = new Player("p9", 1000, 20, 1073741823, 0, 0, ar0); //need new player cause one from last test still persists cause singleton
            Assert.AreEqual(handler.PlayerLogin(invalid), true); //These Checks are here to see if it broke cause I messed up singleton or because actual test is broken
            Assert.AreEqual(handler.PlayerEnterMM("p9-Token"), "InvalidDeck");
        }
        [Test]
        public void PackAddsToCollection()
        {

            Player a = new Player("p1", 1000, 20, 0, 0, 0, null); 
            BoosterPack pack = new BoosterPack();
            Assert.AreEqual(a.AddBoosterToCollection(pack.GetPack(1)), 5);
            Assert.AreEqual(a.GetCollectionInt(), 31);
        }
        [Test]
        public void PackRefundsPartialDuplicates()
        {

            Player a = new Player("p1", 1000, 20, 0, 0, 0, null); BoosterPack pack = new BoosterPack();
            Assert.AreEqual(a.AddBoosterToCollection(pack.GetPack(2)), 5);
            Assert.AreEqual(a.GetCollectionInt(), 992);
            Assert.AreEqual(a.AddBoosterToCollection(pack.GetPack(7)), 4);
            Assert.AreEqual(a.GetCollectionInt(), 34637792);
            Assert.AreEqual(a.Coins, 11);
        }
        [Test]
        public void PackRefundsDuplicates()
        {
            Player a = new Player("p1", 1000, 20, 1073741823, 0, 0, null);
            BoosterPack pack = new BoosterPack();
            Assert.AreEqual(a.AddBoosterToCollection(pack.GetPack(2)),0);

        }
        [Test]
        public void CanCreateTrade()
        {
            Player a = new Player("p1", 1000, 20, 1073741823, 0, 0, null);
            TradeDAO tdao = new TradeDAOImpl();
            Trade trade = new Trade(0, "p1", 1, -1, 2);
            Assert.AreEqual(tdao.CreateTrade(a, trade), true);
        }

        [Test]
        public void CanCancelTrade()
        {
            Player a = new Player("p1", 1000, 20, 1073741823, 0, 0, null);
            TradeDAO tdao = new TradeDAOImpl();
            Trade trade = new Trade(0, "p1", 1, -1, 2);
            tdao.CreateTrade(a, trade);
            List<Trade> trades = tdao.GetAllTrades();
            Assert.AreEqual(tdao.CancelTrade(a, trades[trades.Count-1].ID), true);
        }

        [Test]
        public void CancelTradeReturnstoCollection()
        {
            Player a = new Player("p1", 1000, 20, 1073741823, 0, 0, null);
            TradeDAO tdao = new TradeDAOImpl();
            Trade trade = new Trade(0, "p1", 1, -1, 2);
            tdao.CreateTrade(a, trade);
            List<Trade> trades = tdao.GetAllTrades();
            Assert.AreEqual(tdao.CancelTrade(a, trades[trades.Count - 1].ID), true);
            Assert.AreEqual(a.GetCollectionInt(), 1073741823);
        }

        [Test]
        public void CanCancelTradeAndRecieveMoney()
        {
            Player a = new Player("p1", 1000, 20, 1073741823, 0, 0, null);
            TradeDAO tdao = new TradeDAOImpl();
            Trade trade = new Trade(0, "p1", 1, -1, 2);
            tdao.CreateTrade(a, trade);
            a.Collection[1] = true;
            List<Trade> trades = tdao.GetAllTrades();
            tdao.CancelTrade(a, trades[trades.Count - 1].ID);
            Assert.AreEqual(a.Coins, 21);
        }

    }
}