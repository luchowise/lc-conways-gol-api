using Lc.MicroService.ConwayGoL.Models.Dto;

namespace Lc.MicroService.ConwayGoL.Persistence.Interfaces;

/// <summary>
/// Interface for the board state repository.
/// </summary>
public interface IBoardStateRepository
{
    /// <summary>
    /// Asynchronously saves the board state to the repository.
    /// </summary>
    /// <param name="boardState">The board state to save.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveBoardStateAsync(BoardStateDto boardState);

    /// <summary>
    /// Asynchronously retrieves the board state by its ID.
    /// </summary>
    /// <param name="id">The ID of the board state to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, with the board state as the result.</returns>
    Task<BoardStateDto> GetBoardStateAsync(int id);

    /// <summary>
    /// Asynchronously retrieves all board states from the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with an enumerable collection of all board states as the result.</returns>
    Task<IEnumerable<BoardStateDto>> GetAllBoardStatesAsync();

    /// <summary>
    /// Gets the ID of the last board state.
    /// </summary> 
    /// <returns>A task representing the asynchronous operation, with the ID of the last board state as the result.</returns>
    Task<int> GetLastBoardId();
}

