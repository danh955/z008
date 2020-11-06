// <copyright file="ReviewResult.cs" company="MyProject">
// Copyright (c) MyProject. All rights reserved.
// </copyright>

namespace Scraper
{
    using System;
    using System.Collections.Generic;
    using Scraper.Model;

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
        /// Gets process start time.
        /// </summary>
        public DateTimeOffset StartTime { get; internal set; }

        /// <summary>
        /// Gets process end time.
        /// </summary>
        public DateTimeOffset EndTime { get; internal set; }

        /// <summary>
        /// Gets list of bicycle reviews.
        /// </summary>
        public IEnumerable<Bicycle> Bicycles { get; internal set; }

        /// <summary>
        /// Set all fields to its default values.
        /// </summary>
        internal void Clear()
        {
            this.UrlCount = default;
            this.StartTime = default;
            this.EndTime = default;
        }
    }
}