using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace GCBS_INTERNAL.Models.Booking
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

      

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerBooking>()
                .Property(e => e.ServiceLocation)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerBooking>()
                .Property(e => e.ServiceArea)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerBooking>()
                .Property(e => e.HouseOrHotel)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerBooking>()
                .Property(e => e.Durarion)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerBooking>()
                .Property(e => e.TimeSlot)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerBooking>()
                .Property(e => e.BasePrice)
                .HasPrecision(9, 2);

            modelBuilder.Entity<CustomerBooking>()
                .Property(e => e.ServiceType)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerBooking>()
                .Property(e => e.ServicePrice)
                .HasPrecision(9, 2);

            modelBuilder.Entity<CustomerBooking>()
                .Property(e => e.TotalPrice)
                .HasPrecision(9, 2);

            modelBuilder.Entity<CustomerBooking>()
                .Property(e => e.Notes)
                .IsUnicode(false);
        }
    }
}
