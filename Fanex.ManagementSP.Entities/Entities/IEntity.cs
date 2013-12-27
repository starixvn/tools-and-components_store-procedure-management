// --------------------------------------------------------------------------
// <copyright file="IEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Entities
{
    /// <summary>
    /// Entity interface.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TName">The type of the name.</typeparam>
    /// <typeparam name="TDescription">The type of the description.</typeparam>
    public interface IEntity<TId, TName, TDescription>
    {
        TId Id { get; set; }

        TName Name { get; set; }

        TDescription Description { get; set; }
    }
}
