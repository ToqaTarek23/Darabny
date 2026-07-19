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
    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.HasKey(ch => ch.Id);

            builder.Property(ch => ch.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(ch => ch.LessonCounter)
                .HasDefaultValue(0);

            builder.Property(ch => ch.ChapterLength)
                .HasDefaultValue(0);

            builder.HasOne(ch => ch.Course)
                .WithMany(c => c.Chapters)
                .HasForeignKey(ch => ch.CourseId);

            builder.HasMany(ch => ch.Lessons)
                .WithOne(l => l.Chapter)
                .HasForeignKey(l => l.ChapterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(ch => ch.CourseId)
                .IsRequired();


        }
    }
}
