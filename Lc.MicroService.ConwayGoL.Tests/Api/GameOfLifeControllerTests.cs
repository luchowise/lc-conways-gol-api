using Lc.MicroService.ConwayGoL.Controllers;
using Lc.MicroService.ConwayGoL.Models.Dto;
using Lc.MicroService.ConwayGoL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace Lc.MicroService.ConwayGoL.Tests.Controllers;

public class GameOfLifeControllerTests
{
    private readonly GameOfLifeController _controller;
    private readonly Mock<IGameOfLifeService> _serviceMock;

    public GameOfLifeControllerTests()
    {
        _serviceMock = new Mock<IGameOfLifeService>();
        _controller = new GameOfLifeController(_serviceMock.Object);
    }

    [Fact]
    public async Task UploadBoardState_ShouldReturnOkResultWithBoardId()
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
        _serviceMock.Setup(s => s.UploadBoardStateAsync(It.IsAny<BoardStateDto>())).ReturnsAsync(1);

        // Act
        var result = await _controller.UploadBoardState(boardState);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(1, okResult.Value);
    }

    [Fact]
    public async Task GetNextState_ShouldReturnOkResultWithNextState()
    {
        // Arrange
        var nextState = new BoardStateDto
        {
            Id = 1,
            Board = new List<List<int>>
                {
                    new List<int> { 0, 0, 0 },
                    new List<int> { 1, 0, 1 },
                    new List<int> { 0, 1, 1 }
                }
        };
        _serviceMock.Setup(s => s.GetNextStateAsync(1)).ReturnsAsync(nextState);

        // Act
        var result =await  _controller.GetNextState(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedState = Assert.IsType<BoardStateDto>(okResult.Value);
        Assert.Equal(nextState.Id, returnedState.Id);
        Assert.Equal(nextState.Board, returnedState.Board);
    }

    [Fact]
    public async Task GetNextState_ShouldReturnNotFoundResult()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetNextStateAsync(1)).ReturnsAsync((BoardStateDto)null);

        // Act
        var result = await _controller.GetNextState(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetStatesAway_ShouldReturnOkResultWithStatesAway()
    {
        // Arrange
        var statesAway = new BoardStateDto
        {
            Id = 1,
            Board = new List<List<int>>
                {
                    new List<int> { 0, 0, 0 },
                    new List<int> { 0, 1, 0 },
                    new List<int> { 1, 1, 1 }
                }
        };
        _serviceMock.Setup(s => s.GetStatesAwayAsync(1, 5)).ReturnsAsync(statesAway);

        // Act
        var result = await _controller.GetStatesAway(1, 5);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedState = Assert.IsType<BoardStateDto>(okResult.Value);
        Assert.Equal(statesAway.Id, returnedState.Id);
        Assert.Equal(statesAway.Board, returnedState.Board);
    }

    [Fact]
    public async Task GetStatesAway_ShouldReturnNotFoundResult()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetStatesAwayAsync(1, 5)).ReturnsAsync((BoardStateDto)null);

        // Act
        var result = await _controller.GetStatesAway(1, 5);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetFinalState_ShouldReturnOkResultWithFinalState()
    {
        // Arrange
        var finalState = new BoardStateDto
        {
            Id = 1,
            Board = new List<List<int>>
                {
                    new List<int> { 0, 0, 0 },
                    new List<int> { 0, 0, 0 },
                    new List<int> { 0, 0, 0 }
                }
        };
        _serviceMock.Setup(s => s.GetFinalStateAsync(1, 10)).ReturnsAsync(finalState);

        // Act
        var result = await _controller.GetFinalState(1, 10);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedState = Assert.IsType<BoardStateDto>(okResult.Value);
        Assert.Equal(finalState.Id, returnedState.Id);
        Assert.Equal(finalState.Board, returnedState.Board);
    }

    [Fact]
    public async Task GetFinalState_ShouldReturnNotFoundResult()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetFinalStateAsync(1, 10)).ReturnsAsync((BoardStateDto)null);

        // Act
        var result = await _controller.GetFinalState(1, 10);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetFinalState_ShouldReturnBadRequestWhenNoStableState()
    {
        // Arrange
        const string message = "Board did not reach a stable state.";
        _serviceMock.Setup(s => s.GetFinalStateAsync(1, 10)).ThrowsAsync(new InvalidOperationException(message));

        // Act
        var result = await _controller.GetFinalState(1, 10);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<ErrorDto>(badRequestResult.Value);
        var value = badRequestResult.Value as ErrorDto;
        Assert.Equal(message, value.Message);
    }
}
