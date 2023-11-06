using Microsoft.AspNetCore.Mvc.Rendering;

namespace DSCC_MVC.Models.DTOs;

// GameViewModelDTO is created to handle Create and Edit functionalities
public class GameViewModelDTO
{
    public required GameDTO Game { get; set; }
    public SelectList? Genres { get; set; }
}