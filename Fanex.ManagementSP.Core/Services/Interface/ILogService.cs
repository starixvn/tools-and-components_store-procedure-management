// --------------------------------------------------------------------------
// <copyright file="ILog.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2014.2.25">Created</change>
// </history>

namespace Fanex.ManagementSP.Core.Services.Interface
{
    using System;
    using System.Collections.Generic;
    using Fanex.ManagementSP.Entities.Models;

    public interface ILogService
    {
        IEnumerable<LogExecSp> SearchLog(string name, string username, DateTime from, DateTime to, int filterType);
    }
}
