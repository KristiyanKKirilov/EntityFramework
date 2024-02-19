using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.
                Property(s => s.Name)
                .IsRequired()
                .IsUnicode(true)
                .HasMaxLength(100);

            builder.
                Property(s => s.PhoneNumber)
                .IsRequired()
                .HasMaxLength(10)
                .IsFixedLength();

            builder.
                Property(s => s.Birthday);



        }
    }
}
