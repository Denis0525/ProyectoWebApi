using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapi.Models;

namespace Webapi.Repositorio.IRepositorio
{
    public interface IPeliculasRepositorio :IRepositorio<Pelicula>
    {
         Task<IEnumerable<Pelicula>> GetPeliculasTodoAsync(string url);
    }
}