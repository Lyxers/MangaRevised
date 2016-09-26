using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MangaDownloaderRevised.Classes
{
    static class UrlReplacer
    {
        static public string urlChapterMaker(string chapter, string url)
        {
            String regexchapter = "CHAPTER";
            Regex rgxchapter = new Regex(regexchapter);

            string urlwithchapter = rgxchapter.Replace(url, chapter.ToString());
            return urlwithchapter;
        }

        static public string whitespaceToUnderlineMaker(string name)
        {
            String regex = " ";
            Regex rgx = new Regex(regex);

            string underline = rgx.Replace(name, "_");
            return underline;
        }

        static public string dashToUnderlineMaker(string name)
        {
            String regexchap = "-chap";
            Regex rgxchap = new Regex(regexchap);

            string underline = rgxchap.Replace(name, "");
            String regex = "-";
            Regex rgx = new Regex(regex);

            underline = rgx.Replace(underline, "_");

            
            return underline;
        }

    }

}

