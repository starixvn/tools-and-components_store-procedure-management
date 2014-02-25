// --------------------------------------------------------------------------
// <copyright file="StoreProcedureService.cs" company="Nexcel Solutions Vietnam">
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
    /// The stored procedure service class.
    /// </summary>
    public class StoreProcedureService : BaseService
    {
        private string _apiStoreProcedure;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreProcedureService"/> class.
        /// </summary>
        /// <param name="apiUrl">The API URL.</param>
        public StoreProcedureService(string apiUrl)
            : base(apiUrl)
        {
            _apiStoreProcedure = APISERVER + "storeprocedure";
        }

        /// <summary>
        /// Gets the stored procedure by issue identifier.
        /// </summary>
        /// <param name="issueId">The issue identifier.</param>
        /// <param name="includeChild">If set to <c>true</c> [include child].</param>
        /// <returns>List of stored procedure.</returns>
        public IEnumerable<StoreProcedure> GetByIssueId(string issueId, bool includeChild)
        {
            try
            {
                issueId = issueId.Replace(" ", ",");
                var query = string.Format("?id={0}&includeChild={1}", issueId, includeChild);
                var list = new List<StoreProcedure>();

                var taskRequest = AsyncRequsest.MakeRequest(_apiStoreProcedure + query);

                list = JsonConvert.DeserializeObject<List<StoreProcedure>>(taskRequest.Result);

                return list;
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
        /// <returns>The identifier of stored procedure.</returns>
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
                var list = new List<StoreProcedure>();
                var format = string.Empty;
                format = "?id={0}&serverId={1}&databaseId={2}&name={3}&isFunction={4}&actionName={5}&modifiedById={6}&status={7}&description={8}&issueId={9}&developerId={10}";

                string query = string.Format(format, id, serverId, databaseId, name, isFunction, actionName, modifiedById, status, description, issueId, developerId);

                var taskRequest = AsyncRequsest.MakeRequest(_apiStoreProcedure + query, WebRequestMethods.Http.Post);

                id = Convert.ToInt32(taskRequest.Result);
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
                var format = "?id={0}&modifiedById={1}";
                var query = string.Format(format, id, modifiedById);

                var taskRequest = AsyncRequsest.MakeRequest(_apiStoreProcedure + query, "DELETE");

                var result = taskRequest.Result;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }

        /// <summary>
        /// Updates the status of list stored procedure.
        /// </summary>
        /// <param name="ids">The array identifier.</param>
        /// <param name="statusId">The status identifier.</param>
        /// <param name="username">The username.</param>
        /// <returns>Result of update.</returns>
        public bool UpdateStatus(string ids, int statusId, string username, int modifiedById)
        {
            try
            {
                string query = string.Format("?ids={0}&statusId={1}&username={2}&modifiedById={3}", ids, statusId, username, modifiedById);
                var taskRequest = AsyncRequsest.MakeRequest(_apiStoreProcedure + query, WebRequestMethods.Http.Put);

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
