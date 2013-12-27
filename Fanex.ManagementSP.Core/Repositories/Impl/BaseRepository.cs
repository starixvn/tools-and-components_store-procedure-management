// --------------------------------------------------------------------------
// <copyright file="BaseRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Core.Repositories.Impl
{
    using System;
    using System.Configuration;
    using MySql.Data.MySqlClient;

    /// <summary>
    /// The base repository class.
    /// </summary>
    public class BaseRepository : IDisposable
    {
        private const string ConnectionName = "redmine";

        private string _connectionString = string.Empty;

        private MySqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class.
        /// </summary>
        public BaseRepository()
        {
            try
            {
                _connectionString = ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        protected MySqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Connection.Close();
        }
    }
}
