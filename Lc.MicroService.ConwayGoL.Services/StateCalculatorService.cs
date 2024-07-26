using Lc.MicroService.ConwayGoL.Models.Dto;
using Lc.MicroService.ConwayGoL.Services.Interfaces;

namespace Lc.MicroService.ConwayGoL.Services;

/// <summary>
/// Service to calculate the next state of the board in the Game of Life.
/// </summary>
public class StateCalculatorService : IStateCalculatorService
{
    private readonly INeighborCounterService _neighborCounterService;

    /// <summary>
    /// Initializes a new instance of the <see cref="StateCalculatorService"/> class.
    /// </summary>
    /// <param name="neighborCounterService">The service for counting alive neighbors.</param>
    public StateCalculatorService(INeighborCounterService neighborCounterService)
    {
        _neighborCounterService = neighborCounterService ?? throw new ArgumentNullException(nameof(neighborCounterService));
    }

    /// <inheritdoc />
    public BoardStateDto CalculateNextState(BoardStateDto boardState)
    {
        if (boardState == null)
        {
            throw new ArgumentNullException(nameof(boardState));
        }

        var board = boardState.Board;
        var rows = board.Count;
        var cols = board[0].Count;
        var nextBoard = new List<List<int>>(rows);

        for (var i = 0; i < rows; i++)
        {
            var newRow = new List<int>(cols);
            for (var j = 0; j < cols; j++)
            {
                var aliveNeighbors = _neighborCounterService.CountAliveNeighbors(boardState, i, j);
                if (board[i][j] == 1 && (aliveNeighbors < 2 || aliveNeighbors > 3))
                {
                    newRow.Add(0); // Cell dies
                }
                else if (board[i][j] == 0 && aliveNeighbors == 3)
                {
                    newRow.Add(1); // Cell "resuscitation "
                }
                else
                {
                    newRow.Add(board[i][j]); // Cell untouched
                }
            }
            nextBoard.Add(newRow);
        }

        return new BoardStateDto { Id = boardState.Id, Board = nextBoard };
    }

    /// <inheritdoc />
    public bool AreBoardsEqual(List<List<int>> boardOne, List<List<int>> boardTwo)
    {
        if (boardOne == null || boardTwo == null)
        {
            throw new ArgumentNullException(boardOne == null ? nameof(boardOne) : nameof(boardTwo));
        }

        if (boardOne.Count != boardTwo.Count)
            return false;

        for (var i = 0; i < boardOne.Count; i++)
        {
            if (boardOne[i].Count != boardTwo[i].Count)
                return false;

            for (var j = 0; j < boardOne[i].Count; j++)
            {
                if (boardOne[i][j] != boardTwo[i][j])
                    return false;
            }
        }
        return true;
    }
}
