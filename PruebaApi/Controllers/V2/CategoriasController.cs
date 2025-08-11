using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PruebaApi.Repositorios.IRepositorio;

namespace PruebaApi.Controllers.V2
{
    [Route("api/v{version:apiVersion}/categorias")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio _ctRepo;
        private readonly IMapper _mapper;

         public CategoriasController(ICategoriaRepositorio ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;
        
        }
        
        [HttpGet("GetString")]
        public IEnumerable<string> Get()
        {
            return new string[] { "valor1", "valor2","valor3" };
        }
    }
}