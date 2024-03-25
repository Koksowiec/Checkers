using Checkers.Controllers;
using Checkers.Domain.Entities;
using Checkers.Domain.Interfaces;
using Moq;
using System.Web.Mvc;

namespace ControllerTests
{
    public class HomeControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult()
        {
            // Arrange
            var gameRepository = new Mock<IGameRepository>();
            var controller = new HomeController(gameRepository.Object);
            gameRepository.Setup(g => g.GetAllAsync()).ReturnsAsync(new List<Game>() {new Game() });

            // Act
            var result = await controller.Index() ;

            // Assert
            Assert.NotNull(result);
        }
    }
}
