// --------------------------------------------------------------------------
// <copyright file="UtilityData.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.ManagementSP.UI.Extend;
    using OutlookStyleControls;

    /// <summary>
    /// The utility data class.
    /// </summary>
    public static class UtilityData
    {
        #region Binding Data

        /// <summary>
        /// Bindings the data to grid view store.
        /// </summary>
        /// <param name="outlookGridView">The outlook grid view.</param>
        /// <param name="listStoreProcedure">The list store procedure.</param>
        public static void BindingDataToGridViewStore(OutlookGrid outlookGridView, IEnumerable<StoreProcedure> listStoreProcedure)
        {
            try
            {
                if (listStoreProcedure != null && listStoreProcedure.Any())
                {
                    outlookGridView.Rows.Clear();
                    int rowIndex = 0;

                    foreach (var itemStoreProcedure in listStoreProcedure)
                    {
                        outlookGridView.Rows.Add();

                        BindingDataToGridViewStore(outlookGridView, rowIndex, itemStoreProcedure);

                        rowIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Bindings the data to grid view store.
        /// </summary>
        /// <param name="outlookGridView">The outlook grid view.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="itemStoreProcedure">The item store procedure.</param>
        public static void BindingDataToGridViewStore(OutlookGrid outlookGridView, int rowIndex, StoreProcedure itemStoreProcedure)
        {
            var id = itemStoreProcedure.Id;

            outlookGridView[StoreColumnIndex.Number, rowIndex].Value = rowIndex.ToString();
            outlookGridView[StoreColumnIndex.Server, rowIndex].Value = itemStoreProcedure.Server != null ? itemStoreProcedure.Server.Name : string.Empty;
            outlookGridView[StoreColumnIndex.Database, rowIndex].Value = itemStoreProcedure.Database != null ? itemStoreProcedure.Database.Name : string.Empty;
            outlookGridView[StoreColumnIndex.Name, rowIndex].Value = itemStoreProcedure.Name;
            outlookGridView[StoreColumnIndex.IsFunction, rowIndex].Value = id != 0 ? (itemStoreProcedure.IsFunction ? "1" : "0") : string.Empty;
            outlookGridView[StoreColumnIndex.ActionName, rowIndex].Value = itemStoreProcedure.ActionName != null ? itemStoreProcedure.ActionName : string.Empty;
            outlookGridView[StoreColumnIndex.ModifierUser, rowIndex].Value = itemStoreProcedure.Modifier != null ? itemStoreProcedure.Modifier.Username : string.Empty;
            outlookGridView[StoreColumnIndex.Status, rowIndex].Value = itemStoreProcedure.StatusName != null && id != 0 ? itemStoreProcedure.StatusName : string.Empty;
            outlookGridView[StoreColumnIndex.Description, rowIndex].Value = itemStoreProcedure.Description;
            outlookGridView[StoreColumnIndex.IssueName, rowIndex].Value = string.Format("#{0} {1}", itemStoreProcedure.Issue.Id.ToString(), itemStoreProcedure.Issue.Name);
            outlookGridView[StoreColumnIndex.Id, rowIndex].Value = itemStoreProcedure.Id;
            outlookGridView[StoreColumnIndex.IssueId, rowIndex].Value = itemStoreProcedure.Issue.Id.ToString();
            outlookGridView[StoreColumnIndex.Developer, rowIndex].Value = itemStoreProcedure.Developer != null ? itemStoreProcedure.Developer.Username : string.Empty;
            outlookGridView.Rows[rowIndex].Tag = itemStoreProcedure.Issue.Id.ToString();
            OutlookGridRow row = (OutlookGridRow)outlookGridView.Rows[rowIndex];
            row.IsFavorites = true;
        }

        /// <summary>
        /// Pastes the data to grid view store.
        /// </summary>
        /// <param name="outlookGridView">The outlook grid view.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="itemStoreProcedure">The item store procedure.</param>
        public static void PasteDataToGridViewStore(OutlookGrid outlookGridView, int rowIndex, StoreProcedure itemStoreProcedure)
        {
            outlookGridView[StoreColumnIndex.Number, rowIndex].Value = rowIndex.ToString();
            outlookGridView[StoreColumnIndex.Server, rowIndex].Value = itemStoreProcedure.Server != null ? itemStoreProcedure.Server.Name : string.Empty;
            outlookGridView[StoreColumnIndex.Database, rowIndex].Value = itemStoreProcedure.Database != null ? itemStoreProcedure.Database.Name : string.Empty;
            outlookGridView[StoreColumnIndex.Name, rowIndex].Value = itemStoreProcedure.Name;
            outlookGridView[StoreColumnIndex.IsFunction, rowIndex].Value = itemStoreProcedure.IsFunction ? "1" : "0";
            outlookGridView[StoreColumnIndex.ActionName, rowIndex].Value = itemStoreProcedure.ActionName != null ? itemStoreProcedure.ActionName : string.Empty;
            outlookGridView[StoreColumnIndex.ModifierUser, rowIndex].Value = itemStoreProcedure.Modifier != null ? itemStoreProcedure.Modifier.Username : string.Empty;
            outlookGridView[StoreColumnIndex.Status, rowIndex].Value = itemStoreProcedure.StatusName != null ? itemStoreProcedure.StatusName : string.Empty;
            outlookGridView[StoreColumnIndex.Description, rowIndex].Value = itemStoreProcedure.Description;
        }

        /// <summary>
        /// Bindings the data to grid view server.
        /// </summary>
        /// <param name="gridView">The grid view.</param>
        /// <param name="listServer">The list server.</param>
        public static void BindingDataToGridViewServer(DataGridView gridView, IEnumerable<Server> listServer)
        {
            try
            {
                if (listServer != null && listServer.Any())
                {
                    gridView.Rows.Clear();
                    int index = 1;
                    foreach (var item in listServer)
                    {
                        gridView.Rows.Add(index++, item.Name, item.IPLocal, item.IPUAT, item.IPProduction, item.Description, item.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Bindings the data to grid view database.
        /// </summary>
        /// <param name="gridView">The grid view.</param>
        /// <param name="listDatabase">The list database.</param>
        /// <param name="selectedServerId">The selected server identifier.</param>
        public static void BindingDataToGridViewDatabase(DataGridView gridView, IEnumerable<Database> listDatabase, int selectedServerId)
        {
            try
            {
                if (listDatabase != null && listDatabase.Any())
                {
                    gridView.Rows.Clear();
                    listDatabase = listDatabase.Where(x => x.Server.Id == selectedServerId).ToList();

                    int index = 1;

                    foreach (var item in listDatabase)
                    {
                        gridView.Rows.Add(index++, item.Name, item.Description, item.UserID, string.IsNullOrWhiteSpace(item.Password) ? string.Empty : item.Password, item.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Replaces the server by grid view ComboBox cell.
        /// </summary>
        /// <param name="outlookGirdView">The outlook gird view.</param>
        /// <param name="colIndex">Index of the col.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="listServer">The list server.</param>
        /// <param name="currentValue">The current value.</param>
        public static void ReplaceServerByGridViewComboBoxCell(
            OutlookGrid outlookGirdView,
            int colIndex,
            int rowIndex,
            IEnumerable<Server> listServer,
            string currentValue)
        {
            try
            {
                DataGridViewComboBoxCellExtended cellServer = new DataGridViewComboBoxCellExtended();
                cellServer.DataSource = listServer;
                cellServer.DisplayMember = "Name";
                cellServer.ValueMember = "Id";
                cellServer.FlatStyle = FlatStyle.Flat;
                cellServer.AutoComplete = true;

                outlookGirdView[colIndex, rowIndex] = cellServer;
                outlookGirdView[colIndex, rowIndex].Value = currentValue;
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Replaces the database by grid view ComboBox cell.
        /// </summary>
        /// <param name="outlookGirdView">The outlook gird view.</param>
        /// <param name="colIndex">Index of the col.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="listDatabase">The list database.</param>
        /// <param name="currentValue">The current value.</param>
        /// <param name="currentServer">The current server.</param>
        public static void ReplaceDatabaseByGridViewComboBoxCell(
            OutlookGrid outlookGirdView,
            int colIndex,
            int rowIndex,
            IEnumerable<Database> listDatabase,
            string currentValue,
            string currentServer)
        {
            try
            {
                DataGridViewComboBoxCellExtended cellDatabase = new DataGridViewComboBoxCellExtended();
                cellDatabase.DataSource = listDatabase.Where(x => x.Server.Name.Equals(currentServer, StringComparison.OrdinalIgnoreCase)).ToList();
                cellDatabase.DisplayMember = "Name";
                cellDatabase.ValueMember = "Id";
                cellDatabase.FlatStyle = FlatStyle.Flat;
                cellDatabase.AutoComplete = true;

                outlookGirdView[colIndex, rowIndex] = cellDatabase;
                outlookGirdView[colIndex, rowIndex].Value = currentValue;
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Replaces the is function by grid view ComboBox cell.
        /// </summary>
        /// <param name="outlookGirdView">The outlook gird view.</param>
        /// <param name="colIndex">Index of the col.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="currentValue">The current value.</param>
        public static void ReplaceIsFunctionByGridViewComboBoxCell(OutlookGrid outlookGirdView, int colIndex, int rowIndex, string currentValue)
        {
            try
            {
                DataGridViewComboBoxCellExtended cellIsFunction = new DataGridViewComboBoxCellExtended();
                cellIsFunction.Items.Add("0");
                cellIsFunction.Items.Add("1");
                cellIsFunction.FlatStyle = FlatStyle.Flat;
                cellIsFunction.AutoComplete = true;

                outlookGirdView[colIndex, rowIndex] = cellIsFunction;
                outlookGirdView[colIndex, rowIndex].Value = currentValue;
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Replaces the action by grid view ComboBox cell.
        /// </summary>
        /// <param name="outlookGirdView">The outlook gird view.</param>
        /// <param name="colIndex">Index of the col.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="currentValue">The current value.</param>
        public static void ReplaceActionByGridViewComboBoxCell(OutlookGrid outlookGirdView, int colIndex, int rowIndex, string currentValue)
        {
            try
            {
                DataGridViewComboBoxCellExtended cellAction = new DataGridViewComboBoxCellExtended();
                cellAction.Items.Add("A");
                cellAction.Items.Add("E");
                cellAction.Items.Add("D");
                cellAction.Items.Add("Nested");
                cellAction.Items.Add("Not Used");
                cellAction.FlatStyle = FlatStyle.Flat;
                cellAction.AutoComplete = true;

                outlookGirdView[colIndex, rowIndex] = cellAction;
                outlookGirdView[colIndex, rowIndex].Value = currentValue;
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Replaces the status by grid view ComboBox cell.
        /// </summary>
        /// <param name="outlookGirdView">The outlook gird view.</param>
        /// <param name="colIndex">Index of the col.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="currentValue">The current value.</param>
        public static void ReplaceStatusByGridViewComboBoxCell(OutlookGrid outlookGirdView, int colIndex, int rowIndex, string currentValue)
        {
            try
            {
                DataGridViewComboBoxCellExtended cellStatus = new DataGridViewComboBoxCellExtended();
                cellStatus.Items.Add("In Progress");
                cellStatus.Items.Add("Done");
                cellStatus.Items.Add("UAT");
                cellStatus.Items.Add("UAT [ _xtest]");
                cellStatus.Items.Add("UAT [_xpre]");
                cellStatus.Items.Add("Not required");
                cellStatus.Items.Add("Released");
                cellStatus.Items.Add("Pending");
                cellStatus.FlatStyle = FlatStyle.Flat;

                outlookGirdView[colIndex, rowIndex] = cellStatus;
                outlookGirdView[colIndex, rowIndex].Value = currentValue;
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Replaces the developer by grid view ComboBox cell.
        /// </summary>
        /// <param name="outlookGirdView">The outlook gird view.</param>
        /// <param name="colIndex">Index of the col.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="listUser">The list user.</param>
        /// <param name="currentValue">The current value.</param>
        public static void ReplaceDeveloperByGridViewComboBoxCell(
            OutlookGrid outlookGirdView,
            int colIndex,
            int rowIndex,
            IEnumerable<User> listUser,
            string currentValue)
        {
            try
            {
                DataGridViewComboBoxCellExtended cellDatabase = new DataGridViewComboBoxCellExtended();
                cellDatabase.DataSource = listUser;
                cellDatabase.DisplayMember = "Username";
                cellDatabase.ValueMember = "Id";
                cellDatabase.FlatStyle = FlatStyle.Flat;
                cellDatabase.AutoComplete = true;

                outlookGirdView[colIndex, rowIndex] = cellDatabase;
                outlookGirdView[colIndex, rowIndex].Value = currentValue;
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Bindings the issue to ComboBox.
        /// </summary>
        /// <param name="comboBoxIssue">The combo box issue.</param>
        /// <param name="listIssue">The list issue.</param>
        public static void BindingIssueToComboBox(ComboBox comboBoxIssue, IEnumerable<Issue> listIssue)
        {
            try
            {
                comboBoxIssue.Items.Clear();
                comboBoxIssue.Items.Add("My Issues");

                if (listIssue != null)
                {
                    foreach (var item in listIssue)
                    {
                        comboBoxIssue.Items.Add(item);
                    }

                    comboBoxIssue.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBoxIssue.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    comboBoxIssue.AutoCompleteCustomSource = UtilityConverter.ToAutoCompleteStringCollection(listIssue);
                }

                comboBoxIssue.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Bindings the issue to grid view.
        /// </summary>
        /// <param name="gridView">The grid view.</param>
        /// <param name="listIssue">The list issue.</param>
        public static void BindingIssueToGridView(DataGridView gridView, IEnumerable<Issue> listIssue)
        {
            try
            {
                if (listIssue != null && listIssue.Any())
                {
                    gridView.Rows.Clear();
                    foreach (var item in listIssue)
                    {
                        gridView.Rows.Add(item.Id, item.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        #endregion

        #region Finding

        /// <summary>
        /// Finds the issue identifier in group.
        /// </summary>
        /// <param name="gridViewStore">The grid view store.</param>
        /// <param name="txtIssueId">The text issue identifier.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns>The issue identifier.</returns>
        public static int FindIssueIdInGroup(OutlookGrid gridViewStore, TextBox txtIssueId, int rowIndex)
        {
            var issueId = 0;
            var arrayIssue = txtIssueId.Text.Trim().Split(',');

            try
            {
                var strValue = gridViewStore[StoreColumnIndex.IssueId, rowIndex - 1].EditedFormattedValue.ToString();

                if (!string.IsNullOrWhiteSpace(strValue))
                {
                    issueId = Convert.ToInt32(strValue);
                }
                else
                {
                    for (int i = rowIndex - 1; i >= 0; i--)
                    {
                        if (gridViewStore.Rows[i].Tag != null)
                        {
                            issueId = Convert.ToInt32(gridViewStore.Rows[i].Tag.ToString());

                            return issueId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }

            return issueId;
        }

        /// <summary>
        /// Finds the minimum index of the maximum.
        /// </summary>
        /// <param name="selectedRows">The selected rows.</param>
        /// <param name="maxIndex">The maximum index.</param>
        /// <param name="minIndex">The minimum index.</param>
        public static void FindMinMaxIndex(DataGridViewSelectedRowCollection selectedRows, ref int maxIndex, ref int minIndex)
        {
            if (!ReferenceEquals(selectedRows, null)
                && selectedRows.Count > 0)
            {
                maxIndex = selectedRows[0].Index;
                minIndex = selectedRows[0].Index;

                for (int i = 1; i < selectedRows.Count; i++)
                {
                    maxIndex = maxIndex > selectedRows[i].Index ? maxIndex : selectedRows[i].Index;
                    minIndex = minIndex < selectedRows[i].Index ? minIndex : selectedRows[i].Index;
                }
            }
        }

        #endregion

        #region Check data

        /// <summary>
        /// Determines whether [is valid stored procedure] [the specified outlook grid].
        /// </summary>
        /// <param name="outlookGrid">The outlook grid.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns>True if stored procedure is valid.</returns>
        public static bool IsValidStoredProcedure(this OutlookGrid outlookGrid, int rowIndex)
        {
            var isValid = true;

            for (int i = 0; i < outlookGrid.Columns.Count; i++)
            {
                if (outlookGrid[i, rowIndex].FormattedValueType.Name == "Image")
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }

        /// <summary>
        /// Stores the procedure equals.
        /// </summary>
        /// <param name="objectA">The object a.</param>
        /// <param name="objectB">The object b.</param>
        /// <returns>Compare result.</returns>
        public static bool StoreProcedureEquals(StoreProcedure objectA, StoreProcedure objectB)
        {
            if (string.Equals(objectA.ActionName, objectB.ActionName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(objectA.Database.Name, objectB.Database.Name, StringComparison.OrdinalIgnoreCase)
                && string.Equals(objectA.Description, objectB.Description, StringComparison.OrdinalIgnoreCase)
                && string.Equals(objectA.Developer.Username, objectB.Developer.Username, StringComparison.OrdinalIgnoreCase)
                && string.Equals(objectA.IsFunction.ToString(), objectB.IsFunction.ToString(), StringComparison.OrdinalIgnoreCase)
                && string.Equals(objectA.Name, objectB.Name, StringComparison.OrdinalIgnoreCase)
                && string.Equals(objectA.Server.Name, objectB.Server.Name, StringComparison.OrdinalIgnoreCase)
                && string.Equals(objectA.StatusName, objectB.StatusName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
