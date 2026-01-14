using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using App.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductComment> ProductComments => Set<ProductComment>();



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductComment>()
                .HasOne(pc => pc.User)
                .WithMany()
                .HasForeignKey(pc => pc.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany()
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.NoAction);

            var seedDate = new DateTime(2026, 1, 1);

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "seller", CreatedAt = seedDate },
                new Role { Id = 2, Name = "buyer", CreatedAt = seedDate },
                new Role { Id = 3, Name = "admin", CreatedAt = seedDate }
             );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@site.com",
                    FirstName = "Admin",
                    LastName = "User",
                    Password = "Admin123!", 
                    RoleId = 3,             
                    Enabled = true,
                    CreatedAt = seedDate
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Elektronik", Color = "FF5733", IconCssClass = "bi bi-phone", CreatedAt = seedDate },
                new Category { Id = 2, Name = "Giyim", Color = "33A1FF", IconCssClass = "bi bi-bag", CreatedAt = seedDate },
                new Category { Id = 3, Name = "Ayakkabı", Color = "8E44AD", IconCssClass = "bi bi-shoe", CreatedAt = seedDate },
                new Category { Id = 4, Name = "Kozmetik", Color = "E91E63", IconCssClass = "bi bi-heart", CreatedAt = seedDate },
                new Category { Id = 5, Name = "Kitap", Color = "2ECC71", IconCssClass = "bi bi-book", CreatedAt = seedDate },
                new Category { Id = 6, Name = "Ev & Yaşam", Color = "F1C40F", IconCssClass = "bi bi-house", CreatedAt = seedDate },
                new Category { Id = 7, Name = "Spor", Color = "16A085", IconCssClass = "bi bi-bicycle", CreatedAt = seedDate },
                new Category { Id = 8, Name = "Oyuncak", Color = "E67E22", IconCssClass = "bi bi-emoji-smile", CreatedAt = seedDate },
                new Category { Id = 9, Name = "Aksesuar", Color = "34495E", IconCssClass = "bi bi-watch", CreatedAt = seedDate },
                new Category { Id = 10, Name = "Market", Color = "95A5A6", IconCssClass = "bi bi-cart", CreatedAt = seedDate }
            );
        }

    }
    }

