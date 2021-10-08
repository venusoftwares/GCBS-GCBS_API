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
using GCBS_INTERNAL.Provider;
using log4net;

namespace GCBS_INTERNAL.Controllers.API
{
     [CustomAuthorize]
    public class PermissionKeyValuesController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: api/PermissionKeyValues
        public IQueryable<PermissionKeyValue> GetPermissionKeyValue()
        {
            return db.PermissionKeyValue;
        }

        //// GET: api/PermissionKeyValues/5
        //[ResponseType(typeof(PermissionKeyValue))]
        //public async Task<IHttpActionResult> GetPermissionKeyValue(int id)
        //{
        //    PermissionKeyValue permissionKeyValue = await db.PermissionKeyValue.FindAsync(id);
        //    if (permissionKeyValue == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(permissionKeyValue);
        //}

  
        // PUT: api/PriceMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPermissionKeyValue(PermissionKeyValueVisible permissionKeyValueVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (permissionKeyValueVisible == null)
            {
                return BadRequest();
            }
            PermissionKeyValue permissionKeyValue = await db.PermissionKeyValue.FindAsync(permissionKeyValueVisible.Id);
            permissionKeyValue.Status = permissionKeyValueVisible.Status;
            db.Entry(permissionKeyValue).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
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