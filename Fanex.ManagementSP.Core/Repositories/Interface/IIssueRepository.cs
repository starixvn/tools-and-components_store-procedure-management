// --------------------------------------------------------------------------
// <copyright file="IIssueRepository.cs" company="Nexcel Solutions Vietnam">
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
    /// The issue repository interface.
    /// </summary>
    public interface IIssueRepository
    {
        Issue Get(int id);

        /// <summary>
        /// Gets the favorites by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>List of issue.</returns>
        IEnumerable<Issue> GetFavoritesByUsername(string username);

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
