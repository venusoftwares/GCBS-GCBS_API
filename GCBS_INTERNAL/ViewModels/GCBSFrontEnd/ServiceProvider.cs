using System;

namespace GCBS_INTERNAL.ViewModels.GCBSFrontEnd
{
    public class ServiceProvider
    {
        public string Image { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime? DateOfSignUp { get; set; }
        public bool? Online { get; set; }
    }
}