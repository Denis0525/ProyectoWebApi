using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Webapi.Models;
using Webapi.Repositorio.IRepositorio;
using Webapi.Views.Utilidades;

namespace Webapi.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly IUsuarioRepositorio _repoUsuario;

        public UsuariosController(IUsuarioRepositorio repoUsuario)
        {
            _repoUsuario = repoUsuario;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(new Usuario() { });
        }
        [HttpGet]
        public async Task<IActionResult> GetTodosUsuarios()
        {
            return Json(new { data = await _repoUsuario.GetTodoAsync(CT.RutaUsuariosApi) });
        }
    }
}