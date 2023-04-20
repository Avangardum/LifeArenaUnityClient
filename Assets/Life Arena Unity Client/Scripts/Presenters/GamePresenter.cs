using System;
using System.Threading.Tasks;
using Avangardum.LifeArena.UnityClient.Exceptions;
using Avangardum.LifeArena.UnityClient.Interfaces;

namespace Avangardum.LifeArena.UnityClient.Presenters
{
    public class GamePresenter
    {
        private IServerFacade _serverFacade;
        private IGameView _gameView;

        public GamePresenter(IServerFacade serverFacade, IGameView gameView)
        {
            _serverFacade = serverFacade;
            _gameView = gameView;
            
            GetGameStateLoop();
        }

        private async void GetGameStateLoop()
        {
            var delay = TimeSpan.FromSeconds(1);
            
            while (true)
            {
                try
                {
                    var gameState = await _serverFacade.GetGameState();
                    _gameView.GameState = gameState;
                }
                catch (NoInternetConnectionException)
                {
                    _gameView.ShowNoInternetConnectionMessage();
                }
                catch (ServerUnavailableException)
                {
                    _gameView.ShowServerUnavailableMessage();
                }
                catch (Exception e)
                {
                    _gameView.ShowUnknownErrorMessage(e.ToString());
                }
                
                await Task.Delay(delay);
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}