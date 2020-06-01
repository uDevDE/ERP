using ERP.Server.Entities.Entity;
using System.Data.Entity;

namespace ERP.Server.Entities.Context
{
    public class EmployeeContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<DivisionInfo> DivisionInfos { get; set; }

        public EmployeeContext() : base("NRSqlContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>().HasOptional(x => x.Employee).WithMany().HasForeignKey(x => x.EmployeeId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Device>().HasOptional(x => x.Division).WithMany().HasForeignKey(x => x.DivisionId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Employee>().HasOptional(x => x.Device).WithMany().HasForeignKey(x => x.DeviceId);
            modelBuilder.Entity<Employee>().Property(p => p.LastLogin).HasColumnType("datetime2");

            base.OnModelCreating(modelBuilder);
        }

    }
}
