using System.ComponentModel.DataAnnotations;

namespace Checkers.Domain.Entities
{
    public class Moves
    {
        [Key]
        public int Id { get; set; }
        public int GameId { get; set; }
        public string P1_Moves { get; set; }
        public string P2_Moves {  get; set; }
    }
}
