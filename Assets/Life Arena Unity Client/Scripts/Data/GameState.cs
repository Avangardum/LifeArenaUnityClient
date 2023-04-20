using System;

namespace Avangardum.LifeArena.UnityClient.Data
{
    public record GameState(bool[,] LivingCells, int Generation, TimeSpan TimeUntilNextGeneration, int CellsLeft, 
        int MaxCellsPerPlayerPerGeneration);
}