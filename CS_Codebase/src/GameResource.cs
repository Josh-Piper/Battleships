
using System.Collections.Generic;
using SwinGameSDK;


namespace Battleships {

    public static class GameResources {

        private static readonly Dictionary<string, Bitmap> images = new Dictionary<string, Bitmap>();
        private static readonly Dictionary<string, Font> fonts = new Dictionary<string, Font>();
        private static readonly Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
        private static readonly Dictionary<string, Music> music = new Dictionary<string, Music>();
        private static Bitmap background;
        private static Bitmap animation;
        private static Bitmap loaderFull;
        private static Bitmap loaderEmpty;
        private static Font fontLoading;
        private static SoundEffect soundStart;


        /// <summary>
        /// Gets a `Font` whose `key` matches the given argument
        /// </summary>
        /// <param name="font">Name of font</param>
        /// <returns>The specified Font</returns>
        public static Font GetFont (string font) {

            return fonts[font];

        }

        /// <summary>
        /// Gets a `Bitmap` whose `key` matches the given argument
        /// </summary>
        /// <param name="image">Name of image</param>
        /// <returns>The specified image</returns>
        public static Bitmap GetImage (string image) {

            return images[image];

        }

        /// <summary>
        /// Gets a `SoundEffect` whose `key` mathces the given argument
        /// </summary>
        /// <param name="sound">Name of sound</param>
        /// <returns>The sound with the specified name</returns>
        public static SoundEffect GetSound (string sound) {

            return sounds[sound];

        }

        /// <summary>
        /// Gets `Music` whose `key` matches the given argument
        /// </summary>
        /// <param name="music">Name of music</param>
        /// <returns>The music with the specified name</returns
        public static Music GameMusic (string _music) {

            return music[_music];

        }

        /// <summary>
        /// Loads all the game resourses
        /// </summary>
        public static void LoadResources () {

            ShowLoadingScreen();

            ShowMessage("Loading fonts...", 0);
            LoadFonts();
            SwinGame.Delay(300);
            ShowMessage("Loading images...", 1);
            LoadImages();
            SwinGame.Delay(300);
            ShowMessage("Loading sounds effects...", 2);
            LoadSounds();
            SwinGame.Delay(300);
            ShowMessage("Loading music...", 3);
            LoadMusic();
            SwinGame.Delay(500);

            EndLoadingScreen();

        }

        /// <summary>
        /// Loads all the game fonts from 'Resourses/fonts/'
        /// </summary>
        private static void LoadFonts () {

            NewFont("ArialLarge", "arial.ttf", 80);
            NewFont("Courier", "cour.ttf", 14);
            NewFont("CourierSmall", "cour.ttf", 8);
            NewFont("Menu", "ffaccess.ttf", 8);

        }

        /// <summary>
        /// Loads all the game images from 'Resourses/images/'
        /// </summary>
        private static void LoadImages () {

            // Backgrounds
            NewImage("Menu", "main_page.jpg");
            NewImage("Discovery", "discover.jpg");
            NewImage("Deploy", "deploy.jpg");

            // Deployment
            NewImage("LeftRightButton", "deploy_dir_button_horiz.png");
            NewImage("UpDownButton", "deploy_dir_button_vert.png");
            NewImage("SelectedShip", "deploy_button_hl.png");
            NewImage("PlayButton", "deploy_play_button.png");
            NewImage("RandomButton", "deploy_randomize_button.png");

            // Ships
            for (int i = 0; i < 5; i++) {
                NewImage("ShipLR" + (i + 1), "ship_deploy_horiz_" + (i + 1) + ".png");
                NewImage("ShipUD" + (i + 1), "ship_deploy_vert_" + (i + 1) + ".png");
            }

            // Explosions
            NewImage("Explosion", "explosion.png");
            NewImage("Splash", "splash.png");

        }

        /// <summary>
        /// Loads all the game sounds from 'Resourses/sounds/'
        /// </summary>
        private static void LoadSounds () {

            NewSound("Error", "error.wav");
            NewSound("Hit", "hit.wav");
            NewSound("Sink", "sink.wav");
            NewSound("Siren", "siren.wav");
            NewSound("Miss", "watershot.wav");
            NewSound("Winner", "winner.wav");
            NewSound("Lose", "lose.wav");

        }

        /// <summary>
        /// Loads all the game music from 'Resourses/sounds/'
        /// </summary>
        private static void LoadMusic () {

            NewMusic("Background", "horrordrone.mp3");

        }

        

        private static void ShowLoadingScreen () {

            background = SwinGame.LoadBitmap(SwinGame.PathToResource("SplashBack.png", ResourceKind.BitmapResource));
            animation = SwinGame.LoadBitmap(SwinGame.PathToResource("SwinGameAni.jpg", ResourceKind.BitmapResource));
            fontLoading = SwinGame.LoadFont(SwinGame.PathToResource("arial.ttf", ResourceKind.FontResource), 12);
            soundStart = Audio.LoadSoundEffect(SwinGame.PathToResource("SwinGameStart.wav", ResourceKind.SoundResource));
            loaderFull = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_full.png", ResourceKind.BitmapResource));
            loaderEmpty = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_empty.png", ResourceKind.BitmapResource));

            SwinGame.DrawBitmap(background, 0, 0);
            SwinGame.RefreshScreen();
            SwinGame.ProcessEvents();

            PlaySwinGameIntro();

        }

