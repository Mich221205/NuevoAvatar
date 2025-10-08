using PV_NA_OfertaAcademica.Entities;
using PV_NA_OfertaAcademica.Repository;

namespace PV_NA_OfertaAcademica.Services
{
	public class ProfesorService : IProfesorService
	{
		private readonly IProfesorRepository _repo;

		public ProfesorService(IProfesorRepository repo)
		{
			_repo = repo;
		}

		public async Task<IEnumerable<Profesor>> GetAllAsync() => await _repo.GetAllAsync();

		public async Task<Profesor?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

		public async Task<int> CreateAsync(Profesor profesor)
		{
			// Validaciones de campos requeridos
			if (string.IsNullOrWhiteSpace(profesor.Identificacion))
				throw new Exception("La identificación es obligatoria.");
			if (string.IsNullOrWhiteSpace(profesor.Tipo_Identificacion))
				throw new Exception("El tipo de identificación es obligatorio.");
			if (string.IsNullOrWhiteSpace(profesor.Email))
				throw new Exception("El correo electrónico es obligatorio.");
			if (string.IsNullOrWhiteSpace(profesor.Nombre))
				throw new Exception("El nombre es obligatorio.");

			// Validar email institucional
			if (!profesor.Email.EndsWith("@cuc.ac.cr", StringComparison.OrdinalIgnoreCase))
				throw new Exception("El correo debe ser institucional (@cuc.ac.cr).");

			// Validar duplicados
			if (await _repo.ExisteIdentificacionAsync(profesor.Identificacion))
				throw new Exception("Ya existe un profesor con esa identificación.");
			if (await _repo.ExisteEmailAsync(profesor.Email))
				throw new Exception("Ya existe un profesor con ese correo.");

			// Validar nombre
			if (!System.Text.RegularExpressions.Regex.IsMatch(profesor.Nombre, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ ]+$"))
				throw new Exception("El nombre solo puede contener letras y espacios.");

			// Validar edad mínima (18 años)
			var edad = DateTime.Today.Year - profesor.Fecha_Nacimiento.Year;
			if (profesor.Fecha_Nacimiento > DateTime.Today.AddYears(-edad)) edad--;
			if (edad < 18)
				throw new Exception("El profesor debe ser mayor de edad (mínimo 18 años).");

			return await _repo.InsertAsync(profesor);
		}

		public async Task<int> UpdateAsync(Profesor profesor)
		{
			if (profesor.ID_Profesor <= 0)
				throw new Exception("Debe especificar un ID de profesor válido.");

			return await _repo.UpdateAsync(profesor);
		}

		public async Task<int> DeleteAsync(int id)
		{
			if (id <= 0)
				throw new Exception("Debe especificar un ID de profesor válido.");
			return await _repo.DeleteAsync(id);
		}
	}
}
 