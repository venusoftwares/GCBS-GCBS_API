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
            modelBuilder.Entity<CalenderDetails>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<CalenderDetails>()
                .Property(e => e.cssClass)
                .IsUnicode(false);
        }
    }
}
