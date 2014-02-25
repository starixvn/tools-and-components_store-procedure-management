// --------------------------------------------------------------------------
// <copyright file="LogService.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.ManagementSP.Entities.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// The user service class.
    /// </summary>
    public class LogService : BaseService
    {
        private string _apiUser;

        public LogService(string apiUrl)
            : base(apiUrl)
        {
            _apiUser = APISERVER + "log";
        }

        public IEnumerable<LogExecSp> SearchLog(string name, string username, DateTime from, DateTime to, int filterType)
        {
            try
            {
                var list = new List<LogExecSp>();
                var format = "?name={0}&username={1}&from={2}&to={3}&filterType={4}";
                var query = string.Format(format, name, username, from, to, filterType);

                var taskRequest = AsyncRequsest.MakeRequest(_apiUser + query);

                list = JsonConvert.DeserializeObject<List<LogExecSp>>(taskRequest.Result);

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
