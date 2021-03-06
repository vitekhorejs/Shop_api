﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_api
{
    public class ItemLocal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private int _Price;
        public int Price
        {
            get
            {
                if (Quantity != 0)
                {
                    return _Price * Quantity;
                }
                else
                {
                    return _Price;
                }
            }
            set
            {
                this._Price = value;
            }
        }

        public string Description { get; set; }
        public int Category_id { get; set; }
        public string Image_path { get; set; }
        public int Quantity { get; set; }
    }
}
