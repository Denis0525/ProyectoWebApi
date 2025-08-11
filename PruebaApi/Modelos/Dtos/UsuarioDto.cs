using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaApi.Modelos
{
    public class UsuarioDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Nombre {get; set; }
        public string Password {get; set; } 
        public string Role {get; set; }
        
    }
}