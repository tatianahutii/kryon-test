using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using kryongraphologychallenge.Helpers;
using Newtonsoft.Json.Linq;

namespace kryon_graphology_challenge
{
    public class Program
    {
        const string subscriptionKey = "d8c4e756cb844352a0ca8c58ff96b3ec";
        const string uriBase = "https://cai-ntx-test-kyron-wu2.cognitiveservices.azure.com/vision/v2.0/read/core/asyncBatchAnalyze";

        public static async Task Main(string[] args)
        {
            //   /$$   /$$                                        
            //  | $$  /$$/                                        
            //  | $$ /$$/   /$$$$$$  /$$   /$$  /$$$$$$  /$$$$$$$ 
            //  | $$$$$/   /$$__  $$| $$  | $$ /$$__  $$| $$__  $$
            //  | $$  $$  | $$  \__/| $$  | $$| $$  \ $$| $$  \ $$
            //  | $$\  $$ | $$      | $$  | $$| $$  | $$| $$  | $$
            //  | $$ \  $$| $$      |  $$$$$$$|  $$$$$$/| $$  | $$
            //  |__/  \__/|__/       \____  $$ \______/ |__/  |__/
            //                       /$$  | $$                    
            //                      |  $$$$$$/                    
            //                       \______/                     

            // TODO: STAGE 1 - read and understand what this code is suppose to do //
            string[] paths = { "demo-image-1.jpeg" };

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(uriBase);
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            await Task.WhenAll(paths.Select(path => ProcessFileAsync(path, httpClient)));

            // TODO: STAGE 3 - find the connections between the outputs above //
            Console.WriteLine("\nCan you find the connection between the outputs above?");

            // TODO: STAGE 4 - submit your answers, repo and CV, and join us for a cup of coffee //
            Console.WriteLine("\nSend us your solution with the github repo and your CV to petro.dubyk@nintex.com and wait for our call!\n");
            Console.ReadLine();
        }

        private static async Task ProcessFileAsync(string path, HttpClient httpClient)
        {
            Console.WriteLine($"\nReading challenge file {path}...\n");
            var result = await HandwritingAnalyzer.ReadHandwrittenTextAsync($"./Image-files/{path}", httpClient);
            Console.WriteLine(result.ToString());
        }
    }
}

