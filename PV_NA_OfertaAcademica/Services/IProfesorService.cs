using PV_NA_OfertaAcademica.Entities;

namespace PV_NA_OfertaAcademica.Services
{
	public interface IProfesorService
	{
		Task<IEnumerable<Profesor>> GetAllAsync();
		Task<Profesor?> GetByIdAsync(int id);
		Task<int> CreateAsync(Profesor profesor);
		Task<int> UpdateAsync(Profesor profesor);
		Task<int> DeleteAsync(int id);
	}
}
 