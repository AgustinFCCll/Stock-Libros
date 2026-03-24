using Microsoft.AspNetCore;
using Libros.Services;
using Libros.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Libros.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly ILibroServices service;

        public LibrosController(ILibroServices service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var libros = await service.GetAll();
            return Ok(libros);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var libro = await service.Get(id);
            
            if(libro == null)
            {
                return NotFound();
            }
            return Ok(libro);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLibroDTO createDto)
        {
            await service.Create(createDto);
            return CreatedAtAction(nameof(Get), new { id = 0 }, createDto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLibroDTO updateDto)
        {
            updateDto.Id = id;
            var existeLibro = await service.Get(id);
            if (existeLibro == null)
            {
                return NotFound();
            }

            await service.Update(updateDto);
            return NoContent();
        }

        
        [HttpDelete("{id:int}")] // Eliminar
        public async Task<IActionResult> Delete (int id)
        {
            await service.Delete(id);
            return Ok();
        }

        [HttpPut("{id:int}/prestar")]
        public async Task<IActionResult> TogglePrestado(int id)
        {
            var resultado = await service.TogglePrestado(id);
            if (!resultado) return NotFound();
            return Ok();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter([FromQuery] bool? prestado, [FromQuery] bool? leido)
        {
            var libros = await service.GetByFilter(prestado, leido);
            return Ok(libros);
        }

        [HttpPut("{id:int}/leido")]
        public async Task<IActionResult> ToggleLeido(int id)
        {
            var resultado = await service.ToggleLeido(id);
            if (!resultado) return NotFound();
            return Ok();
        }
    }
}
