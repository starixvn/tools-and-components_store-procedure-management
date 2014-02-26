// --------------------------------------------------------------------------
// <copyright file="ISettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Core.Services.Interface
{
    using System.Collections.Generic;
    using Fanex.ManagementSP.Entities;

    public interface ISettingService
    {
        IEnumerable<EntityBase> GetAll(int settingTypeId);
    }
}
