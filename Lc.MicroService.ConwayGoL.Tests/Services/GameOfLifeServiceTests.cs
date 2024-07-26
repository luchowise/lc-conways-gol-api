using Lc.MicroService.ConwayGoL.Models.Dto;
using Lc.MicroService.ConwayGoL.Persistence.Interfaces;
using Lc.MicroService.ConwayGoL.Services;
using Lc.MicroService.ConwayGoL.Services.Interfaces;
using Moq;
using Xunit;

namespace Lc.MicroService.ConwayGoL.Tests.Services;

/// <summary>
/// Unit tests for the GameOfLifeService.
/// </summary>
public class GameOfLifeServiceTests
{
    private readonly GameOfLifeService _service;
    private readonly Mock<IBoardStateRepository> _repositoryMock;
    private readonly Mock<IStateCalculatorService> _stateCalculatorServiceMock;

    public GameOfLifeServiceTests()
    {
        _repositoryMock = new Mock<IBoardStateRepository>();
        _stateCalculatorServiceMock = new Mock<IStateCalculatorService>();
        _service = new GameOfLifeService(_repositoryMock.Object, _stateCalculatorServiceMock.Object);
    }

    [Fact]
    public async Task UploadBoardState_ShouldReturnBoardId()
    {
        // Arrange
        var boardState = new BoardStateDto
        {
            Board = new List<List<int>>
                {
                    new List<int> { 0, 1, 0 },
                    new List<int> { 0, 0, 1 },
                    new List<int> { 1, 1, 1 }
                }
        };

        // Act
        var result = await _service.UploadBoardStateAsync(boardState);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task GetNextState_ShouldReturnNextStateDTO()
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
        _repositoryMock.Setup(r => r.GetBoardStateAsync(1)).ReturnsAsync(boardState);
        _stateCalculatorServiceMock.Setup(s => s.CalculateNextState(It.IsAny<BoardStateDto>())).Returns(new BoardStateDto
        {
            Id = 1,
            Board = new List<List<int>>
                {
                    new List<int> { 1, 1, 0 },
                    new List<int> { 1, 1, 1 },
                    new List<int> { 0, 1, 1 }
                }
        });

        // Act
        var result = await _service.GetNextStateAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.NotNull(result.Board);
    }

    [Fact]
    public async Task GetStatesAway_ShouldReturnStateAfterNIterations()
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
        _repositoryMock.Setup(r => r.GetBoardStateAsync(1)).ReturnsAsync(boardState);
        _stateCalculatorServiceMock.Setup(s => s.CalculateNextState(It.IsAny<BoardStateDto>())).Returns(new BoardStateDto
        {
            Id = 1,
            Board = new List<List<int>>
                {
                    new List<int> { 1, 1, 0 },
                    new List<int> { 1, 1, 1 },
                    new List<int> { 0, 1, 1 }
                }
        });

        // Act
        var result = await _service.GetStatesAwayAsync(1, 5);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.NotNull(result.Board);
    }

    [Fact]
    public async Task GetFinalState_ShouldReturnFinalStateDto()
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
        _repositoryMock.Setup(r => r.GetBoardStateAsync(1)).ReturnsAsync(boardState);
        _stateCalculatorServiceMock.Setup(s => s.CalculateNextState(It.IsAny<BoardStateDto>())).Returns(new BoardStateDto
        {
            Id = 1,
            Board = new List<List<int>>
                {
                    new List<int> { 1, 1, 0 },
                    new List<int> { 1, 1, 1 },
                    new List<int> { 0, 1, 1 }
                }
        });
        _stateCalculatorServiceMock.Setup(s => s.AreBoardsEqual(It.IsAny<List<List<int>>>(), It.IsAny<List<List<int>>>())).Returns(true);

        // Act
        var result = await _service.GetFinalStateAsync(1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.NotNull(result.Board);
    }

    [Fact]
    public async Task UploadBoardState_ShouldThrowExceptionForNullBoard()
    {
        // Arrange
        BoardStateDto boardState = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UploadBoardStateAsync(boardState));
    }

    [Fact]
    public async Task GetNextState_ShouldThrowExceptionForNonExistentId()
    {
        // Arrange
        var inexistentId = 99;
        _repositoryMock.Setup(r => r.GetBoardStateAsync(It.IsAny<int>())).ReturnsAsync((BoardStateDto)null);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => _service.GetNextStateAsync(inexistentId));
    }

    [Fact]
    public async Task GetStatesAway_ShouldHandleZeroIterations()
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
        _repositoryMock.Setup(r => r.GetBoardStateAsync(1)).ReturnsAsync(boardState);

        // Act
        var result = await _service.GetStatesAwayAsync(1, 0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(boardState, result);
    }

    [Fact]
    public async Task GetFinalStateAsync_ShouldThrowExceptionWhenNoStableState()
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

        _repositoryMock.Setup(r => r.GetBoardStateAsync(1)).ReturnsAsync(boardState);
        _stateCalculatorServiceMock.Setup(s => s.CalculateNextState(It.IsAny<BoardStateDto>())).Returns(boardState);
        _stateCalculatorServiceMock.Setup(s => s.AreBoardsEqual(It.IsAny<List<List<int>>>(), It.IsAny<List<List<int>>>())).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetFinalStateAsync(1, 10));
    }
}