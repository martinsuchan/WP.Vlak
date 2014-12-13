namespace Vlak
{
    public class LevelInfo
    {
        public int LevelNumber { get; set; }
        public int MinSteps { get; set; }
        public string MinTrail { get; set; }
        public int Starts { get; set; }

        public LevelInfo()
        {
            MinSteps = int.MaxValue;
            Starts = 0;
            MinTrail = null;
        }
    }
}