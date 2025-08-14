using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PruebaApi.Modelos;
using PruebaApi.Repositorios;
using PruebaApi.Repositorios.IRepositorio;

namespace PruebaApi.Controllers
{
    [Route("api/v{version:apiVersion}/categorias")]
    [ApiController]
    [ApiVersion("1.0")]
    
    //[Route("api/[controller]")]
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
         [Obsolete("Este endpoint esta absoleto")]
        public IEnumerable<string> Get()
        {
            return new string[] { "valor1", "valor2","valor3"};
        }

        [AllowAnonymous]
        // [ResponseCache(Duration =20)]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore =true)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias()
        {
            var listaCategorias = _ctRepo.GetCategorias();
            var listaCategoriasDto = new List<CategoriaDto>();
            foreach (var lista in listaCategorias)
            {
                listaCategoriasDto.Add(_mapper.Map<CategoriaDto>(lista));
            }
            return Ok(listaCategoriasDto);
        }

        [HttpGet("{categoriaId:int}", Name = "GetCategoria")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategoria(int categoriaId)
        {
            var itemCategoria = _ctRepo.GetCategoria(categoriaId);
            if (itemCategoria == null)
            {
                return NotFound();
            }
            var itemCategoriasDto = _mapper.Map<CategoriaDto>(itemCategoria);
            return Ok(itemCategoriasDto);
        }
        [Authorize(Roles="Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CrearCategoria([FromBody] CrearCategoria crearCategoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (crearCategoriaDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_ctRepo.ExisteCategoria(crearCategoriaDto.Name))
            {
                ModelState.AddModelError("", "La Categoria ya existe");
                return StatusCode(404, ModelState);
            }
            var categoria = _mapper.Map<Categoria>(crearCategoriaDto);

            if (!_ctRepo.crearCategorias(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal al agregar{categoria.Name}");
                return StatusCode(404, ModelState);
            }
            return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.Id }, categoria);
        }
     
        [HttpPatch("{CategoriaId:int}", Name = "ActualizarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult ActualizarCategoria(int CategoriaId, [FromBody] CategoriaDto categoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (categoriaDto == null || CategoriaId != categoriaDto.Id)
            {
                return BadRequest(ModelState);
            }
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal al Actualizar{categoria.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.Id }, categoria);
        }
        [HttpPut("{CategoriaId:int}", Name = "ActualizarCategoriaPut")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult ActualizarCategoriaPut(int CategoriaId, [FromBody] CategoriaDto categoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (categoriaDto == null || CategoriaId != categoriaDto.Id)
            {
                return BadRequest(ModelState);
            }
            var categoriaExistente = _ctRepo.GetCategoria(CategoriaId);
            if (categoriaExistente == null)
            {
                return NotFound($"No se encontro la categoria con ID{CategoriaId}");
            }
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal al Actualizar{categoria.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.Id }, categoria);
        }
        //[Authorize(Roles ="Admin")]
        [HttpDelete("{CategoriaId:int}", Name = "BorrarRegistro")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult BorrarRegistro(int CategoriaId)
        {
            if(!_ctRepo.ExisteCategoria(CategoriaId))
            {
                return NotFound();
            }
            var categoria =_ctRepo.GetCategoria(CategoriaId);
            if(!_ctRepo.BorraCategoria(categoria))
            {
             ModelState.AddModelError("",$"Algo salio mal al borar el registro{categoria.Name}");
             return StatusCode(500,ModelState);
            }
            return NoContent();
        }
    }
}
