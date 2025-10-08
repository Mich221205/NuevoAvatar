using System.Data;

namespace PV_NA_OfertaAcademica.Repository
{
	public interface IDbConnectionFactory
	{
		Task<IDbConnection> CreateConnectionAsync();
	}
}
