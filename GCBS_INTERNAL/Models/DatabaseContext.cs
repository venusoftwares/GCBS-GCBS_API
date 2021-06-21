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
        public virtual DbSet<ServicesMaster> ServicesMasters { get; set; }
        public virtual DbSet<MarginMaster> MarginMaster { get; set; }
        public virtual DbSet<PriceMaster> PriceMaster { get; set; }
        public virtual DbSet<RoleMaster> RoleMaster { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
        }

       
    }
}
