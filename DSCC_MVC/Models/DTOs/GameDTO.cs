using System.ComponentModel.DataAnnotations;

namespace DSCC_MVC.Models.DTOs;

// Data Annotations are used to properly handle inputs
public class GameDTO
{
    public Guid GameId { get; set; }
    [MinLength(5)] public string GameName { get; set; } = string.Empty;
    [MinLength(5)] public string DeveloperName { get; set; } = string.Empty;
    [MinLength(5)] public string EngineName { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; } = DateTime.Today;
    public Guid GameGenreId { get; set; }
}