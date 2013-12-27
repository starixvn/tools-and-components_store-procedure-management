// --------------------------------------------------------------------------
// <copyright file="DatabaseRepository.cs" company="Nexcel Solutions Vietnam">
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
    /// The database repository class.
    /// </summary>
    public class DatabaseRepository : BaseRepository, IDatabaseRepository
    {
        /// <summary>
        /// Gets all the databases..
        /// </summary>
        /// <returns>List of database.</returns>
        public IEnumerable<Database> GetAll()
        {
            IEnumerable<Database> list = null;

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    list = conn.Query<Database, Server, Database>(
                        sql: "spm_database_getall",
                        map: (d, s) => { d.Server = s; return d; },
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
        /// <param name="name">The database name.</param>
        /// <param name="description">The description.</param>
        /// <param name="serverId">The server identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns>Result of update.</returns>
        public bool InsertOrUpdate(int id, string name, string description, int serverId, string userId, string password)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Execute(
                    sql: "spm_databases_insert",
                    param: new
                    {
                        DatabaseId = id,
                        Name = name,
                        Description = description,
                        ServerId = serverId,
                        UserLogin = userId,
                        UserPassword = password
                    },
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
                    sql: "spm_databases_delete",
                    param: new { DatabaseId = id },
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
