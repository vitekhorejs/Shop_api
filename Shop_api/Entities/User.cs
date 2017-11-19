using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Shop_api
{
    public class User :  INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Password { get; set; }

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
        
        public int Permission { get; set; }
        public DateTime Last_login { get; set; }
        public DateTime Registred { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
