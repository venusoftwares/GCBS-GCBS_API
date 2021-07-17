﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCBS_INTERNAL.ViewModels.GCBSFrontEnd
{
    public class PartnerCommon
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string NickName { get; set; }
        public string Image { get; set; }
        public DateTime? DateOfSignUp { get; set; }    
    }

    public class PartnerPhotoGallery
    {
        public string PrimaryImage { get; set; }
        public List<string> SecondaryImage { get; set; }
    }
}