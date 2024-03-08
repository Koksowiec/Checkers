using Checkers.Domain.Entities;
using Checkers.Domain.Interfaces;
using Checkers.Models;
using Checkers.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace Checkers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGameRepository _gameRepository;

        public HomeController(ILogger<HomeController> logger, IGameRepository gameRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
        }

        public async Task<IActionResult> Index()
        {
            var games = await _gameRepository.GetAllAsync();
            var gamesList = games.ToList();
            return View(gamesList);
        }

        public async Task<IActionResult> GameRoom()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame(string gameId, string p1Name)
        {
            await _gameRepository.CreateAsync(gameId, p1Name);

            var request = new RequestViewModel()
            {
                GameId = gameId,
                Method = RequestMethods.create
            };

            return View("GameRoom", request);
        }

        [HttpPost]
        public IActionResult JoinGame(string gameId)
        {
            var request = new RequestViewModel()
            {
                GameId = gameId,
                Method = RequestMethods.join
            };

            return View("GameRoom", request);
        }

        [HttpGet]
        public async Task<IActionResult> GetRooms(string playerName)
        {
            var games = await _gameRepository.GetAllAsync();
            return View("Index", games.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
