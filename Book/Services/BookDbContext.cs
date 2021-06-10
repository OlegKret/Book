using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.Models;
using Microsoft.EntityFrameworkCore;


namespace Book.Services
{
    public class BookDbContext: DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public virtual DbSet<Models.Book> Books { get; set; }
        public virtual DbSet<Models.Author> Authors { get; set; }
        public virtual DbSet<Models.Review> Reviews { get; set; }
        public virtual DbSet<Models.Reviewer> Reviewers { get; set; }
        public virtual DbSet<Models.Category> Categories { get; set; }
        public virtual DbSet<Models.Country> Countries { get; set; }
        public virtual DbSet<Models.BookAuthor> BookAuthors { get; set; }
        public virtual DbSet<Models.BookCategory> BookCategories { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookCategory>()
                .HasKey(bc => new {bc.BookId, bc.CategoryId});
            modelBuilder.Entity<BookCategory>()
                .HasOne(b => b.Book)
                .WithMany(bc => bc.BookCategories)
                .HasForeignKey(b => b.BookId);
            modelBuilder.Entity<BookCategory>()
                .HasOne(c => c.Category)
                .WithMany(bc => bc.BookCategories)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.BookId, ba.AuthorId });
            modelBuilder.Entity<BookAuthor>()
                .HasOne(a => a.Author)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(a => a.AuthorId);
            modelBuilder.Entity<BookAuthor>()
                .HasOne(b => b.Book)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(b => b.BookId);
        }
    }
}
