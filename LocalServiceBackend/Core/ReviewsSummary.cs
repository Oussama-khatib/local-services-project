using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trial
{
    public class ReviewsSummary
    {
        [Key]
        public int ReviewsSummaryId { get; set; }
        //[ForeignKey("serviceProvider")]
        public int ProviderId { get; set; }
        public string? SummaryText { get; set; }

    }
}