        private static void PlaySwinGameIntro () {

            Audio.PlaySoundEffect(soundStart);
            SwinGame.ProcessEvents();
            SwinGame.Delay(600);

        }

        private static void ShowMessage (string message, int number) {

            const int TX = 310;
            const int TY = 493;
            const int TW = 200;
            const int TH = 25;
            const int STEPS = 3;
            const int BG_X = 279;
            const int BG_Y = 453;

            //int fullW = 260 * number / STEPS;
            float fullW = (210f * number / STEPS) + 2.5f;

            SwinGame.DrawBitmap(loaderEmpty, BG_X, BG_Y);

            // Draw progress bar
            Rectangle loadRect = new Rectangle() { X = BG_X + 23.5f, Y = BG_Y + 19.5f, Width = fullW, Height = 14.5f };
            SwinGame.FillRectangle(Color.DodgerBlue, loadRect);


            Rectangle toDraw = default;
            toDraw.X = TX;
            toDraw.Y = TY;
            toDraw.Width = TW;
            toDraw.Height = TH;
            SwinGame.DrawText(message, Color.White, Color.Transparent, fontLoading, FontAlignment.AlignCenter, toDraw);
            /////////////////////////////////////////////////////// Todo /////////////////////////////////////////////////////////
            //SwinGame.DrawTextLines(message, Color.White, Color.Transparent, _LoadingFont, FontAlignment.AlignCenter, TX, TY, TW, TH);

            SwinGame.RefreshScreen();
            SwinGame.ProcessEvents();

        }

        private static void EndLoadingScreen () {

            SwinGame.ProcessEvents();
            SwinGame.ClearScreen();
            SwinGame.RefreshScreen();
            SwinGame.FreeFont(fontLoading);
            SwinGame.FreeBitmap(background);
            SwinGame.FreeBitmap(animation);
            SwinGame.FreeBitmap(loaderEmpty);
            SwinGame.FreeBitmap(loaderFull);

            /////////////////////////////////////////////////////// Todo /////////////////////////////////////////////////////////
            //Audio.FreeSoundEffect(soundStart);

        }

        /// <summary>
        /// Adds a new font to `GameResources.fonts`
        /// </summary>
        /// <param name="fontName"></param>
        /// <param name="filename"></param>
        /// <param name="size"></param>
        private static void NewFont (string fontName, string filename, int size) {

            fonts.Add(fontName, SwinGame.LoadFont(SwinGame.PathToResource(filename, ResourceKind.FontResource), size));

        }

        /// <summary>
        /// Adds a new image to `GameResources.images`
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="filename"></param>
        private static void NewImage (string imageName, string filename) {

            images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(filename, ResourceKind.BitmapResource)));

        }

        private static void NewTransparentColorImage (string imageName, string fileName, Color transColor) {

            images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(fileName, ResourceKind.BitmapResource)));

        }

        /// <summary>
        /// Adds a sounds effect to `GameResources.sounds`
        /// </summary>
        /// <param name="soundName"></param>
        /// <param name="filename"></param>
        private static void NewSound (string soundName, string filename) {

            sounds.Add(soundName, Audio.LoadSoundEffect(SwinGame.PathToResource(filename, ResourceKind.SoundResource)));

        }

        /// <summary>
        /// Adds music to `GameResources.music`
        /// </summary>
        /// <param name="musicName">Given name of music</param>
        /// <param name="filename">Name of file in 'Resources/Sounds/'</param>
        private static void NewMusic (string musicName, string filename) {

            music.Add(musicName, Audio.LoadMusic(SwinGame.PathToResource(filename, ResourceKind.SoundResource)));

        }

        /// <summary>
        /// Free all the game resourses
        /// </summary>
        public static void FreeResources () {

            FreeFonts();
            FreeImages();
            FreeMusic();
            /////////////////////////////////////////////////////// Todo /////////////////////////////////////////////////////////
            /// - This crashes game
            //FreeSounds();
            SwinGame.ProcessEvents();

        }

        /// <summary>
        /// Free all game fonts
        /// </summary>
        private static void FreeFonts () {

            foreach (Font obj in fonts.Values)
                SwinGame.FreeFont(obj);

        }

        /// <summary>
        /// Free all game images
        /// </summary>
        private static void FreeImages () {

            foreach (Bitmap obj in images.Values)
                SwinGame.FreeBitmap(obj);
        }

        /// <summary>
        /// Free all game sound effects
        /// </summary>
        private static void FreeSounds () {

            /////////////////////////////////////////////////////// Todo /////////////////////////////////////////////////////////
            /// This crashes game
            foreach (SoundEffect obj in sounds.Values)
                Audio.FreeSoundEffect(obj);

        }

        /// <summary>
        /// Free all game music
        /// </summary>
        private static void FreeMusic () {

            foreach (Music obj in music.Values)
                Audio.FreeMusic(obj);

        }

    }

}
