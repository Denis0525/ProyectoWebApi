using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapi.Models;
using Webapi.Repositorio.IRepositorio;
using Webapi.Views;

namespace Webapi.Repositorio
{
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {

        private readonly IHttpClientFactory _clienteFactory;
        public CategoriaRepositorio(IHttpClientFactory clienteFactory) : base(clienteFactory)
        {
            _clienteFactory = clienteFactory;
        }
    }
}