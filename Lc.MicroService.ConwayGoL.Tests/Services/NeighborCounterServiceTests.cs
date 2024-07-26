using Lc.MicroService.ConwayGoL.Models.Dto;
using Lc.MicroService.ConwayGoL.Services;
using Lc.MicroService.ConwayGoL.Services.Interfaces;

namespace Lc.MicroService.ConwayGoL.Tests.Services
{
    public class NeighborCounterServiceTests
    {
        private readonly INeighborCounterService _service;

        public NeighborCounterServiceTests()
        {
            _service = new NeighborCounterService();
        }

        [Fact]
        public void CountLiveNeighbors_ShouldReturnCorrectCount()
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
            var result = _service.CountAliveNeighbors(boardState, 1, 1);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void CountLiveNeighbors_ShouldThrowExceptionForInvalidCoordinates()
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

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _service.CountAliveNeighbors(boardState, -1, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _service.CountAliveNeighbors(boardState, 1, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _service.CountAliveNeighbors(boardState, 3, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _service.CountAliveNeighbors(boardState, 1, 3));
        }
    }
}
