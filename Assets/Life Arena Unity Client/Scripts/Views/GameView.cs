using Avangardum.LifeArena.UnityClient.Data;
using Avangardum.LifeArena.UnityClient.Interfaces;
using UnityEngine;
using Zenject;

namespace Avangardum.LifeArena.UnityClient.Views
{
    public class GameView : IGameView
    {
        private IFieldView _fieldView;
        
        [Inject]
        public void Inject(IFieldView fieldView)
        {
            _fieldView = fieldView;
        }

        public GameState GameState
        {
            set
            {
                _fieldView.LivingCells = value.LivingCells;
            }
        }

        public void ShowNoInternetConnectionMessage()
        {
            Debug.LogError("No internet connection");
        }

        public void ShowServerUnavailableMessage()
        {
            Debug.LogError("Server unavailable");
        }

        public void ShowUnknownErrorMessage(string message)
        {
            Debug.LogError(message);
        }
    }
}