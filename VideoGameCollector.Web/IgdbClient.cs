using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace VideoGameCollector.Web
{

    public class IgdbClient 
    {
        private HttpClient _client { get; }

        public IgdbClient(HttpClient client)
        {           
            _client = client;
        }

        public async Task<Game> GetGameById(int id)
        {
            var response = await _client.GetAsync($"/games/{id}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<IEnumerable<Game>>();
            if (!result.Any()) return null;

            return result.First();
        }

        public async Task<IEnumerable<Game>> GetPopularGames(int platformId, int limit = 10)
        {
            var response = await _client.GetAsync($"/games/?fields=name,url,summary&order=popularity:desc&filter[platforms][eq]={platformId}&limit={limit}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<IEnumerable<Game>>();
            if (!result.Any()) return null;

            return result;
        }
    }
}
