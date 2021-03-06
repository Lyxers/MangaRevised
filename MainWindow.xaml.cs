﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MangaDownloaderRevised.Classes;
using System.IO;
using System.Windows.Threading;
using System.Media;
using System.ComponentModel;

namespace MangaDownloaderRevised
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string path = @"C:\Users\Martin\Downloads\MangaTest\";
        const string configPath = @"config.txt";
       

        public MainWindow()
        {            
            InitializeComponent();
            progressBar.Content = "";
           
            
            news.initNews(configPath);
        }

        private void AddManga_Click(object sender, RoutedEventArgs e)
        {

            int i = 0, j = 0;
            string urladdress; //urladdress without sitenumber
            string title = "";
            string titlewithchapter = "";
            DownloadManga.IsEnabled = true;

            try
            {
                Int32.TryParse(chapterBoxA.Text, out i);
                Int32.TryParse(chapterBoxB.Text, out j);
            }
            catch (FormatException)
            {
                Console.WriteLine("{0} or {1}: Bad Format", i, j);
            }
            
            if(chapterBoxA.Text == null && chapterBoxB != null)
            {
                i = j;
            }
            
            do
            {
                urladdress = UrlReplacer.urlChapterMaker(i.ToString(), urlBox.Text);
                if (title == "")
                {
                    titlewithchapter = Fetcher.titleFetch(urladdress);
                    title = UrlReplacer.deleteUpToNumber(titlewithchapter);
                    Console.WriteLine("Title name is: '" + title + "'.\n");
                }
                
                setProgressBar("Adding mangalinks ... ");
                if(Fetcher.imgFetch(urladdress, title, '1') == "" || Fetcher.imgFetch(urladdress, title, '1') == null)
                {
                    urladdress = UrlReplacer.urlChapterMaker(i.ToString().PadLeft(3, '0') , urlBox.Text);
                }
                dataGrid.Items.Add(new EventPropertyChanged() {LinkName = urladdress, ChapterName = i, MangaName = Fetcher.titleFetch(urladdress) });
                i++;
            } while (i <= j);
            setProgressBar("Adding manga chapters finished.");
        }

        private void DownloadManga_Click(object sender, RoutedEventArgs e)
        {
            string urladdress; //address without site number
            string titlewithchapter; //to name images
            string folderPath; //For Directory and save issues
            string fetchurl; //URL with chapter and site number
            string title; //to search right image links
            while(0 < dataGrid.Items.Count)
            {
                urladdress = (dataGrid.Items[0] as EventPropertyChanged).LinkName;
                titlewithchapter = (dataGrid.Items[0] as EventPropertyChanged).MangaName;
                title = UrlReplacer.deleteUpToNumber(titlewithchapter);
                Console.WriteLine("Title name is: '" + title + "'.\n");
                if (checkExtraFolder.IsChecked ?? true)
                {
                    folderPath = path + titlewithchapter;
                } 
                else
                {
                    folderPath = path + title;
                }            
                setProgressBar("Begin Download '" + titlewithchapter + "' ... ");
                Directory.CreateDirectory(folderPath);
                string domtree = Fetcher.imgFetch(urladdress, title, 1);
                for (int site = 1; domtree != null && domtree != ""; domtree = Fetcher.imgFetch(urladdress, title, site++) )
                {
                    fetchurl = Fetcher.imgFetch(urladdress, title, site);
                    Console.WriteLine("Download Link is: '" + fetchurl + "'.\n");
                    //Console.WriteLine(fetchurl);
                    setProgressBar("Downloading: '" + fetchurl +"' ... "); 
                    Downloader.JpgDownloader(folderPath, fetchurl, titlewithchapter + " " + site);
                }
                dataGrid.Items.Remove(dataGrid.Items[0]);
            }
            
            setProgressBar("Downloads finished.");
            MessageBox.Show("Downloads finished.");
            DownloadManga.IsEnabled = false;
        }

        private void setProgressBar(string prowess)
        {
            progressBar.Content = prowess;
            DoEvents();
        }

        public void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(ExitFrames), frame);
            Dispatcher.PushFrame(frame);
        }

        public object ExitFrames(object f)
        {
            ((DispatcherFrame)f).Continue = false;

            return null;
        }

        private void clearDataGrid_Click(object sender, RoutedEventArgs e)
        {
            if(dataGrid.SelectedItem != null)
            {
                object[] dataListItems = new object[dataGrid.Items.Count];
                dataGrid.SelectedItems.CopyTo(dataListItems, 0);
                foreach(object selectedItem in dataListItems)
                {
                    dataGrid.Items.Remove(selectedItem);
                }
                

                if (dataGrid.Items.Count == 0)
                {
                    DownloadManga.IsEnabled = false;
                }
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to delete all list items?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    dataGrid.Items.Clear();
                    DownloadManga.IsEnabled = false;
                }
                else
                {
                    // user clicked no
                }
               
               
            }

            

           
        }

        
    }
}
