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
    public class ProgressConfiguration : IEntityTypeConfiguration<Progress>
    {
        public void Configure(EntityTypeBuilder<Progress> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.IsCompleted)
                .HasDefaultValue(false);


            builder.HasIndex(p => new { p.UserId, p.LessonId })
                .IsUnique();

            builder.HasOne(p => p.User)
                .WithMany(u => u.Progress)
                .HasForeignKey(p => p.UserId);

            builder.HasOne(p => p.Lesson)
                .WithMany(l => l.Progresses)
                .HasForeignKey(p => p.LessonId);


            builder.Property(p => p.UserId)
                .IsRequired();

            builder.Property(p => p.LessonId)
                .IsRequired();

        }
    }
}
