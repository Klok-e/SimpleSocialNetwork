using System;
using DataAccess.Entities;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DataAccess
{
    public class SocialDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public SocialDbContext(DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) :
            base(options, operationalStoreOptions)
        {
        }

        public DbSet<Message>? Messages { get; set; }
        public DbSet<OpMessage>? OpMessages { get; set; }
        public DbSet<OpMessageTag>? OpMessageTags { get; set; }
        public DbSet<Subscription>? Subscriptions { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<TagBan>? TagBans { get; set; }
        public DbSet<TagModerator>? TagModerators { get; set; }
        public DbSet<ApplicationUser>? Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>()
                .HasKey(x => new {x.OpId, x.MessageId});

            modelBuilder.Entity<OpMessageTag>()
                .HasKey(x => new {x.TagId, x.OpId});

            modelBuilder.Entity<Subscription>()
                .HasOne(x => x.Subscriber)
                .WithMany(x => x!.Subscriptions)
                .HasForeignKey(x => x.SubscriberId);
            modelBuilder.Entity<Subscription>()
                .HasOne(x => x.Target)
                .WithMany(x => x!.Subscribers)
                .HasForeignKey(x => x.TargetId);

            modelBuilder.Entity<TagModerator>()
                .HasKey(x => new {x.TagId, x.UserId});
        }
    }
}