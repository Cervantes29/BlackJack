using BlackJackLogicBLL;
using ModelsEL;
using NetTopologySuite.Operation.Valid;

namespace DeckTests

{
    [TestClass]
    public class DeckTest
    {
        /// <summary>
        /// Tests that there is an correct number of cards i the decks
        /// </summary>
        [TestMethod]
        public void TestDeckCreator()
        {
            DeckManager deckCreator = new();
            Random rnd = new Random();
            int randomTestInteger = rnd.Next(500);
            int cardCount = deckCreator.Count(randomTestInteger);
            deckCreator.SetUpDecks(randomTestInteger);
            Assert.AreEqual(cardCount, randomTestInteger * 52);
        }

        /// <summary>
        /// Tests that the common cards (Not aces) are calculated properly
        /// </summary>
        [TestMethod]
        public void TestHandValueExcludingAces()
        {
            DeckManager deckCreator = new();
            Player player = new();
            Card card1 = new Card(SuiteShorthand.s, 13);
            Card card2 = new Card(SuiteShorthand.c, 12);
            Card card3 = new Card(SuiteShorthand.h, 11);
            Card card4 = new Card(SuiteShorthand.d, 10);
            Card card5 = new Card(SuiteShorthand.c, 9);
            Card card6 = new Card(SuiteShorthand.c, 2);
            player.AddCard(card1);
            player.AddCard(card2);
            player.AddCard(card3);
            player.AddCard(card4);
            player.AddCard(card5);
            player.AddCard(card6);

            //int value = deckCreator.Count(result);
            //deckCreator.SetUpDecks(result);
            Assert.AreEqual(player.Hand.HandValueTotal(), 51);
        }

        /// <summary>
        /// Tests a series of different card calculations, all including aces
        /// </summary>
        [TestMethod]
        public void TestAcesHandValue()
        {
            DeckManager deckCreator = new();
            Player player = new();
            Card ace = new Card(SuiteShorthand.s, 1);
            Card two = new Card(SuiteShorthand.c, 2);
            Card three = new Card(SuiteShorthand.c, 3);
            Card nine = new Card(SuiteShorthand.c, 9);
            Card ten = new Card(SuiteShorthand.d, 10);
            Card jack = new Card(SuiteShorthand.h, 11);
            Card queen = new Card(SuiteShorthand.h, 12);
            Card king = new Card(SuiteShorthand.h, 13);

            player.Hand.Clear();
            player.AddCard(ace);
            player.AddCard(ace);
            Assert.AreEqual(player.Hand.HandValueTotal(), 12);
            player.AddCard(ace);
            Assert.AreEqual(player.Hand.HandValueTotal(), 13);
            player.AddCard(ace);
            Assert.AreEqual(player.Hand.HandValueTotal(), 14);
            player.AddCard(jack);
            Assert.AreEqual(player.Hand.HandValueTotal(), 14);
            player.AddCard(two);
            Assert.AreEqual(player.Hand.HandValueTotal(), 16);
            player.AddCard(three);
            Assert.AreEqual(player.Hand.HandValueTotal(), 19);

            player.Hand.Clear();
            player.AddCard(ten);
            player.AddCard(ace);
            Assert.AreEqual(player.Hand.HandValueTotal(), 21);
            player.Hand.Clear();
            player.AddCard(two);
            player.AddCard(nine);
            player.AddCard(three);
            player.Hand.Clear();
            player.AddCard(jack);
            player.AddCard(queen);
            player.AddCard(king);
            Assert.AreEqual(player.Hand.HandValueTotal(), 30);
        }
    }
}