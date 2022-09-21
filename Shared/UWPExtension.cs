using Newtonsoft.Json;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class UWPExtension
    {
        public static async Task<ResponseBody> PostAsync<T>(this string uri, object objectToPost)
        {

            ResponseBody response = new ResponseBody();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Pass the handler to httpclient(from you are calling api)
            HttpClient client = new HttpClient(clientHandler);
            client.DefaultRequestHeaders.Add("auth", StaticContext.Auth);
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
            HttpResponseMessage request = client.PostAsync(uri,
            new StringContent(JsonConvert.SerializeObject(objectToPost), Encoding.UTF8, "application/json")).Result;
            return JsonConvert.DeserializeObject<ResponseBody>(await request.Content.ReadAsStringAsync());
            //string responseBody = await request.Content.ReadAsStringAsync();
        }

        public static async Task<ResponseBody> GetAsync(this string uri)
        {
            ResponseBody response = new ResponseBody();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)
            HttpClient client = new HttpClient(clientHandler);
            client.DefaultRequestHeaders.Add("auth", StaticContext.Auth);
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
            HttpResponseMessage request = await client.GetAsync(uri);
            string responseBody = await request.Content.ReadAsStringAsync();
            //return JsonConvert.DeserializeObject<ResponseBody>(responseBody);
            return response;
        }


        public static async Task<ResponseBody> GetAsyncOnLoad(this string uri)
        {
            ResponseBody response = new ResponseBody();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)
            HttpClient client = new HttpClient(clientHandler);
            client.DefaultRequestHeaders.Add("auth", StaticContext.Auth);
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
            HttpResponseMessage request = await client.GetAsync(uri);
            return JsonConvert.DeserializeObject<ResponseBody>(await request.Content.ReadAsStringAsync());
            //string responseBody = await request.Content.ReadAsStringAsync();
        }

        public static string RandomName()
        {
            int length = 10;
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < length)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }
            return Name;
        }
        public static string GeneratePassword(PasswordOptions opts = null)
        {
           
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 10,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "!@$?_-"                        // non-alphanumeric
        };

            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
        
    }

}