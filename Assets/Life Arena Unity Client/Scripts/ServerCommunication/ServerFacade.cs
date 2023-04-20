using System.Net.Http;
using System.Threading.Tasks;
using Avangardum.LifeArena.Shared;
using Avangardum.LifeArena.UnityClient.Data;
using Avangardum.LifeArena.UnityClient.Interfaces;

namespace Avangardum.LifeArena.UnityClient.ServerCommunication
{
    public class ServerFacade : IServerFacade
    {
        private const string ServerUrl = "http://localhost:5050";
        private const string GameApiRootUrl = ServerUrl + "/Api/Game";
        private const string GetGameStateUrl = GameApiRootUrl + "/GetGameState";
        private const string AddCellUrl = GameApiRootUrl + "/AddCell";
        
        ILivingCellsArrayPreserializer _livingCellsArrayPreserializer;

        public ServerFacade(ILivingCellsArrayPreserializer livingCellsArrayPreserializer)
        {
            _livingCellsArrayPreserializer = livingCellsArrayPreserializer;
        }

        public async Task<GameState> GetGameState()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(GetGameStateUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Server returned {response.StatusCode} status code");
            }
            var content = await response.Content.ReadAsStringAsync();
            throw new System.NotImplementedException();
        }

        public Task AddCell(int x, int y)
        {
            throw new System.NotImplementedException();
        }
    }
}