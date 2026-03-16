using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trial
{
    public class Wallet
    {
        [Key]
        public int WalletId { get; set; }
        //[ForeignKey("user")]
        public int UserId { get; set; }
        public decimal Balance { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }

    }
}
