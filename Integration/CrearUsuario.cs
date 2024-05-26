using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using practica3.Integration.dto;


namespace practica3.Integration
{
    public class CrearUsuario
    {
        private readonly ILogger<CrearUsuario> _logger;

        private const string API_URL = "https://reqres.in/api/users";
        private readonly HttpClient httpClient;

        public CrearUsuario(ILogger<CrearUsuario> logger)
        {
            _logger = logger;
            httpClient = new HttpClient();

        }

        public async Task<ApiResponse> CreateUser(string name, string job)
        {
            string requestUrl = API_URL;
            ApiResponse apiResponse = null;
            try
            {
                var userData = new { name, job };
                var jsonUserData = JsonSerializer.Serialize(userData);
                var requestContent = new StringContent(jsonUserData, Encoding.UTF8, "application/json");
                
                HttpResponseMessage response = await httpClient.PostAsync(requestUrl, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseBody);
                }
                else
                {
                    _logger.LogDebug($"SOLICITUD API INCORRECTA: {response.StatusCode}");
                }
            }
            catch(Exception ex)
            {
                _logger.LogDebug($"ERROR AL LLAMAR API: {ex.Message}");
            }
            return apiResponse;
        }

        public class ApiResponse
        {
            public string Name { get; set; }
            public string Job { get; set; }
            public string Id { get; set; }
            public DateTime CreatedAt { get; set; }
        }

    }
}