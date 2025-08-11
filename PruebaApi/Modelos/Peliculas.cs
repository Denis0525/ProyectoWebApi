using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaApi.Modelos
{
    public class Peliculas
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion {get; set; }
        public int Duracion { get; set; }
        public string? RutaImagen { get; set; }
        public string? RutaLocalImagen { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }
        public DateTime FechadeCreacion { get; set; }
        //RelacionCategoria
        public int categoriaId  { get; set; }
        [ForeignKey("categoriaId")]
        public Categoria Categoria{ get; set; }
    }
}