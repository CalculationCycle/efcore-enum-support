using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppEFCoreEnum.Models
{
    class MainContext: DbContext
    {
        public DbSet<Movie> Movies { get; set; }
    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost\SQLEXPRESS2017;Database=EFCoreEnumSupport;Integrated Security=True; MultipleActiveResultSets=True;");
        }
    }

}
