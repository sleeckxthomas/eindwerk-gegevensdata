using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace eindopdracht
{
    public class ShopContext : DbContext
    {
        public DbSet<gebruiker> gebruikers { get; set; }
        public DbSet<project> projecten { get; set; }
        public DbSet<toegewezenen> toegewezenen { get; set; }
        public DbSet<wetenschapper> wetenschappers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=eindopdracht_gegevensbeheer.db");
    }
}
