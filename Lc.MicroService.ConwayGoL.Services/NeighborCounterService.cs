using Lc.MicroService.ConwayGoL.Models.Dto;
using Lc.MicroService.ConwayGoL.Services.Interfaces;

namespace Lc.MicroService.ConwayGoL.Services;

/// <summary>
/// Service for counting live neighbors in the Game of Life.
/// </summary>
public class NeighborCounterService : INeighborCounterService
{
    public int CountAliveNeighbors(BoardStateDto boardState, int rowIndex, int colIndex)
    {
        var board = boardState.Board;
        var aliveNeighbors = 0;
        var rows = board.Count;
        var cols = board[0].Count;

        if (rowIndex < 0 || rowIndex >= rows || colIndex < 0 || colIndex >= cols)
        {
            throw new ArgumentOutOfRangeException(nameof(rowIndex), "Invalid row or column index.");
        }

        var rowStart = Math.Max(0, rowIndex - 1);
        var rowEnd = Math.Min(rows - 1, rowIndex + 1);
        var colStart = Math.Max(0, colIndex - 1);
        var colEnd = Math.Min(cols - 1, colIndex + 1);

        for (var i = rowStart; i <= rowEnd; i++)
        {
            for (var j = colStart; j <= colEnd; j++)
            {
                if (i == rowIndex && j == colIndex)
                {
                    continue;
                } 
                aliveNeighbors += board[i][j];
            }
        }

        return aliveNeighbors;
    }
}