// --------------------------------------------------------------------------
// <copyright file="Issue.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Entities
{
    using System;

    /// <summary>
    /// The issues class.
    /// </summary>
    public class Issue : EntityBase
    {
        public Tracker Tracker { get; set; }

        public int ProjectId { get; set; }

        public int CategoryId { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public int PriorityId { get; set; }

        public string Environment { get; set; }

        public User Assignee { get; set; }

        public User Author { get; set; }

        public Issue Parent { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }

        /// <summary>
        /// Returns the name of issue <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// The name of issue format {Id}-{Name} <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}-{1}", this.Id, this.Name);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}
