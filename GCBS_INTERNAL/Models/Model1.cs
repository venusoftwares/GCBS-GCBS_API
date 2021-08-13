using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace GCBS_INTERNAL.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

      

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefundTerm>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<RefundTerm>()
                .Property(e => e.Body)
                .IsUnicode(false);
        }
    }
}
