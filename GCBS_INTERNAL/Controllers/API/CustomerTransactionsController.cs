using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GCBS_INTERNAL.Models;

namespace GCBS_INTERNAL.Controllers.API
{
    public class CustomerTransactionsController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/CustomerTransactions
        [ResponseType(typeof(CustomerTransactionsViewModel))]
        public async Task<IHttpActionResult> PostCustomerTransactions(CustomerTransactionsViewModel customerTransactionsViewModel)
        {
            try
            {
                var res = await db.CustomerTransactions
               .Include(x => x.PartnerManagements)
               .Include(x => x.UserManagements)
               .Include(x => x.servicesMasters)
               .Select(x => new CustomerTransactionsViewModel
               {
                   Id = x.Id,
                   Amount = x.Amount,
                   BookingDate = x.BookingDate,
                   BookingId = x.Id,
                   PartnerId = x.PartnerId,
                   PartnerName = x.PartnerManagements.Username + "-" + x.PartnerId,
                   PaymentDate = x.PaymentDate,
                   PaymentMode = x.PaymentMode,
                   PaymentType = x.PaymentType,
                   ServiceCategory = x.servicesMasters.Service,
                   UserMobile = x.UserManagements.MobileNo,
                   Username = x.UserManagements.Username + "-" + x.UserId,
                   ServiceId = x.ServiceId
               }).ToListAsync();

                if (customerTransactionsViewModel != null)
                {
                    if (customerTransactionsViewModel.BookingId > 0)
                    {
                        res = res.Where(x => x.BookingId == customerTransactionsViewModel.BookingId).ToList();
                    }
                    if (customerTransactionsViewModel.ServiceId > 0)
                    {
                        res = res.Where(x => x.ServiceId == customerTransactionsViewModel.ServiceId).ToList();
                    }
                    if (!string.IsNullOrEmpty(customerTransactionsViewModel.UserMobile))
                    {
                        res = res.Where(x => x.UserMobile == customerTransactionsViewModel.UserMobile).ToList();
                    }
                    if (!string.IsNullOrEmpty(customerTransactionsViewModel.BookingDate))
                    {
                        res = res.Where(x => x.BookingDate == customerTransactionsViewModel.BookingDate).ToList();
                    }
                    if (customerTransactionsViewModel.PaymentDateFrom != null && customerTransactionsViewModel.PaymentDateTo != null)
                    {
                        res = res.Where(x => x.PaymentDate >= Convert.ToDateTime(customerTransactionsViewModel.PaymentDateFrom)
                        && x.PaymentDate <= Convert.ToDateTime(customerTransactionsViewModel.PaymentDateFrom)).ToList();
                    }
                    if (!string.IsNullOrEmpty(customerTransactionsViewModel.PaymentType))
                    {
                        res = res.Where(x => x.PaymentType == customerTransactionsViewModel.PaymentType).ToList();
                    }   
                }
                return Ok(res);
            }
            catch(Exception ex)
            {
                throw ex;
            }
          
        }

        // GET: api/CustomerTransactions/5
        [ResponseType(typeof(CustomerTransactions))]
        public async Task<IHttpActionResult> GetCustomerTransactions(int id)
        {
            CustomerTransactions customerTransactions = await db.CustomerTransactions.FindAsync(id);
            if (customerTransactions == null)
            {
                return NotFound();
            }

            return Ok(customerTransactions);
        }        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

       
    }
}