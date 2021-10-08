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
using GCBS_INTERNAL.Services;
using GCBS_INTERNAL.Provider;
using log4net;

namespace GCBS_INTERNAL.Controllers.API
{
    [CustomAuthorize]
    public class UserManagementsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public ImageServices imgser = new ImageServices();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Common common = new Common();
        // GET: api/UserManagements
        public IQueryable<UserManagement> GetUserManagement()
        {
            return db.UserManagement;
        }

        // GET: api/UserManagements/5
        [ResponseType(typeof(UserManagementViewModel))]
        public async Task<IHttpActionResult> GetUserManagement(int id)
        {
            UserManagement userManagement = await db.UserManagement.FindAsync(userDetails.Id);
            if (userManagement == null)
            {
                return NotFound();
            }
            UserManagementViewModel userManagementViewModel = new UserManagementViewModel();
            userManagementViewModel.UserManagements = userManagement;
            userManagementViewModel.imageBase64 = imgser.EditGetFiles(userDetails.Id, common.FolderForRole(userManagement.RoleId));  
            return Ok(userManagementViewModel);
        }
            
        // PUT: api/UserManagements/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserManagement(int id,UserManagementViewModel userManagementViewModel)
        {     
            if (id != userManagementViewModel.UserManagements.Id)
            {
                return BadRequest();
            }
            UserManagement userManagement = new UserManagement();
            using (var d = new DatabaseContext())
            {
                var re = await d.UserManagement.FindAsync(userManagementViewModel.UserManagements.Id);
                userManagement = re;
                userManagement.Name = userManagementViewModel.UserManagements.Name;
                userManagement.Username = userManagementViewModel.UserManagements.Username;
                userManagement.MobileNo = userManagementViewModel.UserManagements.MobileNo;
                userManagement.EmailId = userManagementViewModel.UserManagements.EmailId;
                userManagement.CreatedBy = re.CreatedBy;
                userManagement.CreatedOn = re.CreatedOn;
                userManagement.Status = re.Status;
                d.Dispose();
            }
            userManagement.UpdatedBy = userDetails.Id;
            userManagement.UpdatedOn = DateTime.Now;
            db.Entry(userManagement).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                if (userManagement.Id > 0 && userManagementViewModel.imageBase64.Count() > 0)
                {
                    string FolderName = common.FolderForRole(userManagement.RoleId);
                   
                    imgser.DeleteFiles(userManagement.Id, FolderName);
                    foreach (var imgbase64 in userManagementViewModel.imageBase64)
                    {
                        imgser.SaveImage(imgbase64, FolderName, userManagement.Id, userDetails.Id);
                    }
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                log.Error(ex.Message);
                if (!UserManagementExists(userManagementViewModel.UserManagements.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
        // PUT: api/PriceMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserManagement(UserMasterVisible userMasterVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userMasterVisible == null)
            {
                return BadRequest();
            }
            UserManagement userManagement = await db.UserManagement.FindAsync(userMasterVisible.Id);
            userManagement.Status = userMasterVisible.Status;
            userManagement.UpdatedBy = userDetails.Id;
            userManagement.UpdatedOn = DateTime.Now;
            db.Entry(userManagement).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        
        // POST: api/UserManagements
        [ResponseType(typeof(UserManagement))]
        public async Task<IHttpActionResult> PostUserManagement(UserManagement userManagement)
        {
            if(userManagement!=null)
            {
                userManagement.CreatedBy = userDetails.Id;
                userManagement.CreatedOn = DateTime.Now;
                userManagement.Status = false;
            }   
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }   
            db.UserManagement.Add(userManagement);
            await db.SaveChangesAsync();   
            return CreatedAtRoute("DefaultApi", new { id = userManagement.Id }, userManagement);
        }

        // DELETE: api/UserManagements/5
        [ResponseType(typeof(UserManagement))]
        public async Task<IHttpActionResult> DeleteUserManagement(int id)
        {
            UserManagement userManagement = await db.UserManagement.FindAsync(id);
            if (userManagement == null)
            {
                return NotFound();
            }

            db.UserManagement.Remove(userManagement);
            await db.SaveChangesAsync();

            return Ok(userManagement);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserManagementExists(int id)
        {
            return db.UserManagement.Count(e => e.Id == id) > 0;
        }
    }
}