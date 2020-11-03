// <copyright file="ReviewResult.cs" company="MyProject">
// Copyright (c) MyProject. All rights reserved.
// </copyright>

namespace Scraper
{
    /// <summary>
    /// Review result class.
    /// </summary>
    public class ReviewResult
    {
        /// <summary>
        /// Gets number of URLs processed.
        /// </summary>
        public int UrlCount { get; internal set; }

        /// <summary>
        /// Set all fields to its default values.
        /// </summary>
        internal void Clear()
        {
            this.UrlCount = 0;
        }
    }
}