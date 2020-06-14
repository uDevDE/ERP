using ERP.Server.Entities.Entity;
using System.Data.Entity;

namespace ERP.Server.Entities.Context
{
    public class ElementContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ElementInfo> ElementInfos { get; set; }

        public ElementContext() : base("NRSqlContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ElementInfo>().Property(p => p.Time).HasColumnType("datetime2");
            modelBuilder.Entity<Profile>().HasMany(p => p.ElementInfos).WithMany();

            base.OnModelCreating(modelBuilder);
        }

    }
}
