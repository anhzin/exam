using BankSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Data
{
    public class BankSystemContext : DbContext
    {
        
        public BankSystemContext(DbContextOptions<BankSystemContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User").Property(p => p.RowVersion).IsConcurrencyToken(); ;
            modelBuilder.Entity<Transaction>().ToTable("Transaction");
        }
    }
}
