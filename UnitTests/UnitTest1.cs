using NUnit.Framework;
using MTCG.SystemLogicClasses;
using MTCG.GameplayLogicClasses;


namespace UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void SpotsInvalidDeck()
        {
            Player a = new Player("p1", "123");
            int[] ar = { 0, 1, 1, 3 };
            a.CreateDeck(ar);
            Assert.AreEqual(a.Deck[0], -1);
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
    }
}