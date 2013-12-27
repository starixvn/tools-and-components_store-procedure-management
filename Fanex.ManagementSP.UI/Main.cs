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
    using Fanex.ManagementSP.Client.Services;
    using Fanex.ManagementSP.Entities;
    using Fanex.ManagementSP.UI.ColumnIndex;
    using NPOI.HSSF.UserModel;
    using OutlookStyleControls;
    using Fanex.ManagementSP.UI.Extend;

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

        // Global variables
        private string _currentValue = string.Empty;
        private IEnumerable<Server> _listServer;
        private IEnumerable<Database> _listDatabase;
        private IEnumerable<StoreProcedure> _listStoreProcedure;
        private IEnumerable<Issue> _listIssue;
        private IEnumerable<User> _listUser;

        private HSSFWorkbook _hssfworkbook;
        private string _apiUri = string.Empty;
        private string _redmineIssueUri = string.Empty;
        private bool _isBindingStoreComplete = false;
        private bool _isEventClickComplete = false;
        private StoreProcedure _storeProcedure = new StoreProcedure();

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
            string[] setting = Utility.GetConfigSetting();
            _apiUri = setting[0];
            _redmineIssueUri = setting[1];

            _storeProcedureService = new StoreProcedureService(_apiUri);
            _serverService = new ServerService(_apiUri);
            _databaseService = new DatabaseService(_apiUri);
            _issueService = new IssueService(_apiUri);
            _userService = new UserService(_apiUri);

            txtIssueId.Reset();
            chbQueryAll.Checked = true;

            ////splitContainer1.Panel1.BackColor = Color.FromArgb(188, 199, 216);
            txtIssueId.Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            txtMyIssuesId.Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);

            // Init skin
            SkinDefaultOutlookGrid();
            SkinDefaultGridView(dataGridViewServer);
            SkinDefaultGridView(dataGridViewDatabase);
            SkinDefaultGridView(dataGridViewMyIssue);

            // Init header columns                
            Utility.InitHeaderColumnsDataGridViewServer(dataGridViewServer);
            Utility.InitHeaderColumnsDataGridViewDatabase(dataGridViewDatabase);
            Utility.InitHeaderColumnsDataGridViewMyIssue(dataGridViewMyIssue);

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

            // Binding data to control
            Utility.BindingDataToGridViewServer(dataGridViewServer, _listServer);
            Utility.BindingIssueToGridView(dataGridViewMyIssue, _listIssue);
            Utility.BindingIssueToComboBox(cmboMyIssues, _listIssue);

            // Binding stored procedure data
            string issueIds = txtIssueId.Text.Trim();
            if (!string.IsNullOrWhiteSpace(issueIds))
            {
                this.LoadStore(issueIds);
            }

            txtAPIUrl.Text = _apiUri;
            txtRedmineIssueUri.Text = _redmineIssueUri;
            ////outlookGridStore.MyIssues = _listIssue.ToList();
        }

        #endregion

        #region Event

        #region Others

        /// <summary>
        /// Handles the TextChanged event of the txtIssueId control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void txtIssueId_TextChanged(object sender, EventArgs e)
        {
            ////this.LoadStore(txtIssueId.Text.Trim());            
        }

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
            Utility.InitHeaderColumnsDataGridViewStore(outlookGridStore);

            _listStoreProcedure = _storeProcedureService.GetByIssueId(issueIds, chbQueryAll.Checked);

            if (!ReferenceEquals(_listStoreProcedure, null)
                && _listStoreProcedure.Any())
            {
                Utility.BindingDataToGridViewStore(outlookGridStore, _listStoreProcedure.OrderBy(x => x.Issue.Id).ToList());
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
                Utility.UpdateConfigSetting(value);

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

            ToolStripMenuItem insertMultiAbove = new ToolStripMenuItem(string.Format("Insert {0} above", countSelectedRow));
            ToolStripMenuItem insertMultiBelow = new ToolStripMenuItem(string.Format("Insert {0} below", countSelectedRow));
            ToolStripMenuItem pasteRow = new ToolStripMenuItem("Paste Ctrl + V");
            ToolStripMenuItem deleteMultiRow = new ToolStripMenuItem(menuTextDelete);
            ToolStripMenuItem copyMultiRow = new ToolStripMenuItem("Copy Ctrl + C");

            ToolStripMenuItem statusMultiUpdate = new ToolStripMenuItem("Status");
            ToolStripItem statusInProgress = new ToolStripMenuItem("In Progress");
            ToolStripItem statusDone = new ToolStripMenuItem("Done");
            ToolStripItem statusUAT = new ToolStripMenuItem("UAT");
            ToolStripItem statusUATXTest = new ToolStripMenuItem("UAT [ _xtest]");
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
                            insertMultiAbove, 
                            insertMultiBelow,
                            deleteMultiRow, 
                            copyMultiRow, 
                            pasteRow 
                        });
            return menu;
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
                //// do something here
            }
            catch (Exception)
            {
            }
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

                    if (rowIndex >= 0)
                    {
                        _currentValue = outlookGridStore[colIndex, rowIndex].EditedFormattedValue.ToString();

                        switch (colIndex)
                        {
                            case StoreColumnIndex.Server:
                                _listServer = _serverService.GetAll();
                                Utility.ReplaceServerByGridViewComboBoxCell(outlookGridStore, colIndex, rowIndex, _listServer, _currentValue);
                                break;
                            case StoreColumnIndex.Database:
                                _listDatabase = _databaseService.GetAll();
                                var currentServer = outlookGridStore[StoreColumnIndex.Server, rowIndex].EditedFormattedValue.ToString();
                                Utility.ReplaceDatabaseByGridViewComboBoxCell(outlookGridStore, colIndex, rowIndex, _listDatabase, _currentValue, currentServer);
                                break;
                            case StoreColumnIndex.Name:
                                bool isImageType = outlookGridStore[colIndex, rowIndex].FormattedValueType.Name == "Image";
                                if (isImageType)
                                {
                                    DataGridViewTextBoxCell cellText = new DataGridViewTextBoxCell();
                                    cellText.Value = string.Empty;
                                    outlookGridStore[colIndex, rowIndex] = cellText;
                                }

                                break;
                            case StoreColumnIndex.IsFunction:
                                Utility.ReplaceIsFunctionByGridViewComboBoxCell(outlookGridStore, colIndex, rowIndex, _currentValue);
                                break;
                            case StoreColumnIndex.ActionName:
                                Utility.ReplaceActionByGridViewComboBoxCell(outlookGridStore, colIndex, rowIndex, _currentValue);
                                break;
                            case StoreColumnIndex.Status:
                                Utility.ReplaceStatusByGridViewComboBoxCell(outlookGridStore, colIndex, rowIndex, _currentValue);
                                break;
                            case StoreColumnIndex.Developer:
                                Utility.ReplaceDeveloperByGridViewComboBoxCell(outlookGridStore, colIndex, rowIndex, _listUser, _currentValue);
                                break;
                        }

                        _storeProcedure = Utility.ConvertGridStoreRowToStoredProcedure(outlookGridStore, e.RowIndex, _listDatabase.ToList(), _listServer.ToList(), _listUser.ToList());
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
            if (_isBindingStoreComplete && _isEventClickComplete)
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

                    if (rowIndex > 0)
                    {
                        var newValue = outlookGridStore[colIndex, rowIndex].EditedFormattedValue.ToString();

                        if (outlookGridStore[colIndex, rowIndex].FormattedValueType.Name == "Image")
                        {
                            newValue = string.Empty;
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

            switch (colIndex)
            {
                case StoreColumnIndex.Server:
                    currentValue = outlookGridStore[StoreColumnIndex.Server, e.RowIndex].EditedFormattedValue.ToString();
                    var server = _listServer.Where(x => x.Name.ToLower() == currentValue.ToLower()).FirstOrDefault();

                    if (string.IsNullOrWhiteSpace(currentValue)
                        || server == null)
                    {
                        DataGridViewImageCell cell = new DataGridViewImageCell();
                        outlookGridStore[StoreColumnIndex.Server, e.RowIndex] = cell;
                    }

                    break;
                case StoreColumnIndex.Database:
                    currentValue = outlookGridStore[StoreColumnIndex.Database, e.RowIndex].EditedFormattedValue.ToString();
                    var database = _listDatabase.Where(x => x.Name.ToLower() == currentValue.ToLower()).FirstOrDefault();

                    if (string.IsNullOrWhiteSpace(currentValue)
                        || database == null)
                    {
                        DataGridViewImageCell cell = new DataGridViewImageCell();
                        outlookGridStore[StoreColumnIndex.Database, e.RowIndex] = cell;
                    }

                    break;
                case StoreColumnIndex.Name:
                    currentValue = outlookGridStore[StoreColumnIndex.Name, e.RowIndex].EditedFormattedValue.ToString();
                    if (string.IsNullOrWhiteSpace(currentValue))
                    {
                        DataGridViewImageCell cell = new DataGridViewImageCell();
                        outlookGridStore[StoreColumnIndex.Name, e.RowIndex] = cell;
                    }

                    break;
                case StoreColumnIndex.IsFunction:
                    currentValue = outlookGridStore[StoreColumnIndex.IsFunction, e.RowIndex].EditedFormattedValue.ToString();
                    if (currentValue != "0"
                        || currentValue != "1")
                    {
                        ////DataGridViewImageCell cell = new DataGridViewImageCell();
                        outlookGridStore[StoreColumnIndex.IsFunction, e.RowIndex].Value = "0";
                    }

                    break;
                case StoreColumnIndex.ActionName:
                    currentValue = outlookGridStore[StoreColumnIndex.ActionName, e.RowIndex].EditedFormattedValue.ToString();
                    if (Utility.ConvertActionNameToId(currentValue) < 0)
                    {
                        DataGridViewImageCell cell = new DataGridViewImageCell();
                        outlookGridStore[StoreColumnIndex.ActionName, e.RowIndex] = cell;
                    }

                    break;
                case StoreColumnIndex.Status:
                    currentValue = outlookGridStore[StoreColumnIndex.Status, e.RowIndex].EditedFormattedValue.ToString();
                    if (Utility.ConvertStatusNameToId(currentValue) < 0)
                    {
                        DataGridViewImageCell cell = new DataGridViewImageCell();
                        outlookGridStore[StoreColumnIndex.Status, e.RowIndex] = cell;
                    }

                    break;
                case StoreColumnIndex.Developer:
                    currentValue = outlookGridStore[StoreColumnIndex.Developer, e.RowIndex].EditedFormattedValue.ToString();
                    var user = _listUser.Where(x => x.Username.ToLower() == currentValue.ToLower()).FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(currentValue)
                        || user == null)
                    {
                        DataGridViewImageCell cell = new DataGridViewImageCell();
                        outlookGridStore[StoreColumnIndex.Developer, e.RowIndex] = cell;
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
                        var storeProcedure = Utility.ConvertGridStoreRowToStoredProcedure(
                            outlookGridStore,
                            rowIndex,
                            _listDatabase.ToList(),
                            _listServer.ToList(),
                            _listUser.ToList());

                        if (storeProcedure != null)
                        {
                            int storeId = storeProcedure.Id;
                            if (storeProcedure.Id > 0)
                            {
                                if (!Utility.StoreProcedureEquals(_storeProcedure, storeProcedure))
                                {
                                    ////Update data
                                    _storeProcedureService.InsertOrUpdate(
                                        ref storeId,
                                        storeProcedure.Server.Id,
                                        storeProcedure.Database.Id,
                                        storeProcedure.Name,
                                        storeProcedure.IsFunction,
                                        storeProcedure.ActionName,
                                        storeProcedure.Modifier.Username,
                                        storeProcedure.Status,
                                        storeProcedure.Description,
                                        storeProcedure.Issue.Id,
                                        storeProcedure.Developer.Id);

                                    toolStripStatusStore.Text = Resource.UpdateSuccessfully + "( Row " + (rowIndex + 1).ToString() + ")";
                                }
                            }
                            else
                            {
                                // Find issue id in current selected group
                                int issueId = Utility.FindIssueIdInGroup(outlookGridStore, txtIssueId, rowIndex);

                                // Insert new
                                if (storeProcedure.Server.Id != 0
                                    && storeProcedure.Database.Id != 0
                                    && !string.IsNullOrWhiteSpace(storeProcedure.Name)
                                    && storeProcedure.Status != -1
                                    && !string.IsNullOrWhiteSpace(storeProcedure.ActionName)
                                    && !string.IsNullOrWhiteSpace(storeProcedure.Name))
                                {
                                    _storeProcedureService.InsertOrUpdate(
                                    ref storeId,
                                    storeProcedure.Server.Id,
                                    storeProcedure.Database.Id,
                                    storeProcedure.Name,
                                    storeProcedure.IsFunction,
                                    storeProcedure.ActionName,
                                    storeProcedure.Modifier.Username,
                                    storeProcedure.Status,
                                    storeProcedure.Description,
                                    issueId,
                                    storeProcedure.Developer.Id);

                                    outlookGridStore[StoreColumnIndex.Id, rowIndex].Value = storeId;
                                    outlookGridStore[StoreColumnIndex.IssueId, rowIndex].Value = issueId;

                                    toolStripStatusStore.Text = Resource.AddSuccessfully + "( Row " + (rowIndex + 1).ToString() + ")";
                                }
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

                    Utility.FindMinMaxIndex(selectedRows, ref maxIndex, ref minIndex);

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

        private void DatabaseMenu_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                int databaseId = Convert.ToInt32(((ToolStripMenuItem)sender).Tag.ToString());
                var storeName = outlookGridStore[StoreColumnIndex.Name, outlookGridStore.CurrentRow.Index].FormattedValue.ToString();
                var database = _listDatabase.Where(x => x.Id == databaseId).FirstOrDefault();

                Task.Factory.StartNew(() =>
                {
                    string value = Utility.GetDetailsStoredProcedure(storeName, database.Server, database);
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        Utility.OpenFileDetailStoredProcedure(storeName, value);
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

                    var storeProcedure = Utility.ConvertGridStoreRowToStoredProcedure(
                        outlookGridStore,
                        rowIndex,
                        _listDatabase.ToList(),
                        _listServer.ToList(),
                        _listUser.ToList());

                    Task.Factory.StartNew(() =>
                    {
                        string value = Utility.GetDetailsStoredProcedure(storeProcedure.Name, storeProcedure.Server, storeProcedure.Database);
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            Utility.OpenFileDetailStoredProcedure(storeProcedure.Name, value);
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
                int statusId = Utility.ConvertStatusNameToId(menuItem.Text);

                DataGridViewSelectedRowCollection selectedRows = outlookGridStore.SelectedRows;
                string storeIds = string.Empty;
                int minIndex = 0;
                int maxIndex = 0;
                Utility.FindMinMaxIndex(selectedRows, ref maxIndex, ref minIndex);

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
                    _storeProcedureService.UpdateStatus(storeIds, statusId, Utility.CurrentUsername);

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
                    value += Utility.ConvertDataGridViewStoreRowToString(outlookGridStore, item.Index) + "\n";
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

                    Utility.FindMinMaxIndex(selectedRows, ref maxIndex, ref minIndex);

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

                    Utility.FindMinMaxIndex(selectedRows, ref maxIndex, ref minIndex);

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
                    Utility.FindMinMaxIndex(selectedRows, ref maxIndex, ref minIndex);

                    foreach (DataGridViewRow item in selectedRows)
                    {
                        var strId = outlookGridStore[StoreColumnIndex.Id, item.Index].EditedFormattedValue.ToString();

                        if (!string.IsNullOrWhiteSpace(strId))
                        {
                            int storeId = Convert.ToInt32(strId);

                            _storeProcedureService.Delete(storeId);

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
                                    var storeProcedure = new StoreProcedure
                                    {
                                        Server = new Server { Name = array[0].Trim() },
                                        Database = new Database { Name = array[1].Trim() },
                                        Name = array[2].Trim(),
                                        IsFunction = array[3].Trim() == "1",
                                        ActionName = array[4].Trim(),
                                        Developer = new User { Username = array[5].Trim() },
                                        Status = Utility.ConvertStatusNameToId(array[6].Trim()),
                                        Description = array.Length >= 8 ? array[7].Trim() : string.Empty
                                    };

                                    var server = _listServer.Where(x => x.Name.ToLower() == storeProcedure.Server.Name.ToLower()).FirstOrDefault();

                                    if (server == null)
                                    {
                                        ////errorIndexs.Add(rowIndex);
                                        if (!errorServers.Contains(storeProcedure.Server.Name))
                                        {
                                            errorServers.Add(storeProcedure.Server.Name);
                                        }
                                    }
                                    else
                                    {
                                        var database = _listDatabase.Where(x => x.Name.ToLower() == storeProcedure.Database.Name.ToLower()).FirstOrDefault();

                                        if (database == null)
                                        {
                                            ////errorIndexs.Add(rowIndex);
                                            if (!errorDatabases.Contains(storeProcedure.Database.Name))
                                            {
                                                errorDatabases.Add(storeProcedure.Database.Name);
                                            }
                                        }
                                        else
                                        {
                                            var user = _listUser.Where(x => x.Username.ToLower() == storeProcedure.Developer.Username.ToLower()).FirstOrDefault();

                                            if (user == null)
                                            {
                                                errorUsernames.Add(storeProcedure.Modifier.Username);
                                            }
                                            else
                                            {
                                                var row = new OutlookGridRow();
                                                row.CreateCells(
                                                    outlookGridStore,
                                                    rowIndex,
                                                    storeProcedure.Server.Name,
                                                    storeProcedure.Database.Name,
                                                    storeProcedure.Name,
                                                    storeProcedure.IsFunction ? "1" : "0",
                                                    storeProcedure.ActionName,
                                                    storeProcedure.Developer.Username,
                                                    storeProcedure.StatusName,
                                                    storeProcedure.Description,
                                                     string.Empty,
                                                     string.Empty,
                                                     string.Empty,
                                                     Utility.CurrentUsername);

                                                outlookGridStore.Rows.InsertRange(rowIndex, row);
                                                this.outlookGridStore_CellEndEdit(null, new DataGridViewCellEventArgs(StoreColumnIndex.Id, rowIndex));

                                                outlookGridStore[StoreColumnIndex.IssueId, rowIndex].Value = Utility.FindIssueIdInGroup(outlookGridStore, txtIssueId, rowIndex);

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

                        _storeProcedureService.Delete(storeId);

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
                    base.OnKeyDown(e);
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

                        Utility.ExportExcel(outlookGridStore, _hssfworkbook, fileName);

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

                        Utility.BindingDataToGridViewDatabase(dataGridViewDatabase, _listDatabase, serverId);
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

                    var tempData = _listServer.Where(x => x.Name.ToLower() == server.Name.ToLower()).FirstOrDefault();

                    if (tempData == null || server.Id != 0)
                    {
                        _serverService.InsertOrUpdate(server.Id, server.Name, server.IPLocal, server.IPUAT, server.IPProduction, server.Description);

                        _listServer = _serverService.GetAll();
                        Utility.BindingDataToGridViewServer(dataGridViewServer, _listServer);
                    }
                    else
                    {
                        MessageBox.Show(Resource.ServerAlreadyExists);
                        dataGridViewServer.Rows.RemoveAt(rowIndex);
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
                        DialogResult diaLog = MessageBox.Show("All databases and stored procedures related to this server will be delete!", "Confirm delete database", MessageBoxButtons.YesNo);

                        if (diaLog == DialogResult.Yes)
                        {
                            int serverId = Convert.ToInt32(strServerId);

                            _serverService.Delete(serverId);

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
                    ////CellValue = outlookGrid1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
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
                        DialogResult diaLog = MessageBox.Show("All databases and stored procedures related to this server will be delete!", "Confirm delete database", MessageBoxButtons.YesNo);

                        if (diaLog == DialogResult.Yes)
                        {
                            int serverId = Convert.ToInt32(strId);

                            _serverService.Delete(serverId);
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
        private void dataGridViewDatabase_CellValueChanged(object sender, DataGridViewCellEventArgs e)
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
                            x => x.Name == database.Name
                            && x.Description == database.Description
                            && x.UserID == database.UserID
                            && x.Password == database.Password
                            && x.Server.Id == database.Server.Id).FirstOrDefault();

                        if (tempData == null)
                        {
                            _databaseService.InsertOrUpdate(database.Id, database.Name, database.Description, database.Server.Id, database.UserID, database.Password);

                            _listDatabase = _databaseService.GetAll();
                            Utility.BindingDataToGridViewDatabase(dataGridViewDatabase, _listDatabase, database.Server.Id);
                        }
                        else
                        {
                            MessageBox.Show(Resource.DatabaseAleadyExistsInThisServer);
                            this.dataGridViewDatabase_UserDeletingRow(sender, new DataGridViewRowCancelEventArgs(dataGridViewDatabase.Rows[rowIndex]));
                            this.dataGridViewDatabase.Rows.RemoveAt(rowIndex);
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

                            _databaseService.Delete(databaseId);
                            _listDatabase = _databaseService.GetAll();
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        ////MessageBox.Show(Resource.CanNotDelete);
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
                    ////CellValue = outlookGrid1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
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

                            _databaseService.Delete(databaseId);
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

                if (rowIndex > 0 && colIndex == 2)
                {
                    var strId = dataGridViewMyIssue[0, rowIndex].EditedFormattedValue.ToString();

                    if (!string.IsNullOrWhiteSpace(strId))
                    {
                        int issueId = Convert.ToInt32(strId);

                        _issueService.DeleteFavorite(Utility.CurrentUsername, issueId);

                        _listIssue = _issueService.GetFavoritesByUsername(Utility.CurrentUsername);
                        Utility.BindingIssueToGridView(dataGridViewMyIssue, _listIssue);
                        Utility.BindingIssueToComboBox(cmboMyIssues, _listIssue);
                        ////outlookGridStore.MyIssues = _listIssue.ToList();
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
                    txtIssueId.Text = string.Empty;

                    this.LoadStore(selectedIssue.Id.ToString());

                    cmboMyIssues.SelectedIndex = currentIndex;
                }
            }
            catch (Exception ex)
            {
                ////this.LoadStore(string.Empty);
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
                        Utility.BindingIssueToComboBox(cmboMyIssues, _listIssue);
                    }

                    Utility.BindingIssueToGridView(dataGridViewMyIssue, _listIssue);
                    ////outlookGridStore.MyIssues = _listIssue.ToList();
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
                        Utility.BindingIssueToGridView(dataGridViewMyIssue, _listIssue);
                        Utility.BindingIssueToComboBox(cmboMyIssues, _listIssue);
                        ////outlookGridStore.MyIssues = _listIssue.ToList();
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
                    ////CellValue = outlookGrid1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
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
                        Utility.BindingIssueToGridView(dataGridViewMyIssue, _listIssue);
                        Utility.BindingIssueToComboBox(cmboMyIssues, _listIssue);
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

        #region Skin For Grid

        /// <summary>
        /// Skins the default outlook grid.
        /// </summary>
        private void SkinDefaultOutlookGrid()
        {
            this.outlookGridStore.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.outlookGridStore.DefaultCellStyle = dataGridViewCellStyle2;
            this.outlookGridStore.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;

            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.outlookGridStore.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;

            this.outlookGridStore.GridColor = System.Drawing.SystemColors.Control;
            this.outlookGridStore.RowTemplate.Height = 22;
            this.outlookGridStore.BackgroundColor = System.Drawing.SystemColors.GrayText;
            ////this.outlookGrid1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.outlookGridStore.RowHeadersVisible = false;
            this.outlookGridStore.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.outlookGridStore.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            ////this.outlookGrid1.AllowUserToAddRows = false;
            ////this.outlookGrid1.AllowUserToDeleteRows = false;
            ////this.outlookGrid1.AllowUserToResizeRows = false;
            ////this.outlookGrid1.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.outlookGridStore.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.outlookGridStore.AllowUserToResizeColumns = true;
            this.outlookGridStore.AllowUserToResizeRows = false;

            this.outlookGridStore.ExpandIcon = imageList.Images[0];
            this.outlookGridStore.CollapseIcon = imageList.Images[1];
            this.outlookGridStore.IconLinkRedmine = imageList.Images[2];
            this.outlookGridStore.LinkToRedmineIssue = _redmineIssueUri;
            ////this.outlookGridStore.IconAddFavorites = imageList.Images[3];
            ////this.outlookGridStore.IconRemoveFavorites = imageList.Images[4];
            ////outlookGridStore.UpdateFavoriteIssues = this.UpdateFavoriteIssue;

            this.outlookGridStore.MultiSelect = true;
        }

        /// <summary>
        /// Skins the default grid view.
        /// </summary>
        /// <param name="gridView">The grid view.</param>
        private void SkinDefaultGridView(DataGridView gridView)
        {
            this.outlookGridStore.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            gridView.DefaultCellStyle = dataGridViewCellStyle2;
            gridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;

            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            gridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;

            gridView.GridColor = System.Drawing.SystemColors.Control;
            gridView.RowTemplate.Height = 22;
            gridView.BackgroundColor = System.Drawing.SystemColors.GrayText;
            ////this.outlookGrid1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridView.RowHeadersVisible = false;
            gridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ////this.outlookGrid1.AllowUserToAddRows = false;
            ////this.outlookGrid1.AllowUserToDeleteRows = false;
            ////this.outlookGrid1.AllowUserToResizeRows = false;
            ////this.outlookGrid1.EditMode = DataGridViewEditMode.EditProgrammatically;
            gridView.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            gridView.AllowUserToResizeColumns = true;
            gridView.AllowUserToResizeRows = false;
            gridView.ShowCellErrors = true;
            gridView.ShowCellToolTips = true;
            gridView.ShowEditingIcon = true;
            gridView.ShowRowErrors = true;
        }

        #endregion

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

        private void outlookGridStore_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var colIndex = outlookGridStore.CurrentCell.ColumnIndex;

            if (colIndex == StoreColumnIndex.Server && _isEventClickComplete)
            {
                DataGridViewComboBoxEditingControl combo = e.Control as DataGridViewComboBoxEditingControl;

                if (combo != null)
                {
                    // Remove an existing event-handler, if present, to avoid 
                    // adding multiple handlers when the editing control is reused.
                    combo.SelectedValueChanged -= new EventHandler(ComboBox_SelectedValueChanged);
                    combo.DropDown -= new EventHandler(combo_DropDown);
                    combo.GotFocus -= new EventHandler(combo_DropDown);

                    // Add the event handler. 
                    combo.SelectedValueChanged += new EventHandler(ComboBox_SelectedValueChanged);
                    combo.DropDown += new EventHandler(combo_DropDown);
                    combo.GotFocus += new EventHandler(combo_DropDown);
                }
            }
        }

        private void combo_DropDown(object sender, EventArgs e)
        {
            ((DataGridViewComboBoxEditingControl)sender).BackColor = Color.White;
        }

        private string _value = string.Empty;

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
                    var filterListDatabase = _listDatabase.Where(x => x.Server.Name.ToLower() == serverName.ToLower()).ToList();
                    var database = filterListDatabase.Where(x => x.Name.ToLower() == currentDatabaseName.ToLower()).FirstOrDefault();

                    if (database == null)
                    {
                        database = filterListDatabase.FirstOrDefault();
                        outlookGridStore[StoreColumnIndex.Database, rowIndex].Value = database != null ? database.Name : string.Empty;
                    }
                }
            }

            combo.SelectedValueChanged -= new EventHandler(ComboBox_SelectedValueChanged);
            combo.DropDown -= new EventHandler(combo_DropDown);
            combo.GotFocus -= new EventHandler(combo_DropDown);
        }
    }
}