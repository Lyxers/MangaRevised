using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MangaDownloaderRevised.Classes
{
    public class EventPropertyChanged :INotifyPropertyChanged
    {


        private string _LinkName;
        public string LinkName
        {
            get
            {
                return _LinkName;
            }
            set
            {
                if (_LinkName != value)
                {
                    _LinkName = value;
                    RaisePropertyChanged("LinkName");
                }
            }
        }
        private int _ChapterName;
        public int ChapterName
        {
            get
            {
                return _ChapterName;
            }
            set
            {
                if (_ChapterName != value)
                {
                    _ChapterName = value;
                    RaisePropertyChanged("ChapterName");
                }
            }
        }

        private string _MangaName;
        public string MangaName
        {
            get
            {
                return _MangaName;
            }
            set
            {
                if (_MangaName != value)
                {
                    _MangaName = value;
                    RaisePropertyChanged("SiteName");
                }
            }
        }

        private string _MangaNewsElement;
        public string MangaNewsElement
        {
            get
            {
                return _MangaNewsElement;
            }
            set
            {
                if (_MangaNewsElement != value)
                {
                    _MangaNewsElement = value;
                    RaisePropertyChanged("MangaNewsElement");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }


    }
}
