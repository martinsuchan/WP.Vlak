using Vlak.Model.Enums;

namespace Vlak.Model
{
    public class CargoItem : Item
    {
        public int State { get; set; }

        public bool InTrain { get; set; }

        public const int InTrainState = 3;

        public override bool CanGo
        {
            get { return !InTrain; }
        }

        /// <summary>
        /// 
        /// </summary>
        public CargoType CargoType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Direction Direction { get; set; }
    }
}
