using GCBS_INTERNAL.Models.Booking;
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
        
        public virtual DbSet<RoleMaster> RoleMaster { get; set; }
        public virtual DbSet<BookingDetail> BookingDetails { get; set; }
        public virtual DbSet<ContactEnquiryView> ContactEnquiryView { get; set; }
        public virtual DbSet<HotelMaster> HotelMaster { get; set; }
        public virtual DbSet<InAppNotification> InAppNotification { get; set; } 
        public virtual DbSet<PartnerRating> PartnerRating { get; set; }
        public virtual DbSet<PartnerType> PartnerType { get; set; }
        public virtual DbSet<PaymentDetails> PaymentDetails { get; set; }
        public virtual DbSet<PaymentGateway> PaymentGateway { get; set; }
        public virtual DbSet<DurationMaster> DurationMaster { get; set; }
        public virtual DbSet<RolePermissionMaster> RolePermissionMaster { get; set; }
        public virtual DbSet<SiteSettings> SiteSettings { get; set; }
        public virtual DbSet<SmsSettings> SmsSettings { get; set; }
        public virtual DbSet<SupportEnquiryView> SupportEnquiryView { get; set; }
        public virtual DbSet<UserManagement> UserManagement { get; set; }
        public virtual DbSet<EmailSettings> EmailSettings { get; set; }     
        public virtual DbSet<PermissionKeyValue> PermissionKeyValue { get; set; }

        public virtual DbSet<CityMaster> CityMaster { get; set; }
        public virtual DbSet<CountryMaster> CountryMaster { get; set; }
        public virtual DbSet<LocationMasters> LocationMasters { get; set; }
        public virtual DbSet<StateMaster> StateMaster { get; set; }
        public virtual DbSet<AgenciesMaster> AgenciesMaster { get; set; }
        public virtual DbSet<LanguageMaster> LanguageMaster { get; set; }
        public virtual DbSet<NationalityMaster> NationalityMaster { get; set; }

        public virtual DbSet<EnquiryDetails> EnquiryDetails { get; set; }
        public virtual DbSet<CustomerTransactions> CustomerTransactions { get; set; }  
        public virtual DbSet<PayoutDetails> PayoutDetails { get; set; }

        public virtual DbSet<SiteBannerMaster> SiteBannerMasters { get; set; }
        public virtual DbSet<ImageMaster> ImageMaster { get; set; }
        public virtual DbSet<TaxSettings> TaxSettings { get; set; }

        //Partner Bio
        public virtual DbSet<Orientation> Orientation { get; set; }
        public virtual DbSet<Ethnicity> Ethnicity { get; set; }
        public virtual DbSet<DickSize> DickSize { get; set; }
        public virtual DbSet<Eye> Eye { get; set; }
        public virtual DbSet<Hair> Hair { get; set; }
        public virtual DbSet<Height> Height { get; set; }
        public virtual DbSet<Weight> Weight { get; set; }
        public virtual DbSet<Meeting> Meeting { get; set; }     
        public virtual DbSet<Tit> Tit { get; set; }
        public virtual DbSet<TitType> TitType { get; set; }

        //content Management Items 
        public virtual DbSet<AboutUsContent> AboutUsContent { get; set; }
        public virtual DbSet<DisclaimerContent> DisclaimerContent { get; set; }
        public virtual DbSet<HomePageContent> HomePageContent { get; set; }
        public virtual DbSet<PrivacyAndPolicy> PrivacyAndPolicy { get; set; }
        public virtual DbSet<TermsAndConditions> TermsAndConditions { get; set; }
        public virtual DbSet<Warning18Content> Warning18Content { get; set; }



        public virtual DbSet<BankAccountDetails> BankAccountDetails { get; set; }       
        public virtual DbSet<Availability> Availability { get; set; }
        public virtual DbSet<ServiceDurartionPrice> ServiceDurartionPrice { get; set; }

        public virtual DbSet<CalenderDetails> CalenderDetails { get; set; }
        public virtual DbSet<ReportEnquiryView> ReportEnquiryView { get; set; }


        public virtual DbSet<ContactEnquiryType> ContactEnquiryTypes { get; set; }
        public virtual DbSet<SupportEnquiryType> SupportEnquiryTypes { get; set; }

        //2021-07-30 - 3 Tables
        public virtual DbSet<ReportType> ReportType { get; set; }        
        public virtual DbSet<BookingTerms> BookingTerms { get; set; }   
        public virtual DbSet<RefundTerm> RefundTerms { get; set; }


        public virtual DbSet<CustomerBooking> CustomerBooking { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
        }

       
    }
}
