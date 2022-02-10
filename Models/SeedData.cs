using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibApp.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                UserManager<Customer> userManager = serviceProvider.GetRequiredService<UserManager<Customer>>();

                SeedDBWithRoles(context);
                SeedDBWithMemberShipTypes(context);
                SeedDBWithGenres(context);
                SeedDBWithBooks(context);
                context.SaveChanges();
                
                SeedDBWithCustomers(context, userManager);
                SeedDBWithRentals(context);

                context.SaveChanges();
            }
        }

        public static void SeedDBWithMemberShipTypes(ApplicationDbContext context)
        {
            if (context.MembershipTypes.Any())
            {
                Console.WriteLine("Database already seeded with MemberShipTypes Data");
                return;
            }

            context.MembershipTypes.AddRange(
                new MembershipType
                {
                    Id = 1,
                    SignUpFee = 0,
                    DurationInMonths = 0,
                    DiscountRate = 0,
                    Name = "Basic"
                },
                new MembershipType
                {
                    Id = 2,
                    SignUpFee = 30,
                    DurationInMonths = 1,
                    DiscountRate = 10,
                    Name = "Simple"
                },
                new MembershipType
                {
                    Id = 3,
                    SignUpFee = 90,
                    DurationInMonths = 3,
                    DiscountRate = 15,
                    Name = "Advanced"
                },
                new MembershipType
                {
                    Id = 4,
                    SignUpFee = 300,
                    DurationInMonths = 12,
                    DiscountRate = 20,
                    Name = "Premium"
                });
        }

        public static void SeedDBWithCustomers(ApplicationDbContext context, UserManager<Customer> userManager)
        {
            if (context.Customers.Any())
            {
                Console.WriteLine("Database already seeded with Customers Data");
                return;
            }

            PasswordHasher<Customer> passwordHasher = new PasswordHasher<Customer>();

            Customer firstCustomer = new Customer
            {
                Name = "Paweł Malina",
                HasNewsletterSubscribed = true,
                MembershipTypeId = 3,
                Birthdate = new DateTime(1999, 5, 18),
                Email = "pawel.malina@gmail.com",
                NormalizedEmail = "pawel.malina@gmail.com",
                PasswordHash = passwordHasher.HashPassword(null, "zaq1@WSX"),
                UserName = "pawel.malina@gmail.com",
                NormalizedUserName = "pawel.malina@gmail.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            userManager.CreateAsync(firstCustomer).Wait();
            userManager.AddToRoleAsync(firstCustomer, "user").Wait();

            Customer secondCustomer = new Customer
            {
                Name = "Halina Malina",
                HasNewsletterSubscribed = true,
                MembershipTypeId = 1,
                Birthdate = new DateTime(1988, 5, 22),
                Email = "halina.malina@gmail.com",
                NormalizedEmail = "halina.malina@gmail.com",
                PasswordHash = passwordHasher.HashPassword(null, "zaq1@WSX"),
                UserName = "halina.malina@gmail.com",
                NormalizedUserName = "halina.malina@gmail.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            userManager.CreateAsync(secondCustomer).Wait();
            userManager.AddToRoleAsync(secondCustomer, "storeManager").Wait();

            Customer thirdCustomer = new Customer
            {
                Name = "Anna Malina",
                HasNewsletterSubscribed = false,
                MembershipTypeId = 1,
                Birthdate = new DateTime(2008, 11, 29),
                Email = "anna.malina@gmail.com",
                NormalizedEmail = "anna.malina@gmail.com",
                PasswordHash = passwordHasher.HashPassword(null, "zaq1@WSX"),
                UserName = "anna.malina@gmail.com",
                NormalizedUserName = "anna.malina@gmail.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            userManager.CreateAsync(thirdCustomer).Wait();
            userManager.AddToRoleAsync(thirdCustomer, "owner").Wait();
        }


        public static void SeedDBWithGenres(ApplicationDbContext context)
        {
            if (context.Genre.Any())
            {
                Console.WriteLine("Database already seeded with Genre Data");
                return;
            }

            context.Genre.AddRange(
                 new Genre { Id = 1, Name = "Komedia" },
                new Genre { Id = 2, Name = "Fantasy" },
                new Genre { Id = 3, Name = "Romans" },
                new Genre { Id = 4, Name = "Thriler" },
                new Genre{ Id = 5, Name = "Biografia" },
                new Genre{ Id = 6, Name = "Poemat" }
            );
        }

        public static void SeedDBWithBooks(ApplicationDbContext context)
        {
            if (context.Books.Any())
            {
                Console.WriteLine("Database already seeded with Books Data");
                return;
            }

            context.Books.AddRange(
                new Book
                {
                    Name = "Pan Tadeusz",
                    AuthorName = "Adam Mickiewicz",
                    GenreId = 6,
                    DateAdded = new DateTime(),
                    NumberAvailable = 20,
                    NumberInStock = 20,
                    ReleaseDate = new DateTime(1834, 6, 28)
                },
                 new Book
                 {
                     Name = "Świętoszek",
                     AuthorName = "Moliere",
                     GenreId = 1,
                     DateAdded = new DateTime(),
                     NumberAvailable = 5,
                     NumberInStock = 10,
                     ReleaseDate = new DateTime(1664, 5, 12)
                 }
            );
        }

        public static void SeedDBWithRentals(ApplicationDbContext context)
        {
            if (context.Rentals.Any())
            {
                Console.WriteLine("Database already seeded with Rentals Data");
                return;
            }

            Book firstBook = context.Books.Where(x => x.Name == "Pan Tadeusz").FirstOrDefault();
            Book secondBook = context.Books.Where(x => x.Name == "Świętoszek").FirstOrDefault();

            Customer firstCustomer = context.Customers.Where(x => x.Name == "Halina Malina").FirstOrDefault();
            Customer secondCustomer = context.Customers.Where(x => x.Name == "Paweł Malina").FirstOrDefault();
            Customer thirdCustomer = context.Customers.Where(x => x.Name == "Anna Malina").FirstOrDefault();
            Console.WriteLine("firstCustomer.Name => ");
            Console.WriteLine(firstCustomer.Name);

            context.Rentals.AddRange(
                new Rental
                {
                    Book = firstBook,
                    Customer = secondCustomer,
                    DateRented = DateTime.Now,
                },
                new Rental
                {
                    Book = secondBook,
                    Customer = firstCustomer,
                    DateRented = DateTime.Now,
                },
                new Rental
                {
                    Book = secondBook,
                    Customer = thirdCustomer,
                    DateRented = DateTime.Now,
                }
             );
        }

        public static void SeedDBWithRoles(ApplicationDbContext context)
        {
            if (context.Roles.Any())
            {
                Console.WriteLine("Database already seeded with Roles Data");
                return;
            }

            var roles = new List<IdentityRole>
            {
                new IdentityRole { Name = "User", NormalizedName = "user" },
                new IdentityRole { Name = "StoreManager", NormalizedName = "storemanager"},
                new IdentityRole { Name = "Owner", NormalizedName = "owner"}
            };

            context.Roles.AddRange(roles);
        }
    }
}