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
        public bool IsFunction { get; set; }

        public string ActionName { get; set; }

        public int Status { get; set; }

        public DateTime ModifiedDate { get; set; }

        public Database Database { get; set; }

        public User Developer { get; set; }

        public Issue Issue { get; set; }

        public User Modifier { get; set; }

        public Server Server { get; set; }

        public string StatusName
        {
            get
            {
                string name = string.Empty;

                switch (Status)
                {
                    case (int)Enumerations.Status.InProgress:
                        name = "In Progress";
                        break;
                    case (int)Enumerations.Status.NotRequired:
                        name = "Not Required";
                        break;
                    case (int)Enumerations.Status.Pending:
                        name = "Pending";
                        break;
                    case (int)Enumerations.Status.Released:
                        name = "Released";
                        break;
                    case (int)Enumerations.Status.UAT_XPre:
                        name = "UAT [_xpre]";
                        break;
                    case (int)Enumerations.Status.UAT_XTest:
                        name = "UAT [_xtest]";
                        break;
                    case (int)Enumerations.Status.Done:
                        name = "Done";
                        break;
                    case (int)Enumerations.Status.UAT:
                        name = "UAT";
                        break;
                }

                return name;
            }
        }
    }
}
