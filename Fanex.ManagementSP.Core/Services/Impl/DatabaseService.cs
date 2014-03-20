// --------------------------------------------------------------------------
// <copyright file="DatabaseService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Core.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using Fanex.ManagementSP.Core.Repositories.Impl;
    using Fanex.ManagementSP.Core.Repositories.Interface;
    using Fanex.ManagementSP.Core.Services.Interface;
    using Fanex.ManagementSP.Entities;
    using Fanex.Common;

    /// <summary>
    /// The database service class.
    /// </summary>
    public class DatabaseService : BaseService, IDatabaseService
    {
        private readonly IDatabaseRepository _databaseRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseService"/> class.
        /// </summary>
        public DatabaseService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseService"/> class.
        /// </summary>
        /// <param name="serverService">The server service.</param>
        public DatabaseService(IDatabaseRepository serverService)
        {
            _databaseRepository = serverService ?? new DatabaseRepository();
        }

        /// <summary>
        /// Gets all the databases..
        /// </summary>
        /// <returns>List of database.</returns>
        public IEnumerable<Database> GetAll()
        {
            try
            {
                return _databaseRepository.GetAll();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Inserts the or update.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The database name.</param>
        /// <param name="description">The description.</param>
        /// <param name="serverId">The server identifier.</param>
        /// <param name="userLogin">The user identifier.</param>
        /// <param name="userPassword">The password.</param>
        /// <returns>Result of update.</returns>
        public bool InsertOrUpdate(ref int id, string name, string description, int serverId, string userLogin, string userPassword, int modifiedById)
        {
            try
            {
                userPassword = Cryptography.CfsEnCrypt(userPassword);
                _databaseRepository.InsertOrUpdate(ref id, name, description, serverId, userLogin, userPassword, modifiedById);

                return true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Result of delete.</returns>
        public bool Delete(int id, int modifiedById)
        {
            try
            {
                return _databaseRepository.Delete(id, modifiedById);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }
    }
}
