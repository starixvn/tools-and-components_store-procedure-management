// --------------------------------------------------------------------------
// <copyright file="Server.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Entities
{
    /// <summary>
    /// The server class.
    /// </summary>
    public class Server : EntityBase
    {
        private string _ipLocal;
        private string _ipUat;
        private string _ipProduction;

        public string IPLocal
        {
            get { return !string.IsNullOrWhiteSpace(_ipLocal) ? _ipLocal.Trim() : string.Empty; }
            set { _ipLocal = value; }
        }

        public string IPUAT
        {
            get { return !string.IsNullOrWhiteSpace(_ipUat) ? _ipUat.Trim() : string.Empty; }
            set { _ipUat = value; }
        }

        public string IPProduction
        {
            get { return !string.IsNullOrWhiteSpace(_ipProduction) ? _ipProduction.Trim() : string.Empty; }
            set { _ipProduction = value; }
        }
    }
}
