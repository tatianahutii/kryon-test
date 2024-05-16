using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace kryongraphologychallenge.Helpers
{
    public class HandwritingAnalyzer
    {
        private const int NUMBER_OF_RETRY = 10;
        private const int DELAY = 1000;


        public HandwritingAnalyzer()
        {
        }

        /// <summary>
        /// Gets the handwritten text from the specified image file by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file with handwritten text.</param>

        public static async Task<JToken> ReadHandwrittenTextAsync(string imageFilePath, HttpClient httpClient)
        {
            try
            {
                byte[] byteData = ImageUtil.GetImageAsByteArray(imageFilePath);
                var response = await GetOperationLocation(byteData, httpClient);

                if (!response.IsSuccessStatusCode)
                {
                    string errorString = await response.Content.ReadAsStringAsync();
                    JToken responseObj = JToken.Parse(errorString);

                    return responseObj;
                }

                var operationLocation = response.Headers
                    .GetValues("Operation-Location")
                    .FirstOrDefault();

                var result = await GetImageText(httpClient, operationLocation);

                if (result is null)
                {
                    Console.WriteLine("\nTimeout error.\n");
                }

                return result;

            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
                return null;
            }
        }

        private static async Task<HttpResponseMessage> GetOperationLocation(byte[] byteData, HttpClient _httpClient)
        {
            using var content = new ByteArrayContent(byteData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return await _httpClient.PostAsync(string.Empty, content);
        }

        private static async Task<JToken> GetImageText(HttpClient httpClient, string operationLocation)
        {
            for (int i = 0; i < NUMBER_OF_RETRY; i++)
            {
                await Task.Delay(DELAY);
                var response = await httpClient.GetAsync(operationLocation);
                var contentString = await response.Content.ReadAsStringAsync();

                if (contentString.Contains("\"status\":\"Succeeded\"", StringComparison.Ordinal))
                {
                    return JToken.Parse(contentString);
                }
            }

            return null;
        }
    }
}
