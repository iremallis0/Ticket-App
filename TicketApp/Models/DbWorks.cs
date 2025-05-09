using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
namespace TicketApp.Models
{
    public class DbWorks : DbContext
    {
        public DbWorks(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Comment> Comments { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Company - User : One to Many
            modelBuilder.Entity<Company>()
                .HasMany(d => d.Users)
                .WithOne(e => e.Company)
                .HasForeignKey(e => e.companyId)
                .OnDelete(DeleteBehavior.Restrict);

            // category- ticket : one to many
            modelBuilder.Entity<Category>()
                .HasMany(d => d.Tickets)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.categoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // user ticket : one to many
            modelBuilder.Entity<User>()
                .HasMany(d => d.Tickets)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.userId)
                .OnDelete(DeleteBehavior.Restrict);

            // company - ticket : one to many
            modelBuilder.Entity<Company>()
                .HasMany(d => d.Tickets)
                .WithOne(e => e.Company)
                .HasForeignKey(e => e.companyId)
                .OnDelete(DeleteBehavior.Restrict);

            // ticket- comment
            modelBuilder.Entity<Ticket>()
                .HasMany(d => d.Comments)
                .WithOne(e => e.Ticket)
                .HasForeignKey(e => e.ticketId)
                .OnDelete(DeleteBehavior.Restrict);

            // user comment
            modelBuilder.Entity<User>()
                .HasMany(d => d.Comments)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.userId)
                .OnDelete(DeleteBehavior.Restrict);

            //----------seed

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    companyId = 1,
                    companyName = "Netova Teknoloji"
                }
            );
            modelBuilder.Entity<Company>().HasData(
              new Company
              {
                  companyId = 2,
                  companyName = "Akedaş"
              }
          );
            modelBuilder.Entity<Company>().HasData(
             new Company
             {
                 companyId = 3,
                 companyName = "Kipaş"
             }
         );
            modelBuilder.Entity<Company>().HasData(
            new Company
            {
                companyId = 4,
                companyName = "Ramada"
            }
        );
            modelBuilder.Entity<Company>().HasData(
            new Company
            {
                companyId = 5,
                companyName = "Flera"
            }
        );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    userId = 1,
                    firstName = "İrem",
                    lastName = "ALLİŞ",
                    userType=0, // destek personeli
                    userActive=true,
                    emailAddress = "allis.irem46@gmail.com",
                    phoneNumber = "05432242899",
                    passwordHash = "MR1fMz6ItGLD3nfAFDDibmTdTwuiz5Pz9dq3R0bRQbQ=",
                    companyId = 1
                }
            );
            modelBuilder.Entity<User>().HasData(
               new User
               {
                   userId = 2,
                   firstName = "beril",
                   lastName = "yılmaz",
                   userType = 1, // destek personeli
                   userActive = true,
                   emailAddress = "beril@gmail.com", 
                   phoneNumber ="05465652323",
                   passwordHash = "LHzjTIR2twXK7HOaowNXgJz8fIsfdj56tJT7OscW9mY=",
                   companyId = 2

               }
           );

            //modelBuilder.Entity<Category>().HasData(
            //    new Category
            //    {
            //        categoryId = 1,
            //        categoryName = "Genel"
            //    }
            //);
        }
    }
}
