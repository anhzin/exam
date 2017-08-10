using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class User
    {
        public Guid ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
