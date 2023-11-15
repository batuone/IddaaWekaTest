using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IddaaWekaTest.OgrenmeClass;
using static IddaaWekaTest.SabitDeger;

namespace IddaaWekaTest
{
    class MainServis
    {
        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            List<SINIFLANDIRMA_TEST> testLigler = new List<SINIFLANDIRMA_TEST>();
            SabitDegerler sabitDeger = new SabitDegerler();

            using (var ctx = new IDDAA_Entities())               
            {
                testLigler = ctx.SINIFLANDIRMA_TEST.Where(c => c.ISLENDI == 0).ToList();
            }

            foreach (var item in testLigler)
            {
                LigCalistirServis ligCalistirServis = new LigCalistirServis();
                ligCalistirServis.calistirLigTahminWithArgs(item);
            }
        }
        
    }
}
