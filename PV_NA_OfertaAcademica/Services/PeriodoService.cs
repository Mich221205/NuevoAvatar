using PV_NA_OfertaAcademica.Dtos;
using PV_NA_OfertaAcademica.Entities;
using PV_NA_OfertaAcademica.Repository;

namespace PV_NA_OfertaAcademica.Services
{
    public class PeriodoService
    {
        private readonly PeriodoRepository _repo;

        public PeriodoService(PeriodoRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Periodo>> ObtenerTodosAsync() => await _repo.GetAllAsync();
        public async Task<Periodo?> ObtenerPorIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task CrearAsync(PeriodoCreateDto dto)
        {
            if (dto.Fecha_Fin <= dto.Fecha_Inicio)
                throw new ArgumentException("La fecha de fin debe ser posterior a la fecha de inicio.");

            var periodo = new Periodo
            {
                Anno = dto.Anio,
                Numero = dto.Numero_Periodo,
                Fecha_Inicio = dto.Fecha_Inicio,
                Fecha_Fin = dto.Fecha_Fin
            };

            await _repo.CreateAsync(periodo);
        }

        public async Task ModificarAsync(PeriodoUpdateDto dto)
        {
            if (dto.Fecha_Fin <= dto.Fecha_Inicio)
                throw new ArgumentException("La fecha de fin debe ser posterior a la fecha de inicio.");

            var periodo = new Periodo
            {
                ID_Periodo = dto.ID_Periodo,
                Anno = dto.Anio,
                Numero = dto.Numero_Periodo,
                Fecha_Inicio = dto.Fecha_Inicio,
                Fecha_Fin = dto.Fecha_Fin
            };

            await _repo.UpdateAsync(periodo);
        }

        public async Task EliminarAsync(int id) => await _repo.DeleteAsync(id);
    }
}
