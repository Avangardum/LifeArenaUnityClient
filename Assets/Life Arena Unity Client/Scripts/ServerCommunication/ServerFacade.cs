using System;
using System.Net.Http;
using System.Threading.Tasks;
using Avangardum.LifeArena.Shared;
using Avangardum.LifeArena.UnityClient.Data;
using Avangardum.LifeArena.UnityClient.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace Avangardum.LifeArena.UnityClient.ServerCommunication
{
    public class ServerFacade : IServerFacade
    {
        private const string ServerUrl = "http://localhost:5050";
        private const string GameApiRootUrl = ServerUrl + "/Api/Game";
        private const string GetGameStateUrl = GameApiRootUrl + "/GetState";
        private const string AddCellUrl = GameApiRootUrl + "/AddCell";
        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(1);
        
        ILivingCellsArrayPreserializer _livingCellsArrayPreserializer;

        public ServerFacade(ILivingCellsArrayPreserializer livingCellsArrayPreserializer)
        {
            _livingCellsArrayPreserializer = livingCellsArrayPreserializer;
            Test();
        }

        private async void Test()
        {
            try
            {
                await GetGameState();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public async Task<GameState> GetGameState()
        {
            using var client = new HttpClient();
            client.Timeout = Timeout;
            var response = await client.GetAsync(GetGameStateUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Server returned {response.StatusCode} status code");
            }
            var json = await response.Content.ReadAsStringAsync();
            var gameStateResponse = JsonConvert.DeserializeObject<GameStateResponse>(json);
            var livingCells = _livingCellsArrayPreserializer.Depreserialize(gameStateResponse.LivingCells);
            var gameState = new GameState(livingCells, gameStateResponse.Generation, gameStateResponse.TimeUntilNextGeneration,
                gameStateResponse.CellsLeft, gameStateResponse.MaxCellsPerPlayerPerGeneration);
            return gameState;
        }

        public Task AddCell(int x, int y)
        {
            throw new System.NotImplementedException();
        }
    }
}