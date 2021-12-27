using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace GCBS_INTERNAL.Models.Support
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Support> Support { get; set; }
        public virtual DbSet<SupportType> SupportType { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Support>()
                .Property(e => e.SupportType)
                .IsUnicode(false);

            modelBuilder.Entity<Support>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<Support>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Support>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<SupportType>()
                .Property(e => e.SupportType1)
                .IsUnicode(false);
        }
    }
}
