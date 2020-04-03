using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwinGameSDK;
using static SwinGameSDK.SwinGame;

namespace MyGame
{
    public class GameMain
    {
        public static void Main()
        {
            // Opens a new Graphics Window
            SwinGame.OpenGraphicsWindow("Battle Ships", 800, 600);

            // Load Resources
            
            GameResources.LoadResources();
            SwinGame.PlayMusic(GameResources.GameMusic["Background"]);

            // Game Loop
            do
            {
                HandleUserInput();
                DrawScreen();
            }
            while (!(SwinGame.WindowCloseRequested() == true | CurrentState == GameState.Quitting));
            SwinGame.StopMusic();

            // Free Resources and Close Audio, to end the program.
            FreeResources();
        }
    }
}
