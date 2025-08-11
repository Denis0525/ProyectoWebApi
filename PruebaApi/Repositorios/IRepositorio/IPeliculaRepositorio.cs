using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PruebaApi.Modelos;

namespace PruebaApi.Repositorios.IRepositorio
{
    public interface IPeliculaRepositorio
    {
        ICollection<Peliculas> GetPelicula(int pageNumber, int PageSize);
        int GetTotalPeliculas();
        ICollection<Peliculas> GetPeliculasEnCategorias(int pelId);
        IEnumerable<Peliculas> BuscarPelicula(string nombre);
        Peliculas GetPelicula(int peliculasId);
        bool ExistePelicula(int id);
        bool ExistePelicula(string nombre);
        bool crearPelicula(Peliculas peliculas);
        bool ActualizarPelicula(Peliculas peliculas);
        bool BorraPelicula(Peliculas peliculas);
        bool Guardar();

    }
}