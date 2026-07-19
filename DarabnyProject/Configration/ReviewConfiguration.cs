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
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Comment)
                .HasMaxLength(1000);

            builder.Property(r => r.Rating)
                .HasPrecision(2,1)
                .IsRequired();

            builder.Property(r => r.IsApproved)
                .HasDefaultValue(false);

            builder.HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            builder.HasOne(r => r.Course)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CourseId);

            builder.Property(r => r.UserId)
                .IsRequired();

            builder.Property(r => r.CourseId)
                .IsRequired();

            builder.HasIndex(r => new { r.UserId, r.CourseId })
                .IsUnique();
        }
    }
}
