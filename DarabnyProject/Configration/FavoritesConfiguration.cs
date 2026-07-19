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
    public class FavoritesConfiguration : IEntityTypeConfiguration<Favorites>
    {
        public void Configure(EntityTypeBuilder<Favorites> builder)
        {
            builder.HasKey(f => f.Id);

            builder.HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId);

            builder.HasOne(f => f.Course)
                .WithMany(c => c.Favorites)
                .HasForeignKey(f => f.CourseId);

            builder.Property(f => f.UserId)
                .IsRequired();

            builder.Property(f => f.CourseId)
                .IsRequired();

            builder.HasIndex(f => new { f.UserId, f.CourseId })
                .IsUnique();
        }
    }
    }
