using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using Webapi.Views;

namespace Webapi.Repositorio.IRepositorio
{
    public interface IRepositorio<T> where T : class
    {
         Task<IEnumerable<T>> GetTodoAsync(string url);
         Task<IEnumerable> GetPeliculaEnCategoriaAsync(string url, int categoriaId);
         Task<IEnumerable<T>> Buscar(string url, string nombre);
         Task<T> GetAsync(string url, int Id);
         Task<bool> CrearAsync(string url, T itemCrear, string token);
         Task<bool> CrearPeliculaAsync(string url, T peliculaCrear, string token);
         Task<bool> ActualizarAsync(string url, T itemActualizar, string token);
         Task<bool> ActualizarPeliculaAsync(string url, T PeliculaActualizar, string token);
         Task<bool> BorrarAsync(string url, int Id, string token);
    }
}