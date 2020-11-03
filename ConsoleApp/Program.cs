﻿namespace ConsoleApp
{
    using System;
    using System.Threading.Tasks;
    using Scraper;

    public class Program
    {
        /// <summary>
        /// Start of program.
        /// </summary>
        /// <returns></returns>
        private static async Task Main()
        {
            Console.WriteLine("Hello World!");

            var service = new ScraperService();
            var result = await service.GetFromWebAsync();
        }
    }
}