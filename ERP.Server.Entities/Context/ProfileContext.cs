using ERP.Server.Entities.Entity;
using System.Data.Entity;

namespace ERP.Server.Entities.Context
{
    public class ProfileContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }

        public ProfileContext() : base("NRSqlContext")
        {

        }
    }
}
