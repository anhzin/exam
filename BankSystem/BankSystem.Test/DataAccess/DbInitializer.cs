
using BankSystem.Test.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Data
{
    public class DbInitializer
    {
        public static void Initialize(BankSystemContext context)
        {
            context.Database.EnsureCreated();
           
            

        }
    }
}

