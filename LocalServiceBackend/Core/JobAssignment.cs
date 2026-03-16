using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trial
{
    public class JobAssignment
    {
        [Key]
        public int JobAssignmentId { get; set; }
        //[ForeignKey("serviceProvider")]
        public int ProviderId { get; set; }
        //[ForeignKey("job")]
        public int JobId { get; set; }
        public DateTime? AcceptedAt { get; set; }

    }
}
