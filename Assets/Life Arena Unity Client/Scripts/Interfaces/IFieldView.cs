using System;
using Avangardum.LifeArena.UnityClient.Data;
using UnityEngine;

namespace Avangardum.LifeArena.UnityClient.Interfaces
{
    public interface IFieldView
    {
        event EventHandler<CellClickedEventArgs> CellClicked;
        
        bool[,] LivingCells { set; }
        
        float Zoom { get; set; }
        
        void Move(Vector2 movement);
    }
}