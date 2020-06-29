// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Test;
using IdentityServer4.UserStorage;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the IdentityServer builder
    /// </summary>
    public static class IdentityServerBuilderExtensions
    {
        /// <summary>
        /// Adds test users.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="users">The users.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddIdentityUsers(this IIdentityServerBuilder builder)
        {
            builder.Services.AddScoped<IdentityUserStore>();
            builder.AddProfileService<IdentityUserProfileService>();
            builder.AddResourceOwnerValidator<IdentityUserResourceOwnerPasswordValidator>();

            return builder;
        }
    }
}