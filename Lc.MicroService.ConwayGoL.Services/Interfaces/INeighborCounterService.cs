using Lc.MicroService.ConwayGoL.Models.Dto;

namespace Lc.MicroService.ConwayGoL.Services.Interfaces
{
    /// <summary>
    /// Interface for the Neighbor Counter service.
    /// </summary>
    public interface INeighborCounterService
    {
        /// <summary>
        /// Counts the number of alive neighbors for a given cell in the board state.
        /// </summary>
        /// <param name="boardState">The current state of the board.</param>
        /// <param name="rowIndex">The row index of the cell.</param>
        /// <param name="colIndex">The column index of the cell.</param>
        /// <returns>The number of alive neighbors.</returns>
        int CountAliveNeighbors(BoardStateDto boardState, int rowIndex, int colIndex);
    }
}
