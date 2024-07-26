using Lc.MicroService.ConwayGoL.Models.Dto;

namespace Lc.MicroService.ConwayGoL.Services.Interfaces
{
    /// <summary>
    /// Interface for the Game of Life service.
    /// </summary>
    public interface IGameOfLifeService
    {
        /// <summary>
        /// Asynchronously uploads the board state.
        /// </summary>
        /// <param name="boardState">The board state to upload.</param>
        /// <returns>A task representing the asynchronous operation, with the ID of the uploaded board state as the result.</returns>
        Task<int> UploadBoardStateAsync(BoardStateDto boardState);

        /// <summary>
        /// Asynchronously gets the next state of the board.
        /// </summary>
        /// <param name="id">The ID of the board state.</param>
        /// <returns>A task representing the asynchronous operation, with the next state of the board as the result.</returns>
        Task<BoardStateDto> GetNextStateAsync(int id);

        /// <summary>
        /// Asynchronously gets the state of the board after a given number of steps.
        /// </summary>
        /// <param name="boardId">The ID of the board state.</param>
        /// <param name="numberOfSteps">The number of steps to simulate.</param>
        /// <returns>A task representing the asynchronous operation, with the state of the board after the given number of steps as the result.</returns>
        Task<BoardStateDto> GetStatesAwayAsync(int boardId, int numberOfSteps);

        /// <summary>
        /// Asynchronously gets the final state of the board after a max number of attempts.
        /// </summary>
        /// <param name="id">The ID of the board state.</param>
        /// <param name="maxAttempts">The maximum number of attempts to simulate.</param>
        /// <returns>A task representing the asynchronous operation, with the final state of the board as the result.</returns>
        Task<BoardStateDto> GetFinalStateAsync(int id, int maxAttempts);
    }
}
