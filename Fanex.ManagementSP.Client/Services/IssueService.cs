// --------------------------------------------------------------------------
// <copyright file="IssueService.cs" company="Nexcel Solutions Vietnam">
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
    /// The issue service class.
    /// </summary>
    public class IssueService : BaseService
    {
        private string _apiIssue;

        /// <summary>
        /// Initializes a new instance of the <see cref="IssueService"/> class.
        /// </summary>
        /// <param name="apiUrl">The API URL.</param>
        public IssueService(string apiUrl)
            : base(apiUrl)
        {
            _apiIssue = APISERVER + "issue";
        }

        /// <summary>
        /// Gets the favorites by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>List of issue.</returns>
        public IEnumerable<Issue> GetFavoritesByUsername(string username)
        {
            try
            {
                var list = new List<Issue>();

                var taskRequest = AsyncRequsest.MakeRequest(_apiIssue + "/" + username);

                list = JsonConvert.DeserializeObject<List<Issue>>(taskRequest.Result);

                return list;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<Issue> GetByStoredProcedureId(int id)
        {
            try
            {
                var list = new List<Issue>();

                var taskRequest = AsyncRequsest.MakeRequest(_apiIssue + "?storedProcedureId=" + id);

                list = JsonConvert.DeserializeObject<List<Issue>>(taskRequest.Result);

                return list;
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
                var list = new List<StoreProcedure>();

                string format = "?username={0}&issueId={1}";
                string query = string.Format(format, username, issueId);

                var taskRequest = AsyncRequsest.MakeRequest(_apiIssue + query, WebRequestMethods.Http.Post);

                var result = taskRequest.Result;
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
        /// <returns>The result of delete.</returns>
        public bool DeleteFavorite(string username, int issueId)
        {
            try
            {
                string format = "?username={0}&issueId={1}";
                string query = string.Format(format, username, issueId);

                var taskRequest = AsyncRequsest.MakeRequest(_apiIssue + query, "DELETE");

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
