using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.EXTENSION.Model
{
    public class DefinicaoColunasDTO : INotifyPropertyChanged
    {
        public DefinicaoColunasDTO()
        {
            this.Campos = new List<string>();
        }

        public string _label;
        public string _campo;
        public bool _ordenavel;
        public IList<string> _campos;

        public string Label {

            get {
                return _label;
            }
            set {
                if(_label != value)
                {
                    _label = value;
                    OnPropertyChanged("Label");
                }
            }
        }

        public string Campo
        {
            get
            {
                return _campo;
            }
            set
            {
                if (_campo != value)
                {
                    _campo = value;
                    OnPropertyChanged("Campo");
                }
            }
        }

        public bool Ordenavel
        {
            get
            {
                return _ordenavel;
            }
            set
            {
                if (_ordenavel != value)
                {
                    _ordenavel = value;
                    OnPropertyChanged("Ordenavel");
                }
            }
        }

        public IList<string> Campos
        {
            get
            {
                return _campos;
            }
            set
            {
                if (_campos != value)
                {
                    _campos = value;
                    OnPropertyChanged("campos");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
