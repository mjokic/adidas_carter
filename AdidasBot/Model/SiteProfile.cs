using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdidasBot.Model
{
    public class SiteProfile : INotifyPropertyChanged
    {

        //public SiteProfile(string name, string domain, string inUrlLong, string inUrlShort)
        //{
        //    this.name = name;
        //    this.domain = domain;
        //    this.inUrlLong = inUrlLong;
        //    this.inUrlShort = inUrlShort;
        //}


        public SiteProfile(string name, string domain, string inUrlLong, string inUrlShort, Dictionary<string, string> sizes)
        {
            this.name = name;
            this.domain = domain;
            this.inUrlLong = inUrlLong;
            this.inUrlShort = inUrlShort;
            this.sizes = sizes;
        }


        #region Properties
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        private string domain;

        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        private string inUrlLong;

        public string InUrlLong
        {
            get { return inUrlLong; }
            set { inUrlLong = value; }
        }

        private string inUrlShort;

        public string InUrlShort
        {
            get { return inUrlShort; }
            set { inUrlShort = value; }
        }


        private Dictionary<string, string> sizes;

        public Dictionary<string, string> Sizes
        {
            get { return sizes; }
            set { sizes = value; }
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
