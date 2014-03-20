// --------------------------------------------------------------------------
// <copyright file="IIssueService.cs" company="Nexcel Solutions Vietnam">
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
    /// The issue service interface.
    /// </summary>
    public interface IIssueService
    {
        /// <summary>
        /// Gets the favorites by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>List of issue.</returns>
        IEnumerable<Issue> GetFavoritiesByUsername(string username);

        IEnumerable<Issue> GetByStoredProcedureId(int id);

        /// <summary>
        /// Inserts the favorite.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="issueId">The issue identifier.</param>
        /// <returns>Result of insert.</returns>
        bool InsertFavorite(string username, int issueId);

        /// <summary>
        /// Deletes the favorite.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="issueId">The issue identifier.</param>
        /// <returns>Result of delete.</returns>
        bool DeleteFavorite(string username, int issueId);
    }
}
