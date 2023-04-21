using System;
using Avangardum.LifeArena.UnityClient.Data;
using Avangardum.LifeArena.UnityClient.Interfaces;
using UnityEngine;
using Zenject;

namespace Avangardum.LifeArena.UnityClient.Views
{
    public class GameView : IGameView
    {
        private IFieldView _fieldView;
        private IWindowManager _windowManager;
        
        [Inject]
        public void Inject(IFieldView fieldView, IWindowManager windowManager)
        {
            _fieldView = fieldView;
            _windowManager = windowManager;
            
            fieldView.CellClicked += OnCellClicked;
        }

        public event EventHandler<CellClickedEventArgs> CellClicked;

        public GameState GameState
        {
            set
            {
                _fieldView.LivingCells = value.LivingCells;
                
                _windowManager.IsNoInternetConnectionWindowVisible = false;
                _windowManager.IsServerUnavailableWindowVisible = false;
            }
        }

        public void ShowNoInternetConnectionMessage()
        {
            _windowManager.IsServerUnavailableWindowVisible = false;
            _windowManager.IsNoInternetConnectionWindowVisible = true;
        }

        public void ShowServerUnavailableMessage()
        {
            _windowManager.IsNoInternetConnectionWindowVisible = false;
            _windowManager.IsServerUnavailableWindowVisible = true;
        }

        public void ShowUnknownErrorMessage(string message)
        {
            Debug.LogError(message);
        }
        
        private void OnCellClicked(object sender, CellClickedEventArgs e)
        {
            CellClicked?.Invoke(this, e);
        }
    }
}