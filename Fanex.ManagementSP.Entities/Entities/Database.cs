// --------------------------------------------------------------------------
// <copyright file="Database.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Entities
{
    /// <summary>
    /// The Database class.
    /// </summary>
    public class Database : EntityBase
    {
        public Database()
        {
            UserID = string.Empty;
            Password = string.Empty;
        }

        public string UserID { get; set; }

        public string Password { get; set; }

        public Server Server { get; set; }
    }
}
