using Avangardum.LifeArena.UnityClient.Data;

namespace Avangardum.LifeArena.UnityClient.Interfaces
{
    public interface IGameView
    {
        GameState GameState { set; }
        
        void ShowNoInternetConnectionMessage();
        
        void ShowServerUnavailableMessage();
        
        void ShowUnknownErrorMessage(string message);
    }
}