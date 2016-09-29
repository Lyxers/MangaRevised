using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace MangaDownloaderRevised.Classes
{
    static class Fetcher
    {
        public static string imgFetch(string urlAddress, string title, int site)
        {
            if(title != null && title != "")
            {
                string jpgregex = "(?<=src=\")http.+?(?=\".+?alt=\"" + title.Replace(" ", "\\s") + ")";
                //Console.WriteLine("jpgregex is " + jpgregex);
                return standardFetch(urlAddress, jpgregex, site);
            }

            Console.WriteLine("Url-Link was invalid for imgFetch.");
            return null;
        }
        public static string titleFetch(string urlAddress)
        {
            string titleregex = "(?<=title>).+?(?=\\s-)";
            return standardFetch(urlAddress, titleregex, 1);
        }

        public static string titleWithoutChapterFetch(string urlAddress)
        {
            string titlenochapterregex = "(?<=title>).+?(?=\\s\\d)";
            return standardFetch(urlAddress, titlenochapterregex, 1);
        }

        public static string standardFetch(string urlAddress, string searchregex, int site)
        {
            //HTTP Request
            Regex matchmaker = new Regex(searchregex);
            if (site != 0)
            {
                string regexsite = "SITE";
                Regex rgxsite = new Regex(regexsite);
                urlAddress = rgxsite.Replace(urlAddress, site.ToString());
            }
            
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                Console.WriteLine("Request started for '" + urlAddress + "'.");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Console.WriteLine("Got Response.");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    //Console.WriteLine("Stream received.");

                    StreamReader readStream = null;

                    if (response.CharacterSet == null || response.CharacterSet == "")
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    string data = readStream.ReadToEnd();

                    //Regular Expression to kill off all HTML crap
                    //string regex = "(<([^>]+)>)";

                    //Replace most parts of the html-code
                    //Regex.Replace(data, regex, String.Empty);
                    //System.IO.File.WriteAllText(@"C:\Users\Martin\Downloads\MangaTest\Test.txt", data);
                    //Search jpg-files
                    
                    string matchValue = matchmaker.Match(data).Value;

                    //Match match = Regex.Match(data, searchregex);
                    //string matchValue = match.ToString();

                    //System.IO.File.WriteAllText(@"C:\Users\Martin\Downloads\MangaTest\TestAfterMatch.txt", matchValue);
                    //Console.WriteLine("matchValue was: " + matchValue);
                    response.Close();
                    readStream.Close();
                    return matchValue;
                }
               
            }
            catch
            {
                
            }
            Console.WriteLine("No Link found.\n");
            return null; 
        }

        public static string htmlFetch(string urlAddress, string searchregex, int site)
        {
            //HTTP Request
            Regex matchmaker = new Regex(searchregex);
            Regex htmldeleter = new Regex("(<([^>]+)>)");
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                Console.WriteLine("Request started for '" + urlAddress + "'.");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Console.WriteLine("Got Response.");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    //Console.WriteLine("Stream received.");

                    StreamReader readStream = null;

                    if (response.CharacterSet == null || response.CharacterSet == "")
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    string data = readStream.ReadToEnd();

                    //Regular Expression to kill off all HTML crap
                    //string regex = "(<([^>]+)>)";

                    //Replace most parts of the html-code
                    //data = htmldeleter.Replace(data, String.Empty);
                    //System.IO.File.WriteAllText(@"C:\Users\Martin\Downloads\MangaTest\Test.txt", data);
                    //Search jpg-files
                    
                    string matchValue = matchmaker.Match(data).Value;

                    //Match match = Regex.Match(data, searchregex);
                    //string matchValue = match.ToString();
                    //System.IO.File.WriteAllText(@"C:\Users\Martin\Downloads\MangaTest\TestAfterMatch.txt", matchValue);
                    //Console.WriteLine("matchValue was: " + matchValue);
                    response.Close();
                    readStream.Close();
                    
                    return matchValue;
                }

            }
            catch
            {

            }
            Console.WriteLine("No Link found.\n");
            return null;
        }
        //old outdated use htmlFetches
        public static string[] htmlFetches(string urlAddress, params string[] searchregex)
        {
            //HTTP Request
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                Console.WriteLine("Request started for '" + urlAddress + "'.");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Console.WriteLine("Got Response.");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    //Console.WriteLine("Stream received.");

                    StreamReader readStream = null;

                    if (response.CharacterSet == null || response.CharacterSet == "")
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    string data = readStream.ReadToEnd();
                    if(data != null)
                    {
                        //Regular Expression to kill off all HTML crap
                        //string regex = "(<([^>]+)>)";

                        //Replace most parts of the html-code
                        //data = htmldeleter.Replace(data, String.Empty);
                        //System.IO.File.WriteAllText(@"C:\Users\Martin\Downloads\MangaTest\Test.txt", data);
                        //Search jpg-files

                        string[] result = new string[searchregex.Length];
                        for (int i = 0; i < result.Length; i++)
                            result[i] = Regex.Match(data, searchregex[i]).Value;

                        //Match match = Regex.Match(data, searchregex);
                        //string matchValue = match.ToString();
                        //System.IO.File.WriteAllText(@"C:\Users\Martin\Downloads\MangaTest\TestAfterMatch.txt", matchValue);
                        //Console.WriteLine("matchValue was: " + matchValue);
                        response.Close();
                        readStream.Close();

                        return result;
                    }
                    
                  
                }

            }
            catch
            {

            }
            Console.WriteLine("No Link found.\n");
            return null;
        }
    }
}
