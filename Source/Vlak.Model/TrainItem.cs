using System.Collections.Generic;
using Vlak.Model.Enums;

namespace Vlak.Model
{
    public class TrainItem : CargoItem
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CargoItem> Cargo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TrainState TrainState { get; set; }

        public override bool CanGo
        {
            get { return true; }
        }

        public TrainItem()
        {
            TrainState = TrainState.Running;
            Cargo = new List<CargoItem>();
        }
    }
}
