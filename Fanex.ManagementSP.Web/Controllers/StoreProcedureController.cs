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
    using Fanex.ManagementSP.Entities.Models;

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
        public IEnumerable<StoreProcedureIssue> Get(string id, bool includeChild)
        {
            return _storeProcedureService.GetStoredProcedureIssueByIssueIds(id, includeChild);
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
            int modifiedById,
            int status,
            string description,
            int issueId,
            int developerId,
            int storedProcedureId,
            bool isAddNew)
        {
            description = description == null ? string.Empty : description;
            _storeProcedureService.InsertOrUpdate(ref id, serverId, databaseId, name, isFunction, actionName, modifiedById, status, description, issueId, developerId, storedProcedureId, isAddNew);

            return id;
        }

        /// <summary>
        /// Puts the specified ids.
        /// </summary>
        /// <param name="ids">The array identifier.</param>
        /// <param name="statusId">The status identifier.</param>
        /// <param name="username">The username.</param>
        public void Put(string ids, int statusId, int modifiedById)
        {
            _storeProcedureService.UpdateStatus(ids, statusId, modifiedById);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Delete(int id, int modifiedById)
        {
            _storeProcedureService.Delete(id, modifiedById);
        }

        public IEnumerable<StoreProcedure> GetStoreProcedure(int serverId, int databaseId)
        {
            return _storeProcedureService.GetStoreProcedure(serverId, databaseId);
        }
    }
}
