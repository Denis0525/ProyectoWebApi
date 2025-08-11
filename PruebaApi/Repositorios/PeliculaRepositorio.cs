using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PruebaApi.Data;
using PruebaApi.Modelos;
using PruebaApi.Repositorios.IRepositorio;

namespace PruebaApi.Repositorios
{
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly AplicatioDbContext _bd;

        public PeliculaRepositorio(AplicatioDbContext bd)
        {
            _bd = bd;
        }

        public bool ActualizarPelicula(Peliculas peliculas)
        {
           peliculas.FechadeCreacion = peliculas.FechadeCreacion.ToUniversalTime();
           var peliculaExistente = _bd.Pelicula.Find(peliculas.Id);
           if(peliculaExistente != null)
           {
            _bd.Entry(peliculaExistente).CurrentValues.SetValues(peliculas);
           }
           else
           {
            _bd.Pelicula.Update(peliculas);
           }
           return Guardar();
        }

        public bool BorraPelicula(Peliculas peliculas)
        {
            _bd.Pelicula.Remove(peliculas);
            return Guardar();
        }

        public IEnumerable<Peliculas> BuscarPelicula(string nombre)
        {
           IQueryable<Peliculas> query = _bd.Pelicula;
           if(!string.IsNullOrEmpty(nombre))
           {
            query = query.Where(e => e.Nombre.Contains(nombre)|| e.Descripcion.Contains(nombre));
           }
           return query.ToList();
        }

        public bool crearPelicula(Peliculas peliculas)
        {
            peliculas.FechadeCreacion = peliculas.FechadeCreacion.ToUniversalTime();
            //categoria.FechaCreacion = DateTime.Now;
            _bd.Pelicula.Add(peliculas);
            return Guardar();
        }

        public bool ExistePelicula(int id)
        {
             return _bd.Pelicula.Any(c => c.Id == id);
        }
        public bool ExistePelicula(string nombre)
        {
            bool valor = _bd.Pelicula.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }
        //v1
        // public ICollection<Peliculas> GetPelicula()
        // {
        //      return _bd.Pelicula.OrderBy(c => c.Nombre).ToList();
        // }
        //V2
        public ICollection<Peliculas> GetPelicula(int pageNumber, int PageSize)
        {
            return _bd.Pelicula.OrderBy(c => c.Nombre)
            .Skip((pageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        }
        public int GetTotalPeliculas()
        {
            return _bd.Pelicula.Count();
        }
        public Peliculas GetPelicula(int peliculasId)
        {
            return _bd.Pelicula.FirstOrDefault(c => c.Id == peliculasId);
        }


        public ICollection<Peliculas> GetPeliculasEnCategorias(int pelId)
        {
            return _bd.Pelicula.Include(ca => ca.Categoria).Where(ca => ca.categoriaId == pelId).ToList();
        }
        public bool Guardar()
        {
             return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}