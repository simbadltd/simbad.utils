using System.Data;

namespace Simbad.Utils.Domain.Infrastructure
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}