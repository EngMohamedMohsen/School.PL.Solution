using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.DAL.Configuration
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(S=>S.S_Name).HasColumnName("StudentName").HasColumnType("nvarchar").HasMaxLength(50);

            builder.Property(S =>S.DateOfCreation).HasDefaultValueSql("GETDate()");
        }
    }
}
