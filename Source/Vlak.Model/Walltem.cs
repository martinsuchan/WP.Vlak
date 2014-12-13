namespace Vlak.Model
{
    public class WallItem : Item
    {
        public override bool CanGo
        {
            get { return false; }
        }
    }
}
