// --------------------------------------------------------------------------
// <copyright file="UtilityUI.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.12.27">Created</change>
// </history>

namespace Fanex.ManagementSP.UI
{
    using System.Drawing;
    using System.Windows.Forms;
    using Fanex.ManagementSP.UI.ColumnIndex;
    using OutlookStyleControls;
    using SourceGrid;

    /// <summary>
    /// The utility user interface class.
    /// </summary>
    public static class UtilityUI
    {
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
            //outlookGridView.Columns.Add("IsFunction", "Is Function");

            DataGridViewCheckBoxColumn colIsFunction = new DataGridViewCheckBoxColumn();
            colIsFunction.Name = "IsFunction";
            colIsFunction.HeaderText = "Function";
            outlookGridView.Columns.Add(colIsFunction);

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
            outlookGridView.Columns[StoreColumnIndex.ActionName].Width = 70;
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

        public static void InitHeaderColumnsGridLog(Grid grid)
        {
            grid.Redim(2, 8);
            grid.FixedRows = 2;

            //1 Header Row
            grid[0, 0] = new MyHeader("LogId");
            grid[0, 0].RowSpan = 2;
            grid[0, 0].Column.AutoSizeMode = SourceGrid.AutoSizeMode.MinimumSize;
            grid[0, 1] = new MyHeader("Log Date");
            grid[0, 1].RowSpan = 2;
            grid[0, 2] = new MyHeader("Name");
            grid[0, 2].RowSpan = 2;
            grid[0, 3] = new MyHeader("Modifier");
            grid[0, 3].RowSpan = 2;
            grid[0, 4] = new MyHeader("Action");
            grid[0, 4].RowSpan = 2;
            grid[0, 5] = new MyHeader("Description");
            grid[0, 5].ColumnSpan = 3;
            grid[0, 5].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableStretch;

            ////2 Header Row
            grid[1, 5] = new MyHeader("#");
            grid[1, 6] = new MyHeader("New Value");
            grid[1, 7] = new MyHeader("Old Value");

            grid.AutoStretchColumnsToFitWidth = true;
            grid.AutoSizeCells();
        }

        /// <summary>
        /// Initializes the skin outlook grid.
        /// </summary>
        /// <param name="outlookGridStore">The outlook grid store.</param>
        /// <param name="imageList">The image list.</param>
        /// <param name="redmineIssueUri">The REDMINE issue URI.</param>
        public static void InitSkinOutlookGrid(OutlookGrid outlookGridStore, ImageList imageList, string redmineIssueUri)
        {
            outlookGridStore.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            outlookGridStore.DefaultCellStyle = dataGridViewCellStyle2;
            outlookGridStore.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;

            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            outlookGridStore.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;

            outlookGridStore.GridColor = System.Drawing.SystemColors.Control;
            outlookGridStore.RowTemplate.Height = 22;
            outlookGridStore.BackgroundColor = System.Drawing.SystemColors.GrayText;

            outlookGridStore.RowHeadersVisible = false;
            outlookGridStore.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            outlookGridStore.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;

            outlookGridStore.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            outlookGridStore.AllowUserToResizeColumns = true;
            outlookGridStore.AllowUserToResizeRows = false;

            outlookGridStore.ExpandIcon = imageList.Images[0];
            outlookGridStore.CollapseIcon = imageList.Images[1];
            outlookGridStore.IconLinkRedmine = imageList.Images[2];
            outlookGridStore.LinkToRedmineIssue = redmineIssueUri;

            outlookGridStore.MultiSelect = true;
            outlookGridStore.ShowCellErrors = true;
            outlookGridStore.ShowRowErrors = true;
            outlookGridStore.ShowEditingIcon = true;
            outlookGridStore.ShowCellToolTips = true;
        }

        /// <summary>
        /// Skins the default grid view.
        /// </summary>
        /// <param name="gridView">The grid view.</param>
        public static void InitSkinGridView(DataGridView gridView)
        {
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
            gridView.RowHeadersVisible = false;
            gridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            gridView.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            gridView.AllowUserToResizeColumns = true;
            gridView.AllowUserToResizeRows = false;
            gridView.ShowCellErrors = true;
            gridView.ShowCellToolTips = true;
            gridView.ShowEditingIcon = true;
            gridView.ShowRowErrors = true;
        }
    }

    public class MyHeader : SourceGrid.Cells.ColumnHeader
    {
        public MyHeader(object value)
            : base(value)
        {
            //1 Header Row
            SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
            view.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
            view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            View = view;

            AutomaticSortEnabled = false;
        }
    }
}
