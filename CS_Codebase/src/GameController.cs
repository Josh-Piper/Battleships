
using System;
using System.Collections.Generic;
using SwinGameSDK;


namespace Battleships {

	/// <summary>
	/// The GameController is responsible for controlling the game,
	/// managing user input, and displaying the current state of the game.
	/// </summary>
	public class GameController {

		private static BattleShipsGame myGame;
		private static AIPlayer ai;
		private static readonly Stack<GameState> currentState = new Stack<GameState>();
		private static AIOption aiSettings;


		/// <summary>
		/// Returns the current state of the game, indicating which screen is
		/// currently being used
		/// </summary>
		/// <value>The current state</value>
		/// <returns>The current state</returns>
		public static GameState CurrentState {
			get {
				return currentState.Peek();
			}
		}

		/// <summary>
		/// Returns the human player.
		/// </summary>
		/// <value>the human player</value>
		/// <returns>the human player</returns>
		public static Player HumanPlayer { get; private set; }

		/// <summary>
		/// Returns the computer player.
		/// </summary>
		/// <value>the computer player</value>
		/// <returns>the conputer player</returns>
		public static Player ComputerPlayer {
			get {
				return ai;
			}
		}


		static GameController() {

			// Bottom state will be quitting. If player exits main menu then the game is over
			currentState.Push(GameState.Quitting);

			// At the start the player is viewing the main menu
			currentState.Push(GameState.ViewingMainMenu);

		}

		/// <summary>
		/// Starts a new game.
		/// </summary>
		/// <remarks>
		/// Creates an AI player based upon the _aiSetting.
		/// </remarks>
		public static void StartGame() {

			if (myGame is object)
				EndGame();

			// Create the game
			myGame = new BattleShipsGame();

			// Create the player
			HumanPlayer = new Player(myGame);

			// Create the AI
			switch (aiSettings) {
				case AIOption.Medium: {
					ai = new AIMediumPlayer(myGame);
					break;
				}
				case AIOption.Hard: {
					ai = new AIHardPlayer(myGame);
					break;
				}
				default: {
					ai = new AIHardPlayer(myGame);
					break;
				}
			}

			// Assign events
			myGame.AttackCompleted += AttackCompleted;
			HumanPlayer.PlayerGrid.Changed += GridChanged;
			ai.PlayerGrid.Changed += GridChanged;

			// Add `Deploying` state
			AddNewState(GameState.Deploying);

		}

		/// <summary>
		/// Stops listening to the old game once a new game is started
		/// </summary>
		private static void EndGame() {

			// Remove current event listeners
			myGame.AttackCompleted -= AttackCompleted;
			HumanPlayer.PlayerGrid.Changed -= GridChanged;
			ai.PlayerGrid.Changed -= GridChanged;

		}

		/// <summary>
		/// Listens to the game grids for any changes and redraws the screen
		/// when the grids change
		/// </summary>
		/// <param name="sender">the grid that changed</param>
		/// <param name="args">not used</param>
		private static void GridChanged(object sender, EventArgs args) {

			DrawScreen();
			SwinGame.RefreshScreen();

		}

		/// <summary>
		/// Show hit animation and play hit sound effect
		/// </summary>
		/// <param name="row">Row which was hit</param>
		/// <param name="column">Column which was hit</param>
		/// <param name="showAnimation">Weather to show the hit animation</param>
		private static void PlayHitSequence(int row, int column, bool showAnimation) {

			if (showAnimation) {
				UtilityFunctions.AddExplosion(row, column);
			}

			Audio.PlaySoundEffect(GameResources.GetSound("Hit"));
			UtilityFunctions.DrawAnimationSequence();

		}

		/// <summary>
		/// Show miss animation and plays miss sound effect
		/// </summary>
		/// <param name="row">Row which was shot at</param>
		/// <param name="column">Column which was shot at</param>
		/// <param name="showAnimation">Weather to show the miss animation</param>
		private static void PlayMissSequence(int row, int column, bool showAnimation) {

			if (showAnimation) {
				UtilityFunctions.AddSplash(row, column);
			}

			Audio.PlaySoundEffect(GameResources.GetSound("Miss"));
			UtilityFunctions.DrawAnimationSequence();

		}

		/// <summary>
		/// Listens for attacks to be completed.
		/// </summary>
		/// <param name="sender">the game</param>
		/// <param name="result">the result of the attack</param>
		/// <remarks>
		/// Displays a message, plays sound and redraws the screen
		/// </remarks>
		private static void AttackCompleted(object sender, AttackResult result) {

			bool isHuman = myGame.Player == HumanPlayer;

			if (isHuman) {
				UtilityFunctions.Message = "You " + result.ToString();
			}
			else {
				UtilityFunctions.Message = "The AI " + result.ToString();
			}

			switch (result.Value) {
				case ResultOfAttack.Destroyed: {
					PlayHitSequence(result.Row, result.Column, isHuman);
					Audio.PlaySoundEffect(GameResources.GetSound("Sink"));
					break;
				}
				case ResultOfAttack.GameOver: {
					PlayHitSequence(result.Row, result.Column, isHuman);
					Audio.PlaySoundEffect(GameResources.GetSound("Sink"));
					while (Audio.SoundEffectPlaying(GameResources.GetSound("Sink"))) {
						SwinGame.Delay(10);
						SwinGame.RefreshScreen();
					}
					if (HumanPlayer.IsDestroyed) {
						Audio.PlaySoundEffect(GameResources.GetSound("Lose"));
					}
					else {
						Audio.PlaySoundEffect(GameResources.GetSound("Winner"));
					}
					break;
				}
				case ResultOfAttack.Hit: {
					PlayHitSequence(result.Row, result.Column, isHuman);
					break;
				}
				case ResultOfAttack.Miss: {
					PlayMissSequence(result.Row, result.Column, isHuman);
					break;
				}
				case ResultOfAttack.ShotAlready: {
					Audio.PlaySoundEffect(GameResources.GetSound("Error"));
					break;
				}
			}

		}

		/// <summary>
		/// Completes the deployment phase of the game and
		/// switches to the battle mode (Discovering state)
		/// </summary>
		/// <remarks>
		/// This adds the players to the game before switching
		/// state.
		/// </remarks>
		public static void EndDeployment() {

			// Deploy the players
			myGame.AddDeployedPlayer(HumanPlayer);
			myGame.AddDeployedPlayer(ai);

			SwitchState(GameState.Discovering);

		}

		/// <summary>
		/// Gets the player to attack the indicated row and column.
		/// </summary>
		/// <param name="row">the row to attack</param>
		/// <param name="col">the column to attack</param>
		/// <remarks>
		/// Checks the attack result once the attack is complete
		/// </remarks>
		public static void Attack(int row, int col) {

			AttackResult result = myGame.Shoot(row, col);
			CheckAttackResult(result);

		}

		/// <summary>
		/// Gets the AI to attack.
		/// </summary>
		/// <remarks>
		/// Checks the attack result once the attack is complete.
		/// </remarks>
		private static void AIAttack() {

			AttackResult result = myGame.Player.Attack();
			CheckAttackResult(result);

		}

		/// <summary>
		/// Checks the results of the attack and switches to
		/// Ending the Game if the result was game over.
		/// </summary>
		/// <param name="result">the result of the last
		/// attack</param>
		/// <remarks>Gets the AI to attack if the result switched
		/// to the AI player.</remarks>
		private static void CheckAttackResult(AttackResult result) {

			switch (result.Value) {
				case ResultOfAttack.Miss: {
					if (myGame.Player == ComputerPlayer)
						AIAttack();
					break;
				}
				case ResultOfAttack.GameOver: {
					SwitchState(GameState.EndingGame);
					break;
				}
			}

		}

		/// <summary>
		/// Handles the user SwinGame.
		/// </summary>
		/// <remarks>
		/// Reads key and mouse input and converts these into
		/// actions for the game to perform. The actions
		/// performed depend upon the state of the game.
		/// </remarks>
		public static void HandleUserInput() {

			// Read incoming input events
			SwinGame.ProcessEvents();

			switch (CurrentState) {
				case GameState.ViewingMainMenu: {
					MenuController.HandleMainMenuInput();
					break;
				}
				case GameState.ViewingGameMenu: {
					MenuController.HandleGameMenuInput();
					break;
				}
				case GameState.AlteringSettings: {
					MenuController.HandleSetupMenuInput();
					break;
				}
				case GameState.Deploying: {
					DeploymentController.HandleDeploymentInput();
					break;
				}
				case GameState.Discovering: {
					DiscoveryController.HandleDiscoveryInput();
					break;
				}
				case GameState.EndingGame: {
					EndingGameController.HandleEndOfGameInput();
					break;
				}
				case GameState.ViewingHighScores: {
					HighScoreController.HandleHighScoreInput();
					break;
				}
			}

			UtilityFunctions.UpdateAnimations();
			//SwinGame.RefreshScreen();

		}

		/// <summary>
		/// Draws the current state of the game to the screen.
		/// </summary>
		/// <remarks>
		/// What is drawn depends upon the state of the game.
		/// </remarks>
		public static void DrawScreen() {

			UtilityFunctions.DrawBackground();

			switch (CurrentState) {

				case GameState.ViewingMainMenu: {
					MenuController.DrawMainMenu();
					break;
				}
				case GameState.ViewingGameMenu: {
					MenuController.DrawGameMenu();
					break;
				}
				case GameState.AlteringSettings: {
					MenuController.DrawSettings();
					break;
				}
				case GameState.Deploying: {
					DeploymentController.DrawDeployment();
					break;
				}
				case GameState.Discovering: {
					UtilityFunctions.DrawBackground();
					DiscoveryController.DrawDiscovery();
					break;
				}
				case GameState.EndingGame: {
					EndingGameController.DrawEndOfGame();
					break;
				}
				case GameState.ViewingHighScores: {
					HighScoreController.DrawHighScores();
					break;
				}
			}

			UtilityFunctions.DrawAnimations();
			SwinGame.RefreshScreen();

		}

		/// <summary>
		/// Move the game to a new state. The current state is maintained
		/// so that it can be returned to.
		/// </summary>
		/// <param name="state">the new game state</param>
		public static void AddNewState(GameState state) {

			currentState.Push(state);
			UtilityFunctions.Message = "";

		}

		/// <summary>
		/// End the current state and add in the new state.
		/// </summary>
		/// <param name="newState">the new state of the game</param>
		public static void SwitchState(GameState newState) {

			EndCurrentState();
			AddNewState(newState);

		}

		/// <summary>
		/// Ends the current state, returning to the prior state
		/// </summary>
		public static void EndCurrentState() {

			currentState.Pop();

		}

		/// <summary>
		/// Sets the difficulty for the next level of the game.
		/// </summary>
		/// <param name="setting">the new difficulty level</param>
		public static void SetDifficulty(AIOption setting) {

			aiSettings = setting;

		}
	}
}
