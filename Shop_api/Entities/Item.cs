using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_api
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        
        public string Description { get; set; }
        public int Category_id { get; set; }
        private string _Image_path;
        public string Image_path
        {
            get
            {
                return Shared.Url + this._Image_path;
            }
            set
            {
                this._Image_path = value;
            }
        }
        public override string ToString()
        {
            return Name + " Cena: " + Price;
        }
    }
}
