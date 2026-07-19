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
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(l => l.Type)
                .IsRequired();

            builder.Property(l => l.LessonLength)
                .IsRequired(); 

            builder.Property(l => l.IsFreePreview)
                .HasDefaultValue(false);

            builder.Property(l => l.LessonURL)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(l => l.Chapter)
                .WithMany(ch => ch.Lessons)
                .HasForeignKey(l => l.ChapterId);

            builder.HasMany(l => l.Progresses)
                .WithOne(p => p.Lesson)
                .HasForeignKey(p => p.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(l => l.ChapterId)
               .IsRequired();

        }
    }
}
