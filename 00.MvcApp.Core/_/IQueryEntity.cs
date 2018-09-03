using System.Data;

namespace MvcApp.Core
{
    public interface IQueryEntity
    {
        void Fill(DataRow row);
    }
}
