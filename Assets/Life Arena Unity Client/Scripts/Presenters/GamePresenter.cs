using Avangardum.LifeArena.UnityClient.Interfaces;

namespace Avangardum.LifeArena.UnityClient.Presenters
{
    public class GamePresenter
    {
        private IServerFacade _serverFacade;

        public GamePresenter(IServerFacade serverFacade)
        {
            _serverFacade = serverFacade;
        }
    }
}