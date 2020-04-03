using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwinGameSDK;
using static SwinGameSDK.SwinGame;

namespace MyGame
{
    public class GameController
    {
        private static BattleShipsGame _theGame;
        private static Player _human;
        private static AIPlayer _ai;
        private static Stack<GameState> _state = new Stack<GameState>();
        private static AIOption _aiSetting;

        /// <summary>
        /// Returns the current state of the game, indicating which screen is
        /// currently being used
        /// </summary>
        /// <value>The current state</value>
        /// <returns>The current state</returns>
        public static GameState CurrentState
        {
            get
            {
                return _state.Peek();
            }
        }

        /// <summary>
        /// Returns the human player.
        /// </summary>
        /// <value>the human player</value>
        /// <returns>the human player</returns>
        public static Player HumanPlayer
        {
            get
            {
                return _human;
            }
        }

        /// <summary>
        /// Returns the computer player.
        /// </summary>
        /// <value>the computer player</value>
        /// <returns>the conputer player</returns>
        public static Player ComputerPlayer
        {
            get
            {
                return _ai;
            }
        }

        static GameController()
        {
            // bottom state will be quitting. If player exits main menu then the game is over
            _state.Push(GameState.Quitting);

            // at the start the player is viewing the main menu
            _state.Push(GameState.ViewingMainMenu);
        }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        /// <remarks>
        /// Creates an AI player based upon the _aiSetting.
        /// </remarks>
        public static void StartGame()
        {
            if (_theGame is object)
                EndGame();

            // Create the game
            _theGame = new BattleShipsGame();

            // create the players
            var switchExpr = _aiSetting;
            switch (switchExpr)
            {
                case var @case when @case == AIOption.Medium:
                    {
                        _ai = new AIMediumPlayer(_theGame);
                        break;
                    }

                case var case1 when case1 == AIOption.Hard:
                    {
                        _ai = new AIHardPlayer(_theGame);
                        break;
                    }

                default:
                    {
                        _ai = new AIHardPlayer(_theGame);
                        break;
                    }
            }

            _human = new Player(_theGame);

            // AddHandler _human.PlayerGrid.Changed, AddressOf GridChanged
            global::GameController._ai.PlayerGrid.Changed += GridChanged;
            global::GameController._theGame.AttackCompleted += AttackCompleted;
            AddNewState(GameState.Deploying);
        }

        /// <summary>
        /// Stops listening to the old game once a new game is started
        /// </summary>

        private static void EndGame()
        {
            // RemoveHandler _human.PlayerGrid.Changed, AddressOf GridChanged
            global::GameController._ai.PlayerGrid.Changed -= GridChanged;
            global::GameController._theGame.AttackCompleted -= AttackCompleted;
        }

        /// <summary>
        /// Listens to the game grids for any changes and redraws the screen
        /// when the grids change
        /// </summary>
        /// <param name="sender">the grid that changed</param>
        /// <param name="args">not used</param>
        private static void GridChanged(object sender, EventArgs args)
        {
            DrawScreen();
            SwinGame.RefreshScreen();
        }

        private static void PlayHitSequence(int row, int column, bool showAnimation)
        {
            if (showAnimation)
            {
                UtilityFunctions.AddExplosion(row, column);
            }

            Audio.PlaySoundEffect(GameResources.GameSound("Hit"));
            UtilityFunctions.DrawAnimationSequence();
        }

