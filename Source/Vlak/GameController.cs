using System;
using System.Linq;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Vlak.Model;
using Vlak.Model.Enums;
using Vlak.Resources;

namespace Vlak
{
    public class GameController
    {
        private readonly LevelManager levelManager;
        private readonly IGameView game;
        private readonly ContentManager contentManager;
        private readonly SoundEffectManager soundEffectManager;

        public static GameController Instance
        {
            get { return instance; }
        }
        private static GameController instance;

        public Level Level
        {
            get { return levelManager.Level; }
        }
        public int CurrentLevel
        {
            get { return Settings.CurrentLevel.Value; }
            private set { Settings.CurrentLevel.Value = value; }
        }
        public int Steps { get { return levelManager.Steps; } }
        public int RemainingCargo { get { return levelManager.RemainingCargo; } }
        public LevelInfo Info { get; private set; }

        public GameState State
        {
            get { return Settings.GameState.Value; }
            private set { Settings.GameState.Value = value; }
        }

        public GameController(IGameView game, ContentManager contentManager)
        {
            instance = this;
            this.game = game;
            this.contentManager = contentManager;

            // load the core Level manager
            levelManager = new LevelManager();
            soundEffectManager = new SoundEffectManager(contentManager);

            // load the initial game screen
            StartGame();
        }

        #region ----- Game wide events -----------------------------------

        public void StartGame()
        {
            Settings.StartCount.Value++;
            if (Common.IsTrialMode && Settings.StartCount.Value % 3 == 0)
            {
                Guide.BeginShowMessageBox(AppResources.TrialNotice, AppResources.BuyThisGame, new[] { AppResources.Buy, AppResources.BuyLater }, 0, MessageBoxIcon.None, GoToMarketplace, null);
            }

            switch (State)
            {
                case GameState.StartScreen:
                    LoadStartScreen();
                    break;
                case GameState.LevelChooser:
                    LoadLevelChooser();
                    break;
                case GameState.Help:
                    LoadHelp();
                    break;
                case GameState.Game:
                    LoadLevel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void LoadStartScreen()
        {
            CurrentLevel = Common.StartScreen;
            State = GameState.StartScreen;
            LoadLevel();
            Level.AutoTrail = Common.StartScreenTrail;
        }

        public void LoadHelp()
        {
            CleanUp();

            // Load the level using mapManager
            Level level = contentManager.Load<Level>(@"levels\level100").Clone();
            level.AutoTrail = Common.HelpScreenTrail;

            // load the level into level manager
            levelManager.LoadLevel(level);

            // create level game component
            game.ShowLevel(level);

            State = GameState.Help;
        }

        public void LoadLevelChooser()
        {
            CleanUp();
            State = GameState.LevelChooser;
        }

        public void LoadLevel(int i)
        {
            if (i < Common.FirstLevel || i > Common.LastLevel) return;

            CurrentLevel = i;
            State = GameState.Game;
            LoadLevel();
        }

        public void LoadLevel()
        {
            CleanUp();

            // Load the level using mapManager
            Level level = contentManager.Load<Level>(string.Format(@"levels\level{0}", CurrentLevel)).Clone();
            Info = FindLevelInfo();
            Info.Starts++;

            // load the level into level manager
            levelManager.LoadLevel(level);

            // create level game component
            game.ShowLevel(level);
        }

        public LevelInfo FindLevelInfo()
        {
            LevelInfo li = Settings.Levels.Value.FirstOrDefault(l => l.LevelNumber == CurrentLevel);
            if (li == null)
            {
                Settings.Levels.Value.Add(li = new LevelInfo { LevelNumber = CurrentLevel });
            }
            return li;
        }

        public void FinishedLevel()
        {
            if (Level == null) return;

            if (Steps < Info.MinSteps)
            {
                Info.MinSteps = Steps;
                Info.MinTrail = Level.Trail;
            }

            if (CurrentLevel == Common.LastLevel)
            {
                Guide.BeginShowMessageBox(AppResources.Gratulation, AppResources.FinishedGame, new[] { AppResources.YesIAm}, 0, MessageBoxIcon.None, null, null);
                return;
            }

            CleanUp();

            CurrentLevel++;
            Settings.TopLevel.Value = Math.Max(Settings.TopLevel.Value, CurrentLevel);

            LoadLevel();
        }

        private void CleanUp()
        {
            if (Level != null)
            {
                // destroy and remove level from drawing context
                game.HideLevel();

                // unload the level from level manager
                levelManager.UnloadLevel();
            }
        }

        public void ExitGame()
        {
            game.Exit();
        }

        #endregion

        #region ----- Level wide events ----------------------------------

        public void Move(Direction dir)
        {
            if (Level == null) return;

            levelManager.Move(dir);
        }

        public void Turn()
        {
            if (Level == null) return;

            levelManager.Turn();
        }

        #endregion

        public void PlaySound(GameSound sound)
        {
            soundEffectManager.PlaySound(sound);
        }

        public void SwitchSound()
        {
            Settings.SoundEnabled.Value = !Settings.SoundEnabled.Value;
        }

        public void GoToMarketplace(IAsyncResult result)
        {
            int? index = Guide.EndShowMessageBox(result);  

            // go to "buy this game" marketplace page
            if (index.HasValue && index.Value == 0)
            {
                MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask { ContentType = MarketplaceContentType.Applications };
                marketplaceDetailTask.Show();
            }
        }
    }
}