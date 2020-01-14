using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EmployeeRecordKeeping.Models
{
    public partial class rksContext : DbContext
    {
        public rksContext()
        {
        }

        public rksContext(DbContextOptions<rksContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employee { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=2720572;database=rks");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employee", "rks");

                entity.Property(e => e.Employeeid)
                    .HasColumnName("employeeid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Age)
                    .HasColumnName("age")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Department)
                    .HasColumnName("department")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasColumnName("full_name")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .IsUnicode(false);

                entity.Property(e => e.Position)
                    .HasColumnName("position")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
        }
    }
}
