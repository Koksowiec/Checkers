using Checkers.Domain.Entities;
using Checkers.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Checkers.GameHub
{
    public class GameHub : Hub
    {
        private readonly IGameRepository _gameRepository;
        private readonly IMovesRepository _movesRepository;
        public GameHub(IGameRepository gameRepository,
            IMovesRepository moveRepository) 
        {
            _gameRepository = gameRepository;
            _movesRepository = moveRepository;
        }

        /*
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        */

        public async Task CreateGame(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.Caller.SendAsync("GameCreated");
        }

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
                    await Clients.Caller.SendAsync("YouJoined");
                    await Clients.GroupExcept(gameId, Context.ConnectionId).SendAsync("TableJoined");
                }
            }
        }

        // Method to handle making a move
        public async Task MakeMove(int previousCheckerRow, int previousCheckerColumn, int nextCheckerRow, int nextCheckerColumn, string gameId, string currentPlayer)
        {
            var game = await _gameRepository.GetGameById(gameId);
            if(game != null)
            {
                var move = await _movesRepository.GetMovesById(gameId);

                if(move == null)
                {
                    await _movesRepository.CreateAsync(gameId);
                }

                var newMove = previousCheckerColumn + "_" + previousCheckerRow + "-" + nextCheckerColumn + "_" + nextCheckerRow;
                if (currentPlayer == "P1")
                {
                    await _movesRepository.UpdateP1Moves(gameId, newMove);
                }
                else
                {
                    await _movesRepository.UpdateP2Moves(gameId, newMove);
                }

                await Clients.Caller.SendAsync("YouMoved");
                await Clients.GroupExcept(gameId, Context.ConnectionId).SendAsync("EnemyMoved", newMove);
            }
        }
    }
}
