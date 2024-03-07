using Checkers.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace Checkers.GameHub
{
    public class GameHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task CreateGame(string gameId)
        {
            // Logic to create a new game
            // Notify clients that a new game has been created
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.GroupExcept(gameId, Context.ConnectionId).SendAsync("TableJoined");
        }

        // Method to join an existing game
        public async Task JoinGame(string gameId)
        {
            // Logic to join an existing game
            // Notify clients that a player has joined the game

            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.GroupExcept(gameId, Context.ConnectionId).SendAsync("TableJoined");
        }

        // Method to handle making a move
        public async Task MakeMove(int previousCheckerRow, int previousCheckerColumn, int nextCheckerRow, int nextCheckerColumn)
        {
            // Logic to handle the move
            // Notify clients about the move
        }
    }
}
