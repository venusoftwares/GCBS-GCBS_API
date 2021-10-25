using GCBS_INTERNAL.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using GCBS_INTERNAL.Provider;
namespace GCBS_INTERNAL.Controllers.API.Partner
{
    
     [CustomAuthorize]
    public class PartnerControllController : BaseApiController
    {
        private readonly DatabaseContext db = new DatabaseContext();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [Route("api/getPartnerMyProfile")]
        public async Task<IHttpActionResult> GetPartnerMyProfile()
        {
            try
            {
                log.Info("[GetPartnerMyProfile] Called");
                if (userDetails.RoleId == 3)
                {
                    UserManagementPartnerProfile userManagementPartnerProfile = new UserManagementPartnerProfile();
                    List<Languages> languages = new List<Languages>();
                    //List<Languages> meetings = new List<Languages>();
                    List<Agencies> agencies = new List<Agencies>();
                    var us = await db.UserManagement.FindAsync(userDetails.Id);
                    userManagementPartnerProfile.userManagement = us;
                    const char Separator = '|';
                    if (!string.IsNullOrEmpty(us.Languages))
                    {
                        foreach (var language in us.Languages.Split(Separator))
                        {
                            var lan = await db.LanguageMaster.FindAsync(Convert.ToInt32(language));
                            if(lan!=null)
                            {
                                if (lan.Status)
                                {
                                    if (lan != null)
                                    {
                                        languages.Add(new Languages { ItemId = lan.Id, ItemLanguage = lan.Language });
                                    }
                                }
                            }  
                        }
                    }
                    //if (us.Meeting != null)
                    //{
                    //    foreach (var meeting in us.Meeting.Split(Separator))
                    //    {
                    //        var lan = await db.Meeting.FindAsync(Convert.ToInt32(meeting));
                    //        if(lan!=null)
                    //        {
                    //            if (lan.Status)
                    //            {
                    //                meetings.Add(new Languages { ItemId = lan.Id, ItemLanguage = lan.Meeting1 });
                    //            }
                    //        }
                           
                    //    }
                    //}
                    if(!string.IsNullOrEmpty(us.Agencies))
                    {
                        foreach (var agencis in us.Agencies.Split(Separator))
                        {
                            var age = await db.AgenciesMaster.FindAsync(Convert.ToInt32(agencis));
                            if (age != null)
                            {
                                if (age.Status)
                                {
                                    agencies.Add(new Agencies { ItemId = age.Id, ItemAgencies = age.HotelName });
                                }
                            }

                        }
                    }    
                    userManagementPartnerProfile.Languages = languages;
                    userManagementPartnerProfile.Agencies = agencies;
                    userManagementPartnerProfile.Age = (DateTime.Now.Year - Convert.ToDateTime(us.DateOfBirth).Year);
                    log.Info("[GetPartnerMyProfile] End");
                    return Ok(userManagementPartnerProfile);
                }
                else
                {
                    return Content(HttpStatusCode.NotAcceptable, "Error: Invalid Access");
                }
               
            }
            catch(Exception ex)
            {
                log.Error("[GetPartnerMyProfile]", ex);
                return Content(HttpStatusCode.InternalServerError, "Something went wrong try again");
            }     
        }
        [HttpPut]
        [Route("api/PartnerMyProfile")]
        public async Task<IHttpActionResult> PutPartnerMyProfile(UserManagementPartnerProfile userManagementPartnerProfile)
        {
            try
            {
                log.Info("[PutPartnerMyProfile] Called");
                if (userDetails.RoleId == Constant.PARTNER_ROLE_ID)
                {
                    UserManagement userManagement = new UserManagement();
                    var dbusermangement = await db.UserManagement.FindAsync(userDetails.Id);
                    using(var db2 = new DatabaseContext())
                    {
                        userManagement = userManagementPartnerProfile.userManagement;
                        userManagement.FirstName = dbusermangement.FirstName;
                        userManagement.SecondName = dbusermangement.SecondName;

                        var list = userManagementPartnerProfile.Languages.Select(x => x.ItemId.ToString()).ToList();
                        if(list!=null)
                        {
                            userManagement.Languages = string.Join("|", list);
                        }   
                        var age = userManagementPartnerProfile.Agencies.Select(x => x.ItemId.ToString()).ToList();
                        if(age!=null)
                        {
                            userManagement.Agencies = string.Join("|", age);
                        }
                            
                        userManagement.Password = dbusermangement.Password;
                        userManagement.Id = dbusermangement.Id;
                        userManagement.Username = dbusermangement.Username;
                        userManagement.RoleId = dbusermangement.RoleId;

                        userManagement.DateOfBirth = dbusermangement.DateOfBirth;
                        userManagement.DateOfSignUp = dbusermangement.DateOfSignUp;

                        userManagement.OnlineStatus = dbusermangement.OnlineStatus;
                        userManagement.LastActivateTime = dbusermangement.LastActivateTime;
                        userManagement.LastLogin = dbusermangement.LastLogin;

                        userManagement.DickSize = dbusermangement.DickSize;
                        userManagement.TitType = dbusermangement.TitType;
                        userManagement.Tits = dbusermangement.Tits;
                        userManagement.Height = dbusermangement.Height;
                        userManagement.Weight = dbusermangement.Weight;
                        userManagement.Hair = dbusermangement.Hair;
                        userManagement.Eyes = dbusermangement.Eyes;
                        userManagement.Smoking = dbusermangement.Smoking;
                        userManagement.Drinking = dbusermangement.Drinking;
                        userManagement.Meeting = dbusermangement.Meeting;
                        userManagement.ServiceTypeInCall = dbusermangement.ServiceTypeInCall;
                        userManagement.ServiceTypeOutCall = dbusermangement.ServiceTypeOutCall; 
                        userManagement.Image = dbusermangement.Image;    
                        userManagement.Status = dbusermangement.Status;
                        userManagement.CreatedBy = dbusermangement.CreatedBy;
                        userManagement.CreatedOn = dbusermangement.CreatedOn;
                        userManagement.UpdatedBy = userDetails.Id;
                        userManagement.UpdatedOn = DateTime.Now;

                        db2.Entry(userManagement).State = EntityState.Modified;
                        await db2.SaveChangesAsync();
                        db2.Dispose();
                    }
                    log.Info("[PutPartnerMyProfile] End");
                    return Ok();
                }
                else
                {
                    return Content(HttpStatusCode.NotAcceptable, "Error: Invalid Access");
                }

            }
            catch (Exception ex)
            {
                log.Error("[PutPartnerMyProfile]", ex);
                return Content(HttpStatusCode.InternalServerError, "Something went wrong try again");
            }
        }


        [HttpPut]
        [Route("api/PartnerBioInformation")]
        public async Task<IHttpActionResult> PutPartnerBioInformation(UserBioInformation  userBioInformation)
        {
            try
            {
                log.Info("[PutPartnerBioInformation] Called");
                if (userDetails.RoleId == Constant.PARTNER_ROLE_ID || userDetails.RoleId == Constant.CUSTOMER_ROLE_ID)
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
                        var list = userBioInformation.SelectedMeetings.Select(x => x.ItemId.ToString()).ToList();
                        if(list!=null)
                        {
                            userManagement.Meeting = string.Join("|", list);
                        }        
                        userManagement.Eyes = userBioInformation.SelectedEyes;

                        userManagement.Smoking = userBioInformation.SelectedSmoking;
                        userManagement.Drinking = userBioInformation.SelectedDrinking;

                        userManagement.ServiceTypeInCall = userBioInformation.SelectedServiceTypeInCall;
                        userManagement.ServiceTypeOutCall = userBioInformation.SelectedServiceTypeOutCall;

                        userManagement.UpdatedBy = userDetails.Id;
                        userManagement.UpdatedOn = DateTime.Now;      
                        db2.Entry(userManagement).State = EntityState.Modified;
                        await db2.SaveChangesAsync();
                        db2.Dispose();
                    }
                    log.Info("[PutPartnerBioInformation] End");
                    return Ok();
                }
                else
                {
                    return Content(HttpStatusCode.NotAcceptable, "Error: Invalid Access");
                }

            }
            catch (Exception ex)
            {
                log.Error("[PutPartnerBioInformation]", ex);
                return Content(HttpStatusCode.InternalServerError, "Something went wrong try again");
            }
        }
    }
}
