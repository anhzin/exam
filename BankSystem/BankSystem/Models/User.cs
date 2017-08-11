using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]      
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public decimal Balance { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
