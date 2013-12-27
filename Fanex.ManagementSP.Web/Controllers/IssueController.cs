// --------------------------------------------------------------------------
// <copyright file="IssueController.cs" company="Nexcel Solutions Vietnam">
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
    /// The issue API controller.
    /// </summary>
    public class IssueController : ApiController
    {
        private readonly IIssueService _storeProcedureService = new IssueService();

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>List of issues.</returns>
        public IEnumerable<Issue> Get(string id)
        {
            return _storeProcedureService.GetFavoritiesByUsername(id);
        }

        /// <summary>
        /// Posts the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="issueId">The issue identifier.</param>
        public void Post(string username, int issueId)
        {
            _storeProcedureService.InsertFavorite(username, issueId);
        }

        /// <summary>
        /// Deletes the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="issueId">The issue identifier.</param>
        public void Delete(string username, int issueId)
        {
            _storeProcedureService.DeleteFavorite(username, issueId);
        }
    }
}
