using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trial
{
    public class ProviderService
    {
        [Key]
        public int ServiceId { get; set; }
        //[ForeignKey("serviceProvider")]
        public int ProviderId { get; set; }
        //[ForeignKey("serviceCategory")]
        public int CategoryId { get; set; }

        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public string? Description { get; set; }

    }
}
