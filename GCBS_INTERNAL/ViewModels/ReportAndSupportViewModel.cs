using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Models.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCBS_INTERNAL.ViewModels
{
    public class ReportAndSupportViewModel
    {
        public DateTime CreatedDate { get;set; }
        public string CreatedDateAndTime 
        { 
            get 
            { 
                return CreatedDate.ToString("dd-MM-yyyy hh:ss tt"); 
            } 
        }
        public string Side { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
    }

    public class ReportViewDetails 
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int ReportFrom { get; set; }

        public string ReportType { get; set; }
        public int ReportTo { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
        
        public UserManagement userManagementFrom { get; set; } 
        public UserManagement userManagementTo { get; set; }
        public List<ReportAndSupportViewModel> reportAndSupportViewModels { get; set; }
    }

    public class SupportViewDetails  
    {
        public int id { get; set; } 
        public string SupportType { get; set; } 
        public int UserId { get; set; } 
        public string UserName { get; set; } 
        public string Description { get; set; }

        public int Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public List<ReportAndSupportViewModel> reportAndSupportViewModels { get; set; }
    }
}