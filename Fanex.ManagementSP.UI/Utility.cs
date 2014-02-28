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
    using OutlookStyleControls;
    using Fanex.ManagementSP.Entities;
    using System.Linq;
    /// <summary>
    /// The utility class.
    /// </summary>
    public static class Utility
    {
        #region Others

        public static string CurrentUsername
        {
            get
            {
                return Environment.UserName.ToLower();
            }
        }

        public static int GetUserId(IEnumerable<User> users)
        {
            if (!ReferenceEquals(users, null) && users.Any())
            {
                var user = users.Where(x => string.Equals(x.Username, CurrentUsername, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                if (!ReferenceEquals(user, null))
                {
                    return user.Id;
                }
            }

            return 0;
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

        #region Extension

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
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
