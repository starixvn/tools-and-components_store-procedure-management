// --------------------------------------------------------------------------
// <copyright file="DataGridViewRowComparer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.11.22">Created</change>
// </history>

namespace Fanex.ManagementSP.UI
{
    using System.Collections;
    using System.ComponentModel;
    using System.Windows.Forms;
    using Fanex.ManagementSP.UI.ColumnIndex;

    /// <summary>
    /// Custom object comparer implementation.
    /// </summary>
    public class DataGridViewRowComparer : IComparer
    {
        private int _propertyIndex;
        private ListSortDirection _direction;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridViewRowComparer"/> class.
        /// </summary>
        /// <param name="propertyIndex">Index of the property.</param>
        /// <param name="direction">The direction.</param>
        public DataGridViewRowComparer(int propertyIndex, ListSortDirection direction)
        {
            this._propertyIndex = propertyIndex;
            this._direction = direction;
        }

        #region IComparer Members

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of 
        /// <paramref name="x" /> and <paramref name="y" />
        /// , as shown in the following table.Value Meaning Less than zero <paramref name="x" /> 
        /// is less than <paramref name="y" />. 
        /// Zero <paramref name="x" /> equals <paramref name="y" />. 
        /// Greater than zero <paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public int Compare(object x, object y)
        {
            DataGridViewRow obj1 = (DataGridViewRow)x;
            DataGridViewRow obj2 = (DataGridViewRow)y;

            return CompareStrings(obj1.Cells[StoreColumnIndex.IssueName].Value.ToString(), obj2.Cells[StoreColumnIndex.IssueName].Value.ToString());
        }
        #endregion

        /// <summary>
        /// Compares the strings.
        /// </summary>
        /// <param name="val1">The value 1.</param>
        /// <param name="val2">The value 2.</param>
        /// <returns>Result of compare.</returns>
        private int CompareStrings(string val1, string val2)
        {
            return string.Compare(val1, val2) * (_direction == ListSortDirection.Ascending ? 1 : -1);
        }
    }
}
