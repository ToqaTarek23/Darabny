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
    public class WhatWillLearnConfiguration : IEntityTypeConfiguration<WhatWillLearn>
    {
        public void Configure(EntityTypeBuilder<WhatWillLearn> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Objective)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(w => w.Course)
                .WithMany(c => c.WhatWillLearn)
                .HasForeignKey(w => w.CourseId);

            builder.Property(w => w.CourseId)
                .IsRequired();
        }
    }
}
