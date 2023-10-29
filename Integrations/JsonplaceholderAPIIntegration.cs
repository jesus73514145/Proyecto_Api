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
            string requestUrl = $"{API_URL}";
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync(requestUrl, post);

                if (response.IsSuccessStatusCode)
                {
                 
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<PostDTO>(content);
                }
                else
                {
                   
                    _logger.LogError($"Error al crear un nuevo registro. Código de estado: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
             
                _logger.LogError($"Error al crear un nuevo registro: {ex.Message}");
                return null; 
            }
        }

        // Método para obtener un post específico por ID
        public async Task<PostDTO> GetPostByIdAsync(int id)
        {
            try
            {
                // Realiza una solicitud HTTP GET a la API externa para obtener el objeto por su ID
                string requestUrl = $"{API_URL}{id}";
                HttpResponseMessage response = await _client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Lee y deserializa el contenido de la respuesta en un objeto PostDTO
                    string content = await response.Content.ReadAsStringAsync();
                    PostDTO post = JsonSerializer.Deserialize<PostDTO>(content);
                    return post;
                }
                else
                {
                    _logger.LogError($"La solicitud a la API no fue exitosa. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
          
                _logger.LogError($"Error al obtener el post con ID {id}: {ex.Message}");
            }

          
            return null;
        }

     
        // Método para actualizar un post
        public async Task<PostDTO> UpdatePostAsync(int id, PostDTO updatedPost)
        {
            string requestUrl = $"{API_URL}{id}";
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync(requestUrl, updatedPost);

                if (response.IsSuccessStatusCode)
                {
                    // Si la operación es exitosa, devuelve el objeto PostDTO actualizado
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<PostDTO>(content);
                }
                else
                {
                    // Maneja el caso en que la solicitud no sea exitosa
                    _logger.LogError($"Error al actualizar el registro. Código de estado: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Maneja las excepciones que puedan ocurrir durante la solicitud
                _logger.LogError($"Error al actualizar el registro: {ex.Message}");
                return null; 
            }
        }

        // Método para eliminar un post
        public async Task<bool> DeletePostAsync(int id)
        {
            string requestUrl = $"{API_URL}{id}";
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(requestUrl);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Error al eliminar el registro: {ex.Message}");
                return false;
            }
        }
    }
}