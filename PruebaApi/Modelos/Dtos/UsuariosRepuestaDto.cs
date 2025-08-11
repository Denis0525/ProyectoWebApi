using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaApi.Modelos.Dtos
{
    public class UsuariosRepuestaDto
    {
        public UsuariosDatosDto Usuario { get; set; }
        public string Role { get; set; }
        public string Token {get; set; }
    }
}