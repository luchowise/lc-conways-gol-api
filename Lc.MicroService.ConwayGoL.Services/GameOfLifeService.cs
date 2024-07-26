using Lc.MicroService.ConwayGoL.Models.Dto;
using Lc.MicroService.ConwayGoL.Persistence.Interfaces;
using Lc.MicroService.ConwayGoL.Services.Interfaces;

namespace Lc.MicroService.ConwayGoL.Services
{
    /// <summary>
    /// Service for managing the Game of Life operations.
    /// </summary>
    public class GameOfLifeService : IGameOfLifeService
    {
        private readonly IBoardStateRepository _repository;
        private readonly IStateCalculatorService _stateCalculatorService;
        private static bool _isInitializedNextId = false;
        private static int _nextId = 1;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="GameOfLifeService"/> class.
        /// </summary>
        /// <param name="repository">The repository for board states.</param>
        /// <param name="stateCalculatorService">The service for calculating board states.</param>
        public GameOfLifeService(
            IBoardStateRepository repository,
            IStateCalculatorService stateCalculatorService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _stateCalculatorService = stateCalculatorService ?? throw new ArgumentNullException(nameof(stateCalculatorService));
        }

        /// <inheritdoc />
        public async Task<int> UploadBoardStateAsync(BoardStateDto boardState)
        {
            if (boardState == null)
            {
                throw new ArgumentNullException(nameof(boardState));
            }

            await _semaphore.WaitAsync();
            try
            {
                if (!_isInitializedNextId)
                {
                    _nextId = await _repository.GetLastBoardId() + 1;
                    _isInitializedNextId = true;
                }
                else 
                {
                    _nextId++;
                }
                boardState.Id = _nextId;
            }
            finally
            {
                _semaphore.Release();
            }

            await _repository.SaveBoardStateAsync(boardState);
            return boardState.Id;
        }

        /// <inheritdoc />
        public async Task<BoardStateDto> GetNextStateAsync(int id)
        {
            var boardState = await _repository.GetBoardStateAsync(id);
            if (boardState == null)
            {
                throw new ArgumentException("Invalid ID", nameof(id));
            }

            var nextState = _stateCalculatorService.CalculateNextState(boardState);
            await _repository.SaveBoardStateAsync(boardState);
            return new BoardStateDto { Id = boardState.Id, Board = nextState.Board };
        }

        /// <inheritdoc />
        public async Task<BoardStateDto> GetStatesAwayAsync(int boardId, int numberOfSteps)
        {
            var boardState = await _repository.GetBoardStateAsync(boardId);
            if (boardState == null)
            {
                return null;
            }

            var currentBoardState = boardState;
            for (var i = 0; i < numberOfSteps; i++)
            {
                currentBoardState = _stateCalculatorService.CalculateNextState(currentBoardState);
            }
            var stateAway = new BoardStateDto { Id = boardState.Id, Board = currentBoardState.Board };
            await _repository.SaveBoardStateAsync(stateAway);
            return stateAway;
        }

        /// <inheritdoc />
        public async Task<BoardStateDto> GetFinalStateAsync(int id, int maxAttempts)
        {
            var boardState = await _repository.GetBoardStateAsync(id);

            if (boardState == null)
            {
                return null;
            }

            for (int i = 0; i < maxAttempts; i++)
            {
                var nextState = _stateCalculatorService.CalculateNextState(boardState);
                if (_stateCalculatorService.AreBoardsEqual(boardState.Board, nextState.Board))
                {
                    await _repository.SaveBoardStateAsync(nextState);
                    return nextState; // Stable state
                }
                boardState = nextState;
            }

            throw new InvalidOperationException("Board didn't reach a stable state");
        }
    }
}
