// --------------------------------------------------------------------------
// <copyright file="BaseService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Client.Services
{
    using System;

    /// <summary>
    /// The base service class.
    /// </summary>
    public class BaseService
    {
        ////public const string APISERVER = "http://10.21.10.19:8099/api/";

        ////public const string APISERVER = "http://localhost:60170/api/";        

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService"/> class.
        /// </summary>
        /// <param name="apiUrl">The API URL.</param>
        public BaseService(string apiUrl)
        {
            APISERVER = apiUrl;
        }

        public string APISERVER { get; set; }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        protected void HandleException(Exception ex)
        {
        }
    }
}
