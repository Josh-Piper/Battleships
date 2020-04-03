using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwinGameSDK;
using static SwinGameSDK.SwinGame;

namespace MyGame
{
    public class EndingGameController
    {

        /// <summary>
        /// Draw the end of the game screen, shows the win/lose state
        /// </summary>
        public static void DrawEndOfGame()
        {
            var toDraw = default(Rectangle);
            string whatShouldIPrint;
            DrawField[ComputerPlayer.PlayerGrid, ComputerPlayer, true];
            DrawSmallField[HumanPlayer.PlayerGrid, HumanPlayer];
            toDraw.X = 0;
            toDraw.Y = 250;
            toDraw.Width = SwinGame.ScreenWidth();
            toDraw.Height = SwinGame.ScreenHeight();
            if (HumanPlayer.IsDestroyed)
            {
                whatShouldIPrint = "YOU LOSE!";
            }
            else
            {
                whatShouldIPrint = "-- WINNER --";
            }

            SwinGame.DrawTextLines(whatShouldIPrint, Color.White, Color.Transparent, GameResources.GameFont("ArialLarge"), FontAlignment.AlignCenter, toDraw);
        }

        /// <summary>
        /// Handle the input during the end of the game. Any interaction
        /// will result in it reading in the highsSwinGame.
        /// </summary>
        public static void HandleEndOfGameInput()
        {
            if (SwinGame.MouseClicked(MouseButton.LeftButton) || SwinGame.KeyTyped(KeyCode.VK_RETURN) || SwinGame.KeyTyped(KeyCode.VK_ESCAPE))

            {
                ReadHighScore[HumanPlayer.Score];
                EndCurrentState();
            }
        }
    }
}
