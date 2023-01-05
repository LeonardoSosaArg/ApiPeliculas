using API.Data;
using API.Models;
using API.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace API.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public CategoriaRepository(ApplicationDbContext dbcontext)
        {
              _dbcontext = dbcontext;
        }
        public bool CrearCategoria(Categoria categoria)
        {
            _dbcontext.Categorias.Add(categoria);
            return Guardar();
        }

        public bool DeleteCategoria(Categoria categoria)
        {
            _dbcontext.Categorias.Remove(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(string nombre)
        {
            bool valor = _dbcontext.Categorias.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public bool ExisteCategoria(int id)
        {
            bool valor = _dbcontext.Categorias.Any(c => c.Id == id);
            return valor;
        }

        public Categoria GetCategoria(int id)
        {
            return _dbcontext.Categorias.FirstOrDefault(c => c.Id == id);
        }

        public ICollection<Categoria> GetCategorias()
        {

            return _dbcontext.Categorias.OrderBy(c => c.Nombre).ToList();
        }

        public bool Guardar()
        {
            return _dbcontext.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateCategoria(Categoria categoria)
        {
            _dbcontext.Categorias.Update(categoria);
            return Guardar();
        }
    }
}
