using PV_NA_OfertaAcademica.Entities;

namespace PV_NA_OfertaAcademica.Services
{
    public interface IInstitucionService
    {
        Task<IEnumerable<Institucion>> ListarAsync();
        Task<Institucion?> ObtenerPorIdAsync(int id);
        Task<int> CrearAsync(Institucion institucion);
        Task<bool> ActualizarAsync(Institucion institucion);
        Task<bool> EliminarAsync(int id);

    }
}
