using System;
using System.Net.Http;
using System.Threading.Tasks;
using Avangardum.LifeArena.Shared;
using Avangardum.LifeArena.UnityClient.Data;
using Avangardum.LifeArena.UnityClient.Exceptions;
using Avangardum.LifeArena.UnityClient.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace Avangardum.LifeArena.UnityClient.ServerCommunication
{
    public class ServerFacade : IServerFacade
    {
        private const string InternetConnectionTestUrl = "https://www.google.com";
        private const string ServerUrl = "http://localhost:5050";
        private const string GameApiRootUrl = ServerUrl + "/Api/Game";
        private const string GetGameStateUrl = GameApiRootUrl + "/GetState";
        private const string AddCellUrlTemplate = GameApiRootUrl + "/AddCell?x={0}&y={1}";
        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(1);
        
        ILivingCellsArrayPreserializer _livingCellsArrayPreserializer;

        public ServerFacade(ILivingCellsArrayPreserializer livingCellsArrayPreserializer)
        {
            _livingCellsArrayPreserializer = livingCellsArrayPreserializer;
        }
        
        public async Task<GameState> GetGameState()
        {
            using var client = CreateHttpClient();
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(GetGameStateUrl);
            }
            catch
            {
                await ProcessHttpRequestException();
                throw new Exception(); // Should never be reached
            }
            AssertIsSuccessStatusCode(response);
            var json = await response.Content.ReadAsStringAsync();
            var gameStateResponse = JsonConvert.DeserializeObject<GameStateResponse>(json);
            var livingCells = _livingCellsArrayPreserializer.Depreserialize(gameStateResponse.LivingCells);
            var gameState = new GameState(livingCells, gameStateResponse.Generation, gameStateResponse.TimeUntilNextGeneration,
                gameStateResponse.CellsLeft, gameStateResponse.MaxCellsPerPlayerPerGeneration);
            return gameState;
        }

        public async Task AddCell(int x, int y)
        {
            using var client = CreateHttpClient();
            var url = string.Format(AddCellUrlTemplate, x, y);
            HttpResponseMessage response;
            try
            {
                response = await client.PutAsync(url, null);
            }
            catch
            {
                await ProcessHttpRequestException();
                throw new Exception(); // Should never be reached
            }
            AssertIsSuccessStatusCode(response);
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.Timeout = Timeout;
            return client;
        }

        private static void AssertIsSuccessStatusCode(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Server returned {response.StatusCode} status code");
            }
        }

        private async Task ProcessHttpRequestException()
        {
            if (await IsConnectedToInternet())
            {
                throw new ServerUnavailableException();
            }
            else
            {
                throw new NoInternetConnectionException();
            }
        }

        private async Task<bool> IsConnectedToInternet()
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = Timeout;
                await client.GetAsync(InternetConnectionTestUrl);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}