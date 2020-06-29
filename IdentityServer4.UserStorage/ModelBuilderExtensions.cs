// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer4.UserStorage.Extensions
{
    /// <summary>
    /// Extension methods to define the database schema for the configuration and operational data stores.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Configures the persisted grant context.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="storeOptions">The store options.</param>
        public static void ConfigureIdentityUserContext(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUser>(grant =>
            {
                grant.ToTable("Users");
                grant.Property(x => x.SubjectId).HasMaxLength(200);
                grant.Property(x => x.Username).HasMaxLength(50).IsRequired();
                grant.Property(x => x.Password).HasMaxLength(200).IsRequired();
                grant.Property(x => x.ProviderName).HasMaxLength(200);
                grant.Property(x => x.ProviderSubjectId).HasMaxLength(200);
                grant.HasMany(c => c.Claims).WithOne(c => c.IdentityUser).OnDelete(DeleteBehavior.Cascade);
                grant.HasKey(x => x.UserId);

                grant.HasIndex(x => new { x.SubjectId, x.UserId });
            });

            modelBuilder.Entity<UserClaims>(grant =>
            {
                grant.ToTable("UserClaims");
                grant.Property(x => x.Key).HasMaxLength(200).IsRequired();
                grant.Property(x => x.Value).HasMaxLength(200).IsRequired();
                grant.HasKey(x => x.Id);
            });
        }
    }
}
