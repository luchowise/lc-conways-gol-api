using Lc.MicroService.ConwayGoL.Models.Dto;
using Lc.MicroService.ConwayGoL.Persistence.Interfaces;
using Newtonsoft.Json;
using System.Collections.Concurrent;

/// <summary>
/// This class is for managing board states using file storage.
/// It saves and loads board states from a JSON file.
/// </summary>
public class FileBoardStateRepository : IBoardStateRepository
{
    private readonly string _filePath;
    private ConcurrentDictionary<int, BoardStateDto> _boardStates;

    /// <summary>
    /// Constructor initializes the repository and loads board states from file.
    /// </summary>
    public FileBoardStateRepository(string filePath)
    {
        _filePath = filePath;
        _boardStates = LoadBoardStates();
    }

    /// <summary>
    /// Save the board state asynchronously.
    /// </summary>
    /// <param name="boardState">The board state to save.</param>
    public async Task SaveBoardStateAsync(BoardStateDto boardState)
    {
        _boardStates[boardState.Id] = boardState;
        await SaveBoardStates();
    }

    /// <summary>
    /// Get the board state by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the board state.</param>
    /// <returns>The board state.</returns>
    public Task<BoardStateDto> GetBoardStateAsync(int id)
    {
        _boardStates.TryGetValue(id, out var boardState);
        return Task.FromResult(boardState);
    }

    /// <summary>
    /// Get all board states asynchronously.
    /// </summary>
    /// <returns>An enumerable of all board states.</returns>
    public Task<IEnumerable<BoardStateDto>> GetAllBoardStatesAsync()
    {
        return Task.FromResult(_boardStates.Values.AsEnumerable());
    }


    /// <summary>
    /// Gets the ID of the last board state.
    /// </summary> 
    /// <returns>A task representing the asynchronous operation, with the ID of the last board state as the result.</returns>
    public Task<int> GetLastBoardId()
    {
        return Task.FromResult(_boardStates.Keys.Any() ? _boardStates.Keys.Max() : 0);
    }

    /// <summary>
    /// Load board states from the JSON file.
    /// </summary>
    /// <returns>A dictionary of board states.</returns>
    private ConcurrentDictionary<int, BoardStateDto> LoadBoardStates()
    {
        if (!File.Exists(_filePath))
        {
            return new ConcurrentDictionary<int, BoardStateDto>();
        }

        var json = File.ReadAllText(_filePath);
        return JsonConvert.DeserializeObject<ConcurrentDictionary<int, BoardStateDto>>(json) ?? new ConcurrentDictionary<int, BoardStateDto>();
    }

    /// <summary>
    /// Save board states to the JSON file asynchronously.
    /// </summary>
    private async Task SaveBoardStates()
    {
        var json = JsonConvert.SerializeObject(_boardStates);
        await File.WriteAllTextAsync(_filePath, json);
    }

}
