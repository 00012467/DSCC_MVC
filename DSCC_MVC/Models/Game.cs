namespace DSCC_MVC.Models;

public class Game
{
    public Guid GameId { get; set; }
    public string GameName { get; set; } = string.Empty;
    public string DeveloperName { get; set; } = string.Empty;
    public string EngineName { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; } = DateTime.Today;
    public Genre? GameGenre { get; set; }
}