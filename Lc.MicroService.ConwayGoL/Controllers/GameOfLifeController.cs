using Lc.MicroService.ConwayGoL.Factories.Errors;
using Lc.MicroService.ConwayGoL.Models.Dto;
using Lc.MicroService.ConwayGoL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Lc.MicroService.ConwayGoL.Controllers;

/// <summary>
/// Controller for Conway's Game of Life operations.
/// </summary>
[ApiController]
[Route("api/game-of-life")]
public class GameOfLifeController : ControllerBase
{
    private readonly IGameOfLifeService _gameOfLifeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameOfLifeController"/> class.
    /// </summary>
    /// <param name="gameOfLifeService">Service for Game of Life operations.</param>
    public GameOfLifeController(IGameOfLifeService gameOfLifeService)
    {
        _gameOfLifeService = gameOfLifeService ?? throw new ArgumentNullException(nameof(gameOfLifeService));
    }

    /// <summary>
    /// Uploads the initial board state.
    /// </summary>
    /// <param name="boardState">Board state to upload.</param>
    /// <returns>ID of the uploaded board state.</returns>
    [HttpPost("upload-board-state")]
    public async Task<IActionResult> UploadBoardState([FromBody] BoardStateDto boardState)
    {
        if (boardState == null)
        {
            return BadRequest(ErrorDtoFactory.Create((int)HttpStatusCode.BadRequest, ErrorsConstants.BoardStateNullError));
        }

        if (boardState.Board == null || !boardState.Board.Any() || !boardState.Board.All(row => row.Count == boardState.Board[0].Count))
        {
            return BadRequest(ErrorDtoFactory.Create((int)HttpStatusCode.BadRequest, ErrorsConstants.InvalidBoardStateError));
        }

        var boardId = await _gameOfLifeService.UploadBoardStateAsync(boardState);
        return Ok(boardId);
    }

    /// <summary>
    /// Gets the next board state.
    /// </summary>
    /// <param name="id">ID of the board state.</param>
    /// <returns>Next board state.</returns>
    [HttpGet("next-state/{id}")]
    public async Task<IActionResult> GetNextState(int id)
    {
        var nextState = await _gameOfLifeService.GetNextStateAsync(id);
        if (nextState == null)
        {
            var error = ErrorDtoFactory.Create((int)HttpStatusCode.NotFound, ErrorsConstants.BoardStateNotFoundError);
            return NotFound(error);
        }
        return Ok(nextState);
    }

    /// <summary>
    /// Gets the board state after a number of steps.
    /// </summary>
    /// <param name="boardId">ID of the board state.</param>
    /// <param name="numOfSteps">Number of steps to simulate.</param>
    /// <returns>Board state after the steps.</returns>
    [HttpGet("states-away/{id}/{n}")]
    public async Task<IActionResult> GetStatesAway(int boardId, int numOfSteps)
    {
        var statesAway = await _gameOfLifeService.GetStatesAwayAsync(boardId, numOfSteps);
        if (statesAway == null)
        {
            var error = ErrorDtoFactory.Create((int)HttpStatusCode.NotFound, ErrorsConstants.BoardStateNotFoundError);
            return NotFound(error);
        }
        return Ok(statesAway);
    }

    /// <summary>
    /// Gets the final board state after a maximum number of attempts.
    /// </summary>
    /// <param name="id">ID of the board state.</param>
    /// <param name="maxAttempts">Maximum number of attempts to simulate.</param>
    /// <returns>Final board state.</returns>
    [HttpGet("final-state/{id}/{maxAttempts}")]
    public async Task<IActionResult> GetFinalState(int id, int maxAttempts)
    {
        try
        {
            var finalState = await _gameOfLifeService.GetFinalStateAsync(id, maxAttempts);
            if (finalState == null)
            {
                var error = ErrorDtoFactory.Create((int)HttpStatusCode.NotFound, ErrorsConstants.BoardStateNotFoundError);
                return NotFound(error);
            }
            return Ok(finalState);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ErrorDtoFactory.Create((int)HttpStatusCode.BadRequest, ex.Message));
        }
    }
}
