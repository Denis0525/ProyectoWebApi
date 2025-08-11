using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapi.Models;

namespace Webapi.Repositorio.IRepositorio
{
    public interface IAccountRepositorio : IRepositorio<UsuarioAuth>
    {
        Task<UsuarioAuth> LoginAsync(string url, UsuarioAuth itemCrear);
        Task<bool> RegisterAsync(string url, UsuarioAuth  itemCrear);
    }
}