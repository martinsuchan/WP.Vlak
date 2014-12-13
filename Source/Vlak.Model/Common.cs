using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace Vlak.Model
{
    public static class Common
    {
        #region Gameplay values

        /// <summary>
        /// Number of level used as welcome screen.
        /// </summary>
        public const int StartScreen = 0;

        /// <summary>
        /// First game level.
        /// </summary>
        public const int FirstLevel = 1;

        /// <summary>
        /// Last game level.
        /// </summary>
        public const int LastLevel = 50;

        /// <summary>
        /// Name of the asset with all graphics for this game.
        /// </summary>
        public static string ImageAsset = "images/vlak";

        /// <summary>
        /// Time in milliseconds between each game update.
        /// </summary>
        public const int GameUpdateDelta = 133;

        /// <summary>
        /// Time in updates between game turns.
        /// </summary>
        public static int UpdatesPerTurn = 3;

        #endregion

        #region Map and block sizes

        #region Dynamic values computed at the start of the game

        /// <summary>
        /// Width of the game screen.
        /// </summary>
        public static int W;

        /// <summary>
        /// Height of the game screen.
        /// </summary>
        public static int H;

        /// <summary>
        /// Top margin for centering the game screen.
        /// </summary>
        public static int TopMargin;

        /// <summary>
        /// Left margin for centering the game screen.
        /// </summary>
        public static int LeftMargin
        {
            get
            {
                // left margin should be adapted to orientation, place the game screen away from HW buttons
                switch (Window.CurrentOrientation)
                {
                    case DisplayOrientation.LandscapeLeft:
                    case DisplayOrientation.Default:
                    case DisplayOrientation.Portrait:
                        return LeftMarginThin;
                    case DisplayOrientation.LandscapeRight:
                        return LeftMarginThick;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

       /* public static int XCenter
        {
            get
            {
                if (!xCenter.HasValue)
                {
                    xCenter = LeftMargin + (MapWidth/2)*TileWidth;
                }
                return xCenter.Value;
            }
        }
        private static int? xCenter;*/

        //public static DisplayOrientation Orientation;

        public static GameWindow Window;
        public static int LeftMarginThick;
        public static int LeftMarginThin;

        /// <summary>
        /// Application version
        /// </summary>
        public static readonly string Version;

        static Common()
        {
            string assembly = Assembly.GetCallingAssembly().FullName;
            Version = assembly.Split('=')[1].Split(',')[0];
        }

        public const string StartScreenTrail =
            "rrrrrrrrrrrrrrrrrrruuuuuuuulddddddluuuuuulddddddluuuuuulddddddluuuuuulddddddluuuuuulddddddluuuuuulddddddluuuuuulddddddluuuuuulddddddluuuuuulddddddluuuuuuldddddddddrrrrrrrrrd";
        public const string HelpScreenTrail =
            "rrrrrrdddllllllllllllllllldrrrrrrrrrrrd";

        /// <summary>
        /// Flag indicating if game is running in trial mode or not.
        /// </summary>
        public static bool IsTrialMode
        {
            get
            {
                if (!isTrialMode.HasValue) isTrialMode = Guide.IsTrialMode;
                return isTrialMode.Value;
            }
        }
        private static bool? isTrialMode;

        #endregion

        /// <summary>
        /// Width of each level in tiles.
        /// </summary>
        public const int MapWidth = 20;

        /// <summary>
        /// Height of each level in tiles.
        /// </summary>
        public const int MapHeight = 12;

        /// <summary>
        /// Width of each tile.
        /// </summary>
        public const int TileWidth = 32;

        /// <summary>
        /// Height of each tile.
        /// </summary>
        public const int TileHeight = 32;

        /// <summary>
        /// Width of each tile in original image asset.
        /// </summary>
        public const int HalfTileWidth = 32;

        /// <summary>
        /// Height of each tile in original image asset.
        /// </summary>
        public const int HalfTileHeight = 32;

        #endregion
    }
}
