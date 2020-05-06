using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TestAPI.Models
{
    public class WebApiContext : DbContext
    {
        public DbSet<Equips> Equips { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<Type> Types { get; set; }
        public WebApiContext(DbContextOptions options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"server=HXKHANG; database=CompanyManage;Trusted_Connection=True;");
        //}
    }
}
