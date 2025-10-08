using PV_NA_OfertaAcademica.Repository;
using PV_NA_OfertaAcademica.Entities;
using PV_NA_OfertaAcademica.Dtos;

namespace PV_NA_OfertaAcademica
{
    public static class GrupoEndpoints
    {
        public static void MapGrupoEndpoints(this WebApplication app)
        {
            app.MapGet("/grupo", async (GrupoRepository repo) =>
            {
                var grupos = await repo.GetAllAsync();
                return Results.Ok(grupos);
            });

            app.MapGet("/grupo/{id}", async (int id, GrupoRepository repo) =>
            {
                var grupo = await repo.GetByIdAsync(id);
                return grupo is not null ? Results.Ok(grupo) : Results.NotFound();
            });

            app.MapPost("/grupo", async (GrupoCreateDto dto, GrupoRepository repo) =>
            {
                if (dto.Numero_Grupo <= 0 || string.IsNullOrWhiteSpace(dto.Horario))
                    return Results.BadRequest("Datos inválidos");

                var grupo = new Grupo
                {
                    Numero_Grupo = dto.Numero_Grupo,
                    ID_Curso = dto.ID_Curso,
                    ID_Profesor = dto.ID_Profesor,
                    Horario = dto.Horario,
                    ID_Periodo = dto.ID_Periodo
                };

                await repo.CreateAsync(grupo);
                return Results.Created($"/grupo/{grupo.ID_Grupo}", grupo);
            });

            app.MapPut("/grupo/{id}", async (int id, GrupoUpdateDto dto, GrupoRepository repo) =>
            {
                var existente = await repo.GetByIdAsync(id);
                if (existente is null)
                    return Results.NotFound();

                existente.Numero_Grupo = dto.Numero_Grupo;
                existente.ID_Curso = dto.ID_Curso;
                existente.ID_Profesor = dto.ID_Profesor;
                existente.Horario = dto.Horario;
                existente.ID_Periodo = dto.ID_Periodo;

                await repo.UpdateAsync(existente);
                return Results.Ok(existente);
            });

            app.MapDelete("/grupo/{id}", async (int id, GrupoRepository repo) =>
            {
                var existente = await repo.GetByIdAsync(id);
                if (existente is null)
                    return Results.NotFound();

                await repo.DeleteAsync(id);
                return Results.Ok($"Grupo {id} eliminado correctamente");
            });
        }
    }
}
