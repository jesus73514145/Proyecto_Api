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
            try
            {
                HttpResponseMessage response = await _client.GetAsync(API_URL);
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<PostDTO>>(content);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener todos los posts: {ex.Message}");
                return new List<PostDTO>();
            }
        }

        // Método para crear un nuevo post
        public async Task<PostDTO> CreatePostAsync(PostDTO post)
        {
            try
            {
                StringContent content = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(API_URL, content);
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
                HttpResponseMessage response = await _client.GetAsync(API_URL + id);
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
                StringContent content = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(API_URL + id, content);
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
                HttpResponseMessage response = await _client.DeleteAsync(API_URL + id);
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