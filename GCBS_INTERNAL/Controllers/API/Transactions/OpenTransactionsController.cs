using GCBS_INTERNAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using GCBS_INTERNAL.Provider;
using GCBS_INTERNAL.Models.Booking;
using System.Xml.Serialization;
using System.Text;
using System.IO;

namespace GCBS_INTERNAL.Controllers.API.Transactions
{
     [CustomAuthorize]
    public class OpenTransactionsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        //Service Status completed and Partner status pending
        // GET: api/EnquiryDetails
        [ResponseType(typeof(OpenTransactionsViewModel))]
        public async Task<IHttpActionResult> PostOpenTransactions(OpenTransactionsViewModel enquiryViewModel)
        {
           
            var list = db.PartnerPayoutDetails.Include(x=>x.customerBooking).Include(x=>x.userManagement)
                .OrderByDescending(x=>x.customerBooking.Id)
                .Where(x=>x.Status==false && x.customerBooking.Status == Constant.CUSTOMER_BOOKING_STATUS_COMPLETED) 
                 .ToList();


            List<EnquiryViewModel> res = new List<EnquiryViewModel>();

            foreach (var x in list)
            {
                res.Add(new EnquiryViewModel
                {
                    Id = x.customerBooking.Id,
                    Email = x.userManagement.EmailId,
                    ServicePartner = x.userManagement.FirstName + "_" + x.PartnerId,
                    ServicePartnerId = x.PartnerId,  
                    ServiceStatus = x.customerBooking.Status,
                    ServiceStatusToString = x.customerBooking.Status == 1 ? "Opened" 
                    : x.customerBooking.Status == 2 ? "Canceled" : x.customerBooking.Status == 3 ? "Completed" : x.customerBooking.Status == 4 ? "Rejected" : x.customerBooking.Status == 5 ? "Accepted" : "",
                    Mobile = x.userManagement.MobileNo,
                    ServiceDate = x.customerBooking.DateTime ,
                    TimeSlot = x.customerBooking.TimeSlot
                });
            }


            if (enquiryViewModel != null)
            {
                if (enquiryViewModel.UserId > 0)
                {
                    res = res.Where(x => x.UserId == enquiryViewModel.UserId).ToList();
                }
                if (enquiryViewModel.ServicePartnerId > 0)
                {
                    res = res.Where(x => x.ServicePartnerId == enquiryViewModel.ServicePartnerId).ToList();
                }
                if (enquiryViewModel.ServiceId > 0)
                {
                    res = res.Where(x => x.ServiceId == enquiryViewModel.ServiceId).ToList();
                }
                if (!string.IsNullOrEmpty(enquiryViewModel.Username))
                {
                    res = res.Where(x => x.Username.ToLower() == enquiryViewModel.Username.ToLower()).ToList();
                }
                if (!string.IsNullOrEmpty(enquiryViewModel.Email))
                {
                    res = res.Where(x => x.Email.ToLower() == enquiryViewModel.Email.ToLower()).ToList();
                }
                if (!string.IsNullOrEmpty(enquiryViewModel.Mobile))
                {
                    res = res.Where(x => x.Mobile.ToLower() == enquiryViewModel.Mobile.ToLower()).ToList();
                }
                if (enquiryViewModel.ServiceStatus > 0)
                {
                    res = res.Where(x => x.ServiceStatus == enquiryViewModel.ServiceStatus).ToList();
                }
                if (enquiryViewModel.FromDate != null && enquiryViewModel.ToDate != null)
                {
                    res = res.Where(x => x.ServiceDate >= Convert.ToDateTime(enquiryViewModel.FromDate)
                    && x.ServiceDate <= Convert.ToDateTime(enquiryViewModel.ToDate)).ToList();
                }
            }
            return Ok(res);
        }

        public IHttpActionResult Get()
        {

            var list = db.PartnerPayoutDetails.Include(x => x.customerBooking).Include(x => x.userManagement)
               .OrderByDescending(x => x.customerBooking.Id)
               .Where(x => x.Status == false && x.customerBooking.Status == Constant.CUSTOMER_BOOKING_STATUS_COMPLETED)
                .ToList();

            User2 user2 = new User2();
            List<User> user = new List<User>();

            foreach(var a in list)
            {
                var bank = db.BankAccountDetails.Where(x => x.UserId == a.userManagement.Id).FirstOrDefault();
                user.Add(new User
                {
                    Amount = a.Amount.ToString(),
                    BookingId = a.BookingNo.ToString(),
                    PartnerId = a.PartnerId.ToString(),
                    PartnersFirstName = a.userManagement.FirstName,
                    PartnersBankAccountDetails = (bank == null ?  new BankAccountDetails() : bank)
                });
            }
            user2.User = user;

             // Alternative 1
             XmlSerializer serializer = new XmlSerializer(typeof(List<User>));

            // Alternative 2
            // DataContractSerializer serializer = new DataContractSerializer(typeof(User));

            StringBuilder builder = new StringBuilder();
            using (StringWriter writer = new StringWriter(builder))
            {
                serializer.Serialize(writer, user);

                // alternative 2
                // serializer.WriteObject(writer, user);
            }

            // create XML from your data.
            return new OkXmlDownloadResult(builder.ToString(), "myfile.xml", this);
        }

        public class User2
        {
            public List<User> User { get; set; }
        }
        public class User
        {
            public string BookingId { get; set; }
            public string PartnerId { get; set; }
            public string PartnersFirstName { get; set; }
            public BankAccountDetails PartnersBankAccountDetails { get; set; }
            public string Amount { get; set; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EnquiryDetailsExists(int id)
        {
            return db.EnquiryDetails.Count(e => e.Id == id) > 0;
        }
    }
}
