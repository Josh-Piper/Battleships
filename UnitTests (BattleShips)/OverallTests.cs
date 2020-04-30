using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Battleships;


namespace OverallTests
{
    [TestFixture]


    public class OverallTests
    {
        //declaration
        private BattleShipsGame _game;
        private Player _player;
        private AIPlayer _ai;

        [SetUp]
        public void SetUp()
        {
            _game = new BattleShipsGame();
            _player = new Player(_game);
            _ai = new AIHardPlayer(_game);

        }

        [Test]
        public void TestHere()
        {
           
            GameResources.LoadResources();
           //TO DO
            GameController.SetDifficulty(AIOption.Medium);
            //MenuController.PerformSetupMenuAction();
            //Assert.AreEqual(_ai, )
            
            
        }
    }
}
