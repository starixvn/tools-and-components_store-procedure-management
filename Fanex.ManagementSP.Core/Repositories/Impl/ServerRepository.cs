// --------------------------------------------------------------------------
// <copyright file="ServerRepository.cs" company="Nexcel Solutions Vietnam">
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
    /// The server repository class.
    /// </summary>
    public class ServerRepository : BaseRepository, IServerRepository
    {
        /// <summary>
        /// Gets all of servers.
        /// </summary>
        /// <returns>List of server.</returns>
        public IEnumerable<Server> GetAll()
        {
            IEnumerable<Server> list = null;

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    list = conn.Query<Server>(sql: "spm_servers_getall", commandType: CommandType.StoredProcedure);
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
        /// <param name="name">The server name.</param>
        /// <param name="ipLocal">The IP local.</param>
        /// <param name="ipUat">The IP UAT.</param>
        /// <param name="ipProduction">The IP production.</param>
        /// <param name="description">The description.</param>
        /// <returns>Result of update.</returns>
        public bool InsertOrUpdate(
            int id,
            string name,
            string ipLocal,
            string ipUat,
            string ipProduction,
            string description)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Execute(
                    sql: "spm_servers_insert",
                    param: new
                    {
                        ServerId = id,
                        Name = name,
                        IPLocal = ipLocal,
                        IPUAT = ipUat,
                        IPProduction = ipProduction,
                        Description = description
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
                    sql: "spm_servers_delete",
                    param: new { ServerId = id },
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
