﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(SocialDbContext))]
    partial class SocialDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("DataAccess.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("About")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateBirth")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Login");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DataAccess.Entities.Message", b =>
                {
                    b.Property<int>("OpId")
                        .HasColumnType("int");

                    b.Property<int>("MessageId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<string>("PosterId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SendDate")
                        .HasColumnType("datetime2");

                    b.HasKey("OpId", "MessageId");

                    b.HasIndex("PosterId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("DataAccess.Entities.OpMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<string>("PosterId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SendDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PosterId");

                    b.ToTable("OpMessages");
                });

            modelBuilder.Entity("DataAccess.Entities.OpMessageTag", b =>
                {
                    b.Property<string>("TagId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("OpId")
                        .HasColumnType("int");

                    b.HasKey("TagId", "OpId");

                    b.HasIndex("OpId");

                    b.ToTable("OpMessageTags");
                });

            modelBuilder.Entity("DataAccess.Entities.SecurePassword", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Hashed")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("SecurePasswords");
                });

            modelBuilder.Entity("DataAccess.Entities.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("SubscriberId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TargetId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("SubscriberId");

                    b.HasIndex("TargetId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("DataAccess.Entities.Tag", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("DataAccess.Entities.TagBan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("BanIssuedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Cancelled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModeratorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TagId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ModeratorId");

                    b.HasIndex("TagId");

                    b.HasIndex("UserId");

                    b.ToTable("TagBans");
                });

            modelBuilder.Entity("DataAccess.Entities.TagModerator", b =>
                {
                    b.Property<string>("TagId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.HasKey("TagId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("TagModerators");
                });

            modelBuilder.Entity("DataAccess.Entities.Message", b =>
                {
                    b.HasOne("DataAccess.Entities.ApplicationUser", "Poster")
                        .WithMany()
                        .HasForeignKey("PosterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Poster");
                });

            modelBuilder.Entity("DataAccess.Entities.OpMessage", b =>
                {
                    b.HasOne("DataAccess.Entities.ApplicationUser", "Poster")
                        .WithMany()
                        .HasForeignKey("PosterId");

                    b.Navigation("Poster");
                });

            modelBuilder.Entity("DataAccess.Entities.OpMessageTag", b =>
                {
                    b.HasOne("DataAccess.Entities.OpMessage", "OpMessage")
                        .WithMany("Tags")
                        .HasForeignKey("OpId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OpMessage");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("DataAccess.Entities.SecurePassword", b =>
                {
                    b.HasOne("DataAccess.Entities.ApplicationUser", "User")
                        .WithOne("Password")
                        .HasForeignKey("DataAccess.Entities.SecurePassword", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entities.Subscription", b =>
                {
                    b.HasOne("DataAccess.Entities.ApplicationUser", "Subscriber")
                        .WithMany("Subscriptions")
                        .HasForeignKey("SubscriberId");

                    b.HasOne("DataAccess.Entities.ApplicationUser", "Target")
                        .WithMany("Subscribers")
                        .HasForeignKey("TargetId");

                    b.Navigation("Subscriber");

                    b.Navigation("Target");
                });

            modelBuilder.Entity("DataAccess.Entities.TagBan", b =>
                {
                    b.HasOne("DataAccess.Entities.ApplicationUser", "Moderator")
                        .WithMany()
                        .HasForeignKey("ModeratorId");

                    b.HasOne("DataAccess.Entities.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId");

                    b.HasOne("DataAccess.Entities.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Moderator");

                    b.Navigation("Tag");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entities.TagModerator", b =>
                {
                    b.HasOne("DataAccess.Entities.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tag");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entities.ApplicationUser", b =>
                {
                    b.Navigation("Password");

                    b.Navigation("Subscribers");

                    b.Navigation("Subscriptions");
                });

            modelBuilder.Entity("DataAccess.Entities.OpMessage", b =>
                {
                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
