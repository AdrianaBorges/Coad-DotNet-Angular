using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace COAD.EXTENSION.Model
{
    public class DataGridSelectKeyRow : INotifyPropertyChanged
    {
        public DataGridSelectKeyRow()
        {
            Keys = new List<string>();
        }

        private int _index;
        private IList<string> _keys;
        private string _key;
        
        public int Index {
            get
            {
                return _index;
            }
            set {
                if(_index != value)
                {
                    _index = value;
                    OnPropertyChanged("Index");
                }
            }
        }

        public string Key
        {
            get { return _key; }
            set
            {
                if (_key != value)
                {
                    _key = value;
                    OnPropertyChanged("Key");
                }
            }
        }

        public IList<string> Keys
        {
            get
            {
                return _keys;
            }
            set
            {
                if (_keys != value)
                {
                    _keys = value;
                    OnPropertyChanged("Keys");
                }
            }
        }    

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
