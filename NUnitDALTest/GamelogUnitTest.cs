using GamelogLib;
using ModelsEL;

namespace GamelogTests
{
    public class GamelogTests
    {
        private GamelogManager gamelogManager;
        private PossibleHandsDBGenerator possibleHandsDBGenerator;
        private User user;

        /// <summary>
        /// Sets up some classes that are to be tested
        /// </summary>
        [SetUp]
        public void Setup()
        {
            gamelogManager = new GamelogManager();
            possibleHandsDBGenerator = new PossibleHandsDBGenerator();
            user = new();
        }

        /// <summary>
        /// Tests so that win and loss percentages are calculated properly
        /// </summary>
        [Test]
        public void TestWinPctTest()
        {
            (float, float, float) result;

            result = gamelogManager.PctCalculator(12345f, 0f, 12345f, 321500);
            Assert.That(result.Item1, Is.EqualTo(50f));
        }

        /// <summary>
        /// Tests so that win and loss percentages are calculated properly
        /// </summary>
        [Test]
        public void TestWinPctTest2()
        {
            (float, float, float) result;

            result = gamelogManager.PctCalculator(100f, 0f, 200f, 1000);
            Assert.That(result.Item2, Is.EqualTo(0f));
        }

        /// <summary>
        /// Tests so that win and loss percentages are calculated properly
        /// </summary>
        [Test]
        public void TestWinPctTest3()
        {
            (float, float, float) result;
            result = gamelogManager.PctCalculator(200f, 0f, 800f, 321500);
            Assert.That(result.Item3, Is.EqualTo(80f));
        }

        /// <summary>
        /// Tests so that the number of possible hands are calculated to the correct amount
        /// </summary>
        [Test]
        public void PossiblePlayerHandsTest()
        {
            int possibleDealerHands = possibleHandsDBGenerator.PlayerPossibleHandsList().Count;

            Assert.That(possibleDealerHands, Is.EqualTo(55));
        }

        /// <summary>
        /// Can be used to clean upp test values
        /// </summary>
        [TearDown]
        public void ClearUp()
        {
        }
    }
}