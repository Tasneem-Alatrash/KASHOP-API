using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KASHOP.DAL.Models;
namespace KASHOP.DAL
{
    public class ApplicationDbContext : DbContext
    {
      public DbSet<Category> Categories { get; set; }
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    }
}