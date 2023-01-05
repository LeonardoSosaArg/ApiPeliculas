using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.IRepository
{
    public class PeliculaRepository : IPeliculaRepository
    {
        //INYECCION DE DEPENDENCIA DEL DB CONTEXT
        private readonly ApplicationDbContext _dbcontext;

        public PeliculaRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IEnumerable<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _dbcontext.Peliculas;
            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));
                
            }
            return query.ToList();
        }

        public bool Guardar()
        {
            return _dbcontext.SaveChanges() >= 0 ? true : false;
        }

        bool IPeliculaRepository.CrearPelicula(Pelicula pelicula)
        {
            _dbcontext.Add(pelicula);
            return Guardar();
        }

        bool IPeliculaRepository.DeletePelicula(Pelicula pelicula)
        {
            _dbcontext.Remove(pelicula);
            return Guardar();
        }

        bool IPeliculaRepository.ExistePelicula(string nombre)
        {
            bool valor = _dbcontext.Peliculas.Any(p => p.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        bool IPeliculaRepository.ExistePelicula(int id)
        {
            return _dbcontext.Peliculas.Any(p => p.Id.Equals(id));
        }

        Pelicula IPeliculaRepository.GetPelicula(int id)
        {
            return _dbcontext.Peliculas.FirstOrDefault( c => c.Id.Equals(id));
        }

        ICollection<Pelicula> IPeliculaRepository.GetPeliculas()
        {
            return _dbcontext.Peliculas.OrderBy(p => p.Nombre).ToList();
        }

        ICollection<Pelicula> IPeliculaRepository.GetPeliculasPorCategoria(int idCategoria)
        {
            return _dbcontext.Peliculas.Include(c => c.Categoria).Where(c => c.Id.Equals(idCategoria)).ToList();
        }

        bool IPeliculaRepository.UpdatePelicula(Pelicula pelicula)
        {
            _dbcontext.Update(pelicula);
            return Guardar();
        }

        public Pelicula GetPelicula(int peliculaId)
        {
            return _dbcontext.Peliculas.FirstOrDefault(p => p.Id == peliculaId);
        }
    }
}
