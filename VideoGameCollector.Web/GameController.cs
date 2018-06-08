using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VideoGameCollector.Web.Controllers
{
    [Route("game")]
    public class GameController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IgdbClient _igdbClient;
        private string _apiUrl = "https://api-endpoint.igdb.com";
        private string _userKey = ""; // specify your free user key from https://api.igdb.com/signup

        public GameController(IHttpClientFactory clientFactory, IgdbClient igdbClient)
        {
            _clientFactory = clientFactory;
            _igdbClient = igdbClient;
        }

        [Route("basic")]
        public async Task<IActionResult> Basic()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiUrl}/games/26758"); // 26758 = Super Mario Odyssey

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("user-key", _userKey);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<IEnumerable<Game>>();
                var game = result.First();
                return Content(game.name);
            }
            else
            {
                return Content("There was an error retrieving your game, verify your user-key with igdb.com.");
            }
        }

        [Route("named")]
        public async Task<IActionResult> Named()
        {
            var client = _clientFactory.CreateClient("IgdbNamedClient");
            var response = await client.GetAsync("games/26758");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<IEnumerable<Game>>();
                var game = result.First();
                return Content(game.name);
            }
            else
            {
                return Content("There was an error retrieving your game, verify your user-key with igdb.com.");
            }
        }
        
        [Route("id")]
        public async Task<IActionResult> GetById(int id = 26758)
        {
            var game = await _igdbClient.GetGameById(id);
            return Content(game.name);
        }

        [Route("popular")]
        public async Task<IActionResult> GetPopularGames(int platformId = 130, int limit = 10)
        {
            var games = await _igdbClient.GetPopularGames(platformId, limit);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(games));
        }
    }
}