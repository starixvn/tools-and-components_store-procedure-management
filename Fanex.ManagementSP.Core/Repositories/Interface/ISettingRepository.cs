// --------------------------------------------------------------------------
// <copyright file=" ISettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Core.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.ManagementSP.Entities;

    public interface ISettingRepository
    {
        IEnumerable<EntityBase> GetAll(int settingTypeId);
    }
}
