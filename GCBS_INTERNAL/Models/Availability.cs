namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Availability")]
    public partial class Availability
    {
        public int Id { get; set; }      
        public int UserId { get; set; }     
        public string Time { get; set; }  
        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }
    public class Time
    {
        public List<string> StartTime { get; set; }
        public List<string> EndTime { get; set; }
    }

    public class Root
    {
        public string Day { get; set; }
        public List<Time> Time { get; set; }
    }
    public class RootAvailability
    {
        public Availability Availability { get; set; }
        public List<Root> Times { get; set; }
    }
}
