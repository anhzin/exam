using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystem.Test.Models
{
    public class Transaction : BaseEntity
    {
        public Guid UserID { get; set; }
        public TransactionTypes? Type { get; set; }
        public decimal Amount { get; set; }
        public string Target { get; set; }
        public bool Status { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
    }

    public enum TransactionTypes
    {
        Deposite = 0,
        WithDraw = 1,
        Transfer = 2
    }
}