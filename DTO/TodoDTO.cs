using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Api.DTO
{
     [Table("t_todo")]
    public class TodoDTO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        public int UserId { get; set; }
        
        public string Title { get; set; }
        public string Body { get; set; }
    }
}