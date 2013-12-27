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

    /// <summary>
    /// The stored procedure repository class.
    /// </summary>
    public class StoreProcedureRepository : BaseRepository, IStoreProcedureRepository
    {
        /// <summary>
        /// Gets the stored procedure by issue identifier.
        /// </summary>
        /// <param name="issueId">The issue identifier.</param>
        /// <param name="includeChild">If set to <c>true</c> [include child].</param>
        /// <returns>List stored procedure.</returns>
        public IEnumerable<StoreProcedure> GetByIssueId(string issueId, bool includeChild)
        {
            IEnumerable<StoreProcedure> list = null;

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    list = conn.Query<StoreProcedure, Database, User, Issue, User, Server, StoreProcedure>(
                        sql: "spm_storeprocedures_getbyissueid",
                         param: new { IssueIds = issueId, IncludeChild = includeChild },
                         map: (sp, d, du, i, u, s) => { sp.Database = d; sp.Developer = du; sp.Server = s; sp.Modifier = u; sp.Issue = i; return sp; },
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
        /// <param name="modifiedBy">The modified by.</param>
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
            string modifiedBy,
            int status,
            string description,
            int issueId,
            int developerId)
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
                    parameter.Add("@ModifiedBy", modifiedBy);
                    parameter.Add("@StatusId", status);
                    parameter.Add("@Description", description);
                    parameter.Add("@IssueId", issueId);
                    parameter.Add("@DeveloperUserId", developerId);

                    conn.Execute(
                    sql: "spm_storeprocedures_insert",
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
        public bool Delete(int id)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Execute(
                    sql: "spm_storeprocedures_delete",
                    param: new { StoreId = id },
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
        public bool UpdateStatus(string ids, int statusId, string username)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Execute(
                    sql: "spm_storeprocedures_updatestatus",
                    param: new { StoreId = ids, StatusId = statusId, Username = username },
                    commandType: CommandType.StoredProcedure);
                }
            }
            catch
            {
                throw;
            }

            return false;
        }
    }
}
