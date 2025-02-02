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
    public class AdminConfig : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.Property(A => A.Id).HasColumnName("Code");

            builder.Property(A=>A.Admin_Name).HasColumnName("FullName").HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
        }
    }
}
