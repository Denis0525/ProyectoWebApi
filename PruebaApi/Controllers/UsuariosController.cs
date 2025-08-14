using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaApi.Modelos;
using PruebaApi.Repositorios.IRepositorio;

namespace PruebaApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/usuario")]
    // [ApiVersion("1.0")]
    [ApiVersionNeutral]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usuRepo;
        protected RespuestaAPI _respuestaAPI;
        private readonly IMapper _mapper;

        public UsuariosController(IUsuarioRepositorio usuRepo, IMapper mapper)
        {
            _usuRepo = usuRepo;
            _mapper = mapper;
            this._respuestaAPI = new();
        }
        [HttpGet]
        public ActionResult GetUsuarios()
        {
            var listaUsuarios = _usuRepo.GetUsuarios();
            var listaUsuariosDto = new List<UsuarioDto>();
            foreach (var lista in listaUsuarios)
            {
                listaUsuariosDto.Add(_mapper.Map<UsuarioDto>(lista));
            }
            return Ok(listaUsuariosDto);
        }
        //[Authorize(Roles ="Admin")]
        [HttpGet("{usuarioId}", Name = "GetUsuario")]
        [ResponseCache(CacheProfileName ="Por defecto 30 segundos")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsuario(string usuarioId)
        {
            var itemUsuario = _usuRepo.GetUsuario(usuarioId);
            if (itemUsuario == null)
            {
                return NotFound();
            }
            var itemUsuarioDto = _mapper.Map<PeliculaDto>(itemUsuario);
            return Ok(itemUsuarioDto);
        }
        [HttpPost("Registro")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registro([FromBody] UsuarioRegistroDto usuarioRegistroDto)
        {
            bool ValidarnombreUnico = _usuRepo.IsUniqueUser(usuarioRegistroDto.NombreUsuario);
            
            if (!ValidarnombreUnico)
            {
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMessages.Add("El nombre del usuario ya existe");
                return BadRequest(_respuestaAPI);
            }
            var usuarios = await _usuRepo.Registro(usuarioRegistroDto);
            if (usuarios == null)
            {   
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMessages.Add("Error en el registro");
                return BadRequest(_respuestaAPI);
            }
            _respuestaAPI.StatusCode = HttpStatusCode.OK;
            _respuestaAPI.IsSuccess = true;
            return Ok(_respuestaAPI);
        }
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto usuarioLoginDto)      
        {
            try
            { 
            var respuestaLogin = await _usuRepo.Login(usuarioLoginDto);
          
            if (respuestaLogin.Usuario == null || string.IsNullOrEmpty(respuestaLogin.Token))
            {
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMessages.Add("El nombre del usuario o el password son incorrectos");
                return BadRequest(_respuestaAPI);
            }
                _respuestaAPI.StatusCode = HttpStatusCode.OK;
                _respuestaAPI.IsSuccess = true;
                _respuestaAPI.Result = respuestaLogin;

                return Ok(_respuestaAPI);
            }
         catch(Exception)
         {
             return StatusCode(StatusCodes.Status500InternalServerError,"Eroror");

         }
        }
    }
}
