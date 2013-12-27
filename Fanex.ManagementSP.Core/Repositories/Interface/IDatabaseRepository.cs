// --------------------------------------------------------------------------
// <copyright file="IDatabaseRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Core.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.ManagementSP.Entities;

    /// <summary>
    /// The database repository interface.
    /// </summary>
    public interface IDatabaseRepository
    {
        /// <summary>
        /// Gets all the databases..
        /// </summary>
        /// <returns>List of database.</returns>
        IEnumerable<Database> GetAll();

        /// <summary>
        /// Inserts the or update.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The database name.</param>
        /// <param name="description">The description.</param>
        /// <param name="serverId">The server identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns>Result of update.</returns>
        bool InsertOrUpdate(int id, string name, string description, int serverId, string userId, string password);

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Result of delete.</returns>
        bool Delete(int id);
    }
}
