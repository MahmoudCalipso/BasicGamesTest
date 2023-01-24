namespace Basic_Games_Shelf.DTO
{
    public class TopPlayedGame
    {
        public string? game { get; set; }
        public string? genre { get; set; }
        public string[]? platforms { get; set; }
        public int  totalPlayTime { get; set; }
        public int totalPlayers { get; set; }
    }
}
