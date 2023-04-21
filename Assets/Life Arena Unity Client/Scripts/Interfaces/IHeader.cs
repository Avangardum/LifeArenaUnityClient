using System;
using Avangardum.LifeArena.UnityClient.Data;

namespace Avangardum.LifeArena.UnityClient.Interfaces
{
    public interface IHeader
    {
        event EventHandler<ZoomChangedEventArgs> ZoomPercentageChanged; 

        int Generation { set; }
        TimeSpan NextGenerationInterval { set; }
        TimeSpan TimeUntilNextGeneration { set; }
        int CellsLeft { set; }
        float ZoomPercentage { get; set; }
    }
}