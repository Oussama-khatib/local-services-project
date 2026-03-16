using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trial
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }
        //[ForeignKey("customer")]
        public int CustomerId { get; set; }
        //[ForeignKey("serviceCategory")]
        public int ServiceCategoryId { get; set; }

        public string? Description { get; set; }
        public string? Location { get; set; }
        public bool? IsEmergency { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? AcceptedAt { get;set; }
        public DateTime? CompletedAt { get;set;}

        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
        public ICollection<JobAssignment>? JobAssignments { get; set; }
    }
}
