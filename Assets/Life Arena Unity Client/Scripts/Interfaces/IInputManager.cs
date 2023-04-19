using System;
using UnityEngine;

namespace Avangardum.LifeArena.UnityClient.Interfaces
{
    public interface IInputManager
    {
        public record FieldMovementRequestedEventArgs(Vector2 Movement);
        public record FieldZoomChangeRequestedEventArgs(float ZoomChange);
        
        event EventHandler<FieldMovementRequestedEventArgs> FieldMovementRequested;
        event EventHandler<FieldZoomChangeRequestedEventArgs> FieldZoomChangeRequested;
    }
}