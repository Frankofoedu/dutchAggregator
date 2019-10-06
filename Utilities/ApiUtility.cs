using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
   public static class ApiUtility
    {
        public static async Task<string> GetAsync(HttpClient client, string uri)
        {
            var response = await client.GetAsync(uri);


            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadAsStringAsync();
        }
        public static async Task<string> PostAsync(HttpClient client, string uri, HttpContent content)
        {
            var response = await client.PostAsync(uri, content);


            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadAsStringAsync();
        }
    }
}
