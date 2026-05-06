using Eshop.Module.Localization.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Localization.Data
{
    internal class LocalizationDbContext : DbContext
    {
        public DbSet<Resource> Resources { get; set; }
        public LocalizationDbContext(DbContextOptions<LocalizationDbContext> options) : base(options)
        {
        }

     
    }
}
