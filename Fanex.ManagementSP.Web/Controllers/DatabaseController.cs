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
        /// <param name="userLogin">The user identifier.</param>
        /// <param name="userPassword">The password.</param>
        [AcceptVerbs]
        public int Post(int id, string name, string description, int serverId, string userLogin, string userPassword, int modifiedById)
        {
            description = description != null ? description : string.Empty;
            userLogin = userLogin != null ? userLogin : string.Empty;
            userPassword = userPassword != null ? userPassword : string.Empty;

            _databaseService.InsertOrUpdate(ref id, name, description, serverId, userLogin, userPassword, modifiedById);

            return id;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Delete(int id, int modifiedById)
        {
            _databaseService.Delete(id, modifiedById);
        }
    }
}
