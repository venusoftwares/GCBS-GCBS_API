using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCBS_INTERNAL.ViewModels
{
    public class PartnerDetailsViewModel
    {
        public string RegisterDate { get; set; }
        public int Partner { get; set; }
        public string PartnerName { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
    }
    public class CustomerDetailsViewModel
    {
        public string RegisterDate { get; set; }
        public int Customer { get; set; }
        public string CustomerName { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
    }
}