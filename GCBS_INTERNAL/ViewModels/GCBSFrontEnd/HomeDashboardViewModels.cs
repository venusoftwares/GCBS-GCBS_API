using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCBS_INTERNAL.ViewModels.GCBSFrontEnd
{
    public class HomeDashboardViewModels
    {         
        public HomePageContents homePageContent { get; set; }
        public AboutUs AboutUs { get; set; }
        public TermsAndCondition TermsAndCondition { get; set; }
        public PrivacyPolicy PrivacyPolicy { get; set; }
        public Disclaimer Disclaimer { get; set; }
        public WarningEighteenPlus WarningEighteenPlus { get; set; }
    }
    public class HomePageContents
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Body { get; set; }

    }
    public class AboutUs
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Body { get; set; }

    }
    public class TermsAndCondition
    {
        public string Title { get; set; }    
        public string Body { get; set; }

    }
    public class PrivacyPolicy
    {
        public string Title { get; set; }
        public string Body { get; set; }

    }
    public class Disclaimer
    {
        public string Title { get; set; }
        public string Body { get; set; }

    }
    public class WarningEighteenPlus
    {
        public string Title { get; set; }
        public string Body { get; set; }

    }

}