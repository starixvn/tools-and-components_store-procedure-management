// --------------------------------------------------------------------------
// <copyright file="IssueRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.31">Created</change>
// </history>

namespace Fanex.ManagementSP.Core.Repositories.Impl
{
    using System.Collections.Generic;
    using System.Data;
    using Dapper;
    using Fanex.ManagementSP.Core.Repositories.Interface;
    using Fanex.ManagementSP.Entities;
    using MySql.Data.MySqlClient;
    using System.Linq;

    /// <summary>
    /// The issue repository class.
    /// </summary>
    public class IssueRepository : BaseRepository, IIssueRepository
    {
        public Issue Get(int id)
        {
            Issue issue = new Issue();

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    issue = conn.Query<Issue, User, Issue, Issue>(
                        sql: "spm_issues_getbyid",
                        map: (i, u, pi) => { i.Assignee = u; i.Parent = pi; return i; },
                        param: new { IssueId = id },
                        splitOn: "id",
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }

            return issue;
        }

        /// <summary>
        /// Gets the favorites by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>List of issue.</returns>
        public IEnumerable<Issue> GetFavoritesByUsername(string username)
        {
            IEnumerable<Issue> list = null;

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    list = conn.Query<Issue>(
                        sql: "spm_favorities_issues_getbyuser",
                         param: new { Username = username },
                         commandType: CommandType.StoredProcedure);
                }
            }
            catch
            {
                throw;
            }

            return list;
        }

        /// <summary>
        /// Inserts the favorite.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="issueId">The issue identifier.</param>
        /// <returns>Result of insert.</returns>
        public bool InsertFavorite(string username, int issueId)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Execute(
                    sql: "spm_favorities_issues_insert",
                    param: new
                    {
                        Username = username,
                        IssueId = issueId
                    },
                    commandType: CommandType.StoredProcedure);
                }
            }
            catch
            {
                throw;
            }

            return false;
        }

        /// <summary>
        /// Deletes the favorite.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="issueId">The issue identifier.</param>
        /// <returns>Result of delete.</returns>
        public bool DeleteFavorite(string username, int issueId)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Execute(
                    sql: "spm_favorities_issues_delete",
                    param: new { Username = username, IdIssue = issueId },
                    commandType: CommandType.StoredProcedure);
                }
            }
            catch
            {
                throw;
            }

            return false;
        }


        public IEnumerable<Issue> GetByStoredProcedureId(int id)
        {
            IEnumerable<Issue> list = null;

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    list = conn.Query<Issue>(
                        sql: "spm_issues_getbystoredprocedureid",
                         param: new { Id = id },
                         commandType: CommandType.StoredProcedure);
                }
            }
            catch
            {
                throw;
            }

            return list;
        }
    }
}
