// --------------------------------------------------------------------------
// <copyright file="ServerController.cs" company="Nexcel Solutions Vietnam">
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
    /// The server API controller.
    /// </summary>
    public class ServerController : ApiController
    {
        private readonly IServerService _serverService = new ServerService();

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>List of server.</returns>
        public IEnumerable<Server> Get()
        {
            return _serverService.GetAll();
        }

        /// <summary>
        /// Posts the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The server name.</param>
        /// <param name="ipLocal">The IP local.</param>
        /// <param name="ipUat">The IP UAT.</param>
        /// <param name="ipProduction">The IP production.</param>
        /// <param name="description">The description.</param>
        public int Post(
            int id,
            string name,
            string ipLocal,
            string ipUat,
            string ipProduction,
            string description,
            int modifiedById)
        {
            _serverService.InsertOrUpdate(ref id, name, ipLocal, ipUat, ipProduction, description, modifiedById);

            return id;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Delete(int id, int modifiedById)
        {
            _serverService.Delete(id, modifiedById);
        }
    }
}
