using System;
using System.Linq;
using Microsoft.Devices;
using Vlak.Model;
using Vlak.Model.Enums;

namespace Vlak
{
    /// <summary>
    /// Class responsible hor handling move events on Level, updating all items position,
    /// saving and loading saved games.
    /// This class should be accessible via some interface / dependency injection.
    /// </summary>
    public class LevelManager
    {
        /// <summary>
        /// Current active level.
        /// </summary>
        public Level Level { get; set; }

        /// <summary>
        /// Shortcut to Train item.
        /// </summary>
        public TrainItem Train { get { return Level.Train; } }

        /// <summary>
        /// Shortcut to Gate item.
        /// </summary>
        public GateItem Gate { get { return Level.Gate; } }

        /// <summary>
        /// Number of remaining cargo items to pick.
        /// </summary>
        public int RemainingCargo { get; set; }

        /// <summary>
        /// Number of steps made in current level.
        /// </summary>
        public int Steps;

        /// <summary>
        /// Next move direction.
        /// </summary>
        public Direction? NextDirection { get; set; }

        /// <summary>
        /// Initialize all values to default state.
        /// </summary>
        /// <param name="level"></param>
        public void LoadLevel(Level level)
        {
            Level = level;
            RemainingCargo = level.Map.OfType<CargoItem>().Count();
            Steps = 0;
            NextDirection = null;
        }

        public void UnloadLevel()
        {
            Level = null;
        }

        public void Move(Direction dir)
        {
            NextDirection = dir;
        }

        /// <summary>
        /// Move train one step forwar, pick-up cargo, change direction, explode,
        /// depending on the next field.
        /// </summary>
        public void Turn()
        {
            // if train is not moving yet
            if (Level.State == LevelState.Ready)
            {
                // not started yet, wait
                if (!NextDirection.HasValue) return;
                Level.State = LevelState.Started;
            }
            if (Level.State == LevelState.Started)
            {
                int x, y;
                switch (NextDirection)
                {
                    case Direction.Up:
                        x = Train.X;
                        y = Train.Y - 1;
                        break;
                    case Direction.Down:
                        x = Train.X;
                        y = Train.Y + 1;
                        break;
                    case Direction.Left:
                        x = Train.X - 1;
                        y = Train.Y;
                        break;
                    case Direction.Right:
                        x = Train.X + 1;
                        y = Train.Y;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // safety precausion if there is missing wall in some level
                if (x < 0 || y < 0 || x >= Common.MapWidth || y >= Common.MapHeight)
                {
                    LevelExplode();
                    return;
                }

                // calculate item on the next field
                Item next = Level.Map[x, y] ?? Level.Train.Cargo.FirstOrDefault(c => c.X == x && c.Y == y);
                if (next == null || next.CanGo)
                {
                    Level.Map[x, y] = null;
                    // if there is open gate - level complete
                    if (next is GateItem)
                    {
                        LevelFinish((GateItem)next);
                    }
                    // if there is cargo, pick it and add append it to the train
                    else if (next is CargoItem)
                    {
                        LevelPickItem((CargoItem)next);
                    }
                    else
                    {
                        GameController.Instance.PlaySound( GameSound.Step);
                    }
                    // move the train in the target direction
                    MoveTrain(x, y, NextDirection.Value);
                }
                else
                {
                    // if there is wall, train wagon or closed gate, explode
                    LevelExplode();
                }
            }
        }

        private void LevelExplode()
        {
            Train.Direction = Direction.Right;
            Train.TrainState = TrainState.StartExplosion;
            Level.State = LevelState.Crashed;
            // vibrate the phone during the exposion
            VibrateController.Default.Start(TimeSpan.FromMilliseconds(Common.GameUpdateDelta * 3));
            GameController.Instance.PlaySound(GameSound.Crash);
        }

        private void LevelFinish(GateItem next)
        {
            next.State = GateItem.Hidden;
            Level.State = LevelState.Finished;
            GameController.Instance.PlaySound(GameSound.Win);
        }

        private void LevelPickItem(CargoItem next)
        {
            RemainingCargo--;
            GameController.Instance.PlaySound( GameSound.Cargo);
            // add it to the train chain of cargo
            Train.Cargo.Add(next);
            next.InTrain = true;
            next.State = CargoItem.InTrainState;
            // if all cargo is picked, open the gate
            if (RemainingCargo == 0)
            {
                Gate.State = GateItem.Open;
            }
        }

        private void MoveTrain(int x, int y, Direction dir)
        {
            Steps++;
            switch (dir)
            {
                case Direction.Left:
                    Level.Trail += "l";
                    break;
                case Direction.Up:
                    Level.Trail += "u";
                    break;
                case Direction.Right:
                    Level.Trail += "r";
                    break;
                case Direction.Down:
                    Level.Trail += "d";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("dir");
            }

            Direction oldDir = Train.Direction;
            int oldX = Train.X;
            int oldY = Train.Y;
            // move the train engine in the requested direction
            Train.Direction = dir;
            Train.X = x;
            Train.Y = y;
            // change position of all wagons
            foreach (CargoItem item in Train.Cargo)
            {
                Direction tempDir = item.Direction;
                int tempX = item.X;
                int tempY = item.Y;
                item.Direction = oldDir;
                item.X = oldX;
                item.Y = oldY;
                oldDir = tempDir;
                oldX = tempX;
                oldY = tempY;
            }
        }
    }
}