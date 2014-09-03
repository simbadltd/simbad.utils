using System.Data;

namespace Simbad.Utils.DataAccess
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}