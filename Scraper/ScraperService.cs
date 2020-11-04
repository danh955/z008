// <copyright file="ScraperService.cs" company="MyProject">
// Copyright (c) MyProject. All rights reserved.
// </copyright>

#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task

namespace Scraper
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HtmlAgilityPack;

    /// <summary>
    /// Scraper service class.
    /// This is a single threaded class.
    /// </summary>
    public class ScraperService
    {
        private const string DefaultUrl = @"https://electricbikereview.com/category/bikes";

        private static int count;

        private readonly Uri firstUri;
        private readonly HashSet<Uri> knownPages = new HashSet<Uri>();
        private readonly Queue<Uri> pagesToProcess = new Queue<Uri>();
        private readonly ReviewResult result = new ReviewResult();

        /// <summary>
        /// Initializes a new instance of the <see cref="ScraperService"/> class.
        /// </summary>
        /// <param name="firstUrl">The first URL of website to parse.  If null, use default website URL.</param>
        public ScraperService(string firstUrl = null)
        {
            this.firstUri = new Uri(firstUrl ?? DefaultUrl);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScraperService"/> class.
        /// </summary>
        /// <param name="firstUrl">The first URL of website to parse.  If null, use default website URL.</param>
        public ScraperService(Uri firstUrl)
        {
            this.firstUri = firstUrl ?? new Uri(DefaultUrl);
        }

        /// <summary>
        /// Get a list of.
        /// </summary>
        /// <returns>The ready to go folio result.</returns>
        public async Task<ReviewResult> GetFromWebAsync()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = null;
            int cycle = 0;

            this.result.Clear();
            this.result.StartTime = DateTimeOffset.Now;
            try
            {
                this.pagesToProcess.Clear();
                this.knownPages.Clear();
                this.AddPageToProcess(this.firstUri.ToString());

                while (this.pagesToProcess.Count > 0 || htmlDoc != null)
                {
                    cycle++;
                    Console.WriteLine();
                    Console.WriteLine($"Cycle: {cycle}  pagesToProcess: {this.pagesToProcess.Count}   Is htmlDoc null: {htmlDoc == null}");

                    Task<HtmlDocument> htmlDocTask = null;
                    if (this.pagesToProcess.Count > 0)
                    {
                        Uri url = this.pagesToProcess.Dequeue();
                        Console.WriteLine();
                        Console.WriteLine($"Dequeue: {url}");
                        htmlDocTask = web.LoadFromWebAsync(url.ToString());
                    }

                    // Process HTML document.
                    if (htmlDoc != null)
                    {
                        this.FindAllAnchors(htmlDoc, "//a[contains(@href,'/category/bikes/page/')]");
                        this.FindAllAnchors(htmlDoc, "//div[contains(@class,'review-item-wrapper')]//div[contains(@class,'review-read-more-wrapper')]/a");
                        htmlDoc = null;
                    }

                    // Save retrieve HTML document.
                    if (htmlDocTask != null)
                    {
                        htmlDoc = await htmlDocTask;
                    }
                }

                return this.result;
            }
            finally
            {
                this.result.EndTime = DateTimeOffset.Now;
            }
        }

        /// <summary>
        /// Add page to process.
        /// </summary>
        /// <param name="url">Add URL to list of pages to process.  Ignore if already have it.</param>
        private void AddPageToProcess(string url)
        {
            if (!string.IsNullOrWhiteSpace(url)
                && !url.StartsWith("#", StringComparison.OrdinalIgnoreCase))
            {
                Uri uri = new Uri(this.firstUri, url.TrimEnd('/'));

                if (uri.HostNameType == UriHostNameType.Dns && uri.Host == this.firstUri.Host)
                {
                    if (!this.knownPages.Contains(uri))
                    {
                        count++;
                        this.pagesToProcess.Enqueue(uri);
                        this.knownPages.Add(uri);
                        this.result.UrlCount++;
                        Console.WriteLine($"{count} Enqueue: {uri}");
                    }
                }
            }
        }

        /// <summary>
        /// Find all HTML anchors.
        /// </summary>
        /// <param name="htmlDoc">HTML document to parse.</param>
        /// <param name="xpath">The XPath expression to find the anchors.</param>
        private void FindAllAnchors(HtmlDocument htmlDoc, string xpath)
        {
            HtmlNodeCollection anchors = htmlDoc.DocumentNode.SelectNodes(xpath);
            foreach (var anchorNode in anchors)
            {
                HtmlAttribute attribute = anchorNode.Attributes["href"];
                if (attribute != null)
                {
                    this.AddPageToProcess(attribute.Value);
                }
            }
        }

        //// Example.
        //// HtmlNodeCollection anchors = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class,'review-item-wrapper')]//div[contains(@class,'review-read-more-wrapper')]/a");
    }
}