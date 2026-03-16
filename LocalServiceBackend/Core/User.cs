using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trial
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public byte[] Password { get; set; }
        public string? Role { get; set; }
        public string? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }

        public ICollection<ServiceProvider>? ServiceProviders { get; set; }
        public ICollection<Customer>? Customers { get; set; }
        public ICollection<Wallet>? Wallets { get; set; }
    }
}
