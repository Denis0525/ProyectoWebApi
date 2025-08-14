using System;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Webapi.Models;
using Webapi.Models.ViewModels;
using Webapi.Repositorio.IRepositorio;
using Webapi.Views.Utilidades;

namespace Webapi.Controllers;
public class HomeController : Controller
{
    private readonly IAccountRepositorio _accRepo;
    private readonly ICategoriaRepositorio _repoCategoria;
    private readonly IPeliculasRepositorio _repoPelicula;
    public HomeController(IAccountRepositorio accRepo, ICategoriaRepositorio repoCategoria, IPeliculasRepositorio repoPelicula)
    {
        _accRepo = accRepo;
        _repoCategoria = repoCategoria;
        _repoPelicula = repoPelicula;
    }
    //V1 controller
    // [HttpGet]
    // public async Task<IActionResult> Index()
    // {
    //      IndexVM listaPeliculasCategorias = new IndexVM()
    //     {
    //         ListaCategorias = (IEnumerable<Categoria>)await _repoCategoria.GetTodoAsync(CT.RutaCategoriasApi),
    //         ListaPeliculas = (IEnumerable<Pelicula>)await _repoPelicula.GetPeliculasTodoAsync(CT.RutaPeliculasApi)
    //     };
    //     return View(listaPeliculasCategorias);
    // }

    //V2 soporte para paginacion
    [HttpGet]
    public async Task<IActionResult> Index(int page =1)
    {
        const int pageSize = 5; // o la pagina de tamaño que quieras 
        var url = $"{CT.RutaPeliculasApi}?pageNumber={page}&pageSize={pageSize}";
        var peliculaResponse = await _repoPelicula.GetPeliculasTodoAsync(url);
        IndexVM listaPeliculasCategorias = new IndexVM()
        {
            ListaCategorias = (IEnumerable<Categoria>)await _repoCategoria.GetTodoAsync(CT.RutaCategoriasApi),
            ListaPeliculas = peliculaResponse.Items,
            TotalPages = peliculaResponse.TotalPages,
            CurrentPage = page,
        };
        return View(listaPeliculasCategorias);
    }
    [HttpGet]
    public async Task<IActionResult> IndexCategoria(int id)
    {
        var pelisEnCategoria = await _repoPelicula.GetPeliculaEnCategoriaAsync(CT.RutaPeliculasEnCategoria, id);
        return View(pelisEnCategoria);
    }
    [HttpPost]
    public async Task<IActionResult> IndexBusqueda(string nombre)
    {
        var pelisEncontradas = await _repoPelicula.Buscar(CT.RutaPeliculasBusqueda, nombre);
        return View(pelisEncontradas);
    }
    [HttpGet]
    public IActionResult Login()
    {
        UsuarioAuth usuario = new UsuarioAuth();
        return View(usuario);
    }
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        //cierra la sesion de autentication
        await HttpContext.SignOutAsync();
        //limpia la sesion del usuario, incluyendo cualquier token
        HttpContext.Session.Clear();
        // Elimna la sesion de la cookie manualmente 
        if (Request.Cookies.ContainsKey(".AspNetCore.Session"))
        {
            Response.Cookies.Delete(".AspNetCore.Session");
        }
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult Registro()
    {
        UsuarioAuth usuario = new UsuarioAuth();
        return View(usuario);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registro(UsuarioAuth obj)
    {
        bool result = await _accRepo.RegisterAsync(CT.RutaUsuariosApi + "Registro", obj);
        if (result == false)
        {
            return View();
        }
        TempData["alert"] = "Registro Correcto";
        return RedirectToAction("Login");
    }
    [HttpPost]
    public async Task<IActionResult> Login(UsuarioAuth obj)
    {
        if (!ModelState.IsValid)
            return View(obj);
        UsuarioAuth objUser = await _accRepo.LoginAsync(CT.RutaUsuariosApi + "Login", obj);
        if (string.IsNullOrEmpty(objUser?.Token))
        {
            TempData["alert"] = "Los datos son incorrectos.";
            return View(obj);
        }
        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(ClaimTypes.Email, objUser.NombreUsuario));
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        HttpContext.Session.SetString("JWToken", objUser.Token);
        HttpContext.Session.SetString("Usuario", objUser.NombreUsuario);
        return RedirectToAction("Index");
    }
}