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
    public class ClassesConfig : IEntityTypeConfiguration<Classes>
    {
        public void Configure(EntityTypeBuilder<Classes> builder)
        {
            builder.HasKey(C => C.Id);
            builder.Property(C=>C.Name).HasColumnType("nvarchar").HasMaxLength(50);

            builder.Property(C=>C.DateOfCreation).HasDefaultValueSql("GETDate()");

            builder.HasMany(x => x.Students).WithOne(e => e.Classes).HasForeignKey(e => e.ClassesId);


        }
    }
}
