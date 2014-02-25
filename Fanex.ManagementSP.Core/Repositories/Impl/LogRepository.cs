// --------------------------------------------------------------------------
// <copyright file="LogRepostiroy.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2014.2.25">Created</change>
// </history>

namespace Fanex.ManagementSP.Core.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Dapper;
    using Fanex.ManagementSP.Core.Repositories.Interface;
    using Fanex.ManagementSP.Entities.Models;
    using MySql.Data.MySqlClient;

    public class LogRepository : BaseRepository, ILogRepository
    {
        public IEnumerable<LogExecSp> SearchLog(string name, string username, DateTime from, DateTime to, int filterType)
        {
            IEnumerable<LogExecSp> list = null;

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    list = conn.Query<LogExecSp>(sql: "spm_log_details_search",
                        param: new { Name = name, Username = username, FromDate = from, ToDate = to, FilterType = filterType },
                        commandType: CommandType.StoredProcedure);
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
