using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trial
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        //[ForeignKey("job")]
        public int JobId { get; set; }
        //[ForeignKey("serviceProvider")]
        public int ProviderId { get; set; }
        //[ForeignKey("customer")]
        public int CustomerId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
