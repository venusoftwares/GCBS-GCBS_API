using GCBS_INTERNAL.Controllers.API;
using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Models.DurartionAndServiceType;
using GCBS_INTERNAL.Provider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.Partner
{
    [CustomAuthorize]
    public class PartnerBaseAndAdditionalController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        [HttpGet]
        [Route("api/GetPartnerBaseAndAdditionalList")]
        public async Task<IHttpActionResult> GetPartnerBaseAndAdditionalList()
        {
            try
            {
                return Ok(new ListItems
                {
                    AdditionalPriceDTO = await GetAdditionalPriceDTO(userDetails.Id),
                    BasePriceDTO = await GetBasePriceDTO(userDetails.Id)
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("api/SubmitPartnerBasePrice")]
        public async Task<IHttpActionResult> SubmitPartnerBasePrice(BasePriceDTO basePriceDTO)
        {
            try
            {
                var result = await db.PartnerBasePrice.Include(x => x.DurationAndBasePrice)
                    .Where(x => x.DurationAndBasePrice.DurationOrTime == basePriceDTO.TimeOrDuraration && x.UserId == userDetails.Id)
                    .FirstOrDefaultAsync();

                if (result != null)
                {
                    using (var a = new DatabaseContext())
                    {
                        var baseprice = await a.PartnerBasePrice.FindAsync(result.id);
                        baseprice.BasePrice = basePriceDTO.YourBasePrice;
                        baseprice.Status = basePriceDTO.Status;
                        baseprice.UpdateAt = DateTime.Now;
                        baseprice.UpdateBy = userDetails.Id;

                        a.Entry(baseprice).State = EntityState.Modified;
                        await a.SaveChangesAsync();
                    } 
                }
                else
                {
                    using (var a = new DatabaseContext())
                    {
                        var du = await db.DurationAndBasePrice.Where(x => x.DurationOrTime == basePriceDTO.TimeOrDuraration).FirstOrDefaultAsync();
                        a.PartnerBasePrice.Add(new PartnerBasePrice
                        {
                            CreatedAt = DateTime.Now,
                            CreatedBy = userDetails.Id,
                            BasePrice = basePriceDTO.YourBasePrice,
                            DurationId = du.Id,
                            Status = basePriceDTO.Status,
                            UserId = userDetails.Id  
                        });

                        await a.SaveChangesAsync();
                    } 
                }

                return Ok(basePriceDTO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut]
        [Route("api/SubmitPartnerAdditionalPrice")]
        public async Task<IHttpActionResult> SubmitPartnerAdditionalPrice(AdditionalPriceDTO additionalPriceDTO)
        {
            try
            {
                var result = await db.PartnerAdditionalPrice
                    .Where(x => x.ServiceType == additionalPriceDTO.ServiceType && x.UserId == userDetails.Id)
                    .FirstOrDefaultAsync();

                if (result != null)
                {
                    var a = new DatabaseContext();
                    var baseprice = await a.PartnerAdditionalPrice.FindAsync(result.id);

                    baseprice.Status = additionalPriceDTO.Status;
                    baseprice.UpdateAt = DateTime.Now;
                    baseprice.UpdateBy = userDetails.Id;
                    baseprice.AdditionalPrice = additionalPriceDTO.Price;

                    a.Entry(baseprice).State = EntityState.Modified;
                    await a.SaveChangesAsync();

                }
                else
                {
                    var a = new DatabaseContext(); 
                    a.PartnerAdditionalPrice.Add(new PartnerAdditionalPrice
                    {
                        CreatedAt = DateTime.Now,
                        CreatedBy = userDetails.Id,
                        AdditionalPrice = additionalPriceDTO.Price,
                        ServiceType = additionalPriceDTO.ServiceType,
                        Status = additionalPriceDTO.Status,
                        UserId = userDetails.Id
                    }); 
                    await a.SaveChangesAsync();
                }

                return Ok(additionalPriceDTO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task<List<BasePriceDTO>> GetBasePriceDTO(int partnerId)
        {
            List<BasePriceDTO> basePriceDTOs = new List<BasePriceDTO>();

            var durationList = await db.DurationAndBasePrice.ToListAsync();

            var partnerBasePriceListExists = await db.PartnerBasePrice.Where(x => x.UserId == partnerId).ToListAsync();

            foreach (var i in durationList)
            {

                int Id = 0;

                decimal Price = 0;

                bool Status = false;

                if (partnerBasePriceListExists.Any(x => x.DurationId == i.Id))
                {

                    var response = partnerBasePriceListExists.Where(x => x.DurationId == i.Id).FirstOrDefault();
                    if (response != null)
                    {
                        Id = response.id;
                        Price = response.BasePrice;
                        Status = response.Status;
                    }
                }

                basePriceDTOs.Add(new BasePriceDTO
                {
                    Id = Id,
                    MinBasePrice = i.BasePrice,
                    YourBasePrice = Price == 0 ? i.BasePrice : Price,
                    TimeOrDuraration = i.DurationOrTime,
                    Status = Status
                });
            }

            return basePriceDTOs;
        }

        private async Task<List<AdditionalPriceDTO>> GetAdditionalPriceDTO(int partnerId)
        {
            List<AdditionalPriceDTO> additionalPriceDTOs = new List<AdditionalPriceDTO>();

            var serviceType = await db.PartnerServiceType.ToListAsync();

            var partnerAdditionalPrices = await db.PartnerAdditionalPrice.Where(x => x.UserId == partnerId).ToListAsync();

            foreach (var i in serviceType)
            {

                int Id = 0;
                decimal AdditionalPrice = 0;
                bool Status = false;
                if (partnerAdditionalPrices.Any(x => x.ServiceType == i.ServiceType))
                {
                    var response = partnerAdditionalPrices.Where(x => x.ServiceType == i.ServiceType).FirstOrDefault();
                    if (response != null)
                    {
                        Id = response.id;
                        AdditionalPrice = response.AdditionalPrice;
                        Status = response.Status;
                    }

                }

                additionalPriceDTOs.Add(new AdditionalPriceDTO
                {
                    Id = Id,
                    Price = AdditionalPrice,
                    ServiceType = i.ServiceType,

                    Status = Status
                });
            }

            return additionalPriceDTOs;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private class ListItems
        {
            public List<BasePriceDTO> BasePriceDTO { get; set; }
            public List<AdditionalPriceDTO> AdditionalPriceDTO { get; set; }
        }

        public class BasePriceDTO
        {
            public int Id { get; set; }
            public string TimeOrDuraration { get; set; }
            public decimal MinBasePrice { get; set; }
            public decimal YourBasePrice { get; set; }
            public bool Status { get; set; }
        }

        public class AdditionalPriceDTO
        {
            public int Id { get; set; }
            public string ServiceType { get; set; }
            public decimal Price { get; set; }
            public bool Status { get; set; }
        }
    }
}
