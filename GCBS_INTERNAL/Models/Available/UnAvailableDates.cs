namespace GCBS_INTERNAL.Models.Available
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UnAvailableDates
    {
        public int id { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
