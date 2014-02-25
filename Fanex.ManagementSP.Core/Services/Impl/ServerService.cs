// --------------------------------------------------------------------------
// <copyright file="ServerService.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// The server service class.
    /// </summary>
    public class ServerService : BaseService, IServerService
    {
        private readonly IServerRepository _serverRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerService"/> class.
        /// </summary>
        public ServerService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerService"/> class.
        /// </summary>
        /// <param name="serverService">The server service.</param>
        public ServerService(IServerRepository serverService)
        {
            _serverRepository = serverService ?? new ServerRepository();
        }

        /// <summary>
        /// Gets all of servers.
        /// </summary>
        /// <returns>List of server.</returns>
        public IEnumerable<Server> GetAll()
        {
            try
            {
                return _serverRepository.GetAll();
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
        /// <param name="name">The server name.</param>
        /// <param name="ipLocal">The IP local.</param>
        /// <param name="ipUat">The IP UAT.</param>
        /// <param name="ipProduction">The IP production.</param>
        /// <param name="description">The description.</param>
        /// <returns>Result of update.</returns>
        public bool InsertOrUpdate(
            int id,
            string name,
            string ipLocal,
            string ipUat,
            string ipProduction,
            string description,
            int modifiedById)
        {
            try
            {
                return _serverRepository.InsertOrUpdate(id, name, ipLocal, ipUat, ipProduction, description, modifiedById);
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
                return _serverRepository.Delete(id, modifiedById);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }
    }
}
