﻿// <auto-generated />
using IdentityServer4.UserStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IdentityServer4EntityFramework.Migrations
{
    [DbContext(typeof(IdentityUserDbContext))]
    [Migration("20200629052011_CreateUser")]
    partial class CreateUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5");

            modelBuilder.Entity("IdentityServer4.UserStorage.IdentityUser", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(200);

                    b.Property<string>("ProviderName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(200);

                    b.Property<string>("ProviderSubjectId")
                        .HasColumnType("TEXT")
                        .HasMaxLength(200);

                    b.Property<string>("SubjectId")
                        .HasColumnType("TEXT")
                        .HasMaxLength(200);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.HasKey("UserId");

                    b.HasIndex("SubjectId", "UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("IdentityServer4.UserStorage.UserClaims", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("IdentityUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(200);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("IdentityUserId");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("IdentityServer4.UserStorage.UserClaims", b =>
                {
                    b.HasOne("IdentityServer4.UserStorage.IdentityUser", "IdentityUser")
                        .WithMany("Claims")
                        .HasForeignKey("IdentityUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}