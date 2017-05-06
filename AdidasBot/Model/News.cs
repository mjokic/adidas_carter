using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdidasBot.Model
{
    public class News : INotifyPropertyChanged
    {

        public News(string title, string date, string newsText)
        {
            this.title = title;
            this.date = date;
            this.newsText = newsText;
        }


        #region Properties
        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }


        private string date;

        public string Date
        {
            get { return date; }
            set { date = value;
                OnPropertyChanged("Date");
            }
        }


        private string newsText;

        public string NewsText
        {
            get { return newsText; }
            set { newsText = value;
                OnPropertyChanged("NewsText");
            }
        }

        #endregion


        #region Data binding stuff
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            PropertyChangedEventHandler h = PropertyChanged;

            if (h != null)
            {
                h(this, new PropertyChangedEventArgs(propName));
            }

        }
        #endregion

    }
}
