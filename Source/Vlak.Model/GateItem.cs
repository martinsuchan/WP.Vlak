namespace Vlak.Model
{
    public class GateItem : Item
    {
        public int State { get; set; }

        public const int Open = 1;

        public const int FullOpen = 5;
        public const int Hidden = 6;

        public override bool CanGo
        {
            get { return State >= Open; }
        }
    }
}
