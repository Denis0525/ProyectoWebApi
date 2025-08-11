using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaApi.Modelos
{
    public class Usuario
    {
        [Key]
        public int id { get; set; }
        public string NombreUsuario { get; set; }
        public string Nombre {get; set; }
        public string Password {get; set; }
        public string Role {get; set; }
        
    }
}