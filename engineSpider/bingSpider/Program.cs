using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Google.API.Search;

namespace engineSpider
{
    static class Program
    {
        // Replace this value with your account key.
        private const string AccountKey = "PMS8RMeKgdcwV2HcvPrJpQfJNvD59bg9N0/Zk+B1koQ";

        static void Main(string[] args)
        {
            try
            {
                string arg = "";
                foreach (string n in args)
                {
                    arg = arg + n + " ";
                }
                SearchBing(arg);
                SearchGoogle(arg);
                //Console.ReadKey();
            }
            catch (Exception ex)
            {
                string innerMessage = 
                        (ex.InnerException != null) ? 
                        ex.InnerException.Message : String.Empty;
                Console.WriteLine("{0}\n{1}", ex.Message, innerMessage);
            }
        }

        static void SearchGoogle(string query)
        {
            // Search 32 results of keyword : "Google APIs for .NET"
            GwebSearchClient client = new GwebSearchClient("830090663874.apps.googleusercontent.com");
            IList<IWebResult> results = client.Search(query, 100);
            foreach (IWebResult result in results)
            {
                //Console.WriteLine("[{0}] {1} => {2}", result.Title, result.Content, result.Url);
                Console.WriteLine("[+]" + result.Url);
            } 
        }

        static void SearchBing(string query)
        {
            //Console.WriteLine(query);
            // Create a Bing container.
            string rootUrl = "https://api.datamarket.azure.com/Bing/Search";
            var bingContainer = new engineSpider.BingSearchContainer(new Uri(rootUrl));

            // The market to use.
            string market = "zh-CN";

            // Configure bingContainer to use your credentials.
                    bingContainer.Credentials = new NetworkCredential(AccountKey, AccountKey);

            // Build the query, limiting to 10 results.
            var webQuery = 
                        bingContainer.Web(query, null, null, market, null, null, null, null);
                    webQuery = webQuery.AddQueryOption("$top", 50);

            // Run the query and display the results.
            var webResults = webQuery.Execute();

            foreach (var result in webResults)
            {
                //Console.WriteLine("{0}\n\t{1}", result.Title, result.Url);
                Console.WriteLine("[+]"+result.Url);
            }
        }
    } 
}
