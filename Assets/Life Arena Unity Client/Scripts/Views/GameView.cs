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
        private IHeader _header;
        
        [Inject]
        public void Inject(IFieldView fieldView, IWindowManager windowManager, IHeader header)
        {
            _fieldView = fieldView;
            _windowManager = windowManager;
            _header = header;
            
            _fieldView.CellClicked += OnCellClicked;
            _fieldView.ZoomChanged += OnFieldZoomChanged;
            
            _header.ZoomPercentageChanged += OnHeaderZoomPercentageChanged;
        }

        public event EventHandler<CellClickedEventArgs> CellClicked;

        public GameState GameState
        {
            set
            {
                _fieldView.LivingCells = value.LivingCells;
                
                _windowManager.IsNoInternetConnectionWindowVisible = false;
                _windowManager.IsServerUnavailableWindowVisible = false;
                
                _header.Generation = value.Generation;
                _header.NextGenerationInterval = value.NextGenerationInterval;
                _header.TimeUntilNextGeneration = value.TimeUntilNextGeneration;
                _header.CellsLeft = value.CellsLeft;
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
        
        private void OnHeaderZoomPercentageChanged(object sender, ZoomPercentageChangedEventArgs e)
        {
            _fieldView.ZoomFocusPointMode = ZoomFocusPointMode.ScreenCenter;
            _fieldView.ZoomPercentage = e.ZoomPercentage;
        }
        
        private void OnFieldZoomChanged(object sender, ZoomChangedEventArgs e)
        {
            _header.ZoomPercentage = e.ZoomPercentage;
        }
    }
}