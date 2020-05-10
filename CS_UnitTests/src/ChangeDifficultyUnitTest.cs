
using NUnit.Framework;
using Battleships;


namespace CS_UnitTests {

	[TestFixture]
	public class ChangeDifficultyUnitTest {

		//declaration
		private GameController _game;
		private BattleShipsGame _loadedGame;
		private Player _humanPlayer;

		[SetUp]
		public void SetUp() {

			GameController.StartGame();
			GameResources.LoadResources();
			_game = new GameController();
			_loadedGame = new BattleShipsGame();
			_humanPlayer = new Player(_loadedGame);

		}

		[Test]
		public void TestHere() {

			_loadedGame.AddDeployedPlayer(_humanPlayer);
			_loadedGame.Player.EnemyGrid.HitTile(1, 3);
			_loadedGame.Shoot(1, 2);
			//GameController.SetDifficulty(AIOption.Hard);
			//Assert.AreEqual(_game.getOption, GameController.Difficulty);
			//TestContext.WriteLine(_game.getOption);
			//TestContext.WriteLine(GameController.Difficulty);
			// _game.getOption == GameController.ComputerPlayer.

		}
	}
}
