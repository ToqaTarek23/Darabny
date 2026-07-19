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
    public class CertificatesConfiguration : IEntityTypeConfiguration<Certificates>
    {
        public void Configure(EntityTypeBuilder<Certificates> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.CertificateUrl)
                .HasMaxLength(500);

            builder.HasOne(c => c.User)
                .WithMany(u => u.Certificates)
                .HasForeignKey(c => c.UserId);

            builder.HasOne(c => c.Course)
                .WithMany(c => c.Certificates)
                .HasForeignKey(c => c.CourseId);

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.Property(c => c.CourseId)
                .IsRequired();

        }
    }
}
