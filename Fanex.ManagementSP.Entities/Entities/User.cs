// --------------------------------------------------------------------------
// <copyright file="User.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Entities
{
    /// <summary>
    /// The user class.
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FristName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// Returns the username <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// The username <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Username;
        }
    }
}
