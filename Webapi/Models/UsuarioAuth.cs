using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webapi.Models
{
    public class UsuarioAuth
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "El Usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El Password es obligatorio")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "El password debe estar entre 20 y 40 carecteres")]        public string Password { get; set; }
        public string Token { get; set; }
    }
}