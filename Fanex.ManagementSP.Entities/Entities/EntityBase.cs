// --------------------------------------------------------------------------
// <copyright file="EntityBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Entities
{
    /// <summary>
    /// The base entity class.
    /// </summary>
    public class EntityBase : IEntity<int, string, string>
    {
        public EntityBase()
        {
            this.Id = 0;
            this.Name = string.Empty;
            this.Description = string.Empty;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Returns name <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// The name <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
