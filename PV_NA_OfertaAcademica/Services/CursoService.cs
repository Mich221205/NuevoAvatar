using PV_NA_OfertaAcademica.Dtos;
using PV_NA_OfertaAcademica.Entities;
using PV_NA_OfertaAcademica.Helpers;
using PV_NA_OfertaAcademica.Repository;

namespace PV_NA_OfertaAcademica.Services
{
    // ICursoService define las operaciones disponibles para gestionar cursos.
    public interface ICursoService
    {
        Task<int> Crear(CursoCreateDto dto);
        Task Actualizar(int id, CursoUpdateDto dto);
        Task Eliminar(int id);
        Task<CursoReadDto?> Obtener(int id);
        Task<IEnumerable<CursoReadDto>> Listar();
        Task<IEnumerable<CursoReadDto>> ListarPorCarrera(int idCarrera);
    }

    public class CursoService : ICursoService
    {
        // Inyección de dependencia del repositorio.
        private readonly ICursoRepository _repo;
        public CursoService(ICursoRepository repo) => _repo = repo;

        // Crear valida los datos, verifica que la carrera exista y que no haya otro curso con el mismo nombre en la carrera.
        public async Task<int> Crear(CursoCreateDto dto)
        {
            if (!ValidationUtils.NombreValido(dto.Nombre)) throw new ArgumentException("El nombre solo permite letras/espacios y máx. 100 caracteres.");
            if (!ValidationUtils.NivelValido(dto.Nivel)) throw new ArgumentException("Nivel debe estar entre 1 y 12.");
            if (!await _repo.CarreraExistsAsync(dto.ID_Carrera)) throw new ArgumentException("La carrera no existe.");
            if (await _repo.ExistsByNombreAsync(dto.ID_Carrera, dto.Nombre.Trim())) throw new InvalidOperationException("Ya existe un curso con ese nombre en la carrera.");

            var id = await _repo.CreateAsync(new Curso { Nombre = dto.Nombre.Trim(), Nivel = dto.Nivel, ID_Carrera = dto.ID_Carrera });
            return id;
        }
        // Actualizar verifica que el curso exista, valida los datos y actualiza el curso.
        public async Task Actualizar(int id, CursoUpdateDto dto)
        {
            var actual = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Curso no encontrado.");
            if (!ValidationUtils.NombreValido(dto.Nombre)) throw new ArgumentException("El nombre solo permite letras/espacios y máx. 100 caracteres.");
            if (!ValidationUtils.NivelValido(dto.Nivel)) throw new ArgumentException("Nivel debe estar entre 1 y 12.");
            if (!await _repo.CarreraExistsAsync(dto.ID_Carrera)) throw new ArgumentException("La carrera no existe.");
            if (await _repo.ExistsByNombreAsync(dto.ID_Carrera, dto.Nombre.Trim(), id)) throw new InvalidOperationException("Ya existe un curso con ese nombre en la carrera.");

            actual.Nombre = dto.Nombre.Trim();
            actual.Nivel = dto.Nivel;
            actual.ID_Carrera = dto.ID_Carrera;

            await _repo.UpdateAsync(actual);
        }

        // Eliminar verifica que el curso exista y que no tenga grupos asociados antes de eliminarlo.
        public async Task Eliminar(int id)
        {
            var actual = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Curso no encontrado.");
            if (await _repo.HasGruposAsync(id))
                throw new InvalidOperationException("No se puede eliminar: existen grupos asociados.");
            await _repo.DeleteAsync(id);
        }

        //  Obtener devuelve el curso con el ID especificado o null si no existe.
        public async Task<CursoReadDto?> Obtener(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            return c is null ? null : new CursoReadDto(c.ID_Curso, c.Nombre, c.Nivel, c.ID_Carrera);
        }

        // Listar devuelve todos los cursos.
        public async Task<IEnumerable<CursoReadDto>> Listar()
            => (await _repo.GetAllAsync()).Select(c => new CursoReadDto(c.ID_Curso, c.Nombre, c.Nivel, c.ID_Carrera));

        // ListarPorCarrera devuelve los cursos asociados a la carrera especificada.
        public async Task<IEnumerable<CursoReadDto>> ListarPorCarrera(int idCarrera)
        {
            if (!await _repo.CarreraExistsAsync(idCarrera)) throw new ArgumentException("La carrera no existe.");
            var list = await _repo.GetByCarreraAsync(idCarrera);
            return list.Select(c => new CursoReadDto(c.ID_Curso, c.Nombre, c.Nivel, c.ID_Carrera));
        }
    }
}
