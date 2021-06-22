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
        public virtual DbSet<BookingDetails> BookingDetails { get; set; }
        public virtual DbSet<ContactEnquiryView> ContactEnquiryView { get; set; }
        public virtual DbSet<HotelMaster> HotelMaster { get; set; }
        public virtual DbSet<InAppNotification> InAppNotification { get; set; }
        public virtual DbSet<LocationMaster> LocationMaster { get; set; }
        public virtual DbSet<PartnerRating> PartnerRating { get; set; }
        public virtual DbSet<PartnerType> PartnerType { get; set; }
        public virtual DbSet<PaymentDetails> PaymentDetails { get; set; }
        public virtual DbSet<PaymentGateway> PaymentGateway { get; set; }
        public virtual DbSet<PayoutDetails> PayoutDetails { get; set; }
        public virtual DbSet<RolePermissionMaster> RolePermissionMaster { get; set; }
        public virtual DbSet<SiteSettings> SiteSettings { get; set; }
        public virtual DbSet<SmsSettings> SmsSettings { get; set; }
        public virtual DbSet<SupportEnquiryView> SupportEnquiryView { get; set; }
        public virtual DbSet<UserManagement> UserManagement { get; set; }
        public virtual DbSet<EmailSettings> EmailSettings { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
        }

       
    }
}
