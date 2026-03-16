using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trial
{
    public class ServiceCategory
    {
        [Key]
        public int ServiceCategoryId { get; set; }
        public string? Name { get; set; }
        public string? Icon { get; set; }

        public ICollection<ProviderService>? ProviderServices { get; set; }
        public ICollection<Job>? Jobs { get; set; }
    }
}
