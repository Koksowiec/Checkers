using Checkers.Domain.Entities;
using Checkers.Domain.Interfaces;
using Checkers.Models;
using Microsoft.AspNetCore.SignalR;

namespace Checkers.GameHub
{
    public class GameHub : Hub
    {
        private readonly IGameRepository _gameRepository;
        private readonly IMovesRepository _movesRepository;
        private static IList<ActiveGame> ActiveGames = new List<ActiveGame>();

        public GameHub(IGameRepository gameRepository,
            IMovesRepository moveRepository) 
        {
            _gameRepository = gameRepository;
            _movesRepository = moveRepository;
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            string name = Context.ConnectionId;

            var gameGroup = ActiveGames.Where(g => g.P1Name == name || g.P2Name == name).FirstOrDefault();
            string gameId;
            if (gameGroup != null)
            {
                var player = gameGroup.P1Name == name ? "P1" : "P2";
                gameId = gameGroup.GameName;

                if(gameGroup.P1Name == Context.ConnectionId)
                {
                    gameGroup.P1Name = "";
                }
                else
                {
                    gameGroup.P2Name = "";
                }

                await Clients.GroupExcept(gameId, Context.ConnectionId).SendAsync("PlayerLeft", player);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
            }    

            await base.OnDisconnectedAsync(ex);
        }

        public async Task SendMessage(string gameId, string message, string messageType)
        {
            await Clients.Group(gameId).SendAsync("ReciveMessage", message, messageType);
        }
        

        public async Task CreateGame(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.Caller.SendAsync("GameCreated");
            ActiveGames.Add(new ActiveGame()
            {
                GameName = gameId,
                P1Name = Context.ConnectionId,
                P2Name = string.Empty
            });
        }

        public async Task JoinGame(string gameId, string p2Name)
        {
            var game = await _gameRepository.GetGameById(gameId);
            if (game != null)
            {
                if(game.P2 == string.Empty)
                {
                    await _gameRepository.UpdateGameP2(gameId, p2Name);

                    await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
                    var activeGame = ActiveGames.Where(a => a.GameName == gameId).Select(a => a.P1Name).ToList();
                    if (!activeGame.Any() || string.IsNullOrEmpty(activeGame[0]))
                    {
                        await Clients.Caller.SendAsync("AbandonedGame");
                        return;
                    }

                    // Update game variable
                    game = await _gameRepository.GetGameById(gameId);

                    var gameGroup = ActiveGames.Where(g => g.GameName == gameId).FirstOrDefault();
                    if (gameGroup != null)
                    {
                        gameGroup.P2Name = Context.ConnectionId;
                    }

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

        public async Task YouWin(string gameId, string name)
        {
            await _gameRepository.UpdateWinner(gameId, name);

            await Clients.Caller.SendAsync("HandleVictory");
            await Clients.GroupExcept(gameId, Context.ConnectionId).SendAsync("HandleDefeat");
        }

        public async Task YouLost(string gameId)
        {
            await Clients.Caller.SendAsync("HandleDefeat");
            await Clients.GroupExcept(gameId, Context.ConnectionId).SendAsync("HandleVictory");
        }
    }
}
