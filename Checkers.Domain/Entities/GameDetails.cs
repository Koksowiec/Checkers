using System.ComponentModel.DataAnnotations;

namespace Checkers.Domain.Entities
{
    public class GameDetails
    {
        [Key]
        public int Id { get; set; }
        public int GameId { get; set; }
        public string P1_Color { get; set; }
        public string P2_Color { get; set; }
    }
}
