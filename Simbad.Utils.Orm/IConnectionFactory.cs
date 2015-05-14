using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simbad.Utils.Orm
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
