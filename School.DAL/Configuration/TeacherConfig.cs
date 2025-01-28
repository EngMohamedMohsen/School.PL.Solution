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
    public class TeacherConfig : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(T=>T.ID);

            builder.Property(T=>T.ID).UseIdentityColumn(1000, 1);

            builder.Property(T=>T.T_Name).HasColumnName("TeacherName").HasColumnType("nvarchar").HasMaxLength(50);


            builder.Property(T => T.DateOfCreation).HasDefaultValueSql("GETDate()");
        }
    }
}
