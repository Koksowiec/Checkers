using Microsoft.AspNetCore.SignalR;

namespace Checkers.Models
{
    public class GameHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task CreateGame()
        {
            // Logic to create a new game
            // Notify clients that a new game has been created
            Clients.All.SendAsync("MethodName", "CreateGame Test message SignalR");
        }

        // Method to join an existing game
        public async Task JoinGame()
        {
            // Logic to join an existing game
            // Notify clients that a player has joined the game
            Clients.All.SendAsync("MethodName", "JoinGame Test message SignalR");
        }

        // Method to handle making a move
        public async Task MakeMove(string move)
        {
            // Logic to handle the move
            // Notify clients about the move
            Clients.All.SendAsync("MethodName", "MakeMove Test message SignalR");
        }
    }
}
