// --------------------------------------------------------------------------
// <copyright file="IServerService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Core.Services.Interface
{
    using System.Collections.Generic;
    using Fanex.ManagementSP.Entities;

    /// <summary>
    /// The server service interface.
    /// </summary>
    public interface IServerService
    {
        /// <summary>
        /// Gets all of servers.
        /// </summary>
        /// <returns>List of server.</returns>
        IEnumerable<Server> GetAll();

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
        bool InsertOrUpdate(
            int id,
            string name,
            string ipLocal,
            string ipUat,
            string ipProduction,
            string description);

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Result of delete.</returns>
        bool Delete(int id);
    }
}
