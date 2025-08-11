using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PruebaApi.Data;
using PruebaApi.Modelos;
using PruebaApi.Repositorios.IRepositorio;

namespace PruebaApi.Repositorios
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly AplicatioDbContext _bd;

        public CategoriaRepositorio(AplicatioDbContext bd)
        {
            _bd = bd;
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = categoria.FechaCreacion.ToUniversalTime();
            //Arreglar problema PUT
            var categoriaExistente = _bd.Categorias.Find(categoria.Id);
            if (categoriaExistente != null)
            {
                _bd.Entry(categoriaExistente).CurrentValues.SetValues(categoria);
            }
            else
            {
            _bd.Categorias.Update(categoria);
            }
            return Guardar();
        }

        public bool BorraCategoria(Categoria categoria)
        {
            _bd.Categorias.Remove(categoria);
            return Guardar();
        }

        public bool crearCategorias(Categoria categoria)
        {
            categoria.FechaCreacion = categoria.FechaCreacion.ToUniversalTime();
            //categoria.FechaCreacion = DateTime.Now;
            _bd.Categorias.Add(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(string nombre)
        {
            bool valor = _bd.Categorias.Any(c => c.Name.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public bool ExisteCategoria(int id)
        {
            return _bd.Categorias.Any(c => c.Id == id);
        }

        public Categoria GetCategoria(int CategoriaId)
        {
            return _bd.Categorias.FirstOrDefault(c => c.Id == CategoriaId);
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _bd.Categorias.OrderBy(c => c.Name).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}