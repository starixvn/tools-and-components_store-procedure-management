// --------------------------------------------------------------------------
// <copyright file="UtilityExportExcel.cs" company="Nexcel Solutions Vietnam">
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
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;
    using Fanex.ManagementSP.Entities;
    using NPOI.HPSF;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using OutlookStyleControls;

    /// <summary>
    /// The utility IO class.
    /// </summary>
    public static class UtilityIO
    {
        private static string _configPath = @"C:\Program Files (x86)\Nexcel Solutions\Manage SP Tool\config.txt";

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

            UtilityIO.InitializeWorkbook(hssfworkbook);

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
                                valueCell.SetCellValue((bool)outlookGridStore[j + 1, i].EditedFormattedValue ? 1 : 0);
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
        /// <param name="storeName">Name of the store.</param>
        /// <param name="value">The value.</param>
        public static void OpenFileDetailStoredProcedure(string storeName, string value)
        {
            string path = Path.GetTempPath();

            File.WriteAllText(path + "/" + storeName + ".sql", value);

            Process.Start(path + "/" + storeName + ".sql");
        }

        #endregion
    }
}