        private static void PlayMissSequence(int row, int column, bool showAnimation)
        {
            if (showAnimation)
            {
                UtilityFunctions.AddSplash(row, column);
            }

            Audio.PlaySoundEffect(GameResources.GameSound("Miss"));
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
        private static void AttackCompleted(object sender, AttackResult result)
        {
            bool isHuman;
            isHuman = _theGame.Player == HumanPlayer;
            if (isHuman)
            {
                UtilityFunctions.Message = "You " + result.ToString();
            }
            else
            {
                UtilityFunctions.Message = "The AI " + result.ToString();
            }

            var switchExpr = result.Value;
            switch (switchExpr)
            {
                case var @case when @case == ResultOfAttack.Destroyed:
                    {
                        PlayHitSequence(result.Row, result.Column, isHuman);
                        Audio.PlaySoundEffect(GameResources.GameSound("Sink"));
                        break;
                    }

                case var case1 when case1 == ResultOfAttack.GameOver:
                    {
                        PlayHitSequence(result.Row, result.Column, isHuman);
                        Audio.PlaySoundEffect(GameResources.GameSound("Sink"));
                        while (Audio.SoundEffectPlaying(GameResources.GameSound("Sink")))
                        {
                            SwinGame.Delay(10);
                            SwinGame.RefreshScreen();
                        }

                        if (HumanPlayer.IsDestroyed)
                        {
                            Audio.PlaySoundEffect(GameResources.GameSound("Lose"));
                        }
                        else
                        {
                            Audio.PlaySoundEffect(GameResources.GameSound("Winner"));
                        }

                        break;
                    }

                case var case2 when case2 == ResultOfAttack.Hit:
                    {
                        PlayHitSequence(result.Row, result.Column, isHuman);
                        break;
                    }

                case var case3 when case3 == ResultOfAttack.Miss:
                    {
                        PlayMissSequence(result.Row, result.Column, isHuman);
                        break;
                    }

                case var case4 when case4 == ResultOfAttack.ShotAlready:
                    {
                        Audio.PlaySoundEffect(GameResources.GameSound("Error"));
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
        public static void EndDeployment()
        {
            // deploy the players
            _theGame.AddDeployedPlayer(_human);
            _theGame.AddDeployedPlayer(_ai);
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
        public static void Attack(int row, int col)
        {
            AttackResult result;
            result = _theGame.Shoot(row, col);
            CheckAttackResult(result);
        }

        /// <summary>
        /// Gets the AI to attack.
        /// </summary>
        /// <remarks>
        /// Checks the attack result once the attack is complete.
        /// </remarks>
        private static void AIAttack()
        {
            AttackResult result;
            result = _theGame.Player.Attack();
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
        private static void CheckAttackResult(AttackResult result)
        {
            var switchExpr = result.Value;
            switch (switchExpr)
            {
                case var @case when @case == ResultOfAttack.Miss:
                    {
                        if (_theGame.Player == ComputerPlayer)
                            AIAttack();
                        break;
                    }

                case var case1 when case1 == ResultOfAttack.GameOver:
                    {
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
        public static void HandleUserInput()
        {
            // Read incoming input events
            SwinGame.ProcessEvents();
            var switchExpr = CurrentState;
            switch (switchExpr)
            {
                case var @case when @case == GameState.ViewingMainMenu:
                    {
                        MenuController.HandleMainMenuInput();
                        break;
                    }

                case var case1 when case1 == GameState.ViewingGameMenu:
                    {
                        MenuController.HandleGameMenuInput();
                        break;
                    }

                case var case2 when case2 == GameState.AlteringSettings:
                    {
                        MenuController.HandleSetupMenuInput();
                        break;
                    }

                case var case3 when case3 == GameState.Deploying:
                    {
                        DeploymentController.HandleDeploymentInput();
                        break;
                    }

                case var case4 when case4 == GameState.Discovering:
                    {
                        DiscoveryController.HandleDiscoveryInput();
                        break;
                    }

                case var case5 when case5 == GameState.EndingGame:
                    {
                        EndingGameController.HandleEndOfGameInput();
                        break;
                    }

                case var case6 when case6 == GameState.ViewingHighScores:
                    {
                        HighScoreController.HandleHighScoreInput();
                        break;
                    }
            }

            UtilityFunctions.UpdateAnimations();
        }

        /// <summary>
        /// Draws the current state of the game to the screen.
        /// </summary>
        /// <remarks>
        /// What is drawn depends upon the state of the game.
        /// </remarks>
        public static void DrawScreen()
        {
            UtilityFunctions.DrawBackground();
            var switchExpr = CurrentState;
            switch (switchExpr)
            {
                case var @case when @case == GameState.ViewingMainMenu:
                    {
                        MenuController.DrawMainMenu();
                        break;
                    }

                case var case1 when case1 == GameState.ViewingGameMenu:
                    {
                        MenuController.DrawGameMenu();
                        break;
                    }

                case var case2 when case2 == GameState.AlteringSettings:
                    {
                        MenuController.DrawSettings();
                        break;
                    }

                case var case3 when case3 == GameState.Deploying:
                    {
                        DeploymentController.DrawDeployment();
                        break;
                    }

                case var case4 when case4 == GameState.Discovering:
                    {
                        DiscoveryController.DrawDiscovery();
                        break;
                    }

                case var case5 when case5 == GameState.EndingGame:
                    {
                        EndingGameController.DrawEndOfGame();
                        break;
                    }

                case var case6 when case6 == GameState.ViewingHighScores:
                    {
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
        public static void AddNewState(GameState state)
        {
            _state.Push(state);
            UtilityFunctions.Message = "";
        }

        /// <summary>
        /// End the current state and add in the new state.
        /// </summary>
        /// <param name="newState">the new state of the game</param>
        public static void SwitchState(GameState newState)
        {
            EndCurrentState();
            AddNewState(newState);
        }

        /// <summary>
        /// Ends the current state, returning to the prior state
        /// </summary>
        public static void EndCurrentState()
        {
            _state.Pop();
        }

        /// <summary>
        /// Sets the difficulty for the next level of the game.
        /// </summary>
        /// <param name="setting">the new difficulty level</param>
        public static void SetDifficulty(AIOption setting)
        {
            _aiSetting = setting;
        }
    }
}
