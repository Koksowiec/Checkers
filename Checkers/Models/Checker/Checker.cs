namespace Checkers.Models.Checker
{
    public class Checker
    {
        public Player Player { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public CheckerDirection Direction { get; set; }
        public string Color {  get; set; }
    }
}
