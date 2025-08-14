using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace Webapi.Views.Utilidades
{
    public class CT
    {
        public static string UrlBaseApi = "http://localhost:5196/";
        public static string RutaCategoriasApi = UrlBaseApi + "api/v1/categorias/";
        public static string RutaPeliculasApi = UrlBaseApi + "api/v1/peliculas/";
        public static string RutaUsuariosApi = UrlBaseApi + "api/v1/usuario/";


        //Faltan otras rutas para buscar  y filtar peliculas por categorias
        public static string RutaPeliculasEnCategoria = UrlBaseApi + "api/v1/peliculas/GetPeliculasCategoria/";
        public static string RutaPeliculasBusqueda = UrlBaseApi + "api/v1/peliculas/BuscarPelicula?nombre=";

        
    }
}