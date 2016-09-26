using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using MangaDownloaderRevised.Classes;

namespace MangaDownloaderRevised.Classes
{
    public class News : System.Windows.Controls.ListBox
    {
        public void initNews(string configPath)
        {
            string[] newsItems = System.IO.File.ReadAllLines(configPath);
            string[] mangatownSite;
            string[] mangaiceSite;
            int chapterMangaice = 0;
            int chapterMangatown = 0;
            Regex matchmakerMangaice = new Regex("(?<=Chapter\\s).+");
            Regex matchmakerMangatown = new Regex("\\d+");
            foreach (string newsItemSpace in newsItems)
            {
                string newsItem = UrlReplacer.whitespaceToUnderlineMaker(newsItemSpace.ToLower());
                if (newsItem == null || newsItem == "") continue;
                //take from status of each site!

                mangaiceSite = newsMangaice(newsItem, matchmakerMangaice);
                if (mangaiceSite == null || !int.TryParse(mangaiceSite[3], out chapterMangaice))
                    chapterMangaice = 0;

                mangatownSite = newsMangatown(newsItem, matchmakerMangatown);
                if (mangatownSite == null || !int.TryParse(mangatownSite[3], out chapterMangatown))
                    chapterMangatown = 0;

                if (chapterMangatown > chapterMangaice)
                {
                    addToListBox(mangatownSite);
                }
                else if (mangaiceSite != null && mangaiceSite[1] != "" && mangaiceSite[1] != null)
                {
                    addToListBox(mangaiceSite);
                }

            }
        }

        public string[] newsMangaice(string newsItem, Regex matchmaker)
        {
            try
            {
                string url = "http://www.mangaice.com/manga-rss/" + newsItem + "/";
                //string searchRegex = "(?<=Recent chapters of.+?mangahttp://www.mangaice.com/.+?/).+?(?=\\s-\\sChapter)";
                //string searchRegex = "(?=\\S)(?<=<item>\\s*<title>\\s*).+?(?=s*</title>)";
                //string mangaResult = Fetcher.htmlFetch(url, searchRegex, 0);
                ////Console.WriteLine("mangaResult: " + mangaResult);

                ////mangaResult = Regex.Match(mangaResult, ".+?").Value;
                //searchRegex = "(?<=<pubDate>\\s*).+?UTC";
                //string lstUpdatedResult = Fetcher.htmlFetch(url, searchRegex, 0);
                ////Console.WriteLine("lstUpdatedResult: " + lstUpdatedResult);

                string mangaNameRegex = "(?=\\S)(?<=<item>(?>\\s*)<title>(?>\\s*)).+?(?=(?>\\s*)</title>)";
                string lastUpdateRegex = "(?=\\S)(?<=<pubDate>(?>\\s*)).+?UTC";
                string[] matches = Fetcher.htmlFetches(url, mangaNameRegex, lastUpdateRegex);
                string mangaResult = matches[0];
                string lstUpdatedResult = matches[1];

                string chapterResult = matchmaker.Match(mangaResult).Value;
                //string chapterResult = Regex.Match(mangaResult, "(?<=Chapter\\s).+").Value;
                //Console.WriteLine("chapterResult: " + chapterResult);

                string[] newsPackage = {"mangaice.com", mangaResult, lstUpdatedResult, chapterResult};
                return newsPackage;
            }
            catch
            {
                Console.WriteLine("Mangaice-Link do not exist or can't be found");
            }
            return null;
        }

        public static string[] newsMangatown(string newsItem, Regex matchmaker)
        {
            try
            {
                string url = "http://www.mangatown.com/manga/" + UrlReplacer.dashToUnderlineMaker(newsItem) + "/";
                //Console.WriteLine("THIS IS A FUCKING: " + url + "\n");
                //string searchRegex = "(?=\\S)(?<=<li>\r\n\\s*<a\\shref=\"" + url + ".*?>\r\n\\s*).+?(?=\\s*</a>)";
                //string mangaResult = MangaDownloaderRevised.Classes.Fetcher.htmlFetch(url, searchRegex, 0);
                ////Console.WriteLine("mangaResult: " + mangaResult);

                //searchRegex = "(?<=<span\\sclass=\"time\">).+?(?=</span>)";
                //string lstUpdatedResult = MangaDownloaderRevised.Classes.Fetcher.htmlFetch(url, searchRegex, 0);
                ////Console.WriteLine("lstUpdatedResult: " + lstUpdatedResult);

                string mangaNameRegex = "(?=\\S)(?<=<li>\r\n(?>\\ *)<a\\shref=\"" + url + "[^\"]*\"\\s?>\r\n(?>\\ *))[^<]+?(?=(?>\\s*)</a>)";
                string lastUpdateRegex = "(?<=<span\\sclass=\"time\">)[^<]+(?=</span>)";
                string[] matches = Fetcher.htmlFetches(url, mangaNameRegex, lastUpdateRegex);
                string mangaResult = matches[0];
                string lstUpdatedResult = matches[1];

                string chapterResult = matchmaker.Match(mangaResult).Value;
                Console.WriteLine("chapterResult: " + chapterResult);

                string[] newsPackage = { "mangatown.com", mangaResult, lstUpdatedResult, chapterResult };
                return newsPackage;
            }
            catch
            {
                Console.WriteLine("MangaTown-Link do not exist or can't be found");
            }
            return null;
        }

        public void addToListBox(string[] newsPackage)
        {
            if(true)//newsPackage != null)
            {
                this.Items.Add(newsPackage[1] + "\nby " + newsPackage[0] + "\nat " + newsPackage[2]);
            }
           
        }
    }
}
