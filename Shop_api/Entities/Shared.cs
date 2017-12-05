using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RestSharp;
using System.Net;

namespace Shop_api
{
    public static class Shared
    {
        public static void ShowInfo(string response)
        {
            //MessageBox.Show(response.ToString(), "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            //Input responze = new Input();
            Input responze = SimpleJson.DeserializeObject<Input>(response);
            //$vars = get_class_vars(get_class($responze));
            //string text = responze.GetType().GetProperty("error", null).GetValue(responze, null).ToString();
            //MessageBox.Show(text, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            //Input responze = new Input();
            //responze = response.Data;
            //MessageBox.Show(responze.ToString(), "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            //hor.vit@seznam.cz
            if (responze.Type == "error")
            {
                int my_error;   //jstli je cislo zobrazi prislusnou hlasku, pokud neni zobrazy text vraceny severem
                if (Int32.TryParse(responze.Data, out my_error) == false)
                {
                    MessageBox.Show(responze.Data, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (Errors.error.ContainsKey(my_error))
                    {
                        MessageBox.Show(Errors.error[my_error], "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Neznámá chyba: " + responze.Data, "Info", MessageBoxButton.OK, MessageBoxImage.Information); // může znamenat že server vrátil číslo ltere nesouvisy se znamou chybou
                    }

                }
            }
            
        }
        public static CookieContainer cookiecon;
        public static bool logged;
        /*public static string GetUrl()
        {
           return  "https://student.sps-prosek.cz/~horejvi14/shop/";
        }*/
        public static string Url = "https://student.sps-prosek.cz/~horejvi14/shop/";
    }
}
