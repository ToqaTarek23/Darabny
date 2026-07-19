using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DarabnyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarabnyProject.Configration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(c => c.URL)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(c => c.Price)
                .HasPrecision(18, 2)
                .HasDefaultValue(0);

            builder.Property(c => c.DiscountedPrice)
                .HasPrecision(18, 2);

            builder.Property(c => c.PriceType)
                .IsRequired();

            builder.Property(c => c.StudentNumbers)
                .HasDefaultValue(0);

            builder.Property(c => c.CourseLength)
                .HasDefaultValue(0); 

            builder.Property(c => c.Level)
                .IsRequired(); 

            builder.Property(c => c.Language)
                .HasMaxLength(50)
                .HasDefaultValue("English");

            builder.Property(c => c.ReviewsNumbers)
                .HasDefaultValue(0);

            builder.HasOne(c => c.Category)
                .WithMany(cat => cat.Courses)
                .HasForeignKey(c => c.CategoryId);

            builder.HasOne(c => c.Instructor)
                .WithMany(u => u.TeachingCourses)
                .HasForeignKey(c => c.InstructorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(c => c.Chapters)
                .WithOne(ch => ch.Course)
                .HasForeignKey(ch => ch.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Reviews)
                .WithOne(r => r.Course)
                .HasForeignKey(r => r.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Enrollments)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Favorites)
                .WithOne(f => f.Course)
                .HasForeignKey(f => f.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Certificates)
                .WithOne(cert => cert.Course)
                .HasForeignKey(cert => cert.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Requirements)
                .WithOne(r => r.Course)
                .HasForeignKey(r => r.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.WhatWillLearn)
                .WithOne(w => w.Course)
                .HasForeignKey(w => w.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.CategoryId)
                .IsRequired();

        }
    }
}
