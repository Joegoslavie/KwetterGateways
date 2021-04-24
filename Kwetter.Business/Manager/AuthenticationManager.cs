﻿namespace Kwetter.Business.Manager
{
    using Kwetter.Business.Model;
    using Kwetter.Business.Service;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AuthenticationManager
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<AuthenticationManager> logger;

        /// <summary>
        /// 
        /// </summary>
        private readonly AuthenticationService authService;

        /// <summary>
        /// 
        /// </summary>
        private readonly ProfileService profileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationManager"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="authenticationService"></param>
        /// <param name="profileService"></param>
        public AuthenticationManager(ILogger<AuthenticationManager> logger, AuthenticationService authenticationService, ProfileService profileService)
        {
            this.logger = logger;
            this.authService = authenticationService;
            this.profileService = profileService;
        }

        /// <summary>
        /// Tries to sign in a user.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public async Task<Account> SignIn(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(username));
            }

            // todo: add calls to other services;

            var account = await this.authService.SignIn(username, password).ConfigureAwait(false);
            this.logger.LogInformation($"User {username} authentication successfully");

            return account;
        }

        /// <summary>
        /// Tries to sign up a user.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <returns></returns>
        public async Task<Account> Register(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(username));
            }

            var account = await this.authService.Register(username, password).ConfigureAwait(false);
            this.logger.LogInformation($"@{username} created by user");

            return account;
        }
    }
}