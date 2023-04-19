﻿using System;
using System.Collections.Generic;
using System.Linq;
using Avangardum.LifeArena.UnityClient.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using static Avangardum.LifeArena.UnityClient.Interfaces.IInputManager;

namespace Avangardum.LifeArena.UnityClient.IO
{
    public class InputManager : IInputManager, ITickable
    {
        private const float ScrollZoomSensitivity = 0.1f;

        private Vector2? _lastMousePosition;
        
        public event EventHandler<FieldMovementRequestedEventArgs> FieldMovementRequested;
        public event EventHandler<FieldZoomChangeRequestedEventArgs> FieldZoomChangeRequested;
        
        private bool IsMouseOverField
        {
            get
            {
                PointerEventData eventData = new PointerEventData(EventSystem.current);
                eventData.position = Input.mousePosition;
                List<RaycastResult> raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, raycastResults);
                var isMouseOverField = 
                    !raycastResults.Any() || raycastResults.First().gameObject.CompareTag(Tags.CellView);
                return isMouseOverField;
            }
        }
        
        public void Tick()
        {
            ProcessFieldMovement();
            ProcessFieldZoom();

            void ProcessFieldMovement()
            {
                if (Input.GetMouseButton(0) && IsMouseOverField && _lastMousePosition is { } lastMousePosition)
                {
                    var mousePositionDelta = (Vector2)Input.mousePosition - lastMousePosition;
                    FieldMovementRequested?.Invoke(this, new FieldMovementRequestedEventArgs(mousePositionDelta));
                }

                _lastMousePosition = Input.mousePosition;
            }
            
            void ProcessFieldZoom()
            {
                var zoomChange = Input.mouseScrollDelta.y * ScrollZoomSensitivity;
                if (zoomChange == 0) return;
                FieldZoomChangeRequested?.Invoke(this, new FieldZoomChangeRequestedEventArgs(zoomChange));
            }
        }
    }
}