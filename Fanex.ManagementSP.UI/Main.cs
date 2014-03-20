// --------------------------------------------------------------------------
// <copyright file="Main.cs" company="Nexcel Solutions Vietnam">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using AutoUpdaterDotNET;
    using Fanex.ManagementSP.Client.Services;
    using Fanex.ManagementSP.Entities;
    using Fanex.ManagementSP.UI.ColumnIndex;
    using NPOI.HSSF.UserModel;
    using OutlookStyleControls;
    using Fanex.ManagementSP.Entities.Models;

    /// <summary>
    /// The main class.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    public partial class Main : UserControl
    {
        #region Member

        // Business services
        private StoreProcedureService _storeProcedureService;
        private ServerService _serverService;
        private DatabaseService _databaseService;
        private IssueService _issueService;
        private UserService _userService;
        private LogService _logService;

        // Global variables
        private string _currentValue = string.Empty;
        private IEnumerable<Server> _listServer;
        private IEnumerable<Database> _listDatabase;
        private IEnumerable<StoreProcedureIssue> _listStoreProcedureIssue;
        private IEnumerable<Issue> _listIssue;
        private IEnumerable<User> _listUser;

        private HSSFWorkbook _hssfworkbook;
        private string _apiUri = string.Empty;
        private string _redmineIssueUri = string.Empty;
        private bool _isBindingStoreComplete = false;
        private bool _isEventClickComplete = false;
        private StoreProcedureIssue _storeProcedureIssue = new StoreProcedureIssue();

        private int _userId = 0;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="Main" /> class.
        /// </summary>
        public Main()
        {
            InitializeComponent();

            /* Get setting configuration from CONFIG.TXT
             * 0 : Server API Uri
             * 1 : Redmine link issue
             */
            string[] setting = UtilityIO.GetConfigSetting();
            _apiUri = setting[0];
            _redmineIssueUri = setting[1];

            _storeProcedureService = new StoreProcedureService(_apiUri);
            _serverService = new ServerService(_apiUri);
            _databaseService = new DatabaseService(_apiUri);
            _issueService = new IssueService(_apiUri);
            _userService = new UserService(_apiUri);
            _logService = new LogService(_apiUri);

            chbQueryAll.Checked = true;

            txtIssueId.Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            txtMyIssuesId.Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            txtIssueId.Reset();

            // Init skin
            UtilityUI.InitSkinOutlookGrid(outlookGridStore, imageList, _redmineIssueUri);
            UtilityUI.InitSkinGridView(dataGridViewServer);
            UtilityUI.InitSkinGridView(dataGridViewDatabase);
            UtilityUI.InitSkinGridView(dataGridViewMyIssue);

            // Init header columns                
            UtilityUI.InitHeaderColumnsDataGridViewServer(dataGridViewServer);
            UtilityUI.InitHeaderColumnsDataGridViewDatabase(dataGridViewDatabase);
            UtilityUI.InitHeaderColumnsDataGridViewMyIssue(dataGridViewMyIssue);
            UtilityUI.InitHeaderColumnsGridLog(gridLog);

            // For menu
            toolStripComboBoxSettingStore.SelectedIndex = 0;
            tabControlStore.Appearance = TabAppearance.FlatButtons;
            tabControlStore.ItemSize = new Size(0, 1);
            tabControlStore.SizeMode = TabSizeMode.Fixed;
            txtIssueId.PlaceholderText = Resource.IssuesIdOrStoredProcedureName;
            txtMyIssuesId.PlaceholderText = Resource.EnterYourIssue;

            // Loading
            pictureBoxLoading.Visible = false;
            btnExport.Enabled = false;
            this.outlookGridStore.RowHeadersVisible = true;
            this.outlookGridStore.RowPostPaint += new DataGridViewRowPostPaintEventHandler(outlookGridStore_RowPostPaint);

            this.dataGridViewServer.RowHeadersVisible = true;
            this.dataGridViewServer.RowPostPaint += new DataGridViewRowPostPaintEventHandler(outlookGridStore_RowPostPaint);

            this.dataGridViewDatabase.RowHeadersVisible = true;
            this.dataGridViewDatabase.RowPostPaint += new DataGridViewRowPostPaintEventHandler(outlookGridStore_RowPostPaint);

            this.dataGridViewMyIssue.RowHeadersVisible = true;
            this.dataGridViewMyIssue.RowPostPaint += new DataGridViewRowPostPaintEventHandler(outlookGridStore_RowPostPaint);

            // For log
            dtpFromLog.MinDate = DateTime.Now.AddMonths(-3);
            dtpFromLog.MaxDate = DateTime.Now;
            dtpToLog.MinDate = DateTime.Now.AddMonths(-3);
            dtpToLog.MaxDate = DateTime.Now;
        }

        /// <summary>
        /// Handles the RowPostPaint event of the outlookGridStore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewRowPostPaintEventArgs"/> instance containing the event data.</param>
        private void outlookGridStore_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            string strRowIndex = (e.RowIndex + 1).ToString();
            Font rowFont = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);

            Rectangle headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);

            StringFormat centerFormat = new StringFormat();
            centerFormat.Alignment = StringAlignment.Center;
            centerFormat.LineAlignment = StringAlignment.Center;

            e.Graphics.DrawString(strRowIndex, rowFont, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        #endregion

        #region Load Form

        /// <summary>
        /// Handles the Load event of the Main control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Main_Load(object sender, EventArgs e)
        {
            // Load database and server into collection
            _listDatabase = _databaseService.GetAll();
            _listServer = _serverService.GetAll();
            _listIssue = _issueService.GetFavoritesByUsername(Utility.CurrentUsername);
            _listUser = _userService.GetAll();

            if (ReferenceEquals(_listDatabase, null)
                || ReferenceEquals(_listServer, null)
                || ReferenceEquals(_listIssue, null)
                || ReferenceEquals(_listUser, null))
            {
                MessageBox.Show("Can not connect to server. Please check your internet connection or try again later!", "Error", MessageBoxButtons.OK);
            }

            _userId = Utility.GetUserId(_listUser);

            // Binding data to control
            UtilityData.BindingDataToGridViewServer(dataGridViewServer, _listServer);
            UtilityData.BindingIssueToGridView(dataGridViewMyIssue, _listIssue);
            UtilityData.BindingIssueToComboBox(cmboMyIssues, _listIssue);

            // Binding stored procedure data
            string issueIds = txtIssueId.Text.Trim();
            if (!string.IsNullOrWhiteSpace(issueIds))
            {
                this.LoadStore(issueIds);
            }

            txtAPIUrl.Text = _apiUri;
            txtRedmineIssueUri.Text = _redmineIssueUri;
            txtUsernameLog.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtUsernameLog.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtUsernameLog.AutoCompleteCustomSource = UtilityConverter.ToAutoCompleteStringCollection(_listUser);
            cmboFilterTypeLog.SelectedIndex = 0;

            AutoUpdater.Start(_apiUri + "update-version.xml");
        }

        #endregion

        #region Event

        #region Others

        /// <summary>
        /// Handles the KeyDown event of the txtIssueId control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void txtIssueId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (cmboMyIssues.SelectedIndex != 0
                    && !string.IsNullOrWhiteSpace(txtIssueId.Text.Trim()))
                {
                    cmboMyIssues.SelectedIndex = 0;
                }

                this.LoadStore(txtIssueId.Text.Trim());
            }
        }

        /// <summary>
        /// Loads the store.
        /// </summary>
        /// <param name="issueIds">The issue ids.</param>
        private void LoadStore(string issueIds)
        {
            _isBindingStoreComplete = false;

            pictureBoxLoading.Visible = true;
            //// Reset data
            outlookGridStore.BindData(null, string.Empty);

            //// Init header columns    
            UtilityUI.InitHeaderColumnsDataGridViewStore(outlookGridStore);

            _listStoreProcedureIssue = _storeProcedureService.GetStoredProcedureIssueByIssueIds(issueIds, chbQueryAll.Checked);

            if (!ReferenceEquals(_listStoreProcedureIssue, null)
                && _listStoreProcedureIssue.Any())
            {
                UtilityData.BindingDataToGridViewStore(outlookGridStore, _listStoreProcedureIssue.OrderBy(x => x.Issue.Id).ToList());
                Utility.GroupDataGridViewByIssue(outlookGridStore, StoreColumnIndex.IssueName);
            }

            pictureBoxLoading.Visible = false;

            btnExport.Enabled = outlookGridStore.Rows.Count > 0;

            _isBindingStoreComplete = true;
            toolStripStatusStore.Text = Resource.NoChange;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the toolStripComboBoxSetting control.
        /// Change tab control with current selected menu.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void toolStripComboBoxSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox combo = (ToolStripComboBox)sender;

            tabControlStore.SelectedIndex = combo.SelectedIndex;
        }

        /// <summary>
        /// Handles the Click event of the button UpdateAPIUrl control.
        /// Update setting API Server and REDMINE issue link.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnUpdateAPIUrl_Click(object sender, EventArgs e)
        {
            var apiUri = txtAPIUrl.Text.Trim();
            var redmineIssueUri = txtRedmineIssueUri.Text.Trim();
            var value = string.Format("{0};{1}", apiUri.Trim(';'), redmineIssueUri.Trim(';'));

            if (!string.IsNullOrWhiteSpace(value))
            {
                UtilityIO.UpdateConfigSetting(value);

                MessageBox.Show(Resource.PleaseRestartYourApplication);
            }
        }

        /// <summary>
        /// Handles the Click event of the button Search control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            ShowSearchBox();
        }

        /// <summary>
        /// Shows the search box.
        /// </summary>
        private void ShowSearchBox()
        {
            SearchBox frm = new SearchBox();
            DialogResult r = frm.ShowDialog();

            if (r == DialogResult.OK)
            {
                bool isFound = false;

                for (int i = 0; i < outlookGridStore.Rows.Count; i++)
                {
                    string storeName = outlookGridStore[StoreColumnIndex.Name, i].EditedFormattedValue.ToString();

                    if (string.Equals(frm.SearchText, storeName, StringComparison.OrdinalIgnoreCase))
                    {
                        outlookGridStore.Rows[i].Selected = true;
                        isFound = true;
                    }
                    else
                    {
                        outlookGridStore.Rows[i].Selected = false;
                    }
                }

                if (!isFound)
                {
                    MessageBox.Show(string.Format(Resource.NotFound, frm.SearchText));
                }
            }
        }

        /// <summary>
        /// Generates the context menu strip grid view store.
        /// </summary>
        /// <param name="maxIndex">The maximum index.</param>
        /// <param name="minIndex">The minimum index.</param>
        /// <param name="countSelectedRow">The count selected row.</param>
        /// <returns>Context menu strip.</returns>
        private ContextMenuStrip GenerateContextMenuStripGridViewStore(int maxIndex, int minIndex, int countSelectedRow)
        {
            ////Create the ContextStripMenu
            ContextMenuStrip menu = new ContextMenuStrip();

            string menuTextDelete = countSelectedRow > 1 ? string.Format("Delete Row {0}-{1}", minIndex + 1, maxIndex + 1) : "Delete Row";

            ToolStripMenuItem insertAtBottom = new ToolStripMenuItem("Insert at bottom");
            ToolStripItem insertAtBottom5Row = new ToolStripMenuItem("5 more rows");
            insertAtBottom5Row.Tag = 5;
            ToolStripItem insertAtBottom10Row = new ToolStripMenuItem("10 more rows");
            insertAtBottom10Row.Tag = 10;
            ToolStripItem insertAtBottom20Row = new ToolStripMenuItem("20 more rows");
            insertAtBottom20Row.Tag = 20;
            insertAtBottom.DropDownItems.AddRange(new ToolStripItem[] 
                    {
                        insertAtBottom5Row,
                        insertAtBottom10Row, 
                        insertAtBottom20Row
                    });

            insertAtBottom5Row.MouseDown += new MouseEventHandler(insertAtBottomRow_MouseDown);
            insertAtBottom10Row.MouseDown += new MouseEventHandler(insertAtBottomRow_MouseDown);
            insertAtBottom20Row.MouseDown += new MouseEventHandler(insertAtBottomRow_MouseDown);

            ToolStripMenuItem insertMultiAbove = new ToolStripMenuItem(string.Format("Insert {0} above", countSelectedRow));
            ToolStripMenuItem insertMultiBelow = new ToolStripMenuItem(string.Format("Insert {0} below", countSelectedRow));
            ToolStripMenuItem pasteRow = new ToolStripMenuItem("Paste Ctrl + V");
            ToolStripMenuItem deleteMultiRow = new ToolStripMenuItem(menuTextDelete);
            ToolStripMenuItem copyMultiRow = new ToolStripMenuItem("Copy Ctrl + C");

            ToolStripMenuItem statusMultiUpdate = new ToolStripMenuItem("Status");
            ToolStripItem statusInProgress = new ToolStripMenuItem("In Progress");
            ToolStripItem statusDone = new ToolStripMenuItem("Done");
            ToolStripItem statusUAT = new ToolStripMenuItem("UAT");
            ToolStripItem statusUATXTest = new ToolStripMenuItem("UAT [_xtest]");
            ToolStripItem statusUATXPre = new ToolStripMenuItem("UAT [_xpre]");
            ToolStripItem statusNotRequired = new ToolStripMenuItem("Not required");
            ToolStripItem statusReleased = new ToolStripMenuItem("Released");
            ToolStripItem statusPending = new ToolStripMenuItem("Pending");

            statusMultiUpdate.DropDownItems.AddRange(new ToolStripItem[] 
                    {
                        statusInProgress,
                        statusDone, 
                        statusUAT,
                        statusUATXTest,
                        statusUATXPre, 
                        statusNotRequired,
                        statusReleased, 
                        statusPending 
                    });

            statusInProgress.MouseDown += new MouseEventHandler(StatusUpdateMultiple_MouseDown);
            statusDone.MouseDown += new MouseEventHandler(StatusUpdateMultiple_MouseDown);
            statusUAT.MouseDown += new MouseEventHandler(StatusUpdateMultiple_MouseDown);
            statusUATXTest.MouseDown += new MouseEventHandler(StatusUpdateMultiple_MouseDown);
            statusUATXPre.MouseDown += new MouseEventHandler(StatusUpdateMultiple_MouseDown);
            statusNotRequired.MouseDown += new MouseEventHandler(StatusUpdateMultiple_MouseDown);
            statusReleased.MouseDown += new MouseEventHandler(StatusUpdateMultiple_MouseDown);
            statusPending.MouseDown += new MouseEventHandler(StatusUpdateMultiple_MouseDown);

            insertMultiAbove.MouseDown += new MouseEventHandler(InsertMultiAbove_MouseDown);
            insertMultiBelow.MouseDown += new MouseEventHandler(InsertMultiBelow_MouseDown);
            pasteRow.MouseDown += new MouseEventHandler(PasteRow_MouseDown);
            deleteMultiRow.MouseDown += new MouseEventHandler(DeleteMultiRow_MouseDown);
            copyMultiRow.MouseDown += new MouseEventHandler(CopyMultiRow_MouseDown);

            menu.Items.AddRange(new ToolStripItem[] 
                        {
                            statusMultiUpdate, 
                            insertAtBottom,
                            insertMultiAbove, 
                            insertMultiBelow,
                            deleteMultiRow, 
                            copyMultiRow, 
                            pasteRow 
                        });
            return menu;
        }

        void insertAtBottomRow_MouseDown(object sender, MouseEventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            outlookGridStore.Rows.Insert(outlookGridStore.Rows.Count - 1, (int)item.Tag);
        }

        #endregion

        #region Outlook Grid View Store

        /// <summary>
        /// Handles the DataError event of the outlookGridStore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewDataErrorEventArgs"/> instance containing the event data.</param>
        private void outlookGridStore_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Assign selected value changed event for combo box.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewEditingControlShowingEventArgs"/> instance containing the event data.</param>
        private void outlookGridStore_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var colIndex = outlookGridStore.CurrentCell.ColumnIndex;
            var rowIndex = outlookGridStore.CurrentCell.RowIndex;

            if (colIndex == StoreColumnIndex.Server
                || colIndex == StoreColumnIndex.Database
                || colIndex == StoreColumnIndex.ActionName
                || colIndex == StoreColumnIndex.Developer
                || colIndex == StoreColumnIndex.Status)
            {
                var combo = e.Control as DataGridViewComboBoxEditingControl;

                if (combo != null)
                {
                    if (_isEventClickComplete)
                    {
                        // Remove an existing event-handler, if present, to avoid 
                        // adding multiple handlers when the editing control is reused.
                        combo.SelectedValueChanged -= new EventHandler(ComboBox_SelectedValueChanged);
                        combo.DropDown -= new EventHandler(ComboBox_DropDown);
                        combo.GotFocus -= new EventHandler(ComboBox_DropDown);

                        // Add the event handler. 
                        combo.SelectedValueChanged += new EventHandler(ComboBox_SelectedValueChanged);
                        combo.DropDown += new EventHandler(ComboBox_DropDown);
                        combo.GotFocus += new EventHandler(ComboBox_DropDown);
                    }

                    combo.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }

            if (colIndex == StoreColumnIndex.Name)
            {
                var storedProcedures = GetStoredProcedures(rowIndex);
                AutoCompleteStringCollection list = new AutoCompleteStringCollection();
                foreach (var item in storedProcedures)
                {
                    list.Add(item.Name);
                }

                var textBox = e.Control as DataGridViewTextBoxEditingControl;
                textBox.AutoCompleteCustomSource = list;
                textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }

            e.CellStyle.BackColor = this.dataGridViewDatabase.DefaultCellStyle.BackColor;
        }

        private IEnumerable<StoreProcedure> GetStoredProcedures(int rowIndex)
        {
            var currentServerName = outlookGridStore[StoreColumnIndex.Server, rowIndex].EditedFormattedValue.ToString();
            var currentDatabaseName = outlookGridStore[StoreColumnIndex.Database, rowIndex].EditedFormattedValue.ToString();
            var currentServer = _listServer.Where(x => x.Name.Equals(currentServerName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var currentDatabase = _listDatabase.Where(x => x.Server.Id == currentServer.Id && x.Name.Equals(currentDatabaseName)).FirstOrDefault();

            return _storeProcedureService.GetStoreProcedure(currentServer.Id, currentDatabase.Id);
        }

        private void ComboBox_DropDown(object sender, EventArgs e)
        {
            ((DataGridViewComboBoxEditingControl)sender).BackColor = Color.White;
        }

        private void TextBox_DropDown(object sender, EventArgs e)
        {
            ((DataGridViewTextBoxEditingControl)sender).BackColor = Color.White;
        }

        /// <summary>
        /// Handles the SelectedValueChanged event of the ComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            var combo = (DataGridViewComboBoxEditingControl)sender;
            var colIndex = outlookGridStore.CurrentCell.ColumnIndex;

            if (colIndex == StoreColumnIndex.Server)
            {
                var rowIndex = outlookGridStore.CurrentCell.RowIndex;
                var serverName = combo.Text;

                if (!string.IsNullOrWhiteSpace(serverName)
                    && !string.Equals(_currentValue, serverName, StringComparison.OrdinalIgnoreCase))
                {
                    var currentDatabaseName = outlookGridStore[StoreColumnIndex.Database, rowIndex].EditedFormattedValue.ToString();
                    var filterListDatabase = _listDatabase.Where(x => x.Server.Name.Equals(serverName, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    var database = filterListDatabase.Where(x => x.Name.Equals(currentDatabaseName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                    if (database == null)
                    {
                        database = filterListDatabase.FirstOrDefault();
                        outlookGridStore[StoreColumnIndex.Database, rowIndex].Value = database != null ? database.Name : string.Empty;
                    }
                }
            }

            combo.SelectedValueChanged -= new EventHandler(ComboBox_SelectedValueChanged);
            combo.DropDown -= new EventHandler(ComboBox_DropDown);
            combo.GotFocus -= new EventHandler(ComboBox_DropDown);
        }

        #region Replace TextBox By ComboBox

        /// <summary>
        /// Handles the CellClick event of the outlookGridStore control.
        /// Will replace TextBox by ComboBox control and binding data to this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void outlookGridStore_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                _isEventClickComplete = false;

                if (_isBindingStoreComplete)
                {
                    int colIndex = e.ColumnIndex;
                    int rowIndex = e.RowIndex;
                    var currentServerName = outlookGridStore[StoreColumnIndex.Server, rowIndex].EditedFormattedValue.ToString();
                    var currentDatabaseName = outlookGridStore[StoreColumnIndex.Database, rowIndex].EditedFormattedValue.ToString();

                    if (rowIndex >= 0)
                    {
                        _currentValue = outlookGridStore[colIndex, rowIndex].EditedFormattedValue.ToString();

                        switch (colIndex)
                        {
                            case StoreColumnIndex.Server:
                                _listServer = _serverService.GetAll();
                                UtilityData.ReplaceServerByGridViewComboBoxCell(outlookGridStore, colIndex, rowIndex, _listServer, _currentValue);
                                break;
                            case StoreColumnIndex.Database:
                                _listDatabase = _databaseService.GetAll();
                                UtilityData.ReplaceDatabaseByGridViewComboBoxCell(outlookGridStore, colIndex, rowIndex, _listDatabase, _currentValue, currentServerName);
                                break;
                            case StoreColumnIndex.ActionName:
                                UtilityData.ReplaceActionByGridViewComboBoxCell(outlookGridStore, colIndex, rowIndex, _currentValue);
                                break;
                            case StoreColumnIndex.Status:
                                UtilityData.ReplaceStatusByGridViewComboBoxCell(outlookGridStore, colIndex, rowIndex, _currentValue);
                                break;
                            case StoreColumnIndex.Developer:
                                UtilityData.ReplaceDeveloperByGridViewComboBoxCell(outlookGridStore, colIndex, rowIndex, _listUser, _currentValue);
                                break;
                        }

                        _storeProcedureIssue = UtilityConverter.ToStoredProcedureIssue(outlookGridStore, e.RowIndex, _listDatabase.ToList(), _listServer.ToList(), _listUser.ToList());
                    }
                }

                _isEventClickComplete = true;
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the CellEnter event of the outlookGridStore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void outlookGridStore_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (_isBindingStoreComplete)
            {
                this.outlookGridStore_CellClick(sender, e);
            }
        }

        /// <summary>
        /// Handles the CellLeave event of the outlookGridStore control.
        /// Will replace ComboBox control by TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void outlookGridStore_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (_isBindingStoreComplete)
                {
                    int colIndex = e.ColumnIndex;
                    int rowIndex = e.RowIndex;

                    if (rowIndex > 0 && colIndex != StoreColumnIndex.IsFunction)
                    {
                        var newValue = outlookGridStore[colIndex, rowIndex].EditedFormattedValue.ToString();

                        if (colIndex == StoreColumnIndex.ActionName && newValue.Length == 1)
                        {
                            newValue = newValue.ToUpper();
                        }

                        DataGridViewTextBoxCell cellText = new DataGridViewTextBoxCell();
                        cellText.Value = newValue;

                        outlookGridStore[colIndex, rowIndex] = cellText;

                        if (!string.Equals(_currentValue, newValue))
                        {
                            outlookGridStore[StoreColumnIndex.ModifierUser, rowIndex].Value = Utility.CurrentUsername;

                            if (string.IsNullOrWhiteSpace(outlookGridStore[StoreColumnIndex.Developer, rowIndex].EditedFormattedValue.ToString()))
                            {
                                outlookGridStore[StoreColumnIndex.Developer, rowIndex].Value = Utility.CurrentUsername;
                            }
                        }

                        CheckDataValidateGridViewStore(e);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Checks the data validate grid view store.
        /// </summary>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void CheckDataValidateGridViewStore(DataGridViewCellEventArgs e)
        {
            int colIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            var currentValue = string.Empty;

            if (!UtilityData.RowIsNotEmpty(outlookGridStore, rowIndex))
            {
                return;
            }

            switch (colIndex)
            {
                case StoreColumnIndex.Server:
                    currentValue = outlookGridStore[StoreColumnIndex.Server, e.RowIndex].EditedFormattedValue.ToString();
                    var server = _listServer.Where(x => x.Name.ToLower() == currentValue.ToLower()).FirstOrDefault();

                    if (string.IsNullOrWhiteSpace(currentValue) || server == null)
                    {
                        outlookGridStore[colIndex, rowIndex].ErrorText = "Invalid";
                    }

                    break;
                case StoreColumnIndex.Database:
                    currentValue = outlookGridStore[StoreColumnIndex.Database, e.RowIndex].EditedFormattedValue.ToString();
                    var database = _listDatabase.Where(x => x.Name.ToLower() == currentValue.ToLower()).FirstOrDefault();

                    if (string.IsNullOrWhiteSpace(currentValue) || database == null)
                    {
                        outlookGridStore[colIndex, rowIndex].ErrorText = "Invalid";
                    }

                    break;
                case StoreColumnIndex.Name:
                    currentValue = outlookGridStore[StoreColumnIndex.Name, e.RowIndex].EditedFormattedValue.ToString();
                    if (string.IsNullOrWhiteSpace(currentValue))
                    {
                        outlookGridStore[colIndex, rowIndex].ErrorText = "Invalid";
                    }

                    break;
                case StoreColumnIndex.ActionName:
                    currentValue = outlookGridStore[StoreColumnIndex.ActionName, e.RowIndex].EditedFormattedValue.ToString();
                    if (UtilityConverter.ToActionID(currentValue) < 0)
                    {
                        outlookGridStore[colIndex, rowIndex].ErrorText = "Invalid";
                    }

                    break;
                case StoreColumnIndex.Status:
                    currentValue = outlookGridStore[StoreColumnIndex.Status, e.RowIndex].EditedFormattedValue.ToString();
                    if (UtilityConverter.ToStatusID(currentValue) < 0)
                    {
                        outlookGridStore[colIndex, rowIndex].ErrorText = "Invalid";
                    }

                    break;
                case StoreColumnIndex.Developer:
                    currentValue = outlookGridStore[StoreColumnIndex.Developer, e.RowIndex].EditedFormattedValue.ToString();
                    var user = _listUser.Where(x => x.Username.ToLower() == currentValue.ToLower()).FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(currentValue) || user == null)
                    {
                        outlookGridStore[colIndex, rowIndex].ErrorText = "Invalid";
                    }

                    break;
            }
        }

        #endregion

        #region Update Stored Procedure

        /// <summary>
        /// Handles the CellEndEdit event of the outlookGridStore control.
        /// Save data when user change it on grid view.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs" /> instance containing the event data.</param>
        private void outlookGridStore_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (_isBindingStoreComplete)
                {
                    int rowIndex = e.RowIndex;

                    if (rowIndex > 0 && outlookGridStore.IsValidStoredProcedure(rowIndex))
                    {
                        StoreProcedureIssue storeProcedureIssue = outlookGridStore.ToStoredProcedureIssue(rowIndex, _listDatabase, _listServer, _listUser);
                        List<StoreProcedure> storeProcedures = GetStoredProcedures(rowIndex).ToList();

                        StoreProcedure storedProcedure = storeProcedures.Where(x => x.Name.Equals(storeProcedureIssue.StoreProcedure.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                        int storedProcedureId = storedProcedure != null ? storedProcedure.Id : 0;

                        if (storeProcedureIssue != null)
                        {
                            int storedProcedureIssueId = storeProcedureIssue.Id;
                            if (storedProcedureIssueId > 0)
                            {
                                UpdateNewStoredProcedureIssue(rowIndex, storeProcedureIssue, storedProcedureId, storedProcedureIssueId);
                            }
                            else
                            {
                                InsertNewStoredProcedureIssue(rowIndex, storeProcedureIssue, storedProcedureId, storedProcedureIssueId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        private void InsertNewStoredProcedureIssue(int rowIndex, StoreProcedureIssue storeProcedureIssue, int storedProcedureId, int storedProcedureIssueId)
        {
            // Find issue id in current selected group
            int issueId = UtilityData.FindIssueIdInGroup(outlookGridStore, txtIssueId, rowIndex);

            // Insert new
            if (storeProcedureIssue.StoreProcedure.Server.Id != 0
                && storeProcedureIssue.StoreProcedure.Database.Id != 0
                && !string.IsNullOrWhiteSpace(storeProcedureIssue.StoreProcedure.Name)
                && storeProcedureIssue.Status != -1
                && !string.IsNullOrWhiteSpace(storeProcedureIssue.ActionName)
                && !string.IsNullOrWhiteSpace(storeProcedureIssue.StoreProcedure.Name))
            {
                _storeProcedureService.InsertOrUpdate(
                    ref storedProcedureIssueId,
                    storeProcedureIssue.StoreProcedure.Server.Id,
                    storeProcedureIssue.StoreProcedure.Database.Id,
                    storeProcedureIssue.StoreProcedure.Name,
                    storeProcedureIssue.IsFunction,
                    storeProcedureIssue.ActionName,
                    _userId,
                    storeProcedureIssue.Status,
                    storeProcedureIssue.Description,
                    issueId,
                    storeProcedureIssue.Developer.Id,
                    storedProcedureId,
                    true);

                outlookGridStore[StoreColumnIndex.Id, rowIndex].Value = storedProcedureIssueId;
                outlookGridStore[StoreColumnIndex.IssueId, rowIndex].Value = issueId;
                outlookGridStore[StoreColumnIndex.StoredProcedureId, rowIndex].Value = storedProcedureId;

                toolStripStatusStore.Text = Resource.AddSuccessfully + "( Row " + (rowIndex + 1).ToString() + ")";
                outlookGridStore[StoreColumnIndex.ModifierUser, rowIndex].Value = Utility.CurrentUsername;
            }

        }

        private void UpdateNewStoredProcedureIssue(int rowIndex, StoreProcedureIssue storeProcedureIssue, int storedProcedureId, int storedProcedureIssueId)
        {
            if (!UtilityData.StoreProcedureEquals(_storeProcedureIssue, storeProcedureIssue))
            {
                bool isAddNew = storedProcedureId == 0;

                if (storedProcedureId == 0 && _storeProcedureIssue.StoreProcedure.Id != 0)
                {
                    List<Issue> issues = _issueService.GetByStoredProcedureId(_storeProcedureIssue.StoreProcedure.Id).ToList();
                    var msgIssueRelated = string.Empty;
                    foreach (var item in issues)
                    {
                        msgIssueRelated += "#" + item.ToString() + Environment.NewLine;
                    }

                    CustomMessageBox.MsgBox msgBox = new CustomMessageBox.MsgBox(
                        string.Format("The stored procedure : {0}.{1}.{2}", _storeProcedureIssue.StoreProcedure.Server.Name, _storeProcedureIssue.StoreProcedure.Database.Name, _storeProcedureIssue.StoreProcedure.Name)
                        + Environment.NewLine + "Related to issues :" + Environment.NewLine
                        + msgIssueRelated + Environment.NewLine
                        + "Do you want create new or update all related issues ?",
                        "Update stored procedure information");
                    msgBox.SetButtons(new string[] { "Create New", "Update All" }, new DialogResult[] { DialogResult.Yes, DialogResult.No }, 2);
                    DialogResult r = msgBox.ShowDialog(this);

                    if (r == DialogResult.Yes)
                    {
                        isAddNew = true;
                    }
                    else
                    {
                        isAddNew = false;
                        storedProcedureId = _storeProcedureIssue.StoreProcedure.Id;

                        for (int i = 1; i < outlookGridStore.Rows.Count; i++)
                        {
                            if (outlookGridStore[StoreColumnIndex.StoredProcedureId, i].EditedFormattedValue.ToString()
                                == _storeProcedureIssue.StoreProcedure.Id.ToString())
                            {
                                outlookGridStore[StoreColumnIndex.Name, i].Value = storeProcedureIssue.StoreProcedure.Name;
                            }
                        }
                    }
                }

                // Update data
                _storeProcedureService.InsertOrUpdate(
                    ref storedProcedureIssueId,
                    storeProcedureIssue.StoreProcedure.Server.Id,
                    storeProcedureIssue.StoreProcedure.Database.Id,
                    storeProcedureIssue.StoreProcedure.Name,
                    storeProcedureIssue.IsFunction,
                    storeProcedureIssue.ActionName,
                    _userId,
                    storeProcedureIssue.Status,
                    storeProcedureIssue.Description,
                    storeProcedureIssue.Issue.Id,
                    storeProcedureIssue.Developer.Id,
                    storedProcedureId,
                    isAddNew);

                toolStripStatusStore.Text = Resource.UpdateSuccessfully + "( Row " + (rowIndex + 1).ToString() + ")";
                outlookGridStore[StoreColumnIndex.ModifierUser, rowIndex].Value = Utility.CurrentUsername;
            }
        }

        #endregion

        #region Context Menu

        /// <summary>
        /// Handles the CellMouseDown event of the outlookGridStore control.
        /// Show context strip menu
        /// + Insert 1 above
        /// + Insert 1 below
        /// + Delete row
        /// + Copy
        /// + Paste.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellMouseEventArgs"/> instance containing the event data.</param>
        private void outlookGridStore_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                var issueId = outlookGridStore.Rows[e.RowIndex].Tag;

                ////Checks for correct column index
                if (e.Button == MouseButtons.Right
                    && e.RowIndex != -1
                    && outlookGridStore.CurrentCell != null
                    && issueId == null)
                {
                    DataGridViewSelectedRowCollection selectedRows = outlookGridStore.SelectedRows;

                    int maxIndex = 0;
                    int minIndex = 0;
                    int countSelectedRow = selectedRows.Count;

                    UtilityData.FindMinMaxIndex(selectedRows, ref maxIndex, ref minIndex);

                    ContextMenuStrip menu = GenerateContextMenuStripGridViewStore(maxIndex, minIndex, countSelectedRow);

                    if (outlookGridStore.CurrentCell.RowIndex == e.RowIndex
                        && countSelectedRow == 1)
                    {
                        ToolStripMenuItem viewDetailsStoredProcedure = new ToolStripMenuItem("Open Ctrl + Q");

                        var databaseName = outlookGridStore[StoreColumnIndex.Database, minIndex].FormattedValue.ToString();
                        var listDatabase = _listDatabase.Where(x => x.Name.ToLower() == databaseName.ToLower()).ToList();
                        if (listDatabase.Count > 1)
                        {
                            ToolStripMenuItem databaseMenu;
                            foreach (var item in listDatabase)
                            {
                                databaseMenu = new ToolStripMenuItem(string.Format("{0}.{1}", item.Server.Name, item.Name));
                                databaseMenu.Tag = item.Id.ToString();
                                databaseMenu.MouseDown += new MouseEventHandler(DatabaseMenu_MouseDown);
                                viewDetailsStoredProcedure.DropDownItems.Add(databaseMenu);
                            }
                        }

                        viewDetailsStoredProcedure.MouseDown += new MouseEventHandler(ViewDetailsStoredProcedure_MouseDown);

                        menu.Items.Add(viewDetailsStoredProcedure);

                        ////Assign created context menu strip to the DataGridView
                        outlookGridStore.ContextMenuStrip = menu;
                    }
                    else if (e.RowIndex >= minIndex && e.RowIndex <= maxIndex)
                    {
                        ////Assign created context menu strip to the DataGridView
                        outlookGridStore.ContextMenuStrip = menu;
                    }
                    else
                    {
                        outlookGridStore.ContextMenuStrip = null;
                    }
                }
                else
                {
                    outlookGridStore.ContextMenuStrip = null;
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the DatabaseMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void DatabaseMenu_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                int databaseId = Convert.ToInt32(((ToolStripMenuItem)sender).Tag.ToString());
                var storeName = outlookGridStore[StoreColumnIndex.Name, outlookGridStore.CurrentRow.Index].FormattedValue.ToString();
                var database = _listDatabase.Where(x => x.Id == databaseId).FirstOrDefault();

                Task.Factory.StartNew(() =>
                {
                    string value = UtilityIO.GetDetailsStoredProcedure(storeName, database.Server, database);
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        UtilityIO.OpenFileDetailStoredProcedure(storeName, value);
                    }
                });
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the ViewDetailsStoredProcedure control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void ViewDetailsStoredProcedure_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (outlookGridStore.CurrentCell != null
                    && ((ToolStripMenuItem)sender).DropDownItems.Count == 0)
                {
                    int rowIndex = outlookGridStore.CurrentCell.RowIndex;

                    var storeProcedure = UtilityConverter.ToStoredProcedureIssue(
                        outlookGridStore,
                        rowIndex,
                        _listDatabase.ToList(),
                        _listServer.ToList(),
                        _listUser.ToList());

                    Task.Factory.StartNew(() =>
                    {
                        string value = UtilityIO.GetDetailsStoredProcedure(storeProcedure.StoreProcedure.Name, storeProcedure.StoreProcedure.Server, storeProcedure.StoreProcedure.Database);
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            UtilityIO.OpenFileDetailStoredProcedure(storeProcedure.StoreProcedure.Name, value);
                        }
                    });
                }
            }
            catch
            {
                MessageBox.Show(Resource.CanNotConnectToDatabaseOrSPNotFound);
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the StatusUpdateMultiple control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void StatusUpdateMultiple_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                ToolStripItem menuItem = sender as ToolStripItem;
                int statusId = UtilityConverter.ToStatusID(menuItem.Text);

                DataGridViewSelectedRowCollection selectedRows = outlookGridStore.SelectedRows;
                string storeIds = string.Empty;
                int minIndex = 0;
                int maxIndex = 0;
                UtilityData.FindMinMaxIndex(selectedRows, ref maxIndex, ref minIndex);

                if (selectedRows != null && selectedRows.Count > 0)
                {
                    foreach (DataGridViewRow item in selectedRows)
                    {
                        storeIds += outlookGridStore[StoreColumnIndex.Id, item.Index].EditedFormattedValue + ",";

                        outlookGridStore[StoreColumnIndex.Status, item.Index].Value = menuItem.Text;
                        outlookGridStore[StoreColumnIndex.ModifierUser, item.Index].Value = Utility.CurrentUsername;
                    }
                }

                if (!string.IsNullOrWhiteSpace(storeIds))
                {
                    _storeProcedureService.UpdateStatus(storeIds, statusId, _userId);

                    if (minIndex == maxIndex)
                    {
                        toolStripStatusStore.Text = Resource.UpdateSuccessfully + "( Row " + (minIndex + 1).ToString() + ")";
                    }
                    else
                    {
                        toolStripStatusStore.Text = Resource.UpdateSuccessfully + "( Row " + (minIndex + 1).ToString() + " - " + (maxIndex + 1).ToString() + ")";
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the CopyMultiRow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void CopyMultiRow_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                string value = string.Empty;

                foreach (DataGridViewRow item in outlookGridStore.SelectedRows)
                {
                    value += UtilityConverter.ToString(outlookGridStore, item.Index) + "\n";
                }

                Clipboard.Clear();
                Clipboard.SetText(value, TextDataFormat.Text);
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the InsertMultiBelow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void InsertMultiBelow_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!ReferenceEquals(outlookGridStore.CurrentCell, null))
                {
                    DataGridViewSelectedRowCollection selectedRows = outlookGridStore.SelectedRows;

                    int maxIndex = 0;
                    int minIndex = 0;
                    int countSelectedRow = selectedRows.Count;

                    UtilityData.FindMinMaxIndex(selectedRows, ref maxIndex, ref minIndex);

                    outlookGridStore.Rows.Insert(maxIndex + 1, countSelectedRow);
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the InsertMultiAbove control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void InsertMultiAbove_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!ReferenceEquals(outlookGridStore.CurrentCell, null))
                {
                    DataGridViewSelectedRowCollection selectedRows = outlookGridStore.SelectedRows;

                    int maxIndex = 0;
                    int minIndex = 0;
                    int countSelectedRow = selectedRows.Count;

                    UtilityData.FindMinMaxIndex(selectedRows, ref maxIndex, ref minIndex);

                    if (minIndex > 0)
                    {
                        outlookGridStore.Rows.Insert(minIndex, outlookGridStore.SelectedRows.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the DeleteMultiRow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void DeleteMultiRow_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridViewSelectedRowCollection selectedRows = outlookGridStore.SelectedRows;

                if (selectedRows != null
                    && selectedRows.Count > 0)
                {
                    int minIndex = 0;
                    int maxIndex = 0;
                    UtilityData.FindMinMaxIndex(selectedRows, ref maxIndex, ref minIndex);

                    foreach (DataGridViewRow item in selectedRows)
                    {
                        var strId = outlookGridStore[StoreColumnIndex.Id, item.Index].EditedFormattedValue.ToString();

                        if (!string.IsNullOrWhiteSpace(strId))
                        {
                            int storeId = Convert.ToInt32(strId);

                            _storeProcedureService.Delete(storeId, _userId);

                            outlookGridStore.Rows.RemoveAt(item.Index);
                        }
                        else
                        {
                            if (!outlookGridStore.Rows[item.Index].IsNewRow)
                            {
                                outlookGridStore.Rows.RemoveAt(item.Index);
                            }
                            else
                            {
                                outlookGridStore.CurrentCell = outlookGridStore[StoreColumnIndex.ModifierUser, item.Index];

                                for (int i = 0; i < outlookGridStore.Columns.Count; i++)
                                {
                                    DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                                    cell.Value = string.Empty;
                                    outlookGridStore[i, item.Index] = cell;
                                }

                                for (int i = 0; i < outlookGridStore.Rows.Count; i++)
                                {
                                    outlookGridStore.Rows[i].Selected = false;
                                }
                            }
                        }
                    }

                    if (minIndex == maxIndex)
                    {
                        toolStripStatusStore.Text = Resource.DeleteSuccessfully + "( Row " + (minIndex + 1).ToString() + ")";
                    }
                    else
                    {
                        toolStripStatusStore.Text = Resource.DeleteSuccessfully + "( Row " + (minIndex + 1).ToString() + " - " + (maxIndex + 1).ToString() + ")";
                    }
                }
                else
                {
                    MessageBox.Show(Resource.SelectRowsThatYouWantToDelete);
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the PasteRow control.
        /// Paste data from Clipboard to grid view and save to DB.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void PasteRow_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                string copyValue = Clipboard.GetText();

                if (!string.IsNullOrWhiteSpace(copyValue))
                {
                    copyValue = copyValue.Trim('\n');
                    string[] rowValue = copyValue.Split('\n');

                    if (outlookGridStore.SelectedRows.Count > 0)
                    {
                        var errorIndexs = new List<int>();
                        var errorServers = new List<string>();
                        var errorDatabases = new List<string>();
                        var errorUsernames = new List<string>();

                        if (outlookGridStore.CurrentCell != null)
                        {
                            int rowIndex = outlookGridStore.CurrentCell.RowIndex;
                            int indexArray = 0;

                            foreach (var value in rowValue)
                            {
                                if (!string.IsNullOrWhiteSpace(value))
                                {
                                    string[] array = value.Trim('\t').Split('\t');

                                    int number = 0;
                                    if (int.TryParse(array[0], out number))
                                    {
                                        for (int i = 1; i < array.Length; i++)
                                        {
                                            array[i - 1] = array[i];
                                        }

                                        array[7] = string.Empty;
                                    }

                                    if (!string.IsNullOrWhiteSpace(array[0]) && array.Length >= 7)
                                    {
                                        array[3] = array[3] == "1" || array[3].ToLower() == "true" ? "true" : "false";

                                        var storeProcedureIssue = new StoreProcedureIssue
                                        {
                                            StoreProcedure = new StoreProcedure
                                            {
                                                Server = new Server { Name = array[0].Trim() },
                                                Database = new Database { Name = array[1].Trim() },
                                                Name = array[2].Trim()
                                            },
                                            IsFunction = Convert.ToBoolean(array[3].Trim()),
                                            ActionName = array[4].Trim(),
                                            Developer = new User { Username = array[5].Trim() },
                                            Status = UtilityConverter.ToStatusID(array[6].Trim()),
                                            Description = array.Length >= 8 ? array[7].Trim() : string.Empty
                                        };

                                        var server = _listServer.Where(x => x.Name.ToLower() == storeProcedureIssue.StoreProcedure.Server.Name.ToLower()).FirstOrDefault();

                                        if (server == null)
                                        {
                                            ////errorIndexs.Add(rowIndex);
                                            if (!errorServers.Contains(storeProcedureIssue.StoreProcedure.Server.Name))
                                            {
                                                errorServers.Add(storeProcedureIssue.StoreProcedure.Server.Name);
                                            }
                                        }
                                        else
                                        {
                                            var database = _listDatabase.Where(x => x.Name.ToLower() == storeProcedureIssue.StoreProcedure.Database.Name.ToLower()).FirstOrDefault();

                                            if (database == null)
                                            {
                                                ////errorIndexs.Add(rowIndex);
                                                if (!errorDatabases.Contains(storeProcedureIssue.StoreProcedure.Database.Name))
                                                {
                                                    errorDatabases.Add(storeProcedureIssue.StoreProcedure.Database.Name);
                                                }
                                            }
                                            else
                                            {
                                                var user = _listUser.Where(x => x.Username.ToLower() == storeProcedureIssue.Developer.Username.ToLower()).FirstOrDefault();

                                                if (user == null)
                                                {
                                                    errorUsernames.Add(storeProcedureIssue.Developer.Username);
                                                }
                                                else
                                                {
                                                    var row = new OutlookGridRow();
                                                    row.CreateCells(
                                                        outlookGridStore,
                                                        rowIndex,
                                                        storeProcedureIssue.StoreProcedure.Server.Name,
                                                        storeProcedureIssue.StoreProcedure.Database.Name,
                                                        storeProcedureIssue.StoreProcedure.Name,
                                                        storeProcedureIssue.IsFunction,
                                                        storeProcedureIssue.ActionName,
                                                        storeProcedureIssue.Developer.Username,
                                                        storeProcedureIssue.StatusName,
                                                        storeProcedureIssue.Description,
                                                         string.Empty,
                                                         string.Empty,
                                                         string.Empty,
                                                         Utility.CurrentUsername);

                                                    outlookGridStore.Rows.InsertRange(rowIndex, row);
                                                    this.outlookGridStore_CellEndEdit(null, new DataGridViewCellEventArgs(StoreColumnIndex.Id, rowIndex));

                                                    outlookGridStore[StoreColumnIndex.IssueId, rowIndex].Value = UtilityData.FindIssueIdInGroup(outlookGridStore, txtIssueId, rowIndex);

                                                    rowIndex += 1;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        errorIndexs.Add(indexArray);
                                    }
                                }
                                else
                                {
                                    errorIndexs.Add(indexArray);
                                }

                                indexArray++;
                            }
                        }

                        if (errorIndexs.Count > 0
                            || errorServers.Count > 0
                            || errorDatabases.Count > 0
                            || errorUsernames.Count > 0)
                        {
                            string errorServer = string.Empty;
                            string errorDatabase = string.Empty;
                            string errorUserName = string.Empty;

                            foreach (var item in errorServers)
                            {
                                errorServer += "[" + item + "]" + " ";
                            }

                            foreach (var item in errorDatabases)
                            {
                                errorDatabase += "[" + item + "]" + " ";
                            }

                            foreach (var item in errorUsernames)
                            {
                                errorUserName += "[" + item + "]" + " ";
                            }

                            string message = string.Empty;

                            if (!string.IsNullOrWhiteSpace(errorServer))
                            {
                                message += "+ Server : " + errorServer + "\n";
                            }

                            if (!string.IsNullOrWhiteSpace(errorDatabase))
                            {
                                message += "+ Database : " + errorDatabase + "\n";
                            }

                            if (!string.IsNullOrWhiteSpace(errorUserName))
                            {
                                message += "+ Modified By : " + errorUserName + "\n";
                            }

                            if (!string.IsNullOrWhiteSpace(message))
                            {
                                MessageBox.Show("Check your data not exists !\n" + message);
                            }
                        }
                    }
                    else
                    {
                        int rowIndex = outlookGridStore.CurrentCell.RowIndex;
                        int colIndex = outlookGridStore.CurrentCell.ColumnIndex;
                        int currentRowIndex = rowIndex;
                        int currentColIndex = colIndex;

                        foreach (var item in rowValue)
                        {
                            if (!string.IsNullOrWhiteSpace(item))
                            {
                                string[] arrayItem = item.Split('\t');

                                foreach (var itemCell in arrayItem)
                                {
                                    outlookGridStore[currentColIndex, currentRowIndex].Value = itemCell;
                                    outlookGridStore_CellEndEdit(sender, new DataGridViewCellEventArgs(outlookGridStore.CurrentCell.ColumnIndex, outlookGridStore.CurrentCell.RowIndex));

                                    currentColIndex++;
                                }

                                currentRowIndex++;
                                currentColIndex = colIndex;
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        #endregion

        #region User Deleting Row

        /// <summary>
        /// Handles the UserDeletingRow event of the outlookGridStore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewRowCancelEventArgs"/> instance containing the event data.</param>
        private void outlookGridStore_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                if (e.Row != null)
                {
                    var rowIndex = e.Row.Index;

                    var strStoreId = outlookGridStore[StoreColumnIndex.Id, rowIndex].EditedFormattedValue.ToString();

                    if (!string.IsNullOrWhiteSpace(strStoreId))
                    {
                        int storeId = Convert.ToInt32(strStoreId);

                        _storeProcedureService.Delete(storeId, _userId);

                        toolStripStatusStore.Text = Resource.DeleteSuccessfully + "( Row " + (rowIndex + 1).ToString() + ")";
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    MessageBox.Show(Resource.SelectRowsThatYouWantToDelete);
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        #endregion

        #region Search Box and Paste

        /// <summary>
        /// Handles the KeyDown event of the outlookGridStore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void outlookGridStore_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                if (e.KeyCode == Keys.V)
                {
                    PasteRow_MouseDown(null, null);
                }

                if (e.KeyCode == Keys.Q)
                {
                    ViewDetailsStoredProcedure_MouseDown(sender, null);
                }
            }
            else
            {
                if (e.Control && e.Shift && e.KeyCode == Keys.F)
                {
                    ShowSearchBox();
                }
                else
                {
                    OnKeyDown(e);
                }
            }
        }

        #endregion

        #region Export Excel

        /// <summary>
        /// Handles the Click event of the button Export control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (outlookGridStore.Rows.Count > 1)
                {
                    _hssfworkbook = new HSSFWorkbook();
                    saveFileDialog.Filter = "Excel Files | *.xls";
                    saveFileDialog.DefaultExt = "xls";
                    var dialog = saveFileDialog.ShowDialog();

                    if (dialog == DialogResult.OK)
                    {
                        var fileName = saveFileDialog.FileName;

                        UtilityIO.ExportExcel(outlookGridStore, _hssfworkbook, fileName);

                        CustomMessageBox.MsgBox msgBox = new CustomMessageBox.MsgBox("Export file successfully !", "Export excel");
                        msgBox.SetButtons(new string[] { "Open file", "OK" }, new DialogResult[] { DialogResult.Yes, DialogResult.No }, 2);
                        DialogResult r = msgBox.ShowDialog(this);

                        if (r == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(fileName);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(Resource.NoDataAvailable);
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        #endregion

        #endregion

        #region Data Grid View Server

        /// <summary>
        /// Handles the SelectionChanged event of the dataGridViewServer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void dataGridViewServer_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewServer.CurrentRow != null)
                {
                    var currentRow = dataGridViewServer.CurrentRow.Index;
                    var strServerId = dataGridViewServer[ServerColumnIndex.Id, currentRow].EditedFormattedValue.ToString();

                    if (!string.IsNullOrWhiteSpace(strServerId))
                    {
                        var serverId = Convert.ToInt32(strServerId);

                        UtilityData.BindingDataToGridViewDatabase(dataGridViewDatabase, _listDatabase, serverId);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the CellValueChanged event of the dataGridViewServer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataGridViewServer_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var rowIndex = e.RowIndex;

                if (rowIndex >= 0)
                {
                    var server = new Server
                    {
                        Name = dataGridViewServer[ServerColumnIndex.Name, rowIndex].EditedFormattedValue.ToString(),
                        IPLocal = dataGridViewServer[ServerColumnIndex.IPLocal, rowIndex].EditedFormattedValue.ToString(),
                        IPUAT = dataGridViewServer[ServerColumnIndex.IPUAT, rowIndex].EditedFormattedValue.ToString(),
                        IPProduction = dataGridViewServer[ServerColumnIndex.IPProduction, rowIndex].EditedFormattedValue.ToString(),
                        Description = dataGridViewServer[ServerColumnIndex.Description, rowIndex].EditedFormattedValue.ToString()
                    };

                    var strServerId = dataGridViewServer[ServerColumnIndex.Id, rowIndex].EditedFormattedValue.ToString();
                    if (!string.IsNullOrWhiteSpace(strServerId))
                    {
                        server.Id = Convert.ToInt32(strServerId);
                    }

                    var tempData = _listServer.Where(x => x.Name.Equals(server.Name)).FirstOrDefault();

                    if (tempData == null || server.Id != 0)
                    {
                        int id = server.Id;
                        _serverService.InsertOrUpdate(ref id, server.Name, server.IPLocal, server.IPUAT, server.IPProduction, server.Description, _userId);

                        _listServer = _serverService.GetAll();
                        this.dataGridViewServer[ServerColumnIndex.Id, rowIndex].Value = id;
                        this.dataGridViewServer[ServerColumnIndex.Name, rowIndex].ErrorText = string.Empty;
                    }
                    else
                    {
                        MessageBox.Show(Resource.ServerAlreadyExists);
                        this.dataGridViewServer[ServerColumnIndex.Name, rowIndex].ErrorText = "Invalid";
                        //dataGridViewServer.Rows.RemoveAt(rowIndex);
                    }

                    dataGridViewServer.Rows[rowIndex].Selected = true;
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the UserDeletingRow event of the dataGridViewServer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewRowCancelEventArgs"/> instance containing the event data.</param>
        private void dataGridViewServer_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                if (e.Row != null)
                {
                    var rowIndex = e.Row.Index;

                    var strServerId = dataGridViewServer[ServerColumnIndex.Id, rowIndex].EditedFormattedValue.ToString();

                    if (!string.IsNullOrWhiteSpace(strServerId))
                    {
                        DialogResult diaLog = MessageBox.Show(
                            "All databases and stored procedures related to this server will be delete!",
                            "Confirm delete database",
                            MessageBoxButtons.YesNo);

                        if (diaLog == DialogResult.Yes)
                        {
                            int serverId = Convert.ToInt32(strServerId);

                            _serverService.Delete(serverId, _userId);

                            _listServer = _serverService.GetAll();
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Can not delete this row !");
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the CellMouseDown event of the dataGridViewServer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellMouseEventArgs"/> instance containing the event data.</param>
        private void dataGridViewServer_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                ////Checks for correct column index
                if (e.Button == MouseButtons.Right
                    && e.RowIndex != -1)
                {
                    ////Create the ContextStripMenu for Creating the PO Sub Form
                    ContextMenuStrip menu = new ContextMenuStrip();
                    ToolStripMenuItem serverDeleteRow = new ToolStripMenuItem("Delete Row");

                    serverDeleteRow.MouseDown += new MouseEventHandler(ServerDeleteRow_MouseDown);

                    menu.Items.AddRange(new ToolStripItem[] { serverDeleteRow });

                    ////Assign created context menu strip to the DataGridView
                    dataGridViewServer.ContextMenuStrip = menu;
                }
                else
                {
                    dataGridViewServer.ContextMenuStrip = null;
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the ServerDeleteRow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void ServerDeleteRow_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (dataGridViewServer.CurrentCell != null)
                {
                    var currentRowIndex = dataGridViewServer.CurrentCell.RowIndex;
                    var strId = dataGridViewServer[ServerColumnIndex.Id, currentRowIndex].EditedFormattedValue.ToString();

                    if (!string.IsNullOrWhiteSpace(strId))
                    {
                        DialogResult diaLog = MessageBox.Show(
                            "All databases and stored procedures related to this server will be delete!",
                            "Confirm delete database",
                            MessageBoxButtons.YesNo);

                        if (diaLog == DialogResult.Yes)
                        {
                            int serverId = Convert.ToInt32(strId);

                            _serverService.Delete(serverId, _userId);
                            _listServer = _serverService.GetAll();
                            dataGridViewServer.Rows.RemoveAt(currentRowIndex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        #endregion

        #region Data Grid View Database

        /// <summary>
        /// Handles the CellValueChanged event of the dataGridViewDatabase control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataGridViewDatabase_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var rowIndex = e.RowIndex;

                if (rowIndex >= 0)
                {
                    var database = new Database();
                    database.Name = dataGridViewDatabase[DatabaseColumnIndex.Name, rowIndex].EditedFormattedValue.ToString();
                    database.Description = dataGridViewDatabase[DatabaseColumnIndex.Description, rowIndex].EditedFormattedValue.ToString();
                    database.UserID = dataGridViewDatabase[DatabaseColumnIndex.UserID, rowIndex].EditedFormattedValue.ToString();
                    if (e.ColumnIndex == DatabaseColumnIndex.Password)
                    {
                        database.Password = dataGridViewDatabase[DatabaseColumnIndex.Password, rowIndex].EditedFormattedValue.ToString();
                    }
                    else
                    {
                        database.Password = dataGridViewDatabase.Rows[rowIndex].Tag != null ? dataGridViewDatabase.Rows[rowIndex].Tag.ToString() : string.Empty;
                    }

                    var strDatabaseId = dataGridViewDatabase[DatabaseColumnIndex.Id, rowIndex].EditedFormattedValue.ToString();
                    if (!string.IsNullOrWhiteSpace(strDatabaseId))
                    {
                        database.Id = Convert.ToInt32(strDatabaseId);
                    }

                    var strServerId = dataGridViewServer[ServerColumnIndex.Id, dataGridViewServer.CurrentRow.Index].EditedFormattedValue.ToString();

                    if (!string.IsNullOrWhiteSpace(strServerId))
                    {
                        database.Server = new Server { Id = Convert.ToInt32(strServerId) };

                        var tempData = _listDatabase.Where(
                            x => x.Name.Equals(database.Name, StringComparison.CurrentCultureIgnoreCase)
                            && x.Server.Id == database.Server.Id
                            && x.Id != database.Id).FirstOrDefault();

                        if (tempData == null)
                        {
                            int id = database.Id;
                            _databaseService.InsertOrUpdate(ref id, database.Name, database.Description, database.Server.Id, database.UserID, database.Password, _userId);

                            _listDatabase = _databaseService.GetAll();
                            dataGridViewDatabase[DatabaseColumnIndex.Id, rowIndex].Value = id;
                            this.dataGridViewDatabase[DatabaseColumnIndex.Name, rowIndex].ErrorText = string.Empty;
                        }
                        else
                        {
                            MessageBox.Show(Resource.DatabaseAleadyExistsInThisServer);
                            this.dataGridViewDatabase[DatabaseColumnIndex.Name, rowIndex].ErrorText = "Invalid";
                            this.dataGridViewDatabase.CancelEdit();
                        }

                        dataGridViewDatabase.Rows[rowIndex].Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the UserDeletingRow event of the dataGridViewDatabase control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewRowCancelEventArgs"/> instance containing the event data.</param>
        private void dataGridViewDatabase_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                if (e.Row != null)
                {
                    var rowIndex = e.Row.Index;

                    var strDatabaseId = dataGridViewDatabase[DatabaseColumnIndex.Id, rowIndex].EditedFormattedValue.ToString();

                    if (!string.IsNullOrWhiteSpace(strDatabaseId))
                    {
                        DialogResult diaLog = MessageBox.Show("All stored procedures related to this database will be delete!", "Confirm delete database", MessageBoxButtons.YesNo);

                        if (diaLog == DialogResult.Yes)
                        {
                            int databaseId = Convert.ToInt32(strDatabaseId);

                            _databaseService.Delete(databaseId, _userId);
                            _listDatabase = _databaseService.GetAll();
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the CellMouseDown event of the dataGridViewDatabase control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellMouseEventArgs"/> instance containing the event data.</param>
        private void dataGridViewDatabase_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                ////Checks for correct column index
                if (e.Button == MouseButtons.Right
                    && e.RowIndex != -1)
                {
                    ////Create the ContextStripMenu for Creating the PO Sub Form
                    ContextMenuStrip menu = new ContextMenuStrip();
                    ToolStripMenuItem databaseDeleteRow = new ToolStripMenuItem("Delete Row");

                    databaseDeleteRow.MouseDown += new MouseEventHandler(DatabaseDeleteRow_MouseDown);

                    menu.Items.AddRange(new ToolStripItem[] { databaseDeleteRow });

                    ////Assign created context menu strip to the DataGridView
                    dataGridViewDatabase.ContextMenuStrip = menu;
                }
                else
                {
                    dataGridViewDatabase.ContextMenuStrip = null;
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the DatabaseDeleteRow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void DatabaseDeleteRow_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (dataGridViewDatabase.CurrentCell != null)
                {
                    var currentRowIndex = dataGridViewDatabase.CurrentCell.RowIndex;
                    var strId = dataGridViewDatabase[DatabaseColumnIndex.Id, currentRowIndex].EditedFormattedValue.ToString();

                    if (!string.IsNullOrWhiteSpace(strId))
                    {
                        DialogResult diaLog = MessageBox.Show("All stored procedures related to this database will be delete!", "Confirm delete database", MessageBoxButtons.YesNo);

                        if (diaLog == DialogResult.Yes)
                        {
                            int databaseId = Convert.ToInt32(strId);

                            _databaseService.Delete(databaseId, _userId);
                            _listDatabase = _databaseService.GetAll();
                            dataGridViewDatabase.Rows.RemoveAt(currentRowIndex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the EditingControlShowing event of the dataGridViewDatabase control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewEditingControlShowingEventArgs"/> instance containing the event data.</param>
        private void dataGridViewDatabase_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            int colIndex = dataGridViewDatabase.CurrentCell.ColumnIndex;
            if (colIndex == DatabaseColumnIndex.Password)
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.UseSystemPasswordChar = true;
                }
            }
            else
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.UseSystemPasswordChar = false;
                }
            }
        }

        /// <summary>
        /// Handles the CellFormatting event of the dataGridViewDatabase control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellFormattingEventArgs"/> instance containing the event data.</param>
        private void dataGridViewDatabase_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == DatabaseColumnIndex.Password
                && e.Value != null
                && !string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                dataGridViewDatabase.Rows[e.RowIndex].Tag = e.Value;
                e.Value = new string('*', 10);
            }
        }
        #endregion

        #region Data Grid View My Issue

        /// <summary>
        /// Handles the CellContentClick event of the dataGridViewMyIssue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataGridViewMyIssue_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                int colIndex = e.ColumnIndex;

                if (colIndex == 2)
                {
                    var strId = dataGridViewMyIssue[0, rowIndex].EditedFormattedValue.ToString();

                    if (!string.IsNullOrWhiteSpace(strId))
                    {
                        int issueId = Convert.ToInt32(strId);

                        _issueService.DeleteFavorite(Utility.CurrentUsername, issueId);

                        _listIssue = _issueService.GetFavoritesByUsername(Utility.CurrentUsername);
                        UtilityData.BindingIssueToGridView(dataGridViewMyIssue, _listIssue);
                        UtilityData.BindingIssueToComboBox(cmboMyIssues, _listIssue);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the SelectionChangeCommitted event of the combo box my issues control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void cmboMyIssues_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                int currentIndex = cmboMyIssues.SelectedIndex;

                if (currentIndex > 0)
                {
                    var selectedIssue = (Issue)cmboMyIssues.SelectedItem;
                    txtIssueId.Reset();

                    this.LoadStore(selectedIssue.Id.ToString());

                    cmboMyIssues.SelectedIndex = currentIndex;
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the combo box My Issues control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void cmboMyIssues_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    var suggestText = cmboMyIssues.Text;

                    for (int i = 1; i < cmboMyIssues.Items.Count; i++)
                    {
                        Issue issue = (Issue)cmboMyIssues.Items[i];

                        if (string.Equals(issue.ToString(), suggestText))
                        {
                            txtIssueId.Text = string.Empty;
                            LoadStore(issue.Id.ToString());

                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the Click event of the button AddMyIssue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnAddMyIssue_Click(object sender, EventArgs e)
        {
            try
            {
                var strIssueId = txtMyIssuesId.Text.Trim();

                if (!string.IsNullOrWhiteSpace(strIssueId))
                {
                    var issueId = Convert.ToInt32(strIssueId);

                    _issueService.InsertFavorite(Utility.CurrentUsername, issueId);

                    var oldCount = _listIssue.Count();

                    _listIssue = _issueService.GetFavoritesByUsername(Utility.CurrentUsername);

                    var newCount = _listIssue.Count();
                    if (oldCount == newCount)
                    {
                        MessageBox.Show("Issue not found or already added !");
                    }
                    else
                    {
                        UtilityData.BindingIssueToComboBox(cmboMyIssues, _listIssue);
                        MessageBox.Show(Resource.AddSuccessfully);
                    }

                    UtilityData.BindingIssueToGridView(dataGridViewMyIssue, _listIssue);
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the UserDeletingRow event of the dataGridViewMyIssue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewRowCancelEventArgs"/> instance containing the event data.</param>
        private void dataGridViewMyIssue_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                if (e.Row != null)
                {
                    var rowIndex = e.Row.Index;

                    var strIssueId = dataGridViewMyIssue[0, rowIndex].EditedFormattedValue.ToString();

                    if (!string.IsNullOrWhiteSpace(strIssueId))
                    {
                        int issueId = Convert.ToInt32(strIssueId);

                        _issueService.DeleteFavorite(Utility.CurrentUsername, issueId);

                        _listIssue = _issueService.GetFavoritesByUsername(Utility.CurrentUsername);
                        UtilityData.BindingIssueToGridView(dataGridViewMyIssue, _listIssue);
                        UtilityData.BindingIssueToComboBox(cmboMyIssues, _listIssue);
                    }
                    else
                    {
                        MessageBox.Show("Can not delete this row !");
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the CellMouseDown event of the dataGridViewMyIssue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellMouseEventArgs"/> instance containing the event data.</param>
        private void dataGridViewMyIssue_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                ////Checks for correct column index
                if (e.Button == MouseButtons.Right
                    && e.RowIndex != -1)
                {
                    ////Create the ContextStripMenu for Creating the PO Sub Form
                    ContextMenuStrip menu = new ContextMenuStrip();
                    ToolStripMenuItem myIssueDeleteRow = new ToolStripMenuItem("Delete Row");

                    myIssueDeleteRow.MouseDown += new MouseEventHandler(MyIssueDeleteRow_MouseDown);

                    menu.Items.AddRange(new ToolStripItem[] { myIssueDeleteRow });

                    ////Assign created context menu strip to the DataGridView
                    dataGridViewMyIssue.ContextMenuStrip = menu;
                }
                else
                {
                    dataGridViewMyIssue.ContextMenuStrip = null;
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the MyIssueDeleteRow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void MyIssueDeleteRow_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (dataGridViewMyIssue.CurrentCell != null)
                {
                    var currentRowIndex = dataGridViewMyIssue.CurrentCell.RowIndex;
                    var strId = dataGridViewMyIssue[0, currentRowIndex].EditedFormattedValue.ToString();

                    if (!string.IsNullOrWhiteSpace(strId))
                    {
                        int issueId = Convert.ToInt32(strId);

                        _issueService.DeleteFavorite(Utility.CurrentUsername, issueId);

                        _listIssue = _issueService.GetFavoritesByUsername(Utility.CurrentUsername);
                        UtilityData.BindingIssueToGridView(dataGridViewMyIssue, _listIssue);
                        UtilityData.BindingIssueToComboBox(cmboMyIssues, _listIssue);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex);
            }
        }

        #endregion

        #endregion

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.Main_Load(null, null);
        }

        private void btnSubmitLog_Click(object sender, EventArgs e)
        {
            if (gridLog.RowsCount > 2)
            {
                while (gridLog.RowsCount > 2)
                {
                    gridLog.Rows.Remove(2);
                }
            }

            var logs = _logService.SearchLog(txtNameLog.Text.Trim(),
                            txtUsernameLog.Text.Trim(),
                            dtpFromLog.Value,
                            dtpToLog.Value,
                            cmboFilterTypeLog.SelectedIndex + 1).ToList();

            UtilityData.BindingLogDataToGrid(gridLog, logs);
        }
    }
}