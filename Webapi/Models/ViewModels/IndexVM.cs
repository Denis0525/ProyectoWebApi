using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapi.Models.ViewModels
{
    public class IndexVM
    {
        public IEnumerable<Categoria> ListaCategorias { get; set; }

        public IEnumerable<Pelicula> ListaPeliculas { get; set; }

        public int TotalPages { set; get; } //Total de paginas disponibles
        public int CurrentPage { set; get; } //PaginaActual
    }
}