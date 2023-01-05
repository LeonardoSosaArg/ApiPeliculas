using API.Models;
using System.Collections;
using System.Collections.Generic;

namespace API.Repository.IRepository
{
    public interface IPeliculaRepository
    {
        ICollection<Pelicula> GetPeliculas();
        ICollection<Pelicula> GetPeliculasPorCategoria(int idCategoria);
        IEnumerable<Pelicula> BuscarPelicula(string nombre);
        Pelicula GetPelicula(int id);
        bool ExistePelicula(string nombre);
        bool ExistePelicula(int id);
        bool CrearPelicula(Pelicula pelicula);
        bool UpdatePelicula(Pelicula pelicula);
        bool DeletePelicula(Pelicula pelicula);
        bool Guardar();


    }
}
