// --------------------------------------------------------------------------
// <copyright file="StoreProcedureRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Core.Repositories.Impl
{
    using System.Collections.Generic;
    using System.Data;
    using Dapper;
    using Fanex.ManagementSP.Core.Repositories.Interface;
    using Fanex.ManagementSP.Entities;
    using MySql.Data.MySqlClient;
    using Fanex.ManagementSP.Entities.Models;

    /// <summary>
    /// The stored procedure repository class.
    /// </summary>
    public class StoreProcedureRepository : BaseRepository, IStoreProcedureRepository
    {
        /// <summary>
        /// Gets the stored procedure by issue identifier.
        /// </summary>
        /// <param name="issueIds">The issue identifier.</param>
        /// <param name="includeChild">If set to <c>true</c> [include child].</param>
        /// <returns>List stored procedure.</returns>
        public IEnumerable<StoreProcedureIssue> GetStoredProcedureIssueByIssueIds(string issueIds, bool includeChild)
        {
            IEnumerable<StoreProcedureIssue> list = null;

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    list = conn.Query<StoreProcedureIssue, StoreProcedure, Database, User, Issue, User, Server, StoreProcedureIssue>(
                        sql: "spm_storedprocedure_issues_getbyissueids",
                         param: new { IssueIds = issueIds, IncludeChild = includeChild },
                         map: (spi, sp, d, du, i, u, s) => { spi.StoreProcedure = sp; sp.Database = d; spi.Developer = du; sp.Server = s; spi.Modifier = u; spi.Issue = i; return spi; },
                         splitOn: "Id",
                         commandType: CommandType.StoredProcedure);
                }
            }
            catch
            {
                throw;
            }

            return list;
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
            int developerId,
            int storedProcedureId,
            bool isAddNew)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    var parameter = new DynamicParameters();
                    parameter.Add("@StoreId", value: id, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                    parameter.Add("@ServerId", serverId);
                    parameter.Add("@DatabaseId", databaseId);
                    parameter.Add("@Name", name);
                    parameter.Add("@IsFunction", isFunction);
                    parameter.Add("@ActionName", actionName);
                    parameter.Add("@ModifiedById", modifiedById);
                    parameter.Add("@StatusId", status);
                    parameter.Add("@Description", description);
                    parameter.Add("@IssueId", issueId);
                    parameter.Add("@DeveloperUserId", developerId);
                    parameter.Add("@StoredProcedureId", storedProcedureId);
                    parameter.Add("@IsAddNew", isAddNew);

                    conn.Execute(
                    sql: "spm_storedprocedure_issues_insert",
                    param: parameter,
                    commandType: CommandType.StoredProcedure);

                    id = parameter.Get<int>("@StoreId");
                }
            }
            catch
            {
                throw;
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
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Execute(
                    sql: "spm_storedprocedure_issues_delete",
                    param: new { StoreId = id, ModifiedById = modifiedById },
                    commandType: CommandType.StoredProcedure);
                }
            }
            catch
            {
                throw;
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
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Execute(
                    sql: "spm_storedprocedure_issues_updatestatus",
                    param: new { StoreIds = ids, StatusId = statusId, ModifiedById = modifiedById },
                    commandType: CommandType.StoredProcedure);
                }
            }
            catch
            {
                throw;
            }

            return false;
        }

        public IEnumerable<StoreProcedure> GetStoreProcedure(int serverId, int databaseId)
        {
            IEnumerable<StoreProcedure> list = null;

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    list = conn.Query<StoreProcedure>(sql: "spm_storedprocedures_getall", param: new { ServerId = serverId, DatabaseId = databaseId }, commandType: CommandType.StoredProcedure);
                }
            }
            catch
            {
                throw;
            }

            return list;
        }
    }
}
