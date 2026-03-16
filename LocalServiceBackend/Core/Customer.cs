using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trial
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        //[ForeignKey("user")]
        public int UserId { get; set; }
        public string? DefaultLocation { get; set; }
        public string? TotalJobPosted { get; set; }
        public string? TrustScore { get; set; }

        public ICollection<Job>? Jobs { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
