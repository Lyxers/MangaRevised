using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace MangaDownloaderRevised.Classes
{
    static class Downloader
    {
        static public void JpgDownloader(string path, string link, string sitename)
        {
            try
            {
                using (var client = new WebClient())
                {
                    //Get card name through NameFetcher(). Card name is the title of html page!
                    
                    client.DownloadFile(link, (path + @"/" + sitename + ".jpg"));
                }
            }
            catch (WebException e)
            {
                Console.WriteLine("Error message: " + e);
            }
        }
    }
}
