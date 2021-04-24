﻿namespace Kwetter.Business.Service
{
    using Grpc.Net.Client;
    using Kwetter.Business.Exceptions;
    using Kwetter.Business.Factory;
    using Kwetter.Business.Model;
    using Microservice.ProfileGRPCService;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Profile service.
    /// </summary>
    public class ProfileService
    {
        /// <summary>
        /// Access to the app configuration.
        /// </summary>
        private AppSettings settings;

        public ProfileService(AppSettings settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Gets the profile related to the user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns><see cref="Profile"/>.</returns>
        public async Task<Profile> GetProfile(int userId)
        {
            var response = await this.ProfileClientCall(async client =>
            {
                return await client.GetProfileByIdAsync(new ProfileRequest { UserId = userId });
            });

            if (!response.Status)
            {
                throw new ProfileException(response.Message);
            }

            return ProfileFactory.Parse(response);
        }

        /// <summary>
        /// Gets the profile related to the username.
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns><see cref="Profile"/>.</returns>
        public async Task<Profile> GetProfileByUsername(string username)
        {
            var response = await this.ProfileClientCall(async client =>
            {
                return await client.GetProfileByUsernameAsync(new ProfileRequest { Username = username });
            });

            if (!response.Status)
            {
                throw new ProfileException(response.Message);
            }

            return ProfileFactory.Parse(response);
        }

        /// <summary>
        /// Gets the profiles associated with the given ids.
        /// </summary>
        /// <param name="userIds">List of user ids.</param>
        /// <returns>List of <see cref="Profile"/>.</returns>
        public async Task<IEnumerable<Profile>> GetMultiple(IEnumerable<int> userIds)
        {
            var response = await this.ProfileClientCall(async client =>
            {
                var request = new ProfileRequest();
                request.UserIds.AddRange(userIds);

                return await client.GetMultipleByIdAsync(request);
            });

            if (!response.Status)
            {
                throw new ProfileException(response.Message);
            }

            return ProfileFactory.Parse(response);
        }

        /// <summary>
        /// Create a service call.
        /// </summary>
        /// <typeparam name="TParsedResponse">Parsed response.</typeparam>
        /// <param name="responseHandler">Handler.</param>
        /// <returns></returns>
        private async Task<TParsedResponse> ProfileClientCall<TParsedResponse>(Func<ProfileGRPCService.ProfileGRPCServiceClient, Task<TParsedResponse>> responseHandler)
        {
            using (var channel = GrpcChannel.ForAddress(this.settings.AuthenticationServiceUrl))
            {
                var client = new ProfileGRPCService.ProfileGRPCServiceClient(channel);
                return await responseHandler(client);
            }
        }
    }
}