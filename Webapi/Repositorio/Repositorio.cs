using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapi.Repositorio.IRepositorio;
using System.Collections;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Webapi.Views;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json.Linq;


namespace Webapi.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        // inyeccion de dependecias se debe importar el httpClientetFactory
        private readonly IHttpClientFactory _clientFactory;

        public Repositorio(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IEnumerable<T>> GetTodoAsync(string url)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url);
            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            if (respuesta.IsSuccessStatusCode)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> ActualizarAsync(string url, T itemActualizar, string token = "")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Patch, url);
            //var multipartContent = new MultipartFormDataContent();
            if (itemActualizar != null)
            {
                peticion.Content = new StringContent(JsonConvert.SerializeObject(itemActualizar),
                Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var cliente = _clientFactory.CreateClient();
            //Aqui Valida Token
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            //Asigna el encabezado del token de autorizacion
             cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);
            // validar y regresar booleano
            if (respuesta.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            { return false; }
        }

        public async Task<bool> ActualizarPeliculaAsync(string url, T PeliculaActualizar, string token = "")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Patch, url);
            var multipartContent = new MultipartFormDataContent();

            if (PeliculaActualizar != null)
            {
                foreach (var property in typeof(T).GetProperties())
                {
                    var value = property.GetValue(PeliculaActualizar);
                    if (value != null)
                    {
                        if (property.PropertyType == typeof(IFormFile))
                        {
                            var file = value as IFormFile;
                            if (file != null)
                            {
                                var streamContent = new StreamContent(file.OpenReadStream());
                                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                                multipartContent.Add(streamContent, property.Name, file.FileName);
                            }
                        }
                        else
                        {
                            var stringContent = new StringContent(value.ToString());
                            multipartContent.Add(stringContent, property.Name);
                        }
                    }
                }
            }
            else
            {
                return false;
            }
            peticion.Content = multipartContent;
            var cliente = _clientFactory.CreateClient();
               //Aqui Valida Token
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            //Asigna el encabezado del token de autorizacion
             cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);
            // validar y regresar booleano
            if (respuesta.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            { return false; }
        }


        public async Task<bool> BorrarAsync(string url, int Id, string token = "")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Delete, url + Id);
            //var multipartContent = new MultipartFormDataContent();
            var cliente = _clientFactory.CreateClient();
            //Aqui Valida Token
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            //Asigna el encabezado del token de autorizacion
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);
            // validar y regresar booleano
            if (respuesta.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            { return false; }
        }

        public async Task<IEnumerable<T>> Buscar(string url, string nombre)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url + nombre);
            var cliente = _clientFactory.CreateClient();
            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);
            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                // return JsonConvert.DeserializeObject<T>(jsonString);
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> CrearAsync(string url, T itemCrear, string token = "")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Post, url);
            if (itemCrear != null)
            {
                peticion.Content = new StringContent(JsonConvert.SerializeObject(itemCrear),
                Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }

            var cliente = _clientFactory.CreateClient();
               //Aqui Valida Token
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            //Asigna el encabezado del token de autorizacion
             cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);
            //validar si se actualizo y regresar boleano
            if (respuesta.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CrearPeliculaAsync(string url, T peliculaCrear, string token = "")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Post, url);
            var multipartContent = new MultipartFormDataContent();
            if (peliculaCrear != null)
            {
                foreach (var property in typeof(T).GetProperties())
                {
                    var value = property.GetValue(peliculaCrear);
                    if (value != null)
                    {
                        if (property.PropertyType == typeof(IFormFile))
                        {
                            var file = value as IFormFile;
                            if (file != null)
                            {
                                var streamContent = new StreamContent(file.OpenReadStream());
                                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                                multipartContent.Add(streamContent, property.Name, file.FileName);

                            }
                        }
                        else
                        {
                            var stringContent = new StringContent(value.ToString());
                            multipartContent.Add(stringContent, property.Name);
                        }
                    }
                }
            }
            else
        {
                return false;
            }
            peticion.Content = multipartContent;
            var cliente = _clientFactory.CreateClient();
              //Aqui Valida Token
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            //Asigna el encabezado del token de autorizacion
             cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);
            //validar si se actualizo y regresar boleano
            if (respuesta.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<T> GetAsync(string url, int Id)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url + Id);
            var cliente = _clientFactory.CreateClient();
            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);
            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            else
            {
                return null;
            }
        }
        public async Task<IEnumerable> GetPeliculaEnCategoriaAsync(string url, int categoriaId)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url + categoriaId);
            var cliente = _clientFactory.CreateClient();
            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);
            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        }
    }
}
