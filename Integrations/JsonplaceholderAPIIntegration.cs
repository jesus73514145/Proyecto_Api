using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Proyecto_Api.DTO;

using System.Collections.Generic;
using System.Linq;

namespace Proyecto_Api.Integrations
{
    public class JsonplaceholderAPIIntegration
    {
        private readonly ILogger<JsonplaceholderAPIIntegration> _logger;
        private readonly HttpClient _client;
        private const string API_URL = "https://jsonplaceholder.typicode.com/posts/";

        private List<PostDTO> localPosts = new List<PostDTO>();

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

                string postJson = JsonSerializer.Serialize(post);

                HttpContent content = new StringContent(postJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(requestUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    PostDTO newPost = JsonSerializer.Deserialize<PostDTO>(responseJson);
                    Console.WriteLine(responseJson);
                    Console.WriteLine("Status Code " + response.StatusCode);
                    return newPost;
                }
                else
                {
                    _logger.LogError($"Error al crear el post. Codigo: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error de API: {ex.Message}");

            }
            return null;
        }

        // Método para obtener un post específico por ID
        public async Task<PostDTO> GetPostByIdAsync(int id)
        {
            try
            {
                string requestUrl = $"{API_URL}{id}";
                HttpResponseMessage response = await _client.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    string post = await response.Content.ReadAsStringAsync();
                    PostDTO p = JsonSerializer.Deserialize<PostDTO>(post);
                    Console.WriteLine(p);
                    return p;
                }
                else
                {
                    _logger.LogError($"Error al buscar el post. Codigo: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error API: {ex.Message}");
            }

            return null;
        }

     
        // Método para actualizar un post
        public async Task<PostDTO> UpdatePostAsync(int id, PostDTO post)
        {
            try
            {
                string requestUrl = $"{API_URL}{id}";
                string postJson = JsonSerializer.Serialize(post);
                HttpContent content = new StringContent(postJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(requestUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();

                    PostDTO updatedPost = JsonSerializer.Deserialize<PostDTO>(responseJson);
                    Console.WriteLine(updatedPost);
                    Console.WriteLine(responseJson);
                    Console.WriteLine("Status Code " + response.StatusCode);
                    Console.WriteLine(response.Content);

                    return updatedPost;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error API: {ex.Message}");
            }

            return null;
        }

        // Método para eliminar un post
        public async Task<String> DeletePostAsync(int id)
        {
            try
            {
                string url = $"{API_URL}{id}";

                HttpResponseMessage response = await _client.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseJson);
                    Console.WriteLine("Status Code: " + response.StatusCode);
                    return response.StatusCode.ToString();
                }
                else
                {
                    _logger.LogError($"Error al eliminar el post. Codigo: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error API: {ex.Message}");
            }

            return "No se pudo eliminar";
        }
    }
    
}