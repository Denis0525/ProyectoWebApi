using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaApi.Modelos
{
    public class CategoriaDto
    {
       public int Id { get; set; }
       
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100,ErrorMessage = "El maximo de caracteres es de 100!")]
        public string Name { get; set; }
        public DateTime FechaCreacion{ get; set;}   
    }
}