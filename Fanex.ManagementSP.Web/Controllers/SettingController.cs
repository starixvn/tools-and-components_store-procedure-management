// --------------------------------------------------------------------------
// <copyright file="UserController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.11.22">Created</change>
// </history>

namespace Fanex.ManagementSP.Web.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Fanex.ManagementSP.Core.Services.Impl;
    using Fanex.ManagementSP.Core.Services.Interface;
    using Fanex.ManagementSP.Entities;

    public class SettingController : ApiController
    {
        private ISettingService _settingService = new SettingService();

        public IEnumerable<EntityBase> Get(int settingTypeId)
        {
            return _settingService.GetAll(settingTypeId);
        }
    }
}
