// --------------------------------------------------------------------------
// <copyright file="UtilityConverter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.12.27">Created</change>
// </history>

namespace Fanex.ManagementSP.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using Fanex.ManagementSP.Entities;
    using Fanex.ManagementSP.UI.ColumnIndex;
    using OutlookStyleControls;
    using Fanex.ManagementSP.Entities.Models;

    /// <summary>
    /// The convert utility class.
    /// </summary>
    public static class UtilityConverter
    {
        public static string ToUpperStatus(this string name)
        {
            name = name.ToLower();

            switch (name)
            {
                case "in progress":
                    name = "In Progress";
                    break;
                case "done":
                    name = "Done";
                    break;
                case "uat [ _xtest]":
                    name = "UAT [_xtest]";
                    break;
                case "uat [_xpre]":
                    name = "UAT [_xpre]";
                    break;
                case "not required":
                    name = "Not required";
                    break;
                case "released":
                    name = "Released";
                    break;
                case "pending":
                    name = "Pending";
                    break;
                case "uat":
                    name = "UAT";
                    break;
                default:
                    name = string.Empty;
                    break;
            }

            return name;
        }

        /// <summary>
        /// Converts the status name to identifier.
        /// </summary>
        /// <param name="name">The status name.</param>
        /// <returns>Identifier of status.</returns>
        public static int ToStatusID(this string name)
        {
            int statusId = 0;

            switch (name)
            {
                case "In Progress":
                    statusId = 1;
                    break;
                case "Done":
                    statusId = 2;
                    break;
                case "UAT [_xtest]":
                    statusId = 3;
                    break;
                case "UAT [_xpre]":
                    statusId = 4;
                    break;
                case "Not required":
                    statusId = 5;
                    break;
                case "Released":
                    statusId = 6;
                    break;
                case "Pending":
                    statusId = 7;
                    break;
                case "UAT":
                    statusId = 8;
                    break;
                default:
                    statusId = -1;
                    break;
            }

            return statusId;
        }

        public static string ToUpperAction(this string name)
        {
            name = name.ToLower();

            switch (name)
            {
                case "a":
                    name = "A";
                    break;
                case "e":
                    name = "E";
                    break;
                case "d":
                    name = "D";
                    break;
                case "nested":
                    name = "Nested";
                    break;
                case "not used":
                    name = "Not Used";
                    break;
                default:
                    name = string.Empty;
                    break;
            }

            return name;
        }

        /// <summary>
        /// Converts the action name to identifier.
        /// </summary>
        /// <param name="name">The action name.</param>
        /// <returns>Identifier of action name.</returns>
        public static int ToActionID(this string name)
        {
            int id = 0;
            name = name.ToLower();

            switch (name)
            {
                case "a":
                    id = 1;
                    break;
                case "e":
                    id = 2;
                    break;
                case "d":
                    id = 3;
                    break;
                case "nested":
                    id = 4;
                    break;
                case "not used":
                    id = 5;
                    break;
                default:
                    id = -1;
                    break;
            }

            return id;
        }

        /// <summary>
        /// Converts the issue to auto complete source.
        /// </summary>
        /// <param name="listIssue">The list issue.</param>
        /// <returns>Collection for auto complete source.</returns>
        public static AutoCompleteStringCollection ToAutoCompleteStringCollection(this IEnumerable<Issue> listIssue)
        {
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();

            if (listIssue != null && listIssue.Any())
            {
                foreach (var item in listIssue)
                {
                    collection.Add(item.ToString());
                }
            }

            return collection;
        }

        public static AutoCompleteStringCollection ToAutoCompleteStringCollection(this IEnumerable<User> users)
        {
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();

            if (users != null && users.Any())
            {
                foreach (var item in users)
                {
                    collection.Add(item.Username.ToLower());
                }
            }

            return collection;
        }


        /// <summary>
        /// Converts the data grid view store row to string.
        /// </summary>
        /// <param name="outlookGridStore">The outlook grid store.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns>To string data grid view row.</returns>
        public static string ToString(this OutlookGrid outlookGridStore, int rowIndex)
        {
            string value = string.Format(
                "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}",
                 outlookGridStore[StoreColumnIndex.Server, rowIndex].EditedFormattedValue,
                 outlookGridStore[StoreColumnIndex.Database, rowIndex].EditedFormattedValue,
                 outlookGridStore[StoreColumnIndex.Name, rowIndex].EditedFormattedValue,
                 outlookGridStore[StoreColumnIndex.IsFunction, rowIndex].EditedFormattedValue,
                 outlookGridStore[StoreColumnIndex.ActionName, rowIndex].EditedFormattedValue,
                 outlookGridStore[StoreColumnIndex.ModifierUser, rowIndex].EditedFormattedValue,
                 outlookGridStore[StoreColumnIndex.Status, rowIndex].EditedFormattedValue,
                 outlookGridStore[StoreColumnIndex.Description, rowIndex].EditedFormattedValue);

            return value;
        }

        /// <summary>
        /// Converts the grid store row to stored procedure.
        /// </summary>
        /// <param name="outlookGridStore">The outlook grid store.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="listDatabase">The list database.</param>
        /// <param name="listServer">The list server.</param>
        /// <param name="listUser">The list user.</param>
        /// <returns>The stored procedure.</returns>
        public static StoreProcedureIssue ToStoredProcedureIssue(
            this OutlookGrid outlookGridStore,
            int rowIndex,
            IEnumerable<Database> listDatabase,
            IEnumerable<Server> listServer,
            IEnumerable<User> listUser)
        {
            var storeProcedureIssue = new StoreProcedureIssue();
            storeProcedureIssue.StoreProcedure = new StoreProcedure();

            storeProcedureIssue.StoreProcedure.Server = new Server { Name = outlookGridStore[StoreColumnIndex.Server, rowIndex].EditedFormattedValue.ToString() };
            storeProcedureIssue.StoreProcedure.Database = new Database { Name = outlookGridStore[StoreColumnIndex.Database, rowIndex].EditedFormattedValue.ToString() };
            storeProcedureIssue.StoreProcedure.Name = outlookGridStore[StoreColumnIndex.Name, rowIndex].EditedFormattedValue.ToString();
            storeProcedureIssue.IsFunction = (bool)outlookGridStore[StoreColumnIndex.IsFunction, rowIndex].EditedFormattedValue;
            storeProcedureIssue.ActionName = outlookGridStore[StoreColumnIndex.ActionName, rowIndex].EditedFormattedValue.ToString();
            storeProcedureIssue.ActionName = storeProcedureIssue.ActionName.Length == 1 ? storeProcedureIssue.ActionName.ToUpper() : storeProcedureIssue.ActionName;
            storeProcedureIssue.Modifier = new User { Username = outlookGridStore[StoreColumnIndex.ModifierUser, rowIndex].EditedFormattedValue.ToString() };
            storeProcedureIssue.Status = UtilityConverter.ToStatusID(outlookGridStore[StoreColumnIndex.Status, rowIndex].EditedFormattedValue.ToString());
            storeProcedureIssue.Description = outlookGridStore[StoreColumnIndex.Description, rowIndex].EditedFormattedValue.ToString();
            storeProcedureIssue.Developer = new User { Username = outlookGridStore[StoreColumnIndex.Developer, rowIndex].EditedFormattedValue.ToString() };

            var strIssueId = outlookGridStore[StoreColumnIndex.IssueId, rowIndex].EditedFormattedValue.ToString();
            var strId = outlookGridStore[StoreColumnIndex.Id, rowIndex].EditedFormattedValue.ToString();

            if (!string.IsNullOrWhiteSpace(strIssueId))
            {
                storeProcedureIssue.Issue = new Issue { Id = Convert.ToInt32(strIssueId) };
            }

            if (!string.IsNullOrWhiteSpace(strId))
            {
                storeProcedureIssue.Id = Convert.ToInt32(strId);
            }

            storeProcedureIssue.StoreProcedure.Id = 0;
            var strStoredProcedureId = outlookGridStore[StoreColumnIndex.StoredProcedureId, rowIndex].EditedFormattedValue.ToString();
            if (!string.IsNullOrWhiteSpace(strStoredProcedureId))
            {
                storeProcedureIssue.StoreProcedure.Id = Convert.ToInt32(strStoredProcedureId);
            }

            storeProcedureIssue.StoreProcedure.Server = listServer.Where(x => x.Name.Equals(storeProcedureIssue.StoreProcedure.Server.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            storeProcedureIssue.StoreProcedure.Database = listDatabase.Where(x => x.Name.Equals(storeProcedureIssue.StoreProcedure.Database.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            storeProcedureIssue.Developer = listUser.Where(x => x.Username.Equals(storeProcedureIssue.Developer.Username, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            return storeProcedureIssue;
        }
    }
}
