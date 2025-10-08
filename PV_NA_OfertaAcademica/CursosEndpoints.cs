using PV_NA_OfertaAcademica.Dtos;
using PV_NA_OfertaAcademica.Services;

namespace PV_NA_OfertaAcademica
{
    public static class CursosEndpoints
    {
        // Extensión para mapear los endpoints relacionados con cursos.
        public static RouteGroupBuilder MapCursoEndpoints(this IEndpointRouteBuilder app)
        {
            // Grupo de rutas para /curso
            var g = app.MapGroup("/curso");

            g.MapGet("/", async (ICursoService svc) => Results.Ok(await svc.Listar()));

            g.MapGet("/{id:int}", async (int id, ICursoService svc) =>
            {
                var item = await svc.Obtener(id);
                return item is null ? Results.NotFound(new { error = "No encontrado" }) : Results.Ok(item);
            });

            // Listar cursos por carrera
            g.MapGet("/por-carrera/{idCarrera:int}", async (int idCarrera, ICursoService svc) =>
            {
                try { return Results.Ok(await svc.ListarPorCarrera(idCarrera)); }
                catch (ArgumentException ex) { return Results.BadRequest(new { error = ex.Message }); }
            });

            //
            g.MapPost("/", async (CursoCreateDto dto, ICursoService svc) =>
            {
                try
                {
                    var id = await svc.Crear(dto);
                    return Results.Created($"/curso/{id}", new { ID_Curso = id });
                }
                catch (ArgumentException ex) { return Results.BadRequest(new { error = ex.Message }); }
                catch (InvalidOperationException ex) { return Results.Conflict(new { error = ex.Message }); }
            });

            // 
            g.MapPut("/{id:int}", async (int id, CursoUpdateDto dto, ICursoService svc) =>
            {
                try { await svc.Actualizar(id, dto); return Results.NoContent(); }
                catch (KeyNotFoundException ex) { return Results.NotFound(new { error = ex.Message }); }
                catch (ArgumentException ex) { return Results.BadRequest(new { error = ex.Message }); }
                catch (InvalidOperationException ex) { return Results.Conflict(new { error = ex.Message }); }
            });

            // 
            g.MapDelete("/{id:int}", async (int id, ICursoService svc) =>
            {
                try { await svc.Eliminar(id); return Results.NoContent(); }
                catch (KeyNotFoundException ex) { return Results.NotFound(new { error = ex.Message }); }
                catch (InvalidOperationException ex) { return Results.Conflict(new { error = ex.Message }); }
            });

            return g;
        }
    }
}
