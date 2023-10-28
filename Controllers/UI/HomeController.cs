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

namespace Proyecto_Api.Controllers.UI;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly JsonplaceholderAPIIntegration _jsonplaceholder;
    public HomeController(ILogger<HomeController> logger, JsonplaceholderAPIIntegration jsonplaceholder)
    {
        _logger = logger;
        _jsonplaceholder = jsonplaceholder;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult CreatePost()
    {
        var post = new PostDTO();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(PostDTO post)
    {
        if (ModelState.IsValid)
        {
            await _jsonplaceholder.CreatePostAsync(post);
            TempData["Excelente"] = "Se creo exitosamente ese post";
            return RedirectToAction("Index");
        }
        return View(post);
    }

    public async Task<IActionResult> EditPost(int id)
    {
        var post = await _jsonplaceholder.GetPostByIdAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        return View(post);
    }

    [HttpPost]
    public async Task<IActionResult> EditPost(int id, PostDTO post)
    {
        if (ModelState.IsValid)
        {
            await _jsonplaceholder.UpdatePostAsync(id, post);
            TempData["Excelente"] = "Se edito exitosamente ese post";
            return RedirectToAction("Index");
        }
        return View(post);
    }

    public async Task<IActionResult> ListaPosts()
    {
        var posts = await _jsonplaceholder.GetAllPostsAsync();
        return View(posts);
    }

    public async Task<IActionResult> DeletePost(int id)
    {
        await _jsonplaceholder.DeletePostAsync(id);
        TempData["Excelente"] = "Se elimino exitosamente ese post";
        return RedirectToAction("ListaPosts");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
