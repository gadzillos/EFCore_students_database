using Homework_6.Context.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Homework_6
{
    public class AppContext : DbContext
    {
        public DbSet<Faculty> Faculties { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Student> Students { get; set; }

        public AppContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=universitydb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Faculty>()
                .HasIndex(f => f.Name)
                .IsUnique();

            builder.Entity<Group>()
                .HasIndex(g => g.Number)
                .IsUnique();

            builder.Entity<Faculty>().HasData(
            new Faculty[]
            {
                new Faculty { Id = 1, Name = "Физический факультет"}
            });

            builder.Entity<Group>().HasData(
            new Group[]
            {
                new Group { Id = 1, Name="Радиофизика", Number = "P1101", FacultyRefId = 1},
                new Group { Id = 2, Name="Микроэлектроника", Number = "P1102", FacultyRefId = 1},
                new Group { Id = 3, Name="Общая физика", Number = "P1103", FacultyRefId = 1}
            });
        }
    }
}
