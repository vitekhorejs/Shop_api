using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_api
{
    public class User2 : INotifyPropertyChanged
    {
        private string _Name { get; set; }
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _Surname { get; set; }
        public string Surname
        {
            get { return _Surname; }
            set
            {
                _Surname = value;
                OnPropertyChanged("Surname");
            }
        }

        private string _Mail;
        public string Mail
        {
            get { return _Mail; }
            set
            {
                _Mail = value;
                OnPropertyChanged("Mail");
            }
        }
        private DateTime _Last_login;
        public string Last_login {
            get
            {
                return this._Last_login.ToLongDateString()+" "+this._Last_login.ToLongTimeString();
            }
            set
            {
                this._Last_login = DateTime.ParseExact(value, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        private DateTime _Registred;
        public string Registred {
            get
            {
                return this._Registred.ToLongDateString() + " " + this._Registred.ToShortTimeString();
            }
            set
            {
                this._Registred = DateTime.ParseExact(value, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
