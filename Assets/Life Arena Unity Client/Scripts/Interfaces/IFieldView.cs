using System;

namespace Avangardum.LifeArena.UnityClient.Interfaces
{
    public interface IFieldView
    {
        public record CellClickedEventArgs(int X, int Y);
        
        event EventHandler<CellClickedEventArgs> CellClicked;
        
        bool[,] LivingCells { set; }
        
        float Zoom { set; }
    }
}