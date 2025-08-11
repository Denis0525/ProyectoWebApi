using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Webapi.Models;
using Webapi.Repositorio.IRepositorio;

namespace Webapi.Repositorio
{
    public class AccountRepositorio : Repositorio<UsuarioAuth>, IAccountRepositorio
    {
        //inyeccion de dependecias  se       debe importar  el IHttClientFactory
        private readonly IHttpClientFactory _clientFactory;
        public AccountRepositorio(IHttpClientFactory clientFactory): base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<UsuarioAuth> LoginAsync(string url, UsuarioAuth itemCrear)
        {
            try
            {
                if (itemCrear == null)
                    return new UsuarioAuth();

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(itemCrear), Encoding.UTF8, "application/json")
                };

                var cliente = _clientFactory.CreateClient();

                HttpResponseMessage respuesta = await cliente.SendAsync(request);

                if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonString = await respuesta.Content.ReadAsStringAsync();

                    Console.WriteLine("Respuesta JSON: " + jsonString); // Verifica qué llega

                    var usuarioAuthRespuesta = JsonConvert.DeserializeObject<UsuarioAuthRespuesta>(jsonString);

                    var usuarioAuth = new UsuarioAuth
                    {
                        Id = usuarioAuthRespuesta.Result.Usuario.Id,
                        NombreUsuario = usuarioAuthRespuesta.Result.Usuario.UserName,
                        Nombre = usuarioAuthRespuesta.Result.Usuario.Nombre,
                        Token = usuarioAuthRespuesta.Result.Token
                    };

                    Console.WriteLine($"Token Recibido: {usuarioAuth.Token}");
                    return usuarioAuth;
                }
                else
                {
                    var errorContent = await respuesta.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {respuesta.StatusCode} - {errorContent}");
                    return new UsuarioAuth();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception en LoginAsync: " + ex.Message);
                return new UsuarioAuth();
            }
        }

        public async Task<bool> RegisterAsync(string url, UsuarioAuth itemCrear)
        {
              var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (itemCrear != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(itemCrear), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
           var cliente = _clientFactory.CreateClient();
            HttpResponseMessage respuesta = await cliente.SendAsync(request);
            //validar si se actualizo y regresar boleano
            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}