// --------------------------------------------------------------------------
// <copyright file="DataGridViewComboBoxCellExtended.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.11.22">Created</change>
// </history>

namespace Fanex.ManagementSP.UI.Extend
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// The data grid view combo box cell extend class.
    /// </summary>
    public class DataGridViewComboBoxCellExtended : DataGridViewComboBoxCell
    {
        /// <summary>
        /// Gets the type of the cell's hosted editing control.
        /// </summary>
        /// <returns>The <see cref="T:System.Type" /> of the underlying editing control. This property always returns 
        /// <see cref="T:System.Windows.Forms.DataGridViewComboBoxEditingControl" />.
        /// </returns>
        /// <value>The type of control.</value>
        public override Type EditType
        {
            get
            {
                return typeof(DataGridViewComboBoxEditingControl);
            }
        }

        /// <summary>
        /// Attaches and initializes the hosted editing control.
        /// </summary>
        /// <param name="rowIndex">The index of the cell's parent row.</param>
        /// <param name="initialFormattedValue">The initial value to be displayed in the control.</param>
        /// <param name="dataGridViewCellStyle">A 
        /// <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that determines the appearance of the hosted control.
        /// </param>        
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            ((DataGridViewComboBoxEditingControl)DataGridView.EditingControl).DropDownStyle = ComboBoxStyle.DropDown;
        }
    }
}
