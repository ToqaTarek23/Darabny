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
    public class RequirementsConfiguration : IEntityTypeConfiguration<Requirements>
    {
        public void Configure(EntityTypeBuilder<Requirements> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Requirement)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(r => r.Course)
                .WithMany(c => c.Requirements)
                .HasForeignKey(r => r.CourseId);

            builder.Property(r => r.CourseId)
                .IsRequired();

        }
    }
}
