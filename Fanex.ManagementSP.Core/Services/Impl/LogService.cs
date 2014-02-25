// --------------------------------------------------------------------------
// <copyright file="LogService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2014.2.25">Created</change>
// </history>

namespace Fanex.ManagementSP.Core.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using Fanex.ManagementSP.Core.Repositories.Impl;
    using Fanex.ManagementSP.Core.Repositories.Interface;
    using Fanex.ManagementSP.Core.Services.Interface;
    using Fanex.ManagementSP.Entities.Models;

    public class LogService : BaseService, ILogService
    {
        private readonly ILogRepository _iLogRepository;

        public LogService()
            : this(null)
        {
        }

        public LogService(ILogRepository iLogRepository)
        {
            _iLogRepository = iLogRepository ?? new LogRepository();
        }

        public IEnumerable<LogExecSp> SearchLog(string name, string username, DateTime from, DateTime to, int filterType)
        {
            try
            {
                return _iLogRepository.SearchLog(name, username, from, to, filterType);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }
    }
}
