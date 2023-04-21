using System;
using System.Threading.Tasks;
using Avangardum.LifeArena.UnityClient.Data;
using Avangardum.LifeArena.UnityClient.Exceptions;
using Avangardum.LifeArena.UnityClient.Interfaces;

namespace Avangardum.LifeArena.UnityClient.Presenters
{
    public class GamePresenter
    {
        private IServerFacade _serverFacade;
        private IGameView _gameView;
        private DateTime _lastGameStateUpdateTime;

        public GamePresenter(IServerFacade serverFacade, IGameView gameView)
        {
            _serverFacade = serverFacade;
            _gameView = gameView;
            
            _gameView.CellClicked += OnCellClicked;
            
            GetGameStateLoop();
        }

        private async void GetGameStateLoop()
        {
            var getGameStateDelay = TimeSpan.FromSeconds(1);
            var timerCheckDelay = TimeSpan.FromSeconds(0.1);
            
            while (true)
            {
                await GetGameState();

                while (DateTime.Now - _lastGameStateUpdateTime < getGameStateDelay)
                {
                    await Task.Delay(timerCheckDelay);
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private async Task GetGameState()
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

            _lastGameStateUpdateTime = DateTime.Now;
        }

        private async void OnCellClicked(object sender, CellClickedEventArgs e)
        {
            await AddCell(e.X, e.Y);
        }

        private async Task AddCell(int x, int y)
        {
            try
            {
                var gameState = await _serverFacade.AddCell(x, y);
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
            catch (Exception exception)
            {
                _gameView.ShowUnknownErrorMessage(exception.ToString());
            }
            
            _lastGameStateUpdateTime = DateTime.Now;
        }
    }
}