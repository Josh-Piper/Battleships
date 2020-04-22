
using SwinGameSDK;


namespace Battleships {

	public class GameMain {

		public static void Main() {

			// Opens a new Graphics Window
			SwinGame.OpenGraphicsWindow("Battle Ships", 800, 600);

			// Load Resources
			GameResources.LoadResources();
			SwinGame.PlayMusic(GameResources.GameMusic("Background"));

			// Game Loop
			while (!(SwinGame.WindowCloseRequested() == true | GameController.CurrentState == GameState.Quitting)) {
				GameController.HandleUserInput();
				GameController.DrawScreen();
			}

			// Free Resources and Close Audio, to end the program.
			GameResources.FreeResources();
			SwinGame.StopMusic();

		}

	}

}
