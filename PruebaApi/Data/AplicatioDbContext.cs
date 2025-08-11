using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PruebaApi.Modelos;

namespace PruebaApi.Data
{
    public class AplicatioDbContext : IdentityDbContext<AppUsuario>
    {

        public AplicatioDbContext(DbContextOptions<AplicatioDbContext> options) : base(options)
        {           
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        //Aqui va todas las entidades (Modelos)
        public DbSet<Categoria> Categorias {get; set;} 
        public DbSet<Peliculas> Pelicula {get; set;} 
        public DbSet<Usuario> Usuarios {get; set;}
        public DbSet<AppUsuario> AppUsuarios {get; set;}
    }
}