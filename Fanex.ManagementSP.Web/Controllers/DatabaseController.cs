// --------------------------------------------------------------------------
// <copyright file="DatabaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Web.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Fanex.ManagementSP.Core.Services.Impl;
    using Fanex.ManagementSP.Core.Services.Interface;
    using Fanex.ManagementSP.Entities;

    /// <summary>
    /// The databases API controller.
    /// </summary>
    public class DatabaseController : ApiController
    {
        private readonly IDatabaseService _databaseService = new DatabaseService();

        /// <summary>
        /// Gets all databases.
        /// </summary>
        /// <returns>List of databases.</returns>
        public IEnumerable<Database> Get()
        {
            return _databaseService.GetAll();
        }

        /// <summary>
        /// Posts the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The database name.</param>
        /// <param name="description">The description.</param>
        /// <param name="serverId">The server identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="password">The password.</param>
        [AcceptVerbs]
        public void Post(int id, string name, string description, int serverId, string userId, string password)
        {
            description = description != null ? description : string.Empty;
            userId = userId != null ? userId : string.Empty;
            password = password != null ? password : string.Empty;

            _databaseService.InsertOrUpdate(id, name, description, serverId, userId, password);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Delete(int id)
        {
            _databaseService.Delete(id);
        }
    }
}
