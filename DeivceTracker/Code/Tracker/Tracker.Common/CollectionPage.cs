using System;
using System.Collections.Generic;

namespace Tracker.Common
{
    /// <summary>
    /// Holds relevant information related to a page of a collection of information.
    /// </summary>
    public class CollectionPage<T>
    {
        /// <summary>
        ///     A page of items.
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        ///     Total number of items, regardless of page.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        ///     The number of items that should be shown per page.
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        ///     The page that is being accessed.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        ///     The total number of pages.
        /// </summary>
        public int TotalPages 
        { 
            get
            {
                return (int)(Math.Ceiling(TotalItems / (decimal)ItemsPerPage));
            }
        }
    }
}