using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PruebaApi.Data;
using PruebaApi.Modelos;
using PruebaApi.Modelos.Dtos;

using PruebaApi.Repositorios.IRepositorio;

namespace PruebaApi.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio

    {
        private readonly AplicatioDbContext _bd;
        private string claveSecreta;
        private readonly UserManager<AppUsuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UsuarioRepositorio(AplicatioDbContext bd, IConfiguration confg, UserManager<AppUsuario> userManager, RoleManager<IdentityRole> roleManager,IMapper mapper)
        {
            _bd = bd;
            claveSecreta = confg.GetValue<string>("ApiSettings:Secreta");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public AppUsuario GetUsuario(string usuarioId)
        {
            return _bd.AppUsuarios.FirstOrDefault(c => c.Id == usuarioId);
        }

        public ICollection<AppUsuario> GetUsuarios()
        {
            return _bd.AppUsuarios.OrderBy(c => c.UserName).ToList();
        }

        public bool IsUniqueUser(string usuario)
        {
            var usuarioBd = _bd.AppUsuarios.FirstOrDefault(u => u.UserName == usuario);
            if (usuarioBd == null)
            {
                return true;
            }
            return false;
        }
    

        public async Task<UsuariosRepuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            //var passwordEncriptado = obtenermd5(usuarioLoginDto.Password);
            
            var usuario = _bd.AppUsuarios.FirstOrDefault(
               u => u.UserName.ToLower() == usuarioLoginDto.NombreUsuario.ToLower());
               bool isValid = await _userManager.CheckPasswordAsync(usuario, usuarioLoginDto.Password);
            //Validamos si el usuario no existe  con la combinacion de usuario y contraseña correcta
            if (usuario == null || isValid == false)
            {
                return new UsuariosRepuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }
            //Aqui existe el usuario entonces podemos procesar el login
            var roles = await _userManager.GetRolesAsync(usuario);
            var manejadoToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);
            var tokenDescriptor = new SecurityTokenDescriptor
            
            {
                Subject = new ClaimsIdentity(new Claim[]
                  {
                 new Claim(ClaimTypes.Name, usuario.UserName.ToString()),
                 new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                 }),
                 Expires = DateTime.UtcNow.AddDays(4),
                 SigningCredentials = new (new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadoToken.CreateToken(tokenDescriptor);

            UsuariosRepuestaDto usuariosRepuestaDto = new UsuariosRepuestaDto()
            {
                Token = manejadoToken.WriteToken(token),
         
                Usuario = _mapper.Map<UsuariosDatosDto>(usuario),
            };
            return usuariosRepuestaDto;
        }

        public async Task<UsuariosDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
           // var passwordEncriptado = obtenermd5(usuarioRegistroDto.Password);
            AppUsuario usuario = new AppUsuario()
            {
                UserName = usuarioRegistroDto.NombreUsuario,
                Email = usuarioRegistroDto.NombreUsuario,
                NormalizedEmail = usuarioRegistroDto.NombreUsuario.ToUpper(),
                Nombre = usuarioRegistroDto.Nombre
            };
            var result = await _userManager.CreateAsync(usuario,usuarioRegistroDto.Password);
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("Registro"));
                }
                await _userManager.AddToRoleAsync(usuario, "Admin");
                var usuarioRetornado = _bd.AppUsuarios.FirstOrDefault(u => u.UserName == usuarioRegistroDto.NombreUsuario);
                return _mapper.Map<UsuariosDatosDto>(usuarioRetornado);
            }

        //     _bd.Usuarios.Add(usuario);
        //    // _bd.SaveChanges();
        //     await _bd.SaveChangesAsync();
        //     usuario.Password = passwordEncriptado;
        //     return usuario;
        return new UsuariosDatosDto();
        }
        //Metodo para encriptar la constraseña  con MD5 se usatanto coo en el acceso  como en el registro
        // public static string obtenermd5(string valor)
        // {
        //    // MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
        //    MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
        //     byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
        //     data = x.ComputeHash(data);
        //     string rep = "";
        //     for (int i = 0; i < data.Length; i++)
        //         rep += data[i].ToString("x2").ToLower();
        //     return rep;
        // }
    }

}