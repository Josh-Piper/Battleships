using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Battleships;


namespace ChangeDifficultyUnitTest
{
    [TestFixture]


    public class ChangeDifficultyUnitTest
    {
        //declaration
        private GameController _game;

        [SetUp]
        public void SetUp()
        {
            GameController.StartGame();
           _game = new GameController();
        }

        [Test]
        public void TestHere()
        {
            GameController.SetDifficulty(AIOption.Hard);
            Assert.AreEqual(_game.getOption, GameController.Difficulty);
            TestContext.WriteLine(_game.getOption);
            TestContext.WriteLine(GameController.Difficulty);
           // _game.getOption == GameController.ComputerPlayer.
            
        }
    }
}
