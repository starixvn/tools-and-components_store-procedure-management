// --------------------------------------------------------------------------
// <copyright file="UserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Core.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using Fanex.ManagementSP.Core.Repositories.Impl;
    using Fanex.ManagementSP.Core.Repositories.Interface;
    using Fanex.ManagementSP.Core.Services.Interface;
    using Fanex.ManagementSP.Entities;

    /// <summary>
    /// The user service class.
    /// </summary>
    public class SettingService : BaseService, ISettingService
    {
        private readonly ISettingRepository _settingRepository;

        public SettingService()
            : this(null)
        {
        }

        public SettingService(ISettingRepository serverService)
        {
            _settingRepository = serverService ?? new SettingRepository();
        }

        public IEnumerable<EntityBase> GetAll(int settingTypeId)
        {
            try
            {
                return _settingRepository.GetAll(settingTypeId);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }
    }
}
