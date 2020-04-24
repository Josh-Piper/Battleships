
using System;
using System.Collections.Generic;
using System.IO;
using SwinGameSDK;


namespace Battleships {

	/// <summary>
	/// Controls displaying and collecting high score data.
	/// </summary>
	/// <remarks>
	/// Data is saved to a file.
	/// </remarks>
	public class HighScoreController {

		private const int NAME_WIDTH = 10;
		private const int SCORES_LEFT = 490;

		private static readonly List<Score> _scores = new List<Score>();


		/// <summary>
		/// The score structure is used to keep the name and
		/// score of the top players together.
		/// </summary>
		private partial struct Score : IComparable {

			public string name;
			public int value;

			/// <summary>
			/// Allows scores to be compared to facilitate sorting
			/// </summary>
			/// <param name="obj">the object to compare to</param>
			/// <returns>a value that indicates the sort order</returns>
			public int CompareTo(object obj) {

				if (obj is Score other) {
					return other.value - value;
				}
				else {
					return 0;
				}

			}
		}

		/// <summary>
		/// Loads the scores from the highscores text file.
		/// </summary>
		/// <remarks>
		/// The format is
		/// # of scores
		/// NNNSSS
		/// Where NNN is the name and SSS is the score
		/// </remarks>
		private static void LoadScores() {

			StreamReader input = new StreamReader(SwinGame.PathToResource("highscores.txt"));

			_scores.Clear();

			// Read in the # of scores
			int numScores = Convert.ToInt32(input.ReadLine());
			for (int i = 1; i < numScores; i++) {
				string line = input.ReadLine();
				Score s = new Score {
					name = line.Substring(0, NAME_WIDTH),
					value = Convert.ToInt32(line.Substring(NAME_WIDTH))
				};
			}

			input.Close();

		}

		/// <summary>
		/// Saves the scores back to the highscores text file.
		/// </summary>
		/// <remarks>
		/// The format is
		/// # of scores
		/// NNNSSS
		/// 
		/// Where NNN is the name and SSS is the score
		/// </remarks>
		private static void SaveScores() {

			StreamWriter output = new StreamWriter(SwinGame.PathToResource("highscores.txt"));

			output.WriteLine(_scores.Count);

			foreach (Score s in _scores)
				output.WriteLine(s.name + s.value);

			output.Close();

		}

		/// <summary>
		/// Draws the high scores to the screen.
		/// </summary>
		public static void DrawHighScores() {

			const int SCORES_HEADING = 40;
			const int SCORES_TOP = 80;
			const int SCORE_GAP = 30;

			if (_scores.Count == 0)
				LoadScores();

			SwinGame.DrawText("   High Scores   ", Color.White, GameResources.GetFont("Courier"), SCORES_LEFT, SCORES_HEADING);

			// For all of the scores
			for (int i = 0; i < _scores.Count; i++) {
				Score s;
				s = _scores[i];
				// For scores 1 - 9 use 01 - 09
				if (i < 9) {
					SwinGame.DrawText(" " + (i + 1) + ":   " + s.name + "   " + s.value, Color.White, GameResources.GetFont("Courier"), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
				}
				else {
					SwinGame.DrawText(i + 1 + ":   " + s.name + "   " + s.value, Color.White, GameResources.GetFont("Courier"), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
				}
			}

		}

		/// <summary>
		/// Handles the user input during the top score screen.
		/// </summary>
		/// <remarks></remarks>
		public static void HandleHighScoreInput() {

			if (SwinGame.MouseClicked(MouseButton.LeftButton) || SwinGame.KeyTyped(KeyCode.EscapeKey) || SwinGame.KeyTyped(KeyCode.ReturnKey)) {
				GameController.EndCurrentState();
			}

		}

		/// <summary>
		/// Read the user's name for their highsSwinGame.
		/// </summary>
		/// <param name="value">the player's sSwinGame.</param>
		/// <remarks>
		/// This verifies if the score is a highsSwinGame.
		/// </remarks>
		public static void ReadHighScore(int value) {

			const int ENTRY_TOP = 500;

			if (_scores.Count == 0)
				LoadScores();

			// Return if this score is not a highscore
			if (_scores.Count > 0  &&  value <= _scores[_scores.Count - 1].value)
				return;

			Score s = new Score {
				value = value
			};

			int x = SCORES_LEFT + SwinGame.TextWidth(GameResources.GetFont("Courier"), "Name: ");
			GameController.AddNewState(GameState.ViewingHighScores);
			SwinGame.StartReadingText(Color.White, NAME_WIDTH, GameResources.GetFont("Courier"), x, ENTRY_TOP);

			// Reads the text from the user
			while (SwinGame.ReadingText()) {
				SwinGame.ProcessEvents();
				UtilityFunctions.DrawBackground();
				DrawHighScores();
				SwinGame.DrawText("Name: ", Color.White, GameResources.GetFont("Courier"), SCORES_LEFT, ENTRY_TOP);
				SwinGame.RefreshScreen();
			}

			s.name = SwinGame.TextReadAsASCII();

			if (s.name.Length < 3) {
				s.name += new string(Convert.ToChar(' '), 3 - s.name.Length);
			}

			//scores.RemoveAt(scores.Count - 1);
			_scores.Add(s);
			_scores.Sort();
			GameController.EndCurrentState();

		}

	}

}
