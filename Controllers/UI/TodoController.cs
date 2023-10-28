using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Api.Data;
using Proyecto_Api.Models;
using Proyecto_Api.Service;
using Proyecto_Api.DTO;
namespace Proyecto_Api.Controllers.UI
{

    public class TodoController : Controller
    {
        private readonly PostService _postService;

        public TodoController(PostService postService)
        {
            _postService = postService;
        }

        // GET: Producto
        public async Task<IActionResult> Index()
        {
            var productos = await _postService.GetAll();
            return productos != null ?
                          View(productos) :
                          Problem("Entity set 'ApplicationDbContext.DataPost'  is null.");
        }

        // GET: Producto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _postService.FirstOrDefault(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // GET: Producto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Producto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Title,Body")] TodoDTO todoDTO)
        {
            if (ModelState.IsValid)
            {
                await _postService.CreateOrUpdate(todoDTO);
                return RedirectToAction(nameof(Index));
            }
            return View(todoDTO);
        }

        // GET: Producto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var producto = await _postService.Get(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // POST: Producto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Title,Body")] TodoDTO todoDTO)
        {
            if (id != todoDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _postService.CreateOrUpdate(todoDTO);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_postService.ProductoExists(todoDTO.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todoDTO);
        }

        // GET: Producto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var producto = await _postService.Get(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _postService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}