
using SwinGameSDK;


namespace Battleships {

    public class EndingGameController {

        /// <summary>
        /// Draw the end of the game screen, shows the win/lose state
        /// </summary>
        public static void DrawEndOfGame () {

            Rectangle toDraw = new Rectangle {
                X = 0,
                Y = 250,
                Width = SwinGame.ScreenWidth(),
                Height = SwinGame.ScreenHeight()
            };

            UtilityFunctions.DrawField(GameController.ComputerPlayer.PlayerGrid, GameController.ComputerPlayer, true);
            UtilityFunctions.DrawSmallField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);

            string message = (GameController.HumanPlayer.IsDestroyed) ? "You Lose!" : "-- You Win --";
            SwinGame.DrawText(message, Color.White, Color.Transparent, GameResources.GetFont("ArialLarge"), FontAlignment.AlignCenter, toDraw);
          
        }

        /// <summary>
        /// Handle the input during the end of the game. Any interaction
        /// will result in it reading in the highsSwinGame.
        /// </summary>
        public static void HandleEndOfGameInput () {

            if (SwinGame.MouseClicked(MouseButton.LeftButton) || SwinGame.KeyTyped(KeyCode.ReturnKey) || SwinGame.KeyTyped(KeyCode.EscapeKey)) {
                HighScoreController.ReadHighScore(GameController.HumanPlayer.Score);
                GameController.EndCurrentState();
            }

        }
    }
}
