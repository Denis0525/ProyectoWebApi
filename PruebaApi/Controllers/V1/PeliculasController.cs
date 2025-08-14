using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaApi.Modelos;
using PruebaApi.Modelos.Dtos;
using PruebaApi.Repositorios.IRepositorio;

namespace PruebaApi.Controllers
{
    [Route("api/v{version:apiVersion}/peliculas")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepositorio _pelRepo;
        private readonly IMapper _mapper;

        public PeliculasController(IPeliculaRepositorio pelRepo, IMapper mapper)
        {
            _pelRepo = pelRepo;
            _mapper = mapper;
        }
        //V1
        //   [HttpGet]
        //     [ProducesResponseType(StatusCodes.Status403Forbidden)]
        //     [ProducesResponseType(StatusCodes.Status200OK)]
        //     public IActionResult GetPeliculas()
        //     {
        //         var listaPeliculas = _pelRepo.GetPelicula();
        //         var listaPeliculasDto = new List<PeliculaDto>();
        //         foreach (var lista in listaPeliculas)
        //         {
        //             listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(lista));
        //         }
        //         return Ok(listaPeliculasDto);
        //     }

        //V2 con paginación
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculas([FromQuery] int pageNumber = 1, int pageSize = 15)
        {
            try
            {
                var totalPeliculas = _pelRepo.GetTotalPeliculas();
                var peliculas = _pelRepo.GetPelicula(pageNumber, pageSize);
                if (peliculas == null || !peliculas.Any())
                {
                    return NotFound("No se encontraron Peliculas.");

                }
                var peliculasDto = peliculas.Select(p => _mapper.Map<PeliculaDto>(p)).ToList();
                var response = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalPeliculas / (double)pageSize),
                    TotalItems = totalPeliculas,
                    Items = peliculasDto
                };
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Recuperando datos de la apliacion");
            }   
        }

        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPelicula(int peliculaId)
        {
            var itemPelicula = _pelRepo.GetPelicula(peliculaId);
            if (itemPelicula == null)
            {
                return NotFound();
            }
            var itemPeliculaDto = _mapper.Map<PeliculaDto>(itemPelicula);
            return Ok(itemPeliculaDto);
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CrearPelicula([FromForm] CrearPelicula crearPeliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (crearPeliculaDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_pelRepo.ExistePelicula(crearPeliculaDto.Nombre))
            {
                ModelState.AddModelError("", "La Pelicula ya existe");
                return StatusCode(404, ModelState);
            }
            var peliculas = _mapper.Map<Peliculas>(crearPeliculaDto);

            // if (!_pelRepo.crearPelicula(peliculas))
            // {
            //     ModelState.AddModelError("", $"Algo salio mal al agregar{peliculas.Nombre}");
            //     return StatusCode(404, ModelState);
            // }
            //subida de archivo
            if (crearPeliculaDto.Imagen != null)
            {
                string nombreArchivo = peliculas.Id + System.Guid.NewGuid().ToString() + Path.GetExtension(crearPeliculaDto.Imagen.FileName);
                string rutaArchivo = @"wwwroot\Imagenes\" + nombreArchivo;

                var UbicacionDirectorio = Path.Combine(Directory.GetCurrentDirectory(), rutaArchivo);
                FileInfo file = new FileInfo(UbicacionDirectorio);
                if (file.Exists)
                {
                    file.Delete();
                }
                using (var fileStream = new FileStream(UbicacionDirectorio, FileMode.Create))
                {
                    crearPeliculaDto.Imagen.CopyTo(fileStream);
                }
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                peliculas.RutaImagen = baseUrl + "/Imagenes/" + nombreArchivo;
                peliculas.RutaLocalImagen = rutaArchivo;

            }
            else
            {
                peliculas.RutaImagen = "https://placehold.co/600x400";
            }
            _pelRepo.crearPelicula(peliculas);
            return CreatedAtRoute("GetCategoria", new { categoriaId = peliculas.Id }, peliculas);
        }
        [HttpPatch("{peliculaId:int}", Name = "ActualizarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult ActualizarPelicula(int peliculaId, [FromForm] ActualizarPeliculaDto actualizarPeliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (actualizarPeliculaDto == null || peliculaId != actualizarPeliculaDto.Id)
            {
                return BadRequest(ModelState);
            }
            var pelicula = _mapper.Map<Peliculas>(actualizarPeliculaDto);

            // if (!_pelRepo.ActualizarPelicula(pelicula))
            // {
            //     ModelState.AddModelError("", $"Algo salio mal al Actualizar{pelicula.Nombre}");
            //     return StatusCode(500, ModelState);
            // }
            if (actualizarPeliculaDto.Imagen != null)
            {
                string nombreArchivo = pelicula.Id + System.Guid.NewGuid().ToString() + Path.GetExtension(actualizarPeliculaDto.Imagen.FileName);
                string rutaArchivo = @"wwwroot\Imagenes\" + nombreArchivo;

                var UbicacionDirectorio = Path.Combine(Directory.GetCurrentDirectory(), rutaArchivo);
                FileInfo file = new FileInfo(UbicacionDirectorio);
                if (file.Exists)
                {
                    file.Delete();
                }
                using (var fileStream = new FileStream(UbicacionDirectorio, FileMode.Create))
                {
                    actualizarPeliculaDto.Imagen.CopyTo(fileStream);
                }
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                pelicula.RutaImagen = baseUrl + "/Imagenes/" + nombreArchivo;
                pelicula.RutaLocalImagen = rutaArchivo;

            }
            else
            {
                pelicula.RutaImagen = "https://placehold.co/600x400";
            }
            _pelRepo.ActualizarPelicula(pelicula);
            return NoContent();
            // return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);
        }
        //[Authorize(Roles ="Admin")]
        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult BorrarRegistro(int peliculaId)
        {
            if (!_pelRepo.ExistePelicula(peliculaId))
            {
                return NotFound();
            }
            var pelicula = _pelRepo.GetPelicula(peliculaId);
            if (!_pelRepo.BorraPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal al borar el registro{pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpGet("GetPeliculasCategoria/{categoriaId:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetPeliculasCategoria(int categoriaId)
        {
            try
            {
                var listaPeliculas = _pelRepo.GetPeliculasEnCategorias(categoriaId);
                if (listaPeliculas == null || !listaPeliculas.Any())
                {
                    return NotFound($"No se encontraron peliculas en la Categoria ID {categoriaId}.");
                }
                var itemPelicula = listaPeliculas.Select(pelicula => _mapper.Map<PeliculaDto>(pelicula)).ToList();
                // foreach (var pelicula in listaPeliculas)
                // {
                //     itemPelicula.Add(_mapper.Map<PeliculaDto>(pelicula));
                // }
                 return Ok(itemPelicula);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Datos al recuperar la informacion");

            }
        }
        [HttpGet("BuscarPelicula")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BuscarPelicula(string nombre)
        {
            try
            {
                var resultado = _pelRepo.BuscarPelicula(nombre);
                if (!resultado.Any())
                {
                    return NotFound($"No se encontraron peliculas que coincidan con la busqueda.");
                }
                var peliculasDto = _mapper.Map<IEnumerable<PeliculaDto>>(resultado);
                return Ok(peliculasDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando los datos de la informacion");
            }
        }
    }
}
