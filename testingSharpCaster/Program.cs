using Newtonsoft.Json;
using Sharpcaster;
using Sharpcaster.Interfaces;
using Sharpcaster.Models.Media;
using System;
using System.Net;
using System.Text.Json.Nodes;

namespace testingSharpCaster
{
    public class Program
    {
        private static void Main(string[] args)
        {
            IChromecastLocator locator = new MdnsChromecastLocator();
            var chromecasts = locator.FindReceiversAsync().GetAwaiter().GetResult().ToList();
            var client = new ChromecastClient();
            if(chromecasts.Count <= 0)
            {
                throw new Exception("No chromecasts where found");
            }
            Console.WriteLine($"{chromecasts.Count} chromecasts was found");
            foreach ( var chromecast in chromecasts ) {
                Console.WriteLine(string.Format("{0}:{1} ", chromecast.Name, !string.IsNullOrEmpty(chromecast.Status) ? chromecast.Status:" Not active"));
            }

            Console.WriteLine("testing connectivity");
            //Console.ReadKey();
            var cookies = new Dictionary<string, string>();
                cookies.Add("cookies:","test");
            var media = new Media {
            ContentUrl = "URL",
            CustomData = cookies
            };
            

            
           client.ConnectChromecast(chromecasts.FirstOrDefault()).GetAwaiter().GetResult();
            var application = client.LaunchApplicationAsync("B3419EF5").GetAwaiter().GetResult();
            var loadedstatus = client.GetChannel<IMediaChannel>().LoadAsync(media).GetAwaiter().GetResult();
            Console.ReadKey();

        }
    }
}