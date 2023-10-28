using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Proyecto_Api.DTO;

namespace Proyecto_Api.Integrations
{
    public class JsonplaceholderAPIIntegration
    {
        private readonly ILogger<JsonplaceholderAPIIntegration> _logger;
        private readonly HttpClient _client;
        private const string API_URL = "https://jsonplaceholder.typicode.com/posts/";

        public JsonplaceholderAPIIntegration(ILogger<JsonplaceholderAPIIntegration> logger)
        {
            _logger = logger;
            _client = new HttpClient();
        }

        // Método para listar todos los posts
        public async Task<List<PostDTO>> GetAllPostsAsync()
        {
            string requestUrl = $"{API_URL}";
            List<PostDTO> listado = new List<PostDTO>();
            try
            {
                HttpResponseMessage response = await _client.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    listado = await response.Content.ReadFromJsonAsync<List<PostDTO>>() ?? new List<PostDTO>();

                }
            }
            catch (Exception ex)
            {
                 _logger.LogDebug($"Error al llamar a la API: {ex.Message}");
            }

            return listado;
        }

        // Método para crear un nuevo post
        public async Task<PostDTO> CreatePostAsync(PostDTO post)
        {
            try
            {
                string requestUrl = $"{API_URL}";
                StringContent content = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(requestUrl, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PostDTO>(responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el post: {ex.Message}");
                return null;
            }
        }

        // Método para obtener un post específico por ID
        public async Task<PostDTO> GetPostByIdAsync(int id)
        {
            try
            {
                string requestUrl = $"{API_URL}";
                HttpResponseMessage response = await _client.GetAsync(requestUrl + id);
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PostDTO>(content);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el post con ID {id}: {ex.Message}");
                return null;
            }
        }

        // Método para actualizar un post
        public async Task<PostDTO> UpdatePostAsync(int id, PostDTO post)
        {
            try
            {
                string requestUrl = $"{API_URL}";
                StringContent content = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(requestUrl + id, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PostDTO>(responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el post con ID {id}: {ex.Message}");
                return null;
            }
        }

        // Método para eliminar un post
        public async Task<bool> DeletePostAsync(int id)
        {
            try
            {
                string requestUrl = $"{API_URL}";
                HttpResponseMessage response = await _client.DeleteAsync(requestUrl + id);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el post con ID {id}: {ex.Message}");
                return false;
            }
        }
    }
}