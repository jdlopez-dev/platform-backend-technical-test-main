using AcmeCorporation.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorporation.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
                new Person
                {
                    Id = 1,
                    Name = "Sergio",
                    Age = 45,
                    Document = "59087066C"
                },
                new Person
                {
                    Id = 2,
                    Name = "Carmen",
                    Age = 44,
                    Document = "40596167V"
                }
            );
        }
    }
}