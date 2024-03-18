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

        
        public async Task SendMessage(string message, string messageType)
        {
            await Clients.Caller.SendAsync("ReciveMessage", message, messageType);
        }
        

        public async Task CreateGame(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.Caller.SendAsync("GameCreated");
        }

        public async Task JoinGame(string gameId, string p2Name)
        {
            var game = await _gameRepository.GetGameById(gameId);
            if (game != null)
            {
                if(game.P2 == string.Empty)
                {
                    await _gameRepository.UpdateGameP2(gameId, p2Name);
                    // Update game variable
                    game = await _gameRepository.GetGameById(gameId);

                    await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
                    await Clients.Caller.SendAsync("YouJoined", game.P1, game.P2);
                    await Clients.GroupExcept(gameId, Context.ConnectionId).SendAsync("TableJoined", game.P1, game.P2);
                }
            }
        }

        // Method to handle making a move
        public async Task MakeMove(
            int previousCheckerRow, int previousCheckerColumn, 
            int nextCheckerRow, int nextCheckerColumn, 
            int checkerToDeleteRow, int checkerToDeleteColumn,
            string gameId, string currentPlayer)
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

                var checkerToDelete = checkerToDeleteColumn + "_" + checkerToDeleteRow;

                await Clients.Caller.SendAsync("YouMoved");
                await Clients.GroupExcept(gameId, Context.ConnectionId).SendAsync("EnemyMoved", newMove, checkerToDelete);
            }
        }
    }
}
