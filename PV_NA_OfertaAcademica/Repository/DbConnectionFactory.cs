using System.Data;
using Microsoft.Data.SqlClient;

namespace PV_NA_OfertaAcademica.Repository
{
	public class DbConnectionFactory : IDbConnectionFactory
	{
		private readonly IConfiguration _config;

		public DbConnectionFactory(IConfiguration config)
		{
			_config = config;
		}

		public async Task<IDbConnection> CreateConnectionAsync()
		{
			var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
			await connection.OpenAsync();
			return connection;
		}
	}
}
