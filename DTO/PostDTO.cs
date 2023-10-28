using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Api.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        // probando probando xdxd
        public bool IsDeleted { get; set; } = false;
    }
}