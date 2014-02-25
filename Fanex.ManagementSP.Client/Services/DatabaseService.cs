// --------------------------------------------------------------------------
// <copyright file="DatabaseService.cs" company="Nexcel Solutions Vietnam">
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
    /// The database service class.
    /// </summary>
    public class DatabaseService : BaseService
    {
        private string _apiDatabase;

        public DatabaseService(string apiUrl)
            : base(apiUrl)
        {
            _apiDatabase = APISERVER + "database";
        }

        /// <summary>
        /// Gets all the databases.
        /// </summary>
        /// <returns>List of database.</returns>
        public IEnumerable<Database> GetAll()
        {
            try
            {
                var list = new List<Database>();

                var taskRequest = AsyncRequsest.MakeRequest(_apiDatabase);

                list = JsonConvert.DeserializeObject<List<Database>>(taskRequest.Result);

                return list;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Inserts the or update.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The database name.</param>
        /// <param name="description">The description.</param>
        /// <param name="serverId">The server identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns>Result of updated.</returns>
        public bool InsertOrUpdate(
            int id,
            string name,
            string description,
            int serverId,
            string userId,
            string password,
            int modifiedById)
        {
            try
            {
                var format = "?id={0}&name={1}&description={2}&serverId={3}&userId={4}&password={5}&modifiedById={6}";
                var query = string.Format(format, id, name, description, serverId, userId, System.Web.HttpUtility.UrlEncode(password), modifiedById);

                var taskRequest = AsyncRequsest.MakeRequest(_apiDatabase + query, WebRequestMethods.Http.Post);

                var result = taskRequest.Result;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Result of delete.</returns>
        public bool Delete(int id, int modifiedById)
        {
            try
            {
                var format = "?id={0}&modifiedById={1}";
                var query = string.Format(format, id, modifiedById);

                var taskRequest = AsyncRequsest.MakeRequest(_apiDatabase + query, "DELETE");

                var result = taskRequest.Result;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }
    }
}
