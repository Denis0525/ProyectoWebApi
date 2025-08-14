using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Webapi.Models;


namespace Webapi.Repositorio.IRepositorio
{
    public class PeliculasRepositorio : Repositorio<Pelicula>, IPeliculasRepositorio
    {
        //Inyeccio de dependencias se debe importar el IHttpClientFactory
        private readonly IHttpClientFactory _clienteFactory;
        public PeliculasRepositorio(IHttpClientFactory clienteFactory) : base(clienteFactory)
        {
            _clienteFactory = clienteFactory;
        }

        // //Version mejorada para so´porte de la paginacion
        // public async Task<IEnumerable<Pelicula>> GetPeliculasTodoAsync(string url)
        // {
        //    var peticion = new HttpRequestMessage(HttpMethod.Get, url);
        //    var cliente = _clienteFactory.CreateClient();

        //    HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

        //    if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        var jsonString = await respuesta.Content.ReadAsStringAsync();
        //        // Deserializar a PeliculaResponse
        //        var peliculaResponse = JsonConvert.DeserializeObject<PeliculaResponse>(jsonString);
        //        // Devolver la lista de películas
        //        return peliculaResponse?.Items ?? new List<Pelicula>();
        //    }
        //    else
        //    {
        //        return new List<Pelicula>();
        //    }
        // }
        // public async Task<PeliculaResponse> GetPeliculasTodoAsync(string url)
        // {
        //     var peticion = new HttpRequestMessage(HttpMethod.Get, url);
        //     var cliente = _clienteFactory.CreateClient();

        //     HttpResponseMessage respuesta = await cliente.SendAsync(peticion);
        //     if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
        //     {
        //         var jsonString = await respuesta.Content.ReadAsStringAsync();
        //         //deserealiza a peliculaResponse
        //         var peliculaResponse = JsonConvert.DeserializeObject<PeliculaResponse>(jsonString);
        //         //devover la lista de peliculas 
        //         return peliculaResponse ?? new PeliculaResponse();
        //     }
        //     else
        //     {
        //         return new PeliculaResponse();
        //     }
        // }
        
        public async Task<PeliculaResponse> GetPeliculasTodoAsync(string url)
        {
           var peticion = new HttpRequestMessage(HttpMethod.Get, url);
           var cliente = _clienteFactory.CreateClient();

           HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

           if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
           {
               var jsonString = await respuesta.Content.ReadAsStringAsync();
               // Deserializar a PeliculaResponse
               var peliculaResponse = JsonConvert.DeserializeObject<PeliculaResponse>(jsonString);
               // Devolver la lista de películas
               return peliculaResponse?? new PeliculaResponse();
           }
           else
           {
               return new PeliculaResponse();
           }
        }
    }
}