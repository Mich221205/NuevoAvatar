using PV_NA_OfertaAcademica.Entities;
using PV_NA_OfertaAcademica.Repository;
using System.Text.RegularExpressions;

namespace PV_NA_OfertaAcademica.Services
{
    public class InstitucionService : IInstitucionService
    {
        private readonly IInstitucionRepository _repository;

        public InstitucionService(IInstitucionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Institucion>> ListarAsync()
            => await _repository.GetAllAsync();

        public async Task<Institucion?> ObtenerPorIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        // Validar datos antes de crear o actualizar
        public async Task<int> CrearAsync(Institucion institucion)
        {
            ValidarInstitucion(institucion);
            return await _repository.CreateAsync(institucion);
        }
        // Actualizar también valida
        public async Task<bool> ActualizarAsync(Institucion institucion)
        {
            ValidarInstitucion(institucion);
            return await _repository.UpdateAsync(institucion);
        }

        public async Task<bool> EliminarAsync(int id)
            => await _repository.DeleteAsync(id);

        // validar nombre no vacío y solo letras y espacios
        private void ValidarInstitucion(Institucion inst)
        {
            if (string.IsNullOrWhiteSpace(inst.Nombre))
                throw new ArgumentException("El nombre no puede ser vacío.");

            if (!Regex.IsMatch(inst.Nombre, @"^[A-Za-z\s]+$"))
                throw new ArgumentException("El nombre solo puede contener letras y espacios.");
        }

    }
}
