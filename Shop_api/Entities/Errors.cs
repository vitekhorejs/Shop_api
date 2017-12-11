using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_api
{
    public static class Errors
    {
        //public static Array errors;
        public static Dictionary<int, string> error = new Dictionary<int, string>()
        {
            {1000, "Na server byla odeslána neúplná nebo nesprávna data."},
            {1001, "Nebyl vybrán typ operace."},
            {1002, "Data nebyla poslána v json formátu."},
            {1003, "V databázi je již totožný záznam."},
            {1004, "Registrace proběhla úspěšně. Nyní se mužete přihlásit."},
            {1005, "Při ukládání do databáze došlo k chybě."},
            {1006, "Uživatel s tímto emailem je již zaregistrovaný."},
            {1007, "Na server nebyla poslána žádná data."},
            {1008, "Emailová adresa není platná."},
            {1009, "Špatný email nebo heslo."},

        };

    }
}
