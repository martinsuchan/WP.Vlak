using System.Linq;
using Microsoft.Xna.Framework.Content;
using Vlak.Model.Enums;

namespace Vlak.Model
{
    /// <summary>
    /// Class representing single game level.
    /// </summary>
    public class Level
    {
        /// <summary>
        /// Unique ID of this level.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Password for this level.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Current level state
        /// </summary>
        [ContentSerializerIgnore]
        public LevelState State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ContentSerializerIgnore]
        public TrainItem Train { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ContentSerializerIgnore]
        public GateItem Gate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ContentSerializerIgnore]
        public Item[,] Map { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ContentSerializerIgnore]
        public string Trail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ContentSerializerIgnore]
        public string AutoTrail { get; set; }

        public int[] LevelMap { set; get; }

        #region map loading stuff

        public void LoadMap()
        {
            int i = 0;
            Map = new Item[Common.MapWidth, Common.MapHeight];
            for (int y = 0; y < Common.MapHeight; y++)
            {
                for (int x = 0; x < Common.MapWidth; x++)
                {
                    Map[x, y] = CreateMapItem(LevelMap[i++], x, y);
                }
            }
            Train = Map.OfType<TrainItem>().First();
            Gate = Map.OfType<GateItem>().First();
            Map[Train.X, Train.Y] = null;
        }

        private static Item CreateMapItem(int i, int x, int y)
        {
            switch (i)
            {
                case NUL:
                    return null;

                case KRY:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Crystal };
                case KOR:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Crown };
                case STO:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Tree };
                case JAB:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Apple };
                case KRA:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Cow };
                case TRE:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Cherry };
                case RYB:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Pond };
                case ZIR:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Giraffe };
                case ZMR:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.IceCream };
                case DOR:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Snail };
                case POC:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Computer };
                case AUT:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Car };
                case BAL:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Balloon };
                case BUD:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Clock };
                case SLO:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Elephant };
                case VIN:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Glass };
                case PEN:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Money };
                case LET:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Airplane };
                case LEM:
                    return new CargoItem { X = x, Y = y, CargoType = CargoType.Lemming };

                case VRA:
                    return new GateItem { X = x, Y = y };
                case ZED:
                    return new WallItem { X = x, Y = y };

                case LO1:
                    return new TrainItem { X = x, Y = y, Direction = Direction.Left };
                case LO2:
                    return new TrainItem { X = x, Y = y, Direction = Direction.Up };
                case LO3:
                    return new TrainItem { X = x, Y = y, Direction = Direction.Right };
                case LO4:
                    return new TrainItem { X = x, Y = y, Direction = Direction.Down };
                default:
                    // should not happen
                    return null;
            }
        }

        public const int NUL = 0;

        public const int KRY = 1;
        public const int KOR = 2;
        public const int STO = 3;
        public const int JAB = 4;
        public const int KRA = 5;
        public const int TRE = 6;
        public const int RYB = 7;
        public const int ZIR = 8;
        public const int ZMR = 9;
        public const int DOR = 10;
        public const int POC = 11;
        public const int AUT = 12;
        public const int BAL = 13;
        public const int BUD = 14;
        public const int SLO = 15;
        public const int VIN = 16;
        public const int PEN = 17;
        public const int LET = 18;
        public const int LEM = 19;

        public const int LO1 = 21;
        public const int LO2 = 22;
        public const int LO3 = 23;
        public const int LO4 = 24;

        public const int ZED = 99;
        public const int VRA = 50;

        #endregion

        /// <summary>
        /// Read a Level object from the content pipeline.
        /// </summary>
        public class LevelReader : ContentTypeReader<Level>
        {
            protected override Level Read(ContentReader input, Level existingInstance)
            {
                Level level = new Level
                {
                    Number = input.ReadInt32(),
                    Password = input.ReadString(),
                    LevelMap = input.ReadObject<int[]>()
                };
                return level;
            }
        }

        public Level Clone()
        {
            Level level = new Level
            {
                Number = Number,
                Password = Password,
                LevelMap = LevelMap.Clone() as int[],
                Trail = string.Empty,
            };
            level.LoadMap();
            return level;
        }
    }
}