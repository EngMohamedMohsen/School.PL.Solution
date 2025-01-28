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
            builder.HasKey(C => C.C_Id);

            builder.Property(C=>C.C_Id).UseIdentityColumn(1,1);

            builder.Property(C=>C.C_Name).HasColumnName("ClassName").HasColumnType("nvarchar").HasMaxLength(50);


            builder.Property(C=>C.DateOfCreation).HasDefaultValueSql("GETDate()");

        }
    }
}
