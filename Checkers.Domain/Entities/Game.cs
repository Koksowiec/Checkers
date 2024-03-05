using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Checkers.Domain.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        public string P1 { get; set; } = default!;
        public string P2 { get; set; } = default!;
        public string Winner { get; set; } = default!;
        [DisplayName("Starting Player")]
        public string StartingPlayer {  get; set; } = default!;
    }
}
