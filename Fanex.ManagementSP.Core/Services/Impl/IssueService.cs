// --------------------------------------------------------------------------
// <copyright file="IssueService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Core.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using Fanex.ManagementSP.Core.Repositories.Impl;
    using Fanex.ManagementSP.Core.Repositories.Interface;
    using Fanex.ManagementSP.Core.Services.Interface;
    using Fanex.ManagementSP.Entities;

    /// <summary>
    /// The issue service class.
    /// </summary>
    public class IssueService : BaseService, IIssueService
    {
        private readonly IIssueRepository _issueProcedureRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="IssueService"/> class.
        /// </summary>
        public IssueService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IssueService"/> class.
        /// </summary>
        /// <param name="serverService">The server service.</param>
        public IssueService(IIssueRepository serverService)
        {
            _issueProcedureRepository = serverService ?? new IssueRepository();
        }

        public Issue Get(int id)
        {
            try
            {
                return _issueProcedureRepository.Get(id);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the favorites by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>List of issue.</returns>
        public IEnumerable<Issue> GetFavoritiesByUsername(string username)
        {
            try
            {
                return _issueProcedureRepository.GetFavoritesByUsername(username);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Inserts the favorite.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="issueId">The issue identifier.</param>
        /// <returns>Result of insert.</returns>
        public bool InsertFavorite(string username, int issueId)
        {
            try
            {
                return _issueProcedureRepository.InsertFavorite(username, issueId);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }

        /// <summary>
        /// Deletes the favorite.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="issueId">The issue identifier.</param>
        /// <returns>Result of delete.</returns>
        public bool DeleteFavorite(string username, int issueId)
        {
            try
            {
                return _issueProcedureRepository.DeleteFavorite(username, issueId);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }


        public IEnumerable<Issue> GetByStoredProcedureId(int id)
        {
            try
            {
                return _issueProcedureRepository.GetByStoredProcedureId(id);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }
    }
}
