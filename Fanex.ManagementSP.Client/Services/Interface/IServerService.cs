// --------------------------------------------------------------------------
// <copyright file="IServerService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Client.Services.Interface
{
    using System.Collections.Generic;
    using Fanex.ManagementSP.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IServerService
    {
        IEnumerable<Server> GetAll();

        bool InsertOrUpdate(
            int id,
            string name,
            string ipLocal,
            string ipUat,
            string ipProduction,
            string description);

        bool Delete(int id);
    }
}
