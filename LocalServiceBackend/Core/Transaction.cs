using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trial
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        //[ForeignKey("wallet")]
        public int FromWalletId { get; set; }
        //[ForeignKey("wallet")]
        public int ToWalletId { get; set; }
        //[ForeignKey("job")]
        public int JobId { get; set; }
        public decimal Amount { get; set; }
        public string? Type { get; set; }
    }
}
