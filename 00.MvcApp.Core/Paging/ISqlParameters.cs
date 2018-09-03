using System.Data.SqlClient;

namespace MvcApp.Core.Paging
{
    public interface ISqlParameters
    {
        SqlParameter[] GetParameters();
    }
}
