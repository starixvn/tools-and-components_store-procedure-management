// --------------------------------------------------------------------------
// <copyright file="SettingService.cs" company="Nexcel Solutions Vietnam">
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

    public class SettingService : BaseService
    {
        private string _apiSetting;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="apiUrl">The API URL.</param>
        public SettingService(string apiUrl)
            : base(apiUrl)
        {
            _apiSetting = APISERVER + "setting";
        }

        /// <summary>
        /// Gets all the users.
        /// </summary>
        /// <returns>List of user.</returns>
        public IEnumerable<EntityBase> GetAll(int settingTypeId)
        {
            try
            {
                var list = new List<EntityBase>();

                var taskRequest = AsyncRequsest.MakeRequest(_apiSetting + "?settingTypeId=" + settingTypeId);

                list = JsonConvert.DeserializeObject<List<EntityBase>>(taskRequest.Result);

                return list;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }
    }
}
