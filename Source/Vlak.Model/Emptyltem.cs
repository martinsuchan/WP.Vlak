namespace Vlak.Model
{
    public class EmptyItem : Item
    {
        public override bool CanGo
        {
            get { return true; }
        }
    }
}
