using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace GCBS_INTERNAL.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("name=DatabaseContext")
        {
        }

        public virtual DbSet<ServiceTypes> ServiceTypes { get; set; }
        public virtual DbSet<ServicesMaster> ServicesMaster { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServiceTypes>()
                .Property(e => e.ServiceType)
                .IsUnicode(false);
        }

        public System.Data.Entity.DbSet<GCBS_INTERNAL.Models.ServicesMaster> ServicesMasters { get; set; }
    }
}
