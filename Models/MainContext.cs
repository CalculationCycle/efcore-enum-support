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
        private readonly string _connectionString;
        
        public DbSet<Movie> Movies { get; set; }
    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public MainContext(string connectionString) : base()
        {
            _connectionString = connectionString;
        }
    }

}
