using System;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class SocialDbContext : DbContext
    {
        public SocialDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Message>? Messages { get; set; }
        public DbSet<OpMessage>? OpMessages { get; set; }
        public DbSet<OpMessageTag>? OpMessageTags { get; set; }
        public DbSet<SecurePassword>? SecurePasswords { get; set; }
        public DbSet<Subscription>? Subscriptions { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<TagBan>? TagBans { get; set; }
        public DbSet<TagModerator>? TagModerators { get; set; }
        public DbSet<User>? Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Message>(x =>
                x.HasKey(x => new {x.OpId, x.MessageId}));

            modelBuilder.Entity<OpMessageTag>(x =>
                x.HasKey(x => new {x.TagId, x.OpId}));

            modelBuilder.Entity<Subscription>(x =>
                x.HasKey(x => new {x.SubscriberId, x.TargetId}));

            modelBuilder.Entity<TagModerator>(x =>
                x.HasKey(x => new {x.TagId, x.UserId}));
        }
    }
}