using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DarabnyProject.Models;
using System.Reflection;

namespace DarabnyProject.Data
{
    public class DarabnyDbContext : IdentityDbContext<ApplicationUser>
    {
        public DarabnyDbContext(DbContextOptions<DarabnyDbContext> options) : base(options) { }

        public DarabnyDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(
                    @"Server=DESKTOP-JFKF5FQ\MSSQLSERVER05;Database=DarabnyDB;Trusted_Connection=true;TrustServerCertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Category>     Categories  { get; set; }
        public DbSet<Course>       Courses     { get; set; }
        public DbSet<Chapter>      Chapters    { get; set; }
        public DbSet<Lesson>       Lessons     { get; set; }
        public DbSet<Enrollment>   Enrollments { get; set; }
        public DbSet<Review>       Reviews     { get; set; }
        public DbSet<Favorites>    Favorites   { get; set; }
        public DbSet<Certificates> Certificates { get; set; }
        public DbSet<Progress>     Progress    { get; set; }
        public DbSet<ContactUs>    ContactUs   { get; set; }
        public DbSet<Requirements> Requirements { get; set; }
        public DbSet<WhatWillLearn> WhatWillLearn { get; set; }
    }
}
