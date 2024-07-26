using Lc.MicroService.ConwayGoL.Models.Dto;
using Lc.MicroService.ConwayGoL.Persistence;
using Lc.MicroService.ConwayGoL.Persistence.Interfaces;
using System.Collections.Concurrent;

namespace Lc.MicroService.ConwayGoL.Tests.Persistence;

/// <summary>
/// Unit tests for the InMemoryBoardStateRepository.
/// </summary>
public class FileBoardStateRepositoryTests
{
    private readonly IBoardStateRepository _repository;
    private const string _filePath = "test_states.json";
    public FileBoardStateRepositoryTests()
    {
        _repository = new FileBoardStateRepository(_filePath);
    }

    [Fact]
    public async Task SaveBoardState_ShouldSaveBoardState()
    {
        // Arrange
        var boardState = new BoardStateDto
        {
            Id = 1,
            Board = new List<List<int>>
                {
                    new List<int> { 0, 1, 0 },
                    new List<int> { 0, 0, 1 },
                    new List<int> { 1, 1, 1 }
                }
        };

        // Act
        await _repository.SaveBoardStateAsync(boardState);

        // Assert
        var savedBoardState = await _repository.GetBoardStateAsync(1);
        Assert.Equal(boardState, savedBoardState);
    }

    [Fact]
    public async Task GetBoardState_ShouldReturnBoardState()
    {
        // Arrange
        var boardState = new BoardStateDto
        {
            Id = 1,
            Board = new List<List<int>>
                {
                    new List<int> { 0, 1, 0 },
                    new List<int> { 0, 0, 1 },
                    new List<int> { 1, 1, 1 }
                }
        };
        await _repository.SaveBoardStateAsync(boardState);

        // Act
        var result = await _repository.GetBoardStateAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetBoardState_ShouldReturnNullForNonExistentId()
    {
        //Arrange
        var nonExistentId = 99;
        // Act
        var result = await _repository.GetBoardStateAsync(nonExistentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllBoardStates_ShouldReturnAllBoardStates()
    {
        // Arrange
        var boardState1 = new BoardStateDto
        {
            Id = 1,
            Board = new List<List<int>>
                {
                    new List<int> { 0, 1, 0 },
                    new List<int> { 0, 0, 1 },
                    new List<int> { 1, 1, 1 }
                }
        };
        var boardState2 = new BoardStateDto
        {
            Id = 2,
            Board = new List<List<int>>
                {
                    new List<int> { 1, 0, 1 },
                    new List<int> { 1, 1, 0 },
                    new List<int> { 0, 0, 1 }
                }
        };
        await _repository.SaveBoardStateAsync(boardState1);
        await _repository.SaveBoardStateAsync(boardState2);

        // Act
        var result = await _repository.GetAllBoardStatesAsync();

        // Assert
        Assert.Contains(boardState1, result);
        Assert.Contains(boardState2, result);
    }

    [Fact]
    public async Task SaveBoardState_ShouldUpdateExistingBoardState()
    {
        // Arrange
        var boardState = new BoardStateDto
        {
            Id = 1,
            Board = new List<List<int>>
                {
                    new List<int> { 0, 1, 0 },
                    new List<int> { 0, 0, 1 },
                    new List<int> { 1, 1, 1 }
                }
        };
        await _repository.SaveBoardStateAsync(boardState);

        var updatedBoardState = new BoardStateDto
        {
            Id = 1,
            Board = new List<List<int>>
                {
                    new List<int> { 1, 0, 1 },
                    new List<int> { 1, 1, 0 },
                    new List<int> { 0, 0, 1 }
                }
        };

        // Act
        await _repository.SaveBoardStateAsync(updatedBoardState);

        // Assert
        var savedBoardState = await _repository.GetBoardStateAsync(1);
        Assert.Equal(updatedBoardState, savedBoardState);
    }

    [Fact]
    public async Task GetLastBoardId_ShouldReturnMaxId_WhenBoardStatesExist()
    {
        // Arrange
        var boardStates = new ConcurrentDictionary<int, BoardStateDto>
        {
            [1] = new BoardStateDto { Id = 1, Board = new List<List<int>> { new List<int> { 0, 1 }, new List<int> { 1, 0 } } },
            [2] = new BoardStateDto { Id = 2, Board = new List<List<int>> { new List<int> { 1, 0 }, new List<int> { 0, 1 } } },
            [5] = new BoardStateDto { Id = 5, Board = new List<List<int>> { new List<int> { 1, 1 }, new List<int> { 0, 0 } } }
        };

        typeof(FileBoardStateRepository).GetField("_boardStates", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(_repository, boardStates);

        // Act
        var result = await _repository.GetLastBoardId();

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public async Task GetLastBoardId_ShouldReturnZero_WhenNoBoardStatesExist()
    {
        // Arrange
        var boardStates = new ConcurrentDictionary<int, BoardStateDto>();

        typeof(FileBoardStateRepository).GetField("_boardStates", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(_repository, boardStates);

        // Act
        var result = await _repository.GetLastBoardId();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task GetLastBoardId_ShouldReturnMaxId_AfterAddingNewBoardState()
    {
        // Arrange
        var boardStates = new ConcurrentDictionary<int, BoardStateDto>
        {
            [1] = new BoardStateDto { Id = 1, Board = new List<List<int>> { new List<int> { 0, 1 }, new List<int> { 1, 0 } } },
            [2] = new BoardStateDto { Id = 2, Board = new List<List<int>> { new List<int> { 1, 0 }, new List<int> { 0, 1 } } }
        };

        typeof(FileBoardStateRepository).GetField("_boardStates", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(_repository, boardStates);

        // Act
        boardStates[10] = new BoardStateDto { Id = 10, Board = new List<List<int>> { new List<int> { 1, 1 }, new List<int> { 0, 0 } } };
        var result = await _repository.GetLastBoardId();

        // Assert
        Assert.Equal(10, result);
    }

}