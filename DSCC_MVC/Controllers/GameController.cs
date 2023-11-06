using System.Net.Http.Headers;
using System.Text;
using DSCC_MVC.Models;
using DSCC_MVC.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DSCC_MVC.Controllers
{
    public class GameController : Controller
    {
        private readonly HttpClient _httpClient;

        // Injected IHttpClientFactory to get base url address and created HttpClient
        // The base url is in Program.cs
        public GameController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GamesApiClient");;
        }
        public async Task<IActionResult> Index()
        {
            HeaderClearing();
            var response = await _httpClient.GetAsync("Game");

            if (!response.IsSuccessStatusCode) return View();
            
            var games = JsonConvert.DeserializeObject<List<Game>>(ReadResponse(response));
            
            return View(games);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            HeaderClearing();
            return View(await GetGameById(id));
        }

        public async Task<IActionResult> Create()
        {
            HeaderClearing();

            var gameViewModelDto = await GenerateGameViewModel();
            
            return View(gameViewModelDto);
        }

        [HttpPost]
        public IActionResult Create(GameViewModelDTO gameViewDto)
        {
            if (ModelState.IsValid)
                if (GetResponseFromPost(gameViewDto.Game).IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));
            return View();
        }
        
        public async Task<IActionResult> Edit(Guid id)
        {
            HeaderClearing();
            var game = await GetGameById(id);
            if (game == null) return View();

            var gameViewModelDto = await GenerateGameViewModel(game);
            return View(gameViewModelDto);
        }

        [HttpPost]
        public IActionResult Edit(GameViewModelDTO gameDto)
        {
            if (ModelState.IsValid)
                if (GetResponseFromPut(gameDto.Game).IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));
            return View();
        }
        
        public async Task<IActionResult> Delete(Guid id)
        {
            HeaderClearing();
            return View(await GetGameById(id));
        }

        [HttpPost]
        public IActionResult Delete(Guid id, GameDTO gameDto)
        {
            var response = _httpClient.DeleteAsync(_httpClient.BaseAddress + $"Game/{id}").Result;
            if (!response.IsSuccessStatusCode) return View();
            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// Clears default headers and defines the request type of the data
        /// </summary>
        private void HeaderClearing()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Returns response message as a string
        /// </summary>
        private string ReadResponse(HttpResponseMessage responseMessage)
        {
            return responseMessage.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Handles POST method for GameDTO object and returns the response from api
        /// </summary>
        private HttpResponseMessage GetResponseFromPost(GameDTO game)
        {
            string createGameInfo = JsonConvert.SerializeObject(game);
            
            StringContent stringContentInfo = new StringContent(createGameInfo, Encoding.UTF8, "application/json");
            
            return _httpClient
                .PostAsync(_httpClient.BaseAddress + "Game", stringContentInfo)
                .Result;
        }
        
        /// <summary>
        /// Handles PUT method for GameDTO object and returns the response from api
        /// </summary>
        private HttpResponseMessage GetResponseFromPut(GameDTO game)
        {
            string createGameInfo = JsonConvert.SerializeObject(game);
            
            StringContent stringContentInfo = new StringContent(createGameInfo, Encoding.UTF8, "application/json");
            
            return _httpClient
                .PutAsync(_httpClient.BaseAddress + $"Game/{game.GameId}", stringContentInfo)
                .Result;
        }
        
        /// <summary>
        /// Returns an entity by Id
        /// </summary>
        private async Task<Game?> GetGameById(Guid id)
        {
            var response = await _httpClient.GetAsync($"Game/{id}");

            if (!response.IsSuccessStatusCode) return null;
            
            return JsonConvert.DeserializeObject<Game>(ReadResponse(response));
        }


        /// <summary>
        /// Generates GameViewModel for the POST method that will be passed to corresponding RazorPage
        /// </summary>
        private async Task<GameViewModelDTO?> GenerateGameViewModel()
        {
            var response = await _httpClient.GetAsync("Genre");
            
            if (!response.IsSuccessStatusCode) return null;

            var genres = JsonConvert.DeserializeObject<List<Genre>>(ReadResponse(response));
            return new GameViewModelDTO
            {
                Game = new GameDTO(),
                Genres = new SelectList(genres, "GenreId", "GenreName")
            };
        }
        
        /// <summary>
        /// Generates GameViewModel for the PUT method that will be passed to corresponding RazorPage
        /// </summary>
        private async Task<GameViewModelDTO?> GenerateGameViewModel(Game game)
        {
            var response = await _httpClient.GetAsync("Genre");
            
            if (!response.IsSuccessStatusCode) return null;

            var genres = JsonConvert.DeserializeObject<List<Genre>>(ReadResponse(response));
            return new GameViewModelDTO
            {
                Game = new GameDTO
                {
                    GameId = game.GameId,
                    GameName = game.GameName,
                    DeveloperName = game.DeveloperName,
                    EngineName = game.EngineName,
                    ReleaseDate = game.ReleaseDate,
                    GameGenreId = game.GameGenre.GenreId
                },
                Genres = new SelectList(genres, "GenreId", "GenreName", game.GameGenre.GenreId)
            };
        }
        
    }
}