using System;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DataAccess
{
    public class SocialDbContext : DbContext
    {
        public SocialDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<OpMessage> OpMessages { get; set; } = null!;
        public DbSet<OpMessageTag> OpMessageTags { get; set; } = null!;
        public DbSet<Subscription> Subscriptions { get; set; } = null!;
        public DbSet<SecurePassword> SecurePasswords { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<TagBan> TagBans { get; set; } = null!;
        public DbSet<TagModerator> TagModerators { get; set; } = null!;
        public DbSet<ApplicationUser> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>()
                .HasKey(x => new {x.OpId, x.MessageId});
            modelBuilder.Entity<Message>()
                .Property(x => x.MessageId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
            modelBuilder.Entity<Message>()
                .HasOne(x => x.OpMessage)
                .WithMany(x => x!.Messages)
                .HasForeignKey(x => x.OpId);
            modelBuilder.Entity<Message>()
                .HasOne(x => x.Poster)
                .WithMany(x => x!.Messages);

            modelBuilder.Entity<OpMessageTag>()
                .HasKey(x => new {x.TagId, x.OpId});

            modelBuilder.Entity<Subscription>()
                .HasOne(x => x.Subscriber)
                .WithMany(x => x.Subscriptions)
                .HasForeignKey(x => x.SubscriberId);
            modelBuilder.Entity<Subscription>()
                .HasOne(x => x.Target)
                .WithMany(x => x.Subscribers)
                .HasForeignKey(x => x.TargetId);

            modelBuilder.Entity<TagModerator>()
                .HasKey(x => new {x.TagId, x.UserId});
        }
    }
}