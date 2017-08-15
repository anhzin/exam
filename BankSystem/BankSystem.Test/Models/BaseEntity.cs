using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Test.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid ID { get; set; }
    }
}
