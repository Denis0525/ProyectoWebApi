using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Webapi.Models;
using Webapi.Models.ViewModels;
using Webapi.Repositorio.IRepositorio;
using Webapi.Controllers;
using Webapi.Views.Utilidades;
using Microsoft.AspNetCore.Authorization;

namespace Webapi.Controllers
{
     [Authorize]
    public class PeliculasController : Controller
    {
        private readonly IPeliculasRepositorio _repoPeliculas;
        private readonly ICategoriaRepositorio _repoCategoria;

        public PeliculasController(IPeliculasRepositorio repoPeliculas, ICategoriaRepositorio repoCategoria)
        {
            _repoPeliculas = repoPeliculas;
            _repoCategoria = repoCategoria;

        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(new Pelicula() { });
        }
        // [HttpGet]
        // public async Task<IActionResult> GetTodoPeliculas()
        // {
        //     return Json(new { data = await _repoPeliculas.GetPeliculasTodoAsync(CT.RutaPeliculasApi) });
        // }
        [HttpGet]
        public async Task<IActionResult> GetTodoPeliculas()
        {
            var resultado = await _repoPeliculas.GetPeliculasTodoAsync(CT.RutaPeliculasApi);
            return Json(new { data = resultado.Items });
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            IEnumerable<Categoria> ctList = (IEnumerable<Categoria>)await _repoCategoria.GetTodoAsync(CT.RutaCategoriasApi);
            PeliculasVM objVM = new PeliculasVM()
            {
                ListaCategorias = ctList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                }),
                Pelicula = new Pelicula()
            };
            return View(objVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pelicula pelicula)
        {
            IEnumerable<Categoria> ctList = (IEnumerable<Categoria>)
            await _repoCategoria.GetTodoAsync(CT.RutaCategoriasApi);

            PeliculasVM objVM = new PeliculasVM()
            {
                ListaCategorias = ctList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                }),
                Pelicula = new Pelicula()
            };
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {

                    pelicula.Imagen = files[0];//Asignar el InforFile directamente
                }
                else
                {
                    return View(objVM);
                }
                await _repoPeliculas.CrearPeliculaAsync(CT.RutaPeliculasApi, pelicula, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));
            }
            return View(objVM);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            IEnumerable<Categoria> ctList = (IEnumerable<Categoria>)await _repoCategoria.GetTodoAsync(CT.RutaCategoriasApi);
            PeliculasVM objVM = new PeliculasVM()
            {
                ListaCategorias = ctList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                }),
                Pelicula = new Pelicula()
            };
            if (id == null)
            {
                return NotFound();
            }
            //para mostrar los datos  en el formulario Edit
            objVM.Pelicula = await _repoPeliculas.GetAsync(CT.RutaPeliculasApi, id.GetValueOrDefault());
            if (objVM.Pelicula == null)
            {
                return NotFound();
            }
            return View(objVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Pelicula pelicula)
        {
            IEnumerable<Categoria> ctList = (IEnumerable<Categoria>)
            await _repoCategoria.GetTodoAsync(CT.RutaCategoriasApi);

            PeliculasVM objVM = new PeliculasVM()
            {
                ListaCategorias = ctList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                }),
                Pelicula = new Pelicula()
            };

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {

                    pelicula.Imagen = files[0];//Asignar el InforFile directamente
                }
                else
                {
                    return View(objVM);
                }
                await _repoPeliculas.ActualizarPeliculaAsync(CT.RutaPeliculasApi + pelicula.Id, pelicula, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));
            }
            return View(objVM);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _repoPeliculas.BorrarAsync(CT.RutaPeliculasApi, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Borrado Correctamente" });
            }
            return Json(new { success = false, message = "Borrado Corretamente" });
        }
        [HttpGet]
        public async Task<IActionResult> GetPeliculasEnCategorias(int id)
        {
            return Json(new { data = await _repoPeliculas.GetPeliculaEnCategoriaAsync(CT.RutaPeliculasEnCategoria, id) });
        }
    }
}