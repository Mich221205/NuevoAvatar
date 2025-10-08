using PV_NA_OfertaAcademica.Entities;
using PV_NA_OfertaAcademica.Services;

namespace PV_NA_OfertaAcademica
{
    public static class InstitucionEndpoints
    {
        public static void MapInstitucionEndpoints(this WebApplication app)
        {
            app.MapGet("/institucion", async (IInstitucionService service) =>
                Results.Ok(await service.ListarAsync()));

            app.MapGet("/institucion/{id}", async (int id, IInstitucionService service) =>
            {
                var inst = await service.ObtenerPorIdAsync(id);
                return inst is not null ? Results.Ok(inst) : Results.NotFound();
            });

            app.MapPost("/institucion", async (Institucion inst, IInstitucionService service) =>
            {
                var id = await service.CrearAsync(inst);
                return Results.Created($"/institucion/{id}", inst);
            });

            app.MapPut("/institucion/{id}", async (int id, Institucion inst, IInstitucionService service) =>
            {
                inst.ID_Institucion = id;
                return await service.ActualizarAsync(inst) ? Results.Ok() : Results.NotFound();
            });

            app.MapDelete("/institucion/{id}", async (int id, IInstitucionService service) =>
                await service.EliminarAsync(id) ? Results.Ok() : Results.NotFound());
        }

    }
}
