// --------------------------------------------------------------------------
// <copyright file="UserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Client.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Fanex.ManagementSP.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// The user service class.
    /// </summary>
    public class UserService : BaseService
    {
        private string _apiUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="apiUrl">The API URL.</param>
        public UserService(string apiUrl)
            : base(apiUrl)
        {
            _apiUser = APISERVER + "user";
        }

        /// <summary>
        /// Gets all the users.
        /// </summary>
        /// <returns>List of user.</returns>
        public IEnumerable<User> GetAll()
        {
            try
            {
                var list = new List<User>();

                var taskRequest = AsyncRequsest.MakeRequest(_apiUser);

                list = JsonConvert.DeserializeObject<List<User>>(taskRequest.Result);

                return list;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }
    }
}
