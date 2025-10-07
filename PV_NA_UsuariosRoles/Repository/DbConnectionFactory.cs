using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace PV_NA_UsuariosRoles.Repository
{
    public class DbConnectionFactory
    {
        private readonly IConfiguration _config;
        public DbConnectionFactory(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection CreateConnection()
        {
            string connectionString = _config.GetConnectionString("DefaultConnection")!;
            return new SqlConnection(connectionString);
        }
    }
}
