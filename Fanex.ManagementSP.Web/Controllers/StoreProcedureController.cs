// --------------------------------------------------------------------------
// <copyright file="StoreProcedureController.cs" company="Nexcel Solutions Vietnam">
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
    /// The stored procedure API controller.
    /// </summary>
    public class StoreProcedureController : ApiController
    {
        private readonly IStoreProcedureService _storeProcedureService = new StoreProcedureService();

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="includeChild">If set to <c>true</c> [include child].</param>
        /// <returns>List stored procedure.</returns>
        public IEnumerable<StoreProcedure> Get(string id, bool includeChild)
        {
            return _storeProcedureService.GetByIssueId(id, includeChild);
        }

        /// <summary>
        /// Posts the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="serverId">The server identifier.</param>
        /// <param name="databaseId">The database identifier.</param>
        /// <param name="name">The stored name.</param>
        /// <param name="isFunction">If set to <c>true</c> [is function].</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="modifiedBy">The modified by.</param>
        /// <param name="status">The status.</param>
        /// <param name="description">The description.</param>
        /// <param name="issueId">The issue identifier.</param>
        /// <param name="developerId">The developer identifier.</param>
        /// <returns>Stored procedure identifier.</returns>
        public int Post(
            int id,
            int serverId,
            int databaseId,
            string name,
            bool isFunction,
            string actionName,
            string modifiedBy,
            int status,
            string description,
            int issueId,
            int developerId)
        {
            description = description == null ? string.Empty : description;
            _storeProcedureService.InsertOrUpdate(ref id, serverId, databaseId, name, isFunction, actionName, modifiedBy, status, description, issueId, developerId);

            return id;
        }

        /// <summary>
        /// Puts the specified ids.
        /// </summary>
        /// <param name="ids">The array identifier.</param>
        /// <param name="statusId">The status identifier.</param>
        /// <param name="username">The username.</param>
        public void Put(string ids, int statusId, string username)
        {
            _storeProcedureService.UpdateStatus(ids, statusId, username);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Delete(int id)
        {
            _storeProcedureService.Delete(id);
        }
    }
}
