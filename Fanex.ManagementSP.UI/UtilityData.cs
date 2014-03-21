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
    using SourceGrid;
    using Fanex.ManagementSP.Entities.Models;

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
        /// <param name="listStoreProcedureIssue">The list store procedure.</param>
        public static void BindingDataToGridViewStore(OutlookGrid outlookGridView, IEnumerable<StoreProcedureIssue> listStoreProcedureIssue)
        {
            try
            {
                if (listStoreProcedureIssue != null && listStoreProcedureIssue.Any())
                {
                    outlookGridView.Rows.Clear();
                    int rowIndex = 0;

                    foreach (var itemStoreProcedure in listStoreProcedureIssue)
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
        /// <param name="itemStoreProcedureIssue">The item store procedure.</param>
        public static void BindingDataToGridViewStore(OutlookGrid outlookGridView, int rowIndex, StoreProcedureIssue itemStoreProcedureIssue)
        {
            var id = itemStoreProcedureIssue.Id;

            outlookGridView[StoreColumnIndex.Number, rowIndex].Value = rowIndex.ToString();
            outlookGridView[StoreColumnIndex.Server, rowIndex].Value = itemStoreProcedureIssue.StoreProcedure.Server != null ? itemStoreProcedureIssue.StoreProcedure.Server.Name : string.Empty;
            outlookGridView[StoreColumnIndex.Database, rowIndex].Value = itemStoreProcedureIssue.StoreProcedure.Database != null ? itemStoreProcedureIssue.StoreProcedure.Database.Name : string.Empty;
            outlookGridView[StoreColumnIndex.Name, rowIndex].Value = itemStoreProcedureIssue.StoreProcedure.Name;
            outlookGridView[StoreColumnIndex.IsFunction, rowIndex].Value = id != 0 ? itemStoreProcedureIssue.IsFunction : false;
            outlookGridView[StoreColumnIndex.ActionName, rowIndex].Value = itemStoreProcedureIssue.ActionName != null ? itemStoreProcedureIssue.ActionName : string.Empty;
            outlookGridView[StoreColumnIndex.ModifierUser, rowIndex].Value = itemStoreProcedureIssue.Modifier != null ? itemStoreProcedureIssue.Modifier.Username : string.Empty;
            outlookGridView[StoreColumnIndex.Status, rowIndex].Value = itemStoreProcedureIssue.StatusName != null && id != 0 ? itemStoreProcedureIssue.StatusName : string.Empty;
            outlookGridView[StoreColumnIndex.Description, rowIndex].Value = itemStoreProcedureIssue.Description;
            outlookGridView[StoreColumnIndex.IssueName, rowIndex].Value = string.Format("#{0} {1}", itemStoreProcedureIssue.Issue.Id.ToString(), itemStoreProcedureIssue.Issue.Name);
            outlookGridView[StoreColumnIndex.Id, rowIndex].Value = itemStoreProcedureIssue.Id;
            outlookGridView[StoreColumnIndex.IssueId, rowIndex].Value = itemStoreProcedureIssue.Issue.Id.ToString();
            outlookGridView[StoreColumnIndex.Developer, rowIndex].Value = itemStoreProcedureIssue.Developer != null ? itemStoreProcedureIssue.Developer.Username : string.Empty;
            outlookGridView[StoreColumnIndex.StoredProcedureId, rowIndex].Value = itemStoreProcedureIssue.StoreProcedure != null ? itemStoreProcedureIssue.StoreProcedure.Id : 0;
            outlookGridView.Rows[rowIndex].Tag = itemStoreProcedureIssue.Issue.Id.ToString();
            OutlookGridRow row = (OutlookGridRow)outlookGridView.Rows[rowIndex];
            row.IsFavorites = true;
        }

        /// <summary>
        /// Pastes the data to grid view store.
        /// </summary>
        /// <param name="outlookGridView">The outlook grid view.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="itemStoreProcedureIssue">The item store procedure.</param>
        public static void PasteDataToGridViewStore(OutlookGrid outlookGridView, int rowIndex, StoreProcedureIssue itemStoreProcedureIssue)
        {
            outlookGridView[StoreColumnIndex.Number, rowIndex].Value = rowIndex.ToString();
            outlookGridView[StoreColumnIndex.Server, rowIndex].Value = itemStoreProcedureIssue.StoreProcedure.Server != null ? itemStoreProcedureIssue.StoreProcedure.Server.Name : string.Empty;
            outlookGridView[StoreColumnIndex.Database, rowIndex].Value = itemStoreProcedureIssue.StoreProcedure.Database != null ? itemStoreProcedureIssue.StoreProcedure.Database.Name : string.Empty;
            outlookGridView[StoreColumnIndex.Name, rowIndex].Value = itemStoreProcedureIssue.StoreProcedure.Name;
            outlookGridView[StoreColumnIndex.IsFunction, rowIndex].Value = itemStoreProcedureIssue.IsFunction ? "1" : "0";
            outlookGridView[StoreColumnIndex.ActionName, rowIndex].Value = itemStoreProcedureIssue.ActionName != null ? itemStoreProcedureIssue.ActionName : string.Empty;
            outlookGridView[StoreColumnIndex.ModifierUser, rowIndex].Value = itemStoreProcedureIssue.Modifier != null ? itemStoreProcedureIssue.Modifier.Username : string.Empty;
            outlookGridView[StoreColumnIndex.Status, rowIndex].Value = itemStoreProcedureIssue.StatusName != null ? itemStoreProcedureIssue.StatusName : string.Empty;
            outlookGridView[StoreColumnIndex.Description, rowIndex].Value = itemStoreProcedureIssue.Description;
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
                var cellServer = new DataGridViewComboBoxCellExtended();
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
                var cellDatabase = new DataGridViewComboBoxCellExtended();
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
                var cellAction = new DataGridViewComboBoxCellExtended();
                cellAction.Items.Add("A");
                cellAction.Items.Add("E");
                cellAction.Items.Add("D");
                cellAction.Items.Add("Nested");
                cellAction.Items.Add("Not Used");
                cellAction.FlatStyle = FlatStyle.Flat;
                cellAction.AutoComplete = true;

                outlookGirdView[colIndex, rowIndex] = cellAction;
                outlookGirdView[colIndex, rowIndex].Value = currentValue.ToUpperAction();
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
                var cellStatus = new DataGridViewComboBoxCellExtended();
                cellStatus.Items.Add("In Progress");
                cellStatus.Items.Add("Done");
                cellStatus.Items.Add("UAT");
                cellStatus.Items.Add("UAT [_xtest]");
                cellStatus.Items.Add("UAT [_xpre]");
                cellStatus.Items.Add("Not required");
                cellStatus.Items.Add("Released");
                cellStatus.Items.Add("Pending");
                cellStatus.FlatStyle = FlatStyle.Flat;

                outlookGirdView[colIndex, rowIndex] = cellStatus;
                outlookGirdView[colIndex, rowIndex].Value = currentValue.ToUpperStatus();
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
                var cellDatabase = new DataGridViewComboBoxCellExtended();
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
                gridView.Rows.Clear();

                if (listIssue != null && listIssue.Any())
                {
                    foreach (var item in listIssue)
                    {
                        gridView.Rows.Add(item.Id, "#" + item.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        public static void BindingLogDataToGrid(Grid grid, List<LogExecSp> logs)
        {
            int index = 0;

            foreach (var itemLog in logs)
            {
                index = grid.Rows.Count;

                if (itemLog.ActionTypeId == 2)
                {
                    BindingLogDataRowDatabase(grid, index, itemLog);
                }

                if (itemLog.ActionTypeId == 3)
                {
                    BindingLogDataRowServer(grid, index, itemLog);
                }

                if (itemLog.ActionTypeId == 1)
                {
                    BindingLogDataRowStoreProcedure(grid, index, itemLog);
                }
            }
        }

        private static void BindingLogDataRowStoreProcedure(Grid grid, int index, LogExecSp itemLog)
        {
            BindingLogDataRow(grid, index, 9, itemLog);
            grid[index, 5] = new SourceGrid.Cells.Cell("Server", typeof(string));
            grid[index + 1, 5] = new SourceGrid.Cells.Cell("Database", typeof(string));
            grid[index + 2, 5] = new SourceGrid.Cells.Cell("Name", typeof(string));
            grid[index + 3, 5] = new SourceGrid.Cells.Cell("Is Function", typeof(string));
            grid[index + 4, 5] = new SourceGrid.Cells.Cell("Action", typeof(string));
            grid[index + 5, 5] = new SourceGrid.Cells.Cell("Status", typeof(string));
            grid[index + 6, 5] = new SourceGrid.Cells.Cell("Description", typeof(string));
            grid[index + 7, 5] = new SourceGrid.Cells.Cell("Issue", typeof(string));
            grid[index + 8, 5] = new SourceGrid.Cells.Cell("Developer", typeof(string));

            if (!string.IsNullOrWhiteSpace(itemLog.Data))
            {
                var storeProcedure = itemLog.ToStoreProcedureIssue(itemLog.Data);
                BindingLogDataRowStoreProcedureDetails(grid, index, 6, storeProcedure);
            }

            if (!string.IsNullOrWhiteSpace(itemLog.OldData))
            {
                var storeProcedure = itemLog.ToStoreProcedureIssue(itemLog.OldData);
                BindingLogDataRowStoreProcedureDetails(grid, index, 7, storeProcedure);
            }
        }

        private static void BindingLogDataRowServer(Grid grid, int index, LogExecSp itemLog)
        {
            BindingLogDataRow(grid, index, 5, itemLog);
            grid[index, 5] = new SourceGrid.Cells.Cell("Name", typeof(string));
            grid[index + 1, 5] = new SourceGrid.Cells.Cell("Description", typeof(string));
            grid[index + 2, 5] = new SourceGrid.Cells.Cell("IP Local", typeof(string));
            grid[index + 3, 5] = new SourceGrid.Cells.Cell("IP UAT", typeof(string));
            grid[index + 4, 5] = new SourceGrid.Cells.Cell("IP Production", typeof(string));

            if (!string.IsNullOrWhiteSpace(itemLog.Data))
            {
                var server = itemLog.ToServer(itemLog.Data);
                BindingLogDataRowServerDetails(grid, index, 6, server);
            }

            if (!string.IsNullOrWhiteSpace(itemLog.OldData))
            {
                var server = itemLog.ToServer(itemLog.OldData);
                BindingLogDataRowServerDetails(grid, index, 7, server);
            }
        }

        private static void BindingLogDataRowDatabase(Grid grid, int index, LogExecSp itemLog)
        {
            BindingLogDataRow(grid, index, 4, itemLog);
            grid[index, 5] = new SourceGrid.Cells.Cell("Name", typeof(string));
            grid[index + 1, 5] = new SourceGrid.Cells.Cell("Description", typeof(string));
            grid[index + 2, 5] = new SourceGrid.Cells.Cell("Server", typeof(string));
            grid[index + 3, 5] = new SourceGrid.Cells.Cell("User Login", typeof(string));

            if (!string.IsNullOrWhiteSpace(itemLog.Data))
            {
                var database = itemLog.ToDatabase(itemLog.Data);
                BindingLogDataRowDatabaseDetails(grid, index, 6, database);
            }

            if (!string.IsNullOrWhiteSpace(itemLog.OldData))
            {
                var database = itemLog.ToDatabase(itemLog.OldData);
                BindingLogDataRowDatabaseDetails(grid, index, 7, database);
            }
        }

        private static void BindingLogDataRowStoreProcedureDetails(Grid grid, int index, int colIndex, StoreProcedureIssue storeProcedureIssue)
        {
            grid[index, colIndex] = new SourceGrid.Cells.Cell(storeProcedureIssue.StoreProcedure.Server.Name, typeof(string));
            grid[index + 1, colIndex] = new SourceGrid.Cells.Cell(storeProcedureIssue.StoreProcedure.Database.Name, typeof(string));
            grid[index + 2, colIndex] = new SourceGrid.Cells.Cell(storeProcedureIssue.StoreProcedure.Name, typeof(string));
            grid[index + 3, colIndex] = new SourceGrid.Cells.Cell(storeProcedureIssue.IsFunction.ToString(), typeof(string));
            grid[index + 4, colIndex] = new SourceGrid.Cells.Cell(storeProcedureIssue.ActionName, typeof(string));
            grid[index + 5, colIndex] = new SourceGrid.Cells.Cell(storeProcedureIssue.StatusName, typeof(string));
            grid[index + 6, colIndex] = new SourceGrid.Cells.Cell(storeProcedureIssue.Description, typeof(string));
            grid[index + 7, colIndex] = new SourceGrid.Cells.Cell(storeProcedureIssue.Issue.Id, typeof(string));
            grid[index + 8, colIndex] = new SourceGrid.Cells.Cell(storeProcedureIssue.Developer.Username, typeof(string));
        }

        private static void BindingLogDataRowServerDetails(Grid grid, int index, int colIndex, Server server)
        {
            grid[index, colIndex] = new SourceGrid.Cells.Cell(server.Name, typeof(string));
            grid[index + 1, colIndex] = new SourceGrid.Cells.Cell(server.Description, typeof(string));
            grid[index + 2, colIndex] = new SourceGrid.Cells.Cell(server.IPLocal, typeof(string));
            grid[index + 3, colIndex] = new SourceGrid.Cells.Cell(server.IPUAT, typeof(string));
            grid[index + 4, colIndex] = new SourceGrid.Cells.Cell(server.IPProduction, typeof(string));
        }

        private static void BindingLogDataRowDatabaseDetails(Grid grid, int index, int colIndex, Database database)
        {
            grid[index, colIndex] = new SourceGrid.Cells.Cell(database.Name, typeof(string));
            grid[index + 1, colIndex] = new SourceGrid.Cells.Cell(database.Description, typeof(string));
            grid[index + 2, colIndex] = new SourceGrid.Cells.Cell(database.Server.Name, typeof(string));
            grid[index + 3, colIndex] = new SourceGrid.Cells.Cell(database.UserID, typeof(string));
        }

        private static void BindingLogDataRow(Grid grid, int index, int rowSpan, LogExecSp itemLog)
        {
            grid.Rows.InsertRange(index, rowSpan);
            grid[index, 0] = new SourceGrid.Cells.Cell(itemLog.Id, typeof(string));
            grid[index, 0].RowSpan = rowSpan;
            grid[index, 1] = new SourceGrid.Cells.Cell(itemLog.LogDate, typeof(string));
            grid[index, 1].RowSpan = rowSpan;
            grid[index, 2] = new SourceGrid.Cells.Cell(itemLog.Name, typeof(string));
            grid[index, 2].RowSpan = rowSpan;
            grid[index, 3] = new SourceGrid.Cells.Cell(itemLog.Modifier, typeof(string));
            grid[index, 3].RowSpan = rowSpan;
            grid[index, 4] = new SourceGrid.Cells.Cell(itemLog.ActionName, typeof(string));
            grid[index, 4].RowSpan = rowSpan;
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
                if (!string.IsNullOrWhiteSpace(outlookGrid[i, rowIndex].ErrorText))
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
        public static bool IsEquals(this StoreProcedureIssue objectA, StoreProcedureIssue objectB)
        {
            if (string.Equals(objectA.ActionName, objectB.ActionName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(objectA.StoreProcedure.Database.Name, objectB.StoreProcedure.Database.Name, StringComparison.OrdinalIgnoreCase)
                && string.Equals(objectA.Description, objectB.Description, StringComparison.OrdinalIgnoreCase)
                && string.Equals(objectA.Developer.Username, objectB.Developer.Username, StringComparison.OrdinalIgnoreCase)
                && string.Equals(objectA.IsFunction.ToString(), objectB.IsFunction.ToString(), StringComparison.OrdinalIgnoreCase)
                && string.Equals(objectA.StoreProcedure.Name, objectB.StoreProcedure.Name)
                && string.Equals(objectA.StoreProcedure.Server.Name, objectB.StoreProcedure.Server.Name, StringComparison.OrdinalIgnoreCase)
                && string.Equals(objectA.StatusName, objectB.StatusName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public static bool RowIsNotEmpty(OutlookGrid outlookGrid, int rowIndex)
        {
            if (!string.IsNullOrWhiteSpace(outlookGrid[StoreColumnIndex.Server, rowIndex].FormattedValue.ToString())
                || !string.IsNullOrWhiteSpace(outlookGrid[StoreColumnIndex.Database, rowIndex].FormattedValue.ToString())
                || !string.IsNullOrWhiteSpace(outlookGrid[StoreColumnIndex.Name, rowIndex].FormattedValue.ToString())
                || !string.IsNullOrWhiteSpace(outlookGrid[StoreColumnIndex.ActionName, rowIndex].FormattedValue.ToString())
                || !string.IsNullOrWhiteSpace(outlookGrid[StoreColumnIndex.Status, rowIndex].FormattedValue.ToString()))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
