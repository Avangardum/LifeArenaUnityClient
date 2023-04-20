﻿using Avangardum.LifeArena.UnityClient.Interfaces;
using UnityEngine;

namespace Avangardum.LifeArena.UnityClient.Presenters
{
    public class FieldInputPresenter
    {
        private readonly IInputManager _inputManager;
        private readonly IFieldView _fieldView;

        public FieldInputPresenter(IInputManager inputManager, IFieldView fieldView)
        {
            _inputManager = inputManager;
            _fieldView = fieldView;
            
            _inputManager.FieldMovementRequested += OnFieldMovementRequested;
            _inputManager.FieldZoomChangeRequested += OnFieldZoomChangeRequested;
        }

        private void OnFieldMovementRequested(object sender, IInputManager.FieldMovementRequestedEventArgs e)
        {
            _fieldView.Move(e.Movement);
        }

        private void OnFieldZoomChangeRequested(object sender, IInputManager.FieldZoomChangeRequestedEventArgs e)
        {
            _fieldView.Zoom += e.ZoomChange;
        }
    }
}