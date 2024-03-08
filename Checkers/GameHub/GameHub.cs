using Checkers.Domain.Entities;
using Checkers.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Checkers.GameHub
{
    public class GameHub : Hub
    {
        private readonly IGameRepository _gameRepository;
        public GameHub(IGameRepository gameRepository) 
        {
            _gameRepository = gameRepository;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task CreateGame(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.GroupExcept(gameId, Context.ConnectionId).SendAsync("TableJoined");
        }

        // Method to join an existing game
        public async Task JoinGame(string gameId)
        {
            var game = await _gameRepository.GetGameById(gameId);
            if (game != null)
            {
                if(game.P2 == string.Empty)
                {
                    // temporarly set the p2name to be "P2"
                    await _gameRepository.UpdateGameP2(gameId, "P2");
                    await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
                    await Clients.GroupExcept(gameId, Context.ConnectionId).SendAsync("TableJoined");
                }
            }
        }

        // Method to handle making a move
        public async Task MakeMove(int previousCheckerRow, int previousCheckerColumn, int nextCheckerRow, int nextCheckerColumn)
        {
            // Logic to handle the move
            // Notify clients about the move
        }
    }
}
