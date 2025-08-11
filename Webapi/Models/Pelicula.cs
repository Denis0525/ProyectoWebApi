using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Webapi.Views;

namespace Webapi.Models
{
    public class Pelicula
    {
        public Pelicula()
        {
            FechadeCreacion = DateTime.Now;
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion {get; set; }
        public int Duracion { get; set; }
        public IFormFile Imagen { get; set; }
        public string? RutaImagen { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }
        //RelacionCategoria
        public int categoriaId  { get; set; }
        public Categoria Categoria{ get; set; }
        public DateTime FechadeCreacion { get; private set; }
    }
}