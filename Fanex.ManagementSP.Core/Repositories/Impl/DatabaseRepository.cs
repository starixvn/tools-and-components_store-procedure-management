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
                        sql: "spm_databases_getall",
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
        /// <param name="userLogin">The user identifier.</param>
        /// <param name="userPassword">The password.</param>
        /// <returns>Result of update.</returns>
        public bool InsertOrUpdate(
            ref int id,
            string name,
            string description,
            int serverId,
            string userLogin,
            string userPassword,
            int modifiedById)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    var parameter = new DynamicParameters();
                    parameter.Add("@DatabaseId", value: id, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                    parameter.Add("@Name", name);
                    parameter.Add("@Description", description);
                    parameter.Add("@ServerId", serverId);
                    parameter.Add("@UserLogin", userLogin);
                    parameter.Add("@UserPassword", userPassword);
                    parameter.Add("@ModifiedById", modifiedById);

                    conn.Execute(sql: "spm_databases_insert", param: parameter, commandType: CommandType.StoredProcedure);

                    id = parameter.Get<int>("@DatabaseId");
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
                    sql: "spm_databases_delete",
                    param: new
                    {
                        DatabaseId = id,
                        ModifiedById = modifiedById
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
    }
}
