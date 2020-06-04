using ERP.Server.Entities.Entity;
using System.Data.Entity;

namespace ERP.Server.Entities.Context
{
    public class FilterContext : DbContext
    {
        public DbSet<ElementFilter> Filters { get; set; }

        public FilterContext() : base("NRSqlContext")
        {

        }

    }
}
