using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

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
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Message>()
                .HasOne(x => x.OpMessage)
                .WithMany(x => x!.Messages)
                .HasForeignKey(x => x.OpId);
            modelBuilder.Entity<Message>()
                .HasOne(x => x.Poster)
                .WithMany(x => x!.Messages);

            modelBuilder.Entity<OpMessage>()
                .HasOne(x => x.Poster)
                .WithMany(x => x!.Posts);

            modelBuilder.Entity<OpMessageTag>()
                .HasKey(x => new {x.TagId, x.OpId});
            modelBuilder.Entity<OpMessageTag>()
                .HasOne(x => x.Tag)
                .WithMany(x => x.OpMessages)
                .HasForeignKey(x => x.TagId);
            modelBuilder.Entity<OpMessageTag>()
                .HasOne(x => x.OpMessage)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.OpId);

            modelBuilder.Entity<Subscription>()
                .HasOne(x => x.Subscriber)
                .WithMany(x => x!.Subscriptions)
                .HasForeignKey(x => x.SubscriberId);
            modelBuilder.Entity<Subscription>()
                .HasOne(x => x.Target)
                .WithMany(x => x!.Subscribers)
                .HasForeignKey(x => x.TargetId);

            modelBuilder.Entity<TagBan>()
                .HasOne(x => x.Tag)
                .WithMany(x => x!.Bans);
            modelBuilder.Entity<TagBan>()
                .HasOne(x => x.User)
                .WithMany(x => x!.BansReceived);
            modelBuilder.Entity<TagBan>()
                .HasOne(x => x.Moderator)
                .WithMany(x => x!.BansIssued);

            modelBuilder.Entity<TagModerator>()
                .HasKey(x => new {x.TagId, x.UserId});
            modelBuilder.Entity<TagModerator>()
                .HasOne(x => x.Tag)
                .WithMany(x => x!.Moderators)
                .HasForeignKey(x => x.TagId);
            modelBuilder.Entity<TagModerator>()
                .HasOne(x => x.User)
                .WithMany(x => x!.ModeratorOfTags)
                .HasForeignKey(x => x.UserId);
        }
    }
}