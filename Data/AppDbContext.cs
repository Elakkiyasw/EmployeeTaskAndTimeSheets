using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WEBAPI_Demo.Models;
namespace WEBAPI_Demo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; }
    }
}
