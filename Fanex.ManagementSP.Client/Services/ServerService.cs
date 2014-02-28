// --------------------------------------------------------------------------
// <copyright file="ServerService.cs" company="Nexcel Solutions Vietnam">
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
    /// The server service class.
    /// </summary>
    public class ServerService : BaseService
    {
        private string _apiServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerService"/> class.
        /// </summary>
        /// <param name="apiUrl">The API URL.</param>
        public ServerService(string apiUrl)
            : base(apiUrl)
        {
            _apiServer = APISERVER + "server";
        }

        /// <summary>
        /// Gets all the servers.
        /// </summary>
        /// <returns>List of server.</returns>
        public IEnumerable<Server> GetAll()
        {
            try
            {
                var list = new List<Server>();

                var taskRequest = AsyncRequsest.MakeRequest(_apiServer);

                list = JsonConvert.DeserializeObject<List<Server>>(taskRequest.Result);

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
        /// <param name="name">The server name.</param>
        /// <param name="ipLocal">The IP local.</param>
        /// <param name="ipUat">The IP UAT.</param>
        /// <param name="ipProduction">The IP production.</param>
        /// <param name="description">The description.</param>
        /// <returns>Result of update.</returns>
        public bool InsertOrUpdate(
            int id,
            string name,
            string ipLocal,
            string ipUat,
            string ipProduction,
            string description,
            int modifiedById)
        {
            try
            {
                name = name.Trim();
                ipLocal = ipLocal.Trim();
                ipUat = ipUat.Trim();
                ipProduction = ipProduction.Trim();
                description = description.Trim();

                var format = "?id={0}&name={1}&ipLocal={2}&ipUat={3}&ipProduction={4}&description={5}&modifiedById={6}";
                var query = string.Format(format, id, name, ipLocal, ipUat, ipProduction, description, modifiedById);

                var taskRequest = AsyncRequsest.MakeRequest(_apiServer + query, WebRequestMethods.Http.Post);

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

                var taskRequest = AsyncRequsest.MakeRequest(_apiServer + query, "DELETE");

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
