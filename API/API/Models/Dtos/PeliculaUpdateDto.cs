using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static API.Models.Pelicula;

namespace API.Models.Dtos
{
    public class PeliculaUpdateDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El id es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]    
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La descripción es obligatoria")]  
        public string Duracion { get; set; }
        [Required(ErrorMessage = "La duración es obligatoria")]
        public TipoClasificacion Clasificacion { get; set; }
        public int categoriaId { get; set; }
    }
}
