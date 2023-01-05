using API.Models;
using API.Models.Dtos;
using API.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers

{
    [Authorize]
    [Route("api/Peliculas")]
    [ApiController]
    public class PeliculasController : Controller
    {

        private readonly IPeliculaRepository _peliculaRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostingEnviroment;

        public PeliculasController(IPeliculaRepository peliculaRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _peliculaRepository = peliculaRepository;
            _mapper = mapper;
            _webHostingEnviroment = webHostEnvironment;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Getpeliculas()
        {
            //USAR ESTE
            var listaPeliculas = _peliculaRepository.GetPeliculas();

            var listaPeliculasDto = new List<PeliculaDto>();
            foreach (var item in listaPeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(item));
            }

            return Ok(listaPeliculasDto);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}", Name = "GetPelicula")]
        public IActionResult GetPelicula(int id)
        {
            var itemPelicula = _peliculaRepository.GetPelicula(id);
            if (itemPelicula == null)
            {
                return NotFound();
            }

            var itemPeliculaDto = _mapper.Map<PeliculaDto>(itemPelicula);
            return Ok(itemPeliculaDto);
        }


        [HttpPost]
        public IActionResult PostPelicula([FromForm] PeliculaCreateDto peliculaCreateDto)
        {

            if (peliculaCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_peliculaRepository.ExistePelicula(peliculaCreateDto.Nombre))
            {
                ModelState.AddModelError("", "La Pelicula ya existe!");
                return StatusCode(404, ModelState);
            }

            //FUNCIONALIDAD SUBIDA DE ARCHIVOS
            var archivo = peliculaCreateDto.Foto;
            string rutaImagen = _webHostingEnviroment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            if (archivo.Length > 0)
            {
                //nueva imagen
                var nombreFoto = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaImagen,@"fotos");
                var extension = Path.GetExtension(archivos[0].FileName);

                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreFoto + extension ),FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }
                peliculaCreateDto.RutaImagen = @"\fotos\" + nombreFoto + extension;
            }


          
            var pelicula = _mapper.Map<Pelicula>(peliculaCreateDto);

            if (!_peliculaRepository.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal en la alta del Registro {pelicula.Nombre}.");
                return StatusCode(404, ModelState);
            }

            //return Ok();

            return CreatedAtRoute("GetPelicula", new { id = pelicula.Id}, pelicula);
        }

        [HttpPatch("{PeliculaId:int}", Name = "UpdatePelicula")]
        public IActionResult UpdatePelicula(int PeliculaId, [FromBody] PeliculaUpdateDto PeliculaDto)
        {

            if (PeliculaDto == null || PeliculaId != PeliculaDto.Id)
            {
                return BadRequest(ModelState);
            }

            var pelicula = _mapper.Map<Pelicula>(PeliculaDto);

            if (!_peliculaRepository.UpdatePelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{PeliculaId:int}", Name = "DeletePelicula")]
        public IActionResult DeletePelicula(int PeliculaId)
        {

            if (!_peliculaRepository.ExistePelicula(PeliculaId))
            {
                return NotFound();
            }

            var pelicula = _peliculaRepository.GetPelicula(PeliculaId);

            //SI NO EXISTE
            if (!_peliculaRepository.DeletePelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("GetPeliculasPorCategoria/{idCategoria:int}")]
        public IActionResult GetPeliculasPorCategoria(int idCategoria)
        {
            var listaPeliculas = _peliculaRepository.GetPeliculasPorCategoria(idCategoria);
            if (listaPeliculas == null)
            {
                return NotFound();
            }

            var itemPeliculaDto = new List<PeliculaDto>();
            foreach (var peli in listaPeliculas)
            {
                itemPeliculaDto.Add(_mapper.Map<PeliculaDto>(peli));
            }

            return Ok(itemPeliculaDto);
        }

        [AllowAnonymous]
        [HttpGet("Buscar")]
        public IActionResult Buscar(string nombre)
        {
            try
            {
               var itemPeliculaDto = _peliculaRepository.BuscarPelicula(nombre);
                if (itemPeliculaDto.Any())
                {
                    return Ok(itemPeliculaDto);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Error al obtener pelicula.");
            }
           
        }

    }
}
