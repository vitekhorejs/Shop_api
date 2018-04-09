using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using System.Web.Script.Serialization;

namespace Shop_api
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public string Image_path { get; set; }
        private string _Image_path;
        public string Image_path
        {
            get
            {
                return Shared.Url+this._Image_path;
            }
            set
            {
                this._Image_path = value;
            }
        }
        [ScriptIgnore]
        public string Relative_Image_Path
        {
            get
            {
                return _Image_path;
            }
        }

    }
}