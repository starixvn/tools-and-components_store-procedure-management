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

    /// <summary>
    /// The convert utility class.
    /// </summary>
    public static class UtilityConverter
    {
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
                case "UAT [ _xtest]":
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
        public static StoreProcedure ToStoredProcedure(
            this OutlookGrid outlookGridStore,
            int rowIndex,
            List<Database> listDatabase,
            List<Server> listServer,
            List<User> listUser)
        {
            var storeProcedure = new StoreProcedure();

            storeProcedure.Server = new Server { Name = outlookGridStore[StoreColumnIndex.Server, rowIndex].EditedFormattedValue.ToString() };
            storeProcedure.Database = new Database { Name = outlookGridStore[StoreColumnIndex.Database, rowIndex].EditedFormattedValue.ToString() };
            storeProcedure.Name = outlookGridStore[StoreColumnIndex.Name, rowIndex].EditedFormattedValue.ToString();

            if (outlookGridStore[StoreColumnIndex.Name, rowIndex].FormattedValueType.Name == "Image")
            {
                storeProcedure.Name = string.Empty;
            }

            storeProcedure.IsFunction = outlookGridStore[StoreColumnIndex.IsFunction, rowIndex].EditedFormattedValue.ToString() == "1";
            storeProcedure.ActionName = outlookGridStore[StoreColumnIndex.ActionName, rowIndex].EditedFormattedValue.ToString();
            storeProcedure.Modifier = new User { Username = outlookGridStore[StoreColumnIndex.ModifierUser, rowIndex].EditedFormattedValue.ToString() };
            storeProcedure.Status = UtilityConverter.ToStatusID(outlookGridStore[StoreColumnIndex.Status, rowIndex].EditedFormattedValue.ToString());
            storeProcedure.Description = outlookGridStore[StoreColumnIndex.Description, rowIndex].EditedFormattedValue.ToString();
            storeProcedure.Developer = new User { Username = outlookGridStore[StoreColumnIndex.Developer, rowIndex].EditedFormattedValue.ToString() };

            var strIssueId = outlookGridStore[StoreColumnIndex.IssueId, rowIndex].EditedFormattedValue.ToString();
            var strId = outlookGridStore[StoreColumnIndex.Id, rowIndex].EditedFormattedValue.ToString();

            if (!string.IsNullOrWhiteSpace(strIssueId))
            {
                storeProcedure.Issue = new Issue { Id = Convert.ToInt32(strIssueId) };
            }

            if (!string.IsNullOrWhiteSpace(strId))
            {
                storeProcedure.Id = Convert.ToInt32(strId);
            }

            storeProcedure.Server = listServer.Where(x => x.Name.Equals(storeProcedure.Server.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            storeProcedure.Database = listDatabase.Where(x => x.Name.Equals(storeProcedure.Database.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            storeProcedure.Developer = listUser.Where(x => x.Username.Equals(storeProcedure.Developer.Username, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            return storeProcedure;
        }
    }
}
