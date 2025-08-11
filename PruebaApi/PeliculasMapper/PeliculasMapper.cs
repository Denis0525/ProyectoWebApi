using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PruebaApi.Modelos;
using PruebaApi.Modelos.Dtos;

namespace PruebaApi.PeliculasMapper
{
    public class PeliculasMapper : Profile
    {
        public PeliculasMapper()
        {
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, CrearCategoria>().ReverseMap();
            CreateMap<Peliculas, PeliculaDto>().ReverseMap();
            CreateMap<Peliculas, CrearPelicula>().ReverseMap();
            CreateMap<Peliculas, ActualizarPeliculaDto>().ReverseMap();
            CreateMap<AppUsuario, UsuariosDatosDto>().ReverseMap();
            CreateMap<AppUsuario, UsuarioDto>().ReverseMap();
        }
        }
    }