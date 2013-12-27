// --------------------------------------------------------------------------
// <copyright file="Utility.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.11.22">Created</change>
// </history>

namespace Fanex.ManagementSP.UI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using Fanex.ManagementSP.Entities;
    using Fanex.ManagementSP.UI.ColumnIndex;
    using Fanex.ManagementSP.UI.Extend;
    using NPOI.HPSF;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using OutlookStyleControls;

    /// <summary>
    /// The utility class.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
    public static class Utility
    {
        private static string _configPath = @"C:\Program Files (x86)\Nexcel Solutions\Manage SP Tool\config.txt";

        #region Others

        public static string CurrentUsername
        {
            get
            {
                return Environment.UserName.ToLower();
            }
        }

        #endregion

        #region Init DataGirdView Header Columns

        /// <summary>
        /// Initializes the data grid view stored procedure.
        /// </summary>
        /// <param name="outlookGridView">The outlook grid view.</param>
        public static void InitHeaderColumnsDataGridViewStore(OutlookGrid outlookGridView)
        {
            // Add column server            
            outlookGridView.Columns.Add("No", "#");
            outlookGridView.Columns.Add("Server", "Server");
            outlookGridView.Columns.Add("Database", "Database");
            outlookGridView.Columns.Add("SP_Function", "SP/Function");
            outlookGridView.Columns.Add("IsFunction", "Is Function");
            outlookGridView.Columns.Add("Actions", "Actions");
            outlookGridView.Columns.Add("Developer", "Developer");
            outlookGridView.Columns.Add("Status", "Status");
            outlookGridView.Columns.Add("Description", "Description");
            outlookGridView.Columns.Add("Issue", "Issue");
            outlookGridView.Columns.Add("StoreId", "StoreId");
            outlookGridView.Columns.Add("IssueId", "IssueId");
            outlookGridView.Columns.Add("ModifiedBy", "Modified By");

            // Init width            
            var totalWith = outlookGridView.Width;
            ////outlookGridView.Columns[StoreColumnIndex.Number].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            outlookGridView.Columns[StoreColumnIndex.Server].Width = 100;
            outlookGridView.Columns[StoreColumnIndex.Database].Width = 100;
            outlookGridView.Columns[StoreColumnIndex.Name].Width = 270;
            outlookGridView.Columns[StoreColumnIndex.IsFunction].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            outlookGridView.Columns[StoreColumnIndex.ActionName].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            outlookGridView.Columns[StoreColumnIndex.ModifierUser].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            outlookGridView.Columns[StoreColumnIndex.Status].Width = 80;
            outlookGridView.Columns[StoreColumnIndex.Description].Width = 100;
            outlookGridView.Columns[StoreColumnIndex.Developer].Width = 100;

            // Init align text
            for (int i = 0; i < outlookGridView.Columns.Count; i++)
            {
                outlookGridView.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Hide column issue 
            outlookGridView.Columns[StoreColumnIndex.Number].Visible = false;
            outlookGridView.Columns[StoreColumnIndex.IssueName].Visible = false;
            outlookGridView.Columns[StoreColumnIndex.IssueId].Visible = false;
            outlookGridView.Columns[StoreColumnIndex.Id].Visible = false;

            // Disable edit
            outlookGridView.Columns[StoreColumnIndex.Number].ReadOnly = true;
            outlookGridView.Columns[StoreColumnIndex.ModifierUser].ReadOnly = true;
        }

        /// <summary>
        /// Initializes the data grid view server.
        /// </summary>
        /// <param name="dataGridView">The data grid view.</param>
        public static void InitHeaderColumnsDataGridViewServer(DataGridView dataGridView)
        {
            dataGridView.Columns.Clear();
            //// Add column server            
            dataGridView.Columns.Add("No", "#");
            dataGridView.Columns.Add("Name", "Name");
            dataGridView.Columns.Add("IPLocal", "IP Local");
            dataGridView.Columns.Add("IPUAT", "IP UAT");
            dataGridView.Columns.Add("IPProduction", "IP Production");
            dataGridView.Columns.Add("Description", "Description");
            dataGridView.Columns.Add("Id", "Id");

            // Align text
            // Init align text
            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                dataGridView.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Hide columns
            dataGridView.Columns[ServerColumnIndex.Number].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView.Columns[ServerColumnIndex.Number].ReadOnly = true;
            dataGridView.Columns[ServerColumnIndex.Id].Visible = false;
            dataGridView.Columns[ServerColumnIndex.Number].Visible = false;

            // Init width
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Initializes the data grid view database.
        /// </summary>
        /// <param name="dataGridView">The data grid view.</param>
        public static void InitHeaderColumnsDataGridViewDatabase(DataGridView dataGridView)
        {
            dataGridView.Columns.Clear();
            //// Add column server            
            dataGridView.Columns.Add("No", "#");
            dataGridView.Columns.Add("Name", "Name");
            dataGridView.Columns.Add("Description", "Description");
            dataGridView.Columns.Add("UserID", "User ID");
            dataGridView.Columns.Add("Password", "Password");
            dataGridView.Columns.Add("Id", "Id");

            // Align text
            // Init align text
            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                dataGridView.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Hide columns
            dataGridView.Columns[DatabaseColumnIndex.Number].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView.Columns[DatabaseColumnIndex.Number].ReadOnly = true;
            dataGridView.Columns[DatabaseColumnIndex.Id].Visible = false;
            dataGridView.Columns[DatabaseColumnIndex.Number].Visible = false;

            // Init width
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Initializes the data grid view my issue.
        /// </summary>
        /// <param name="dataGridView">The data grid view.</param>
        public static void InitHeaderColumnsDataGridViewMyIssue(DataGridView dataGridView)
        {
            dataGridView.Columns.Clear();
            //// Add column server            
            dataGridView.Columns.Add("IssueId", "#");
            dataGridView.Columns.Add("Name", "Name");

            DataGridViewLinkColumn link = new DataGridViewLinkColumn();
            link.Text = "Remove";
            link.HeaderText = "Remove";
            link.UseColumnTextForLinkValue = true;

            dataGridView.Columns.Add(link);

            // Align text
            // Init align text
            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                dataGridView.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Columns[i].ReadOnly = true;
            }

            dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridView.Columns[0].Visible = false;

            // Init width
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.MultiSelect = true;
        }

        #endregion

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

                    for (int i = 0; i < outlookGridView.Rows.Count; i++)
                    {
                        outlookGridView[StoreColumnIndex.Number, i].Value = (i + 1).ToString();
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
                if (listServer != null
                    && listServer.Any())
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
                HandleException(ex);
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
                HandleException(ex);
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
        public static void ReplaceServerByGridViewComboBoxCell(OutlookGrid outlookGirdView, int colIndex, int rowIndex, IEnumerable<Server> listServer, string currentValue)
        {
            try
            {
                ////var listDatabase = _databaseService.GetAll().ToList();
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
        public static void ReplaceDatabaseByGridViewComboBoxCell(OutlookGrid outlookGirdView, int colIndex, int rowIndex, IEnumerable<Database> listDatabase, string currentValue, string currentServer)
        {
            try
            {
                DataGridViewComboBoxCellExtended cellDatabase = new DataGridViewComboBoxCellExtended();
                cellDatabase.DataSource = listDatabase.Where(x => x.Server.Name.ToLower() == currentServer.ToLower()).ToList();
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
                ////var listDatabase = _databaseService.GetAll().ToList();
                DataGridViewComboBoxCellExtended cellIsFunction = new DataGridViewComboBoxCellExtended();
                cellIsFunction.Items.Add("0");
                cellIsFunction.Items.Add("1");
                cellIsFunction.FlatStyle = FlatStyle.Flat;

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
                ////var listDatabase = _databaseService.GetAll().ToList();
                DataGridViewComboBoxCellExtended cellAction = new DataGridViewComboBoxCellExtended();
                cellAction.Items.Add("A");
                cellAction.Items.Add("E");
                cellAction.Items.Add("D");
                cellAction.Items.Add("Nested");
                cellAction.Items.Add("Not Used");
                cellAction.FlatStyle = FlatStyle.Flat;

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
                ////var listDatabase = _databaseService.GetAll().ToList();
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
        public static void ReplaceDeveloperByGridViewComboBoxCell(OutlookGrid outlookGirdView, int colIndex, int rowIndex, IEnumerable<User> listUser, string currentValue)
        {
            try
            {
                ////var listDatabase = _databaseService.GetAll().ToList();
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
                    comboBoxIssue.AutoCompleteCustomSource = ConvertIssueToAutocompleteSource(listIssue);
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
                HandleException(ex);
            }
        }

        #endregion

        #region Grouping & Sorting!!

        /// <summary>
        /// This event is called when the user clicks on a cell.
        /// in this particular case we check to see if one of the column headers.
        /// was clicked. If so, the grid will be sorted based on the clicked column.
        /// Note: this handler is not implemented optimally. It is merely used for demonstration purposes.
        /// </summary>
        /// <param name="outlookGridView">The outlook grid view.</param>
        /// <param name="columnIndex">Index of the column.</param>
        public static void GroupDataGridViewByIssue(OutlookGrid outlookGridView, int columnIndex)
        {
            ListSortDirection direction = ListSortDirection.Ascending;

            // set the column to be grouped
            outlookGridView.GroupTemplate.Column = outlookGridView.Columns[columnIndex];

            outlookGridView.Sort(new DataGridViewRowComparer(columnIndex, direction));
        }

        #endregion Grouping & Sorting!! - handle column clicks

        #region Convert

        /// <summary>
        /// Converts the status name to identifier.
        /// </summary>
        /// <param name="name">The status name.</param>
        /// <returns>Identifier of status.</returns>
        public static int ConvertStatusNameToId(string name)
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
        public static int ConvertActionNameToId(string name)
        {
            int id = -1;
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
        /// Converts the issue to autocomplete source.
        /// </summary>
        /// <param name="listIssue">The list issue.</param>
        /// <returns>Collection for auto complete source.</returns>
        public static AutoCompleteStringCollection ConvertIssueToAutocompleteSource(IEnumerable<Issue> listIssue)
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
        public static string ConvertDataGridViewStoreRowToString(OutlookGrid outlookGridStore, int rowIndex)
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
        /// <param name="_listDatabase">The _list database.</param>
        /// <param name="_listServer">The _list server.</param>
        /// <param name="_listUser">The _list user.</param>
        /// <returns>The stored procedure.</returns>
        public static StoreProcedure ConvertGridStoreRowToStoredProcedure(OutlookGrid outlookGridStore, int rowIndex, List<Database> _listDatabase, List<Server> _listServer, List<User> _listUser)
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
            storeProcedure.Status = Utility.ConvertStatusNameToId(outlookGridStore[StoreColumnIndex.Status, rowIndex].EditedFormattedValue.ToString());
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

            storeProcedure.Server = _listServer.Where(x => x.Name.ToLower() == storeProcedure.Server.Name.ToLower()).FirstOrDefault();

            ////_listDatabase = _listDatabase.Where(x => x.Server.Id == storeProcedure.Server.Id).ToList();

            storeProcedure.Database = _listDatabase.Where(x => x.Name.ToLower() == storeProcedure.Database.Name.ToLower()).FirstOrDefault();

            storeProcedure.Developer = _listUser.Where(x => x.Username.ToLower() == storeProcedure.Developer.Username.ToLower()).FirstOrDefault();

            return storeProcedure;
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

        #region Render

        /// <summary>
        /// Renders the space.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns>The space.</returns>
        public static string RenderSpace(int length)
        {
            string value = string.Empty;

            for (int i = 0; i < length; i++)
            {
                value += " ";
            }

            return value;
        }

        #endregion

        #region Export Excel

        /// <summary>
        /// Exports the excel.
        /// </summary>
        /// <param name="outlookGridStore">The outlook grid store.</param>
        /// <param name="hssfworkbook">The HSSF workbook.</param>
        /// <param name="filename">The filename.</param>
        public static void ExportExcel(OutlookGrid outlookGridStore, HSSFWorkbook hssfworkbook, string filename)
        {
            string[] titles = { "ServerName", "DBName", "SPName", "IsFunction" };

            Utility.InitializeWorkbook(hssfworkbook);

            Dictionary<string, ICellStyle> styles = CreateStyles(hssfworkbook);

            ISheet sheet = hssfworkbook.CreateSheet("Stored Procedure");
            IPrintSetup printSetup = sheet.PrintSetup;
            printSetup.Landscape = true;
            sheet.FitToPage = true;
            sheet.HorizontallyCenter = true;

            ////header row
            IRow headerRow = sheet.CreateRow(0);
            headerRow.HeightInPoints = 15;
            ICell headerCell;
            for (int i = 0; i < titles.Length; i++)
            {
                headerCell = headerRow.CreateCell(i);
                headerCell.SetCellValue(titles[i]);
                headerCell.CellStyle = styles["cell"];
            }

            int rowIndex = 1;

            for (int i = 0; i < outlookGridStore.Rows.Count; i++)
            {
                var issueId = outlookGridStore.Rows[i].Tag;

                if (issueId == null)
                {
                    if (!string.IsNullOrWhiteSpace(outlookGridStore[1, i].EditedFormattedValue.ToString()))
                    {
                        IRow rowData = sheet.CreateRow(rowIndex);
                        rowData.HeightInPoints = 15;
                        ICell valueCell;
                        for (int j = 0; j < 4; j++)
                        {
                            valueCell = rowData.CreateCell(j);
                            if (j == 3)
                            {
                                valueCell.SetCellValue(Convert.ToInt32(outlookGridStore[j + 1, i].EditedFormattedValue.ToString()));
                            }
                            else
                            {
                                valueCell.SetCellValue(outlookGridStore[j + 1, i].EditedFormattedValue.ToString());
                            }

                            valueCell.CellStyle = styles["cell"];
                        }

                        rowIndex = rowIndex + 1;
                    }
                }
            }

            ////finally set column widths, the width is measured in units of 1/256th of a character width            
            sheet.SetColumnWidth(0, 15 * 256);
            sheet.SetColumnWidth(1, 15 * 256);
            sheet.SetColumnWidth(2, 80 * 256);
            sheet.SetColumnWidth(3, 15 * 256);

            WriteToFile(hssfworkbook, filename);
        }

        #endregion

        #region Read App Setting

        /// <summary>
        /// Gets the configuration setting.
        /// </summary>
        /// <returns>Configuration string.</returns>
        public static string[] GetConfigSetting()
        {
            string[] value = new[] { string.Empty, string.Empty };

            try
            {
                var text = File.ReadAllText(_configPath);

                return !string.IsNullOrWhiteSpace(text) ? text.Split(';') : value;
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }

            return value;
        }

        /// <summary>
        /// Updates the configuration setting.
        /// </summary>
        /// <param name="value">The value.</param>
        public static void UpdateConfigSetting(string value)
        {
            try
            {
                File.WriteAllText(_configPath, value);
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        #endregion

        #region Handle Exception

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exection">The exception.</param>
        public static void HandleException(Exception exection)
        {
            ////MessageBox.Show(exection.Message);
            System.Diagnostics.Debug.WriteLine(exection.Message);
        }

        #endregion

        #region Get Details Store Procedure

        /// <summary>
        /// Gets the details stored procedure.
        /// </summary>
        /// <param name="storeName">Name of the stored procedure.</param>
        /// <param name="server">The server.</param>
        /// <param name="database">The database.</param>
        /// <returns>Content of stored procedure.</returns>
        public static string GetDetailsStoredProcedure(string storeName, Server server, Database database)
        {
            string value = string.Empty;

            try
            {
                if (server != null
                    && database != null
                    && !string.IsNullOrWhiteSpace(server.IPLocal)
                    && !string.IsNullOrWhiteSpace(storeName))
                {
                    using (SqlConnection conn = new SqlConnection(
                        string.Format(
                        @"Data Source={0};Initial Catalog={1};Connect Timeout=10;integrated security=false; user id={2}; password={3}",
                        server.IPLocal,
                        database.Name,
                        database.UserID,
                        database.Password)))
                    {
                        SqlCommand command = new SqlCommand();
                        command.CommandText = "sys.sp_helptext";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@objname", storeName));
                        command.CommandTimeout = 60;
                        command.Connection = conn;
                        conn.Open();

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        DataTable tb = ds.Tables[0];

                        foreach (DataRow item in tb.Rows)
                        {
                            value += item[0].ToString();
                        }

                        conn.Close();
                    }
                }
                else
                {
                    MessageBox.Show(Resource.CanNotConnectToDatabaseOrSPNotFound, storeName);
                }
            }
            catch
            {
                MessageBox.Show(Resource.CanNotConnectToDatabaseOrSPNotFound, storeName);
            }

            return value;
        }

        /// <summary>
        /// Opens the file detail stored procedure.
        /// </summary>
        /// <param name="storeName">Name of the stored procedure.</param>
        /// <param name="value">The value.</param>
        public static void OpenFileDetailStoredProcedure(string storeName, string value)
        {
            string path = Path.GetTempPath();

            File.WriteAllText(path + "/" + storeName + ".sql", value);

            Process.Start(path + "/" + storeName + ".sql");
        }

        #endregion

        #region Check data

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

        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="hssfworkbook">The HSSF workbook.</param>
        /// <param name="filename">The filename.</param>
        private static void WriteToFile(HSSFWorkbook hssfworkbook, string filename)
        {
            ////Write the stream data of workbook to the root directory
            FileStream file = new FileStream(filename, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
        }

        /// <summary>
        /// Initializes the workbook.
        /// </summary>
        /// <param name="hssfworkbook">The HSSF workbook.</param>
        private static void InitializeWorkbook(HSSFWorkbook hssfworkbook)
        {
            hssfworkbook = new HSSFWorkbook();

            ////create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "R&D Team";
            hssfworkbook.DocumentSummaryInformation = dsi;

            ////create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "Stored Procedure.";
            hssfworkbook.SummaryInformation = si;
        }

        /// <summary>
        /// Creates the styles.
        /// </summary>
        /// <param name="wb">The work book.</param>
        /// <returns>Dictionary style.</returns>
        private static Dictionary<string, ICellStyle> CreateStyles(IWorkbook wb)
        {
            Dictionary<string, ICellStyle> styles = new Dictionary<string, ICellStyle>();
            ICellStyle style;
            IFont titleFont = wb.CreateFont();
            titleFont.FontHeightInPoints = (short)18;
            titleFont.Boldweight = (short)FontBoldWeight.BOLD;
            style = wb.CreateCellStyle();
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            style.VerticalAlignment = VerticalAlignment.CENTER;
            style.SetFont(titleFont);
            styles.Add("title", style);

            titleFont.FontHeightInPoints = (short)10;
            titleFont.Boldweight = (short)FontBoldWeight.BOLD;
            style = wb.CreateCellStyle();
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;
            style.VerticalAlignment = VerticalAlignment.CENTER;
            style.FillForegroundColor = IndexedColors.LIGHT_CORNFLOWER_BLUE.Index;
            style.FillPattern = FillPatternType.SOLID_FOREGROUND;
            style.SetFont(titleFont);
            styles.Add("subject", style);

            IFont monthFont = wb.CreateFont();
            monthFont.FontHeightInPoints = (short)11;
            monthFont.Color = IndexedColors.WHITE.Index;
            style = wb.CreateCellStyle();
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            style.VerticalAlignment = VerticalAlignment.CENTER;
            style.FillForegroundColor = IndexedColors.GREY_50_PERCENT.Index;
            style.FillPattern = FillPatternType.SOLID_FOREGROUND;
            style.SetFont(monthFont);
            style.WrapText = true;
            styles.Add("header", style);

            style = wb.CreateCellStyle();
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;
            style.WrapText = true;
            styles.Add("cell", style);

            return styles;
        }

        #region Extension

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        #endregion
    }
}
