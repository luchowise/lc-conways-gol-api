using Lc.MicroService.ConwayGoL.Models.Dto;
using Lc.MicroService.ConwayGoL.Services.Interfaces;
using Lc.MicroService.ConwayGoL.Services;
using Moq;

namespace Lc.MicroService.ConwayGoL.Tests.Services;

/// <summary>
/// Unit tests for the StateCalculatorService.
/// </summary>
public class StateCalculatorServiceTests
{
    private readonly IStateCalculatorService _service;
    private readonly Mock<INeighborCounterService> _neighborCounterServiceMock;

    public StateCalculatorServiceTests()
    {
        _neighborCounterServiceMock = new Mock<INeighborCounterService>();
        _service = new StateCalculatorService(_neighborCounterServiceMock.Object);
    }

    /// <summary>
    /// Tests if CalculateNextState returns the correct next state of the board.
    /// </summary>
    [Fact]
    public void CalculateNextState_ShouldReturnNextStateDTO()
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

        _neighborCounterServiceMock.Setup(s => s.CountAliveNeighbors(It.IsAny<BoardStateDto>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((BoardStateDto bs, int row, int col) =>
            {
                if (row == 1 && col == 1)
                {
                    return 3; 
                }
                return 1;
            });

        // Act
        var result = _service.CalculateNextState(boardState);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(3, result.Board.Count);
    }

    /// <summary>
    /// Tests if AreBoardsEqual returns true for equal boards.
    /// </summary>
    [Fact]
    public void AreBoardsEqual_ShouldReturnTrueForEqualBoards()
    {
        // Arrange
        var board1 = new List<List<int>>
            {
                new List<int> { 0, 1, 0 },
                new List<int> { 0, 0, 1 },
                new List<int> { 1, 1, 1 }
            };

        var board2 = new List<List<int>>
            {
                new List<int> { 0, 1, 0 },
                new List<int> { 0, 0, 1 },
                new List<int> { 1, 1, 1 }
            };

        // Act
        var result = _service.AreBoardsEqual(board1, board2);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests if AreBoardsEqual returns false for different boards.
    /// </summary>
    [Fact]
    public void AreBoardsEqual_ShouldReturnFalseForDifferentBoards()
    {
        // Arrange
        var board1 = new List<List<int>>
            {
                new List<int> { 0, 1, 0 },
                new List<int> { 0, 0, 1 },
                new List<int> { 1, 1, 1 }
            };

        var board2 = new List<List<int>>
            {
                new List<int> { 0, 0, 0 },
                new List<int> { 0, 0, 0 },
                new List<int> { 0, 0, 0 }
            };

        // Act
        var result = _service.AreBoardsEqual(board1, board2);

        // Assert
        Assert.False(result);
    }
}