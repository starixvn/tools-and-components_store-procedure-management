// --------------------------------------------------------------------------
// <copyright file="StoreProcedureService.cs" company="Nexcel Solutions Vietnam">
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
    /// The stored procedure service class.
    /// </summary>
    public class StoreProcedureService : BaseService, IStoreProcedureService
    {
        private readonly IStoreProcedureRepository _storeProcedureRepository;

        public StoreProcedureService()
            : this(null)
        {
        }

        public StoreProcedureService(IStoreProcedureRepository serverService)
        {
            _storeProcedureRepository = serverService ?? new StoreProcedureRepository();
        }

        /// <summary>
        /// Gets the stored procedure by issue identifier.
        /// </summary>
        /// <param name="issueId">The issue identifier.</param>
        /// <param name="includeChild">If set to <c>true</c> [include child].</param>
        /// <returns>List stored procedure.</returns>
        public IEnumerable<StoreProcedure> GetByIssueId(string issueId, bool includeChild)
        {
            try
            {
                return _storeProcedureRepository.GetByIssueId(issueId, includeChild);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Inserts the or update.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="serverId">The server identifier.</param>
        /// <param name="databaseId">The database identifier.</param>
        /// <param name="name">The stored procedure name.</param>
        /// <param name="isFunction">If set to <c>true</c> [is function].</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="modifiedById">The modified by.</param>
        /// <param name="status">The status.</param>
        /// <param name="description">The description.</param>
        /// <param name="issueId">The issue identifier.</param>
        /// <param name="developerId">The developer identifier.</param>
        /// <returns>Identifier stored procedure.</returns>
        public bool InsertOrUpdate(
           ref int id,
            int serverId,
            int databaseId,
            string name,
            bool isFunction,
            string actionName,
            int modifiedById,
            int status,
            string description,
            int issueId,
            int developerId)
        {
            try
            {
                return _storeProcedureRepository.InsertOrUpdate(
                    ref id,
                    serverId,
                    databaseId,
                    name,
                    isFunction,
                    actionName,
                    modifiedById,
                    status,
                    description,
                    issueId,
                    developerId);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Result of delete.</returns>
        public bool Delete(int id, int modifiedById)
        {
            try
            {
                return _storeProcedureRepository.Delete(id, modifiedById);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="ids">The array identifier.</param>
        /// <param name="statusId">The status identifier.</param>
        /// <param name="username">The username.</param>
        /// <returns>Result of update.</returns>
        public bool UpdateStatus(string ids, int statusId, int modifiedById)
        {
            try
            {
                return _storeProcedureRepository.UpdateStatus(ids, statusId, modifiedById);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }
    }
}
