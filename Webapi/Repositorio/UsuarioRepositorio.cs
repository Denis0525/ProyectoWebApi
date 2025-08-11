using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapi.Models;
using Webapi.Repositorio.IRepositorio;

namespace Webapi.Repositorio
{
    public class UsuarioRepositorio : Repositorio<Usuario>, IUsuarioRepositorio
    {
        private readonly IHttpClientFactory _clienteFactory;
        public UsuarioRepositorio(IHttpClientFactory clienteFactory) : base(clienteFactory)
        {
            _clienteFactory = clienteFactory;
        }
    }
}