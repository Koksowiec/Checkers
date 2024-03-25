using Checkers.Controllers;
using Checkers.Domain.Entities;
using Checkers.Domain.Interfaces;
using Checkers.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Xml.Linq;


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

        [Fact]
        public async Task JoinGame_ReturnsViewResult()
        {
            // Arrange
            var gameRepository = new Mock<IGameRepository>();
            var controller = new HomeController(gameRepository.Object);
            var gameId = "1234";
            var p2name = "4321";
            var request = new RequestViewModel()
            {
                GameId = gameId,
                Method = RequestMethods.join,
                P2Name = p2name
            };

            // Act
            var result = controller.JoinGame(gameId, p2name) as ViewResult;

            // Assert
            Assert.Equal("GameRoom", result.ViewName);
            var model = result.ViewData.Model as RequestViewModel;
            Assert.Equal(request.GameId, model.GameId);
            Assert.Equal(request.P2Name, model.P2Name);
        }
    }


}

