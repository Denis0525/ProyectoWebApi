using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PruebaApi.Modelos;
using PruebaApi.Modelos.Dtos;

namespace PruebaApi.Repositorios.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        ICollection<AppUsuario> GetUsuarios();
        AppUsuario GetUsuario(string usuarioId);
        bool IsUniqueUser(string usuario);
        Task<UsuariosRepuestaDto> Login(UsuarioLoginDto usuarioLoginDto);
        Task<UsuariosDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto);
    }
}