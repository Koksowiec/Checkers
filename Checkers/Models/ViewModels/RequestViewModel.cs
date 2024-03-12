namespace Checkers.Models.ViewModels
{
    public class RequestViewModel
    {
        public string GameId { get; set; }
        public RequestMethods Method { get; set; }
        public string P1Name { get; set; } = string.Empty;
        public string P2Name { get; set; } = string.Empty;
    }
}
