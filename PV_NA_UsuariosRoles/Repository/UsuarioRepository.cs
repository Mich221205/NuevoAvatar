using Dapper;
using PV_NA_UsuariosRoles.Entities;
using System.Data;

namespace PV_NA_UsuariosRoles.Repository
{
    public class UsuarioRepository
    {
        private readonly DbConnectionFactory _factory;

        public UsuarioRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Obtiene un usuario por su correo electrónico incluyendo información del rol
        /// </summary>
        public async Task<Usuario?> GetUsuarioByEmailAsync(string email)
        {
            using IDbConnection conn = _factory.CreateConnection();
            string query = @"SELECT U.ID_Usuario, U.Email, U.Contrasena, R.Nombre AS Rol, U.ID_Rol
                             FROM Usuario U
                             INNER JOIN Rol R ON U.ID_Rol = R.ID_Rol
                             WHERE U.Email = @Email";
            return await conn.QueryFirstOrDefaultAsync<Usuario>(query, new { Email = email });
        }

        /// <summary>
        /// Obtiene un usuario por su ID incluyendo información del rol
        /// </summary>
        public async Task<Usuario?> GetUsuarioByIdAsync(int id)
        {
            using IDbConnection conn = _factory.CreateConnection();
            string query = @"SELECT U.ID_Usuario, U.Email, U.Contrasena, R.Nombre AS Rol, U.ID_Rol
                             FROM Usuario U
                             INNER JOIN Rol R ON U.ID_Rol = R.ID_Rol
                             WHERE U.ID_Usuario = @Id";
            return await conn.QueryFirstOrDefaultAsync<Usuario>(query, new { Id = id });
        }
    }
}