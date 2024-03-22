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

        [HttpPost]
        public async Task<IActionResult> CreateGame(string gameId, string p1Name)
        {
            await _gameRepository.CreateAsync(gameId, p1Name);

            var request = new RequestViewModel()
            {
                GameId = gameId,
                Method = RequestMethods.create,
                P1Name = p1Name
            };

            return View("GameRoom", request);
        }

        [HttpPost]
        public IActionResult JoinGame(string gameId, string p2Name)
        {
            var request = new RequestViewModel()
            {
                GameId = gameId,
                Method = RequestMethods.join,
                P2Name = p2Name
            };

            return View("GameRoom", request);
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var winners = await _gameRepository.GetDasboard();
            if(winners != null)
            {
                var topUsers = winners
                    .GroupBy(u => u)
                    .Select(g => new { User = g.Key, Count = g.Count() })
                    .OrderByDescending(u => u.Count)
                    .Take(10);

                return Ok(topUsers);
            }

            return BadRequest();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
