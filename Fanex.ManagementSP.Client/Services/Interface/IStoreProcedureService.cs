// --------------------------------------------------------------------------
// <copyright file="IStoreProcedureService.cs" company="Nexcel Solutions Vietnam">
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
    public interface IStoreProcedureService
    {
        IEnumerable<StoreProcedure> GetByIssueId(string issueId);

        bool InsertOrUpdate(
           int id,
           int serverId,
           int databaseId,
           string name,
           bool isFunction,
           string actionName,
           string modifiedBy,
           int status,
           string description,
           int issueId);

        bool Delete(int id);
    }
}
