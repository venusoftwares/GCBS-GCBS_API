using GCBS_INTERNAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API.Partner
{
    
    [Authorize]
    public class PartnerControllController : BaseApiController
    {
        private readonly DatabaseContext db = new DatabaseContext();
        [Route("api/getPartnerMyProfile")]
        public async Task<IHttpActionResult> GetPartnerMyProfile()
        {
            try
            {
                if(userDetails.RoleId == 3)
                {
                    UserManagementPartnerProfile userManagementPartnerProfile = new UserManagementPartnerProfile();
                    List<Languages> languages = new List<Languages>();
                    var us = await db.UserManagement.FindAsync(userDetails.Id);
                    userManagementPartnerProfile.userManagement = us;
                    const char Separator = '|';
                    if (us.Languages != null)
                    {
                        foreach (var language in us.Languages.Split(Separator))
                        {
                            var lan = await db.LanguageMaster.FindAsync(Convert.ToInt32(language));
                            languages.Add(new Languages { ItemId = lan.Id, ItemLanguage = lan.Language });
                        }
                    }
                    userManagementPartnerProfile.Languages = languages;
                    userManagementPartnerProfile.Age = (DateTime.Now.Year - Convert.ToDateTime(us.DateOfBirth).Year);
                    return Ok(userManagementPartnerProfile);
                }
                else
                {
                    return Content(HttpStatusCode.NotAcceptable, "Error: Invalid Access");
                }
               
            }
            catch(Exception ex)
            {
                throw ex;
            }     
        }
        [HttpPut]
        [Route("api/PartnerMyProfile")]
        public async Task<IHttpActionResult> PutPartnerMyProfile(UserManagementPartnerProfile userManagementPartnerProfile)
        {
            try
            {
                if (userDetails.RoleId == 3)
                {
                    UserManagement userManagement = new UserManagement();
                    var dbusermangement = await db.UserManagement.FindAsync(userDetails.Id);
                    using(var db2 = new DatabaseContext())
                    {
                        userManagement = userManagementPartnerProfile.userManagement;
                        userManagement.FirstName = dbusermangement.FirstName;
                        userManagement.SecondName = dbusermangement.SecondName;

                        var list = userManagementPartnerProfile.Languages.Select(x => x.ItemId.ToString()).ToList();
                        userManagement.Languages = string.Join("|", list);

                        userManagement.Password = dbusermangement.Password;
                        userManagement.Id = dbusermangement.Id;
                        userManagement.Username = dbusermangement.Username;
                        userManagement.RoleId = dbusermangement.RoleId;

                        userManagement.DateOfBirth = dbusermangement.DateOfBirth;
                        userManagement.DateOfSignUp = dbusermangement.DateOfSignUp;

                        userManagement.OnlineStatus = dbusermangement.OnlineStatus;
                        userManagement.LastActivateTime = dbusermangement.LastActivateTime;
                        userManagement.LastLogin = dbusermangement.LastLogin;

                        userManagement.Status = dbusermangement.Status;
                        userManagement.CreatedBy = dbusermangement.CreatedBy;
                        userManagement.CreatedOn = dbusermangement.CreatedOn;
                        userManagement.UpdatedBy = userDetails.Id;
                        userManagement.UpdatedOn = DateTime.Now;

                        db2.Entry(userManagement).State = EntityState.Modified;
                        await db2.SaveChangesAsync();
                        db2.Dispose();
                    }     
                    return Ok();
                }
                else
                {
                    return Content(HttpStatusCode.NotAcceptable, "Error: Invalid Access");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPut]
        [Route("api/PartnerBioInformation")]
        public async Task<IHttpActionResult> PutPartnerBioInformation(UserBioInformation  userBioInformation)
        {
            try
            {
                if (userDetails.RoleId == 3 || userDetails.RoleId == 9)
                {
                    UserManagement userManagement = new UserManagement();
                    var dbusermangement = await db.UserManagement.FindAsync(userDetails.Id);
                    using (var db2 = new DatabaseContext())
                    {
                        userManagement = dbusermangement;
                        userManagement.DickSize = userBioInformation.SelectedDickSize;
                        userManagement.TitType = userBioInformation.SelectedTitType;
                        userManagement.Tits = userBioInformation.SelectedTits;
                        userManagement.Height = userBioInformation.SelectedHeight;
                        userManagement.Weight = userBioInformation.SelectedWeight;
                        userManagement.Hair = userBioInformation.SelectedHair;
                        userManagement.Eyes = userBioInformation.SelectedEyes;    
                        userManagement.UpdatedBy = userDetails.Id;
                        userManagement.UpdatedOn = DateTime.Now;      
                        db2.Entry(userManagement).State = EntityState.Modified;
                        await db2.SaveChangesAsync();
                        db2.Dispose();
                    }
                    return Ok();
                }
                else
                {
                    return Content(HttpStatusCode.NotAcceptable, "Error: Invalid Access");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
