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
            {1010, "Heslo musí obsahovat více než 5 znaků"},
            {1011, "Nejdříve se musíte přihlásit."},
            {1012, "Nelze získat položky košíku."},
            {1013, "Košík je prázdný."},
            {1014, "Při komuniakci s databází došlo k chybě."},
            {1015, "Maximální počet kusů je 250."},
            {1016, "Zatím nemáte žádné obědnávky."},
        };

    }
}
