using System.ComponentModel.DataAnnotations;

namespace Checkers.Models.DbModels
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
