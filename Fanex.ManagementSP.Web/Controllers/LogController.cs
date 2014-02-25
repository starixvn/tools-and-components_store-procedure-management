// --------------------------------------------------------------------------
// <copyright file="LogControllerController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2014.2.25">Created</change>
// </history>

namespace Fanex.ManagementSP.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using Fanex.ManagementSP.Core.Services.Impl;
    using Fanex.ManagementSP.Core.Services.Interface;
    using Fanex.ManagementSP.Entities.Models;

    public class LogController : ApiController
    {
        private readonly ILogService _logService = new LogService();

        public IEnumerable<LogExecSp> Get(string name, string username, DateTime from, DateTime to, int filterType)
        {
            return _logService.SearchLog(name, username, from, to, filterType);
        }
    }
}
