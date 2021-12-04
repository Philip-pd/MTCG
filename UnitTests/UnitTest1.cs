using NUnit.Framework;
using MTCG.SystemLogicClasses;


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
    }
}