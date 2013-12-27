// --------------------------------------------------------------------------
// <copyright file="Status.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Core.Enumerations
{
    /// <summary>
    /// The status.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// The in progress.
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// The status is done.
        /// </summary>
        Done = 2,

        /// <summary>
        /// The UAT [_XTEST].
        /// </summary>
        UAT_XTest = 3,

        /// <summary>
        /// The UAT [_XPRE].
        /// </summary>
        UAT_XPre = 4,

        /// <summary>
        /// The not required.
        /// </summary>
        NotRequired = 5,

        /// <summary>
        /// The released.
        /// </summary>
        Released = 6,

        /// <summary>
        /// The pending.
        /// </summary>
        Pending = 7
    }
}
