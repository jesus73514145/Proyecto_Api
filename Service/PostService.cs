using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Proyecto_Api.Models;
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
    }
}