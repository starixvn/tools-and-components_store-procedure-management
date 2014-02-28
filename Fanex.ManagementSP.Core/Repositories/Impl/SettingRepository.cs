// --------------------------------------------------------------------------
// <copyright file="SettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.11.18">Created</change>
// </history>

namespace Fanex.ManagementSP.Core.Repositories.Impl
{
    using System.Collections.Generic;
    using System.Data;
    using Dapper;
    using Fanex.ManagementSP.Core.Repositories.Interface;
    using Fanex.ManagementSP.Entities;
    using MySql.Data.MySqlClient;

    public class SettingRepository : BaseRepository, ISettingRepository
    {
        /// <summary>
        /// Gets all the users.
        /// </summary>
        /// <returns>List of user.</returns>
        public IEnumerable<EntityBase> GetAll(int settingTypeId)
        {
            IEnumerable<EntityBase> list = null;

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    list = conn.Query<EntityBase>(
                        sql: "spm_settings_getall",
                        param: new { SettingTypeId = settingTypeId },
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
