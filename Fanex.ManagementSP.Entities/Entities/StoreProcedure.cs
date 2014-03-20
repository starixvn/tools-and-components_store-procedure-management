// --------------------------------------------------------------------------
// <copyright file="StoreProcedure.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Entities
{
    using System;
    using Fanex.ManagementSP.Enumerations;

    /// <summary>
    /// The stored procedure class.
    /// </summary>
    public class StoreProcedure : EntityBase
    {
        public Database Database { get; set; }

        public Server Server { get; set; }
    }
}
