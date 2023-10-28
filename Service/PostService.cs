using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Proyecto_Api.Models;
using Proyecto_Api.DTO;

using Proyecto_Api.Data;

namespace Proyecto_Api.Service
{
    public class PostService
    {
        private readonly ILogger<PostService> _logger;
        private readonly ApplicationDbContext _context;
        public PostService(ILogger<PostService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<TodoDTO> CreateOrUpdate(TodoDTO p)
        {
            // Regla de Negocio 1
            if (p.Title == "")
            {
                throw new SystemException("No se puede ingresar valores nulos");
            }
            // Regla de Negocio 2

            // Verificar si el producto ya existe en la base de datos
            var existingProducto = await _context.DataPost.FindAsync(p.Id);

            if (existingProducto != null)
            {
                // El producto ya existe, así que actualizamos sus propiedades
                _context.Entry(existingProducto).CurrentValues.SetValues(p);
            }
            else
            {
                // El producto no existe, así que lo agregamos
                _context.DataPost.Add(p);
            }

            await _context.SaveChangesAsync();
            return p;
        }

        public async Task<List<TodoDTO>?> GetAll()
        {
            if (_context.DataPost == null)
                return null;
            return await _context.DataPost.ToListAsync();
        }

        public async Task<TodoDTO?> Get(int? id)
        {
            if (id == null || _context.DataPost == null)
            {
                return null;
            }

            var producto = await _context.DataPost.FindAsync(id);
            if (producto == null)
            {
                return null;
            }
            return producto;
        }

        public async Task<TodoDTO?> FirstOrDefault(int? id)
        {
            var producto = await _context.DataPost
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return null;
            }
            return producto;
        }

        public async Task Delete(int? id)
        {
            var producto = await _context.DataPost.FindAsync(id);
            if (producto != null)
            {
                _context.DataPost.Remove(producto);
            }
            await _context.SaveChangesAsync();
        }

        public bool ProductoExists(int id)
        {
            return (_context.DataPost?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}