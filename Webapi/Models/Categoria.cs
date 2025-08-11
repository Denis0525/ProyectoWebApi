using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webapi.Models
{
    public class Categoria
    {
                public Categoria()
        {
            FechaCreacion = DateTime.Now;

        }
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es Obligatorio")]
        public string Name { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}