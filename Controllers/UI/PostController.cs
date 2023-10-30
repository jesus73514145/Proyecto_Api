using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Proyecto_Api.Integrations;
using Proyecto_Api.DTO;
using Proyecto_Api.Models;
using Proyecto_Api.Data;

namespace Proyecto_Api.Controllers.UI
{

    public class PostController : Controller
    {
        private readonly ILogger<PostController> _logger;
        private readonly JsonplaceholderAPIIntegration _jsonplaceholder;

        public PostController(ILogger<PostController> logger, JsonplaceholderAPIIntegration jsonplaceholder)
        {
            _logger = logger;
            _jsonplaceholder = jsonplaceholder;
        }

        public async Task<IActionResult> ListaPosts()
        {
            try
            {
                var posts = await _jsonplaceholder.GetAllPostsAsync();
                return View(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de posts.");
                return View("Error");
            }
        }


        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostDTO post)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Intento de crear un post con modelo inválido.");
                return View(post);
            }

            try
            {
                var newPost = await _jsonplaceholder.CreatePostAsync(post);
                TempData["Excelente"] = "Post creado con éxito!";
                return RedirectToAction(nameof(DetallePostCreado), post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un post.");
                ModelState.AddModelError("", "Error al crear el post. Inténtalo de nuevo.");
                return View(post);
            }
        }

        public async Task<IActionResult> DetallePostCreado(PostDTO post)
        {

            if (post == null)
            {
                _logger.LogWarning("DetallePostCreado recibió un post nulo.");
                return NotFound();
            }

            return View("DetallePostCreado", post);
        }

        public async Task<IActionResult> DetallePostEditado(PostDTO post)
        {

            if (post == null)
            {
                _logger.LogWarning("DetallePostEditado recibió un post nulo.");
                return NotFound();
            }

            return View("DetallePostEditado", post);
        }

        public async Task<IActionResult> DetallePost(int id)
        {

            try
            {
                PostDTO post = await _jsonplaceholder.GetPostByIdAsync(id);
                if (post == null)
                {
                    _logger.LogWarning($"No se encontró el post con ID {id}.");
                    return NotFound();
                }
                return View("DetallePost", post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener detalles del post con ID {id}.");
                return View("Error");
            }
        }



        public async Task<IActionResult> EditPost(int id)
        {
            try
            {
                PostDTO post = await _jsonplaceholder.GetPostByIdAsync(id);
                if (post == null)
                {
                    _logger.LogWarning($"No se encontró el post con ID {id} para editar.");
                    return NotFound();
                }
                return View("EditPost", post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el post con ID {id} para editar.");
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int id, PostDTO post)
        {

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Intento de editar un post con modelo inválido.");
                return View(post);
            }

            try
            {
                var updatedPost = await _jsonplaceholder.UpdatePostAsync(id, post);
                TempData["Excelente"] = "Post editado con éxito!";
                return RedirectToAction(nameof(DetallePostEditado), post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al editar el post con ID {id}.");
                ModelState.AddModelError("", "Error al editar el post. Inténtalo de nuevo.");
                return View(post);
            }
        }



        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                await _jsonplaceholder.DeletePostAsync(id);
                TempData["Message"] = "Post eliminado con éxito!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar el post con ID {id}.");
                TempData["Error"] = "Error al eliminar el post. Inténtalo de nuevo.";
            }

            return RedirectToAction(nameof(ListaPosts));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}