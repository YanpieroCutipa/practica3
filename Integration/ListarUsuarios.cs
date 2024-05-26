using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using practica3.Integration.dto;

namespace practica3.Integration
{
    public class ListarUsuarios
    {
        private readonly ILogger<ListarUsuarios> _logger;

        private const string API_URL = "https://reqres.in/api/users";
        private readonly HttpClient httpClient;

        public ListarUsuarios(ILogger<ListarUsuarios> logger)
        {
            _logger = logger;
            httpClient = new HttpClient();

        }

        public async Task<List<Usuario>> GetAllUser()
        {

            string requestUrl = API_URL;
            List<Usuario> listado = new List<Usuario>();
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    if (apiResponse != null)
                    {
                        listado = apiResponse.Data ?? new List<Usuario>();
                    }
                }
            }
            catch(Exception ex){
                _logger.LogDebug($"Error al llamar a la API: {ex.Message}");
            }
            return listado;

        }
        class ApiResponse
        {
            public int Page { get; set; }
            public int PerPage { get; set; }
            public int Total { get; set; }
            public int TotalPages { get; set; }
            public List<Usuario> Data { get; set; }
            public Support Support { get; set; }
        }

    }
}