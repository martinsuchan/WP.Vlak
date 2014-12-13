using System.Collections.Generic;
using System.Text;
using Vlak.Model;
using Vlak.Model.Enums;

namespace Vlak
{
    public class Settings
    {
        // Persistent user settings from the settings page
        public static readonly Setting<GameState> GameState = new Setting<GameState>("GameState", Model.Enums.GameState.StartScreen);
        public static readonly Setting<int> CurrentLevel = new Setting<int>("CurrentLevel", Common.FirstLevel);
        public static readonly Setting<int> TopLevel = new Setting<int>("ReachedLevel", Common.FirstLevel);
        public static readonly Setting<bool> SoundEnabled = new Setting<bool>("SoundEnabled", true);
        public static readonly Setting<int> StartCount = new Setting<int>("StartCount", 0);

        public static readonly Setting<List<LevelInfo>> Levels = new Setting<List<LevelInfo>>("Levels", new List<LevelInfo>());

        #region Overrides of ISettings

        public void Cleanup()
        {
            GameState.Value = GameState.DefaultValue;
            CurrentLevel.Value = Common.FirstLevel;
            if (TopLevel.Value > Common.LastLevel) TopLevel.Value = Common.LastLevel;
            if (TopLevel.Value < Common.FirstLevel) TopLevel.Value = Common.FirstLevel;
        }

        #endregion
    }
}