using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_api
{
    public class Order
    {
        public  int Id { get; set; }
        public  int User_id { get; set; }
        public  int Price { get; set; }
        public  int Status { get; set; }
        public override string ToString()
        {
            return "obejdavka cislo: " + Id/*+ " Status:" + Status*/;
        }
    }
}
