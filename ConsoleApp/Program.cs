namespace ConsoleApp
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
            var service = new ScraperService();
            var result = await service.GetFromWebAsync();

            TimeSpan runTime = result.EndTime - result.StartTime;
            Console.WriteLine($"Run time of {runTime}");
        }
    }
}