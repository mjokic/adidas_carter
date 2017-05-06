using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdidasBot.Model
{
    public class Dummy: INotifyPropertyChanged
    {



        #region Properties
        private string siteKey;

        public string SiteKey
        {
            get { return siteKey; }
            set { siteKey = value;
                OnPropertyChanged(SiteKey);
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
