using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trial
{
    public class ServiceProvider
    {
        [Key]
        public int ProviderId { get; set; }
        //[ForeignKey("user")]
        public int UserId { get; set; }
        public string? Biography { get; set; }
        public int? YearOfExperience { get; set; }
        public string? Location { get; set; }
        public int? TrustScore { get; set; }

        public ICollection<ProviderService>? ProviderServices { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<ReviewsSummary>? ReviewsSummaries { get; set; }
        public ICollection<JobAssignment>? JobAssignments { get; set; }
    }
}
