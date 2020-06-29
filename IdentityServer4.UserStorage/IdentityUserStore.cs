﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System;

namespace IdentityServer4.UserStorage
{
    /// <summary>
    /// Store for test users
    /// </summary>
    public class IdentityUserStore
    {
        private readonly IdentityUserDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityUserStore"/> class.
        /// </summary>
        /// <param name="users">The users.</param>
        public IdentityUserStore(IdentityUserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool ValidateCredentials(string username, string password)
        {
            var user = FindByUsername(username);

            if (user != null)
            {
                if (string.IsNullOrWhiteSpace(user.Password) && string.IsNullOrWhiteSpace(password))
                {
                    return true;
                }

                return user.Password.Equals(password);
            }

            return false;
        }

        /// <summary>
        /// Finds the user by subject identifier.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        public IdentityUser FindBySubjectId(string subjectId)
        {
            return _dbContext.Users.FirstOrDefault(x => x.SubjectId == subjectId);
        }

        /// <summary>
        /// Finds the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public IdentityUser FindByUsername(string username)
        {
            return _dbContext.Users.FirstOrDefault(x => x.Username == username);
        }

        /// <summary>
        /// Finds the user by external provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public IdentityUser FindByExternalProvider(string provider, string userId)
        {
            return _dbContext.Users.FirstOrDefault(x =>
                x.ProviderName == provider &&
                x.ProviderSubjectId == userId);
        }

        /// <summary>
        /// Automatically provisions a user.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="claims">The claims.</param>
        /// <returns></returns>
        public IdentityUser AutoProvisionUser(string provider, string userId, List<Claim> claims)
        {
            ICollection<UserClaims> filtered = new List<UserClaims>();

            foreach (var claim in claims)
            {
                // if the external system sends a display name - translate that to the standard OIDC name claim
                if (claim.Type == ClaimTypes.Name)
                {
                    filtered.Add(new UserClaims(JwtClaimTypes.Name, claim.Value));
                }
                // if the JWT handler has an outbound mapping to an OIDC claim use that
                else if (JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.ContainsKey(claim.Type))
                {
                    filtered.Add(new UserClaims(JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap[claim.Type], claim.Value));
                }
                // copy the claim as-is
                else
                {
                    filtered.Add(new UserClaims(claim.Type, claim.Value));
                }
            }

            // if no display name was provided, try to construct by first and/or last name
            if (!filtered.Any(x => x.Key == JwtClaimTypes.Name))
            {
                var first = filtered.FirstOrDefault(x => x.Key == JwtClaimTypes.GivenName)?.Value;
                var last = filtered.FirstOrDefault(x => x.Key == JwtClaimTypes.FamilyName)?.Value;
                if (first != null && last != null)
                {
                    filtered.Add(new UserClaims(JwtClaimTypes.Name, first + " " + last));
                }
                else if (first != null)
                {
                    filtered.Add(new UserClaims(JwtClaimTypes.Name, first));
                }
                else if (last != null)
                {
                    filtered.Add(new UserClaims(JwtClaimTypes.Name, last));
                }
            }

            // create a new unique subject id
            var sub = CryptoRandom.CreateUniqueId();

            // check if a display name is available, otherwise fallback to subject id
            var name = filtered.FirstOrDefault(c => c.Key == JwtClaimTypes.Name)?.Value ?? sub;

            // create new user
            var user = new IdentityUser
            {
                SubjectId = sub,
                Username = name,
                ProviderName = provider,
                ProviderSubjectId = userId,
                Claims = filtered
            };

            // add user to in-memory store
            _dbContext.Users.Add(user);

            _dbContext.SaveChanges();

            return user;
        }
    }
}