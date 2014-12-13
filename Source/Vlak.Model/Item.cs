namespace Vlak.Model
{
    /// <summary>
    /// Base object for all game items.
    /// </summary>
    public abstract class Item
    {
        /// <summary>
        /// Poisiton on the game plan.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Poisiton on the game plan.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Can the train go to this field? If no => explosion.
        /// </summary>
        public abstract bool CanGo { get; }
    }
}
