using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PruebaApi.Modelos;

namespace PruebaApi.Repositorios.IRepositorio
{
    public interface ICategoriaRepositorio
    {
        ICollection<Categoria> GetCategorias();
        Categoria GetCategoria(int CategoriaId);
        bool ExisteCategoria(int id);
        bool ExisteCategoria(string nombre);
        bool crearCategorias(Categoria categoria);
        bool ActualizarCategoria(Categoria categoria);
        bool BorraCategoria(Categoria categoria);
        bool Guardar();
    }
}