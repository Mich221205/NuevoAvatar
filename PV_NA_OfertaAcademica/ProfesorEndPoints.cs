using PV_NA_OfertaAcademica.Entities;
using PV_NA_OfertaAcademica.Services;

namespace PV_NA_OfertaAcademica
{
	public static class ProfesorEndPoints
	{
		public static void MapProfesorEndpoints(this WebApplication app)
		{
			var group = app.MapGroup("/profesor");

			group.MapGet("/", async (IProfesorService service) =>
			{
				var data = await service.GetAllAsync();
				return Results.Ok(data);
			});

			group.MapGet("/{id}", async (int id, IProfesorService service) =>
			{
				var data = await service.GetByIdAsync(id);
				return data is not null ? Results.Ok(data) : Results.NotFound();
			});

			group.MapPost("/", async (Profesor profesor, IProfesorService service) =>
			{
				try
				{
					var result = await service.CreateAsync(profesor);
					return Results.Ok(new { Mensaje = "Profesor registrado correctamente." });
				}
				catch (Exception ex)
				{
					return Results.BadRequest(new { Error = ex.Message });
				} 
			});

			group.MapPut("/", async (Profesor profesor, IProfesorService service) =>
			{
				try
				{
					var result = await service.UpdateAsync(profesor);
					return Results.Ok(new { Mensaje = "Profesor actualizado correctamente." });
				}
				catch (Exception ex)
				{
					return Results.BadRequest(new { Error = ex.Message });
				}
			});

			group.MapDelete("/{id}", async (int id, IProfesorService service) =>
			{
				try
				{
					var result = await service.DeleteAsync(id);
					return Results.Ok(new { Mensaje = "Profesor eliminado correctamente." });
				}
				catch (Exception ex)
				{
					return Results.BadRequest(new { Error = ex.Message });
				}
			});
		}
	}
}
