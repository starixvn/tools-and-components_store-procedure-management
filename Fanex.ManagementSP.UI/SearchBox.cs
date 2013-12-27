// --------------------------------------------------------------------------
// <copyright file="SearchBox.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.11.22">Created</change>
// </history>

namespace Fanex.ManagementSP.UI
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Forms;

    /// <summary>
    /// The search box class.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
    public partial class SearchBox : Form
    {
        public SearchBox()
        {
            InitializeComponent();

            btnFindAll.Enabled = false;
        }

        public string SearchText { get; set; }

        /// <summary>
        /// Handles the Click event of the button FindAll control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnFindAll_Click(object sender, EventArgs e)
        {
            this.SearchText = txtNameStoredProcedure.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Handles the TextChanged event of the textbox NameStoredProcedure control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void txtNameStoredProcedure_TextChanged(object sender, EventArgs e)
        {
            btnFindAll.Enabled = !string.IsNullOrWhiteSpace(txtNameStoredProcedure.Text.Trim());
        }
    }
}
