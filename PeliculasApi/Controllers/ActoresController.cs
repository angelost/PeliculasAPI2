using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOs;
using PeliculasApi.Entidades;
using PeliculasApi.Helpers;
using PeliculasApi.Servicios;

namespace PeliculasApi.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController: CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "actores";

        public ActoresController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos): base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos; 
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            //var queryable = context.Actores.AsQueryable();
            //await HttpContext.InsertarParametroPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);
            //var entidades = await queryable.Paginar(paginacionDTO).ToListAsync();
            //return mapper.Map<List<ActorDTO>>(entidades);

            return await Get<Actor, ActorDTO>(paginacionDTO);
        }

        [HttpGet("{id}", Name = "obtenerActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var entidad = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if (entidad == null) { return NotFound(); }
            var entidadDTO = mapper.Map<ActorDTO>(entidad);
            entidadDTO.FechaFormateada = entidadDTO.FechaNacimiento.ToShortDateString();
            return entidadDTO;

            //return await Get<Actor, ActorDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var entidad = mapper.Map<Actor>(actorCreacionDTO);

            if (actorCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                    entidad.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor, actorCreacionDTO.Foto.ContentType);
                }
            }

            context.Add(entidad);
            await context.SaveChangesAsync();
            var dto = mapper.Map<ActorDTO>(entidad);
            return new CreatedAtRouteResult("obtenerActor", new { id = entidad.Id }, dto);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument)
        {
            //if (patchDocument == null) { return BadRequest(); }
            //var entidadDB = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            //if (entidadDB == null) { return NotFound(); }
            //var entidadDTO = mapper.Map<ActorPatchDTO>(entidadDB);
            //patchDocument.ApplyTo(entidadDTO, ModelState);
            //var esValido = TryValidateModel(entidadDTO);
            //if (!esValido) { return BadRequest(ModelState); }
            //mapper.Map(entidadDTO, entidadDB);
            //await context.SaveChangesAsync();
            //return NoContent();

            return await Patch<Actor, ActorPatchDTO>(id, patchDocument);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ActorCreacionDTO actorCreacionDTO)
        {
            var actorDB = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if (actorDB == null) { return NotFound(); }

            actorDB = mapper.Map(actorCreacionDTO, actorDB);

            if (actorCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                    actorDB.Foto = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor, actorDB.Foto, actorCreacionDTO.Foto.ContentType);
                }
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            //var existe = await context.Actores.AnyAsync(x => x.Id == id);
            //if (!existe) { return NotFound(); }
            //context.Remove(new Actor() { Id = id });
            //await context.SaveChangesAsync();
            //return NoContent();

            return await Delete<Actor>(id);
        }
    }
}
