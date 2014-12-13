using Vlak.Model;

namespace Vlak
{
    public interface IGameView
    {
        void ShowLevel(Level level);
        void HideLevel();
        void Exit();
    }
}