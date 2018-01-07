using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_api
{
    public class Order
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public int Price { get; set; }
        public int Status { get; set; }
       
        private DateTime _Ordered;
        public string Ordered
        {
            get
            {
                return this._Ordered.ToLongDateString();
            }
            set
            {
                this._Ordered = DateTime.ParseExact(value, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }
}
