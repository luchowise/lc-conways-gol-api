using Lc.MicroService.ConwayGoL.Models.Dto;

namespace Lc.MicroService.ConwayGoL.Services.Interfaces
{
    /// <summary>
    /// Interface for the State Calculator service.
    /// </summary>
    public interface IStateCalculatorService
    {
        /// <summary>
        /// Calculates the next state of the board.
        /// </summary>
        /// <param name="boardState">The current state of the board.</param>
        /// <returns>The next state of the board.</returns>
        BoardStateDto CalculateNextState(BoardStateDto boardState);

        /// <summary>
        /// Compares 2 boards to establish if they are equal.
        /// </summary>
        /// <param name="boardOne">The first board to compare.</param>
        /// <param name="boardTwo">The second board to compare.</param>
        /// <returns>true or false.</returns>
        bool AreBoardsEqual(List<List<int>> boardOne, List<List<int>> boardTwo);
    }
}
