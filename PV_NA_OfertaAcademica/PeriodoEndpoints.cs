using Dapper;
using PV_NA_OfertaAcademica.Dtos;
using PV_NA_OfertaAcademica.Entities;
using PV_NA_OfertaAcademica.Services;
using System.Data;

namespace PV_NA_OfertaAcademica.Controllers
{
    public static class PeriodoEndpoints
    {
        // Agrupa y mapea los endpoints relacionados con "Periodo"
        public static void MapPeriodoEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/periodo")
                           .WithOpenApi()
                           .WithTags("Periodo");
                          // .RequireAuthorization(); // requiere token JWT validado con USR5

            //  Obtener todos los periodos
            group.MapGet("/", async (PeriodoService service) =>
            {
                var periodos = await service.ObtenerTodosAsync();
                return Results.Ok(periodos);
            })
            .WithSummary("Obtiene todos los periodos")
            .WithDescription("Retorna la lista completa de periodos registrados.");

            //  Obtener periodo por ID
            group.MapGet("/{id}", async (int id, PeriodoService service) =>
            {
                var periodo = await service.ObtenerPorIdAsync(id);
                return periodo is null ? Results.NotFound("Periodo no encontrado") : Results.Ok(periodo);
            })
            .WithSummary("Obtiene un periodo por ID");

            // 🟢 Crear nuevo periodo
            group.MapPost("/", async (PeriodoCreateDto dto, PeriodoService service) =>
            {
                if (dto == null) return Results.BadRequest("Datos requeridos");
                if (dto.Fecha_Fin <= dto.Fecha_Inicio)
                    return Results.BadRequest("La fecha fin debe ser posterior a la fecha inicio");

                await service.CrearAsync(dto);
                return Results.Created($"/periodo/{dto.Anio}", dto);
            })
            .WithSummary("Crea un nuevo periodo")
            .WithDescription("Crea un nuevo periodo en la base de datos");

            //  Modificar periodo
            group.MapPut("/{id}", async (int id, PeriodoUpdateDto dto, PeriodoService service) =>
            {
                if (dto == null) return Results.BadRequest("Datos requeridos");
                if (dto.Fecha_Fin <= dto.Fecha_Inicio)
                    return Results.BadRequest("La fecha fin debe ser posterior a la fecha inicio");

                dto.ID_Periodo = id;
                var existe = await service.ObtenerPorIdAsync(id);
                if (existe is null) return Results.NotFound("Periodo no encontrado");

                await service.ModificarAsync(dto);
                return Results.NoContent();
            })
            .WithSummary("Modifica un periodo existente");

            //  Eliminar periodo
            group.MapDelete("/{id}", async (int id, PeriodoService service) =>
            {
                var existe = await service.ObtenerPorIdAsync(id);
                if (existe is null) return Results.NotFound("Periodo no encontrado");

                await service.EliminarAsync(id);
                return Results.Ok($"Periodo con ID {id} eliminado correctamente");
            })
            .WithSummary("Elimina un periodo existente");
        }
    }
}
