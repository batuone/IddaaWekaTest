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


            args = new string[] { "AVU", "EvSahibi" };

            LigCalistirServis ligCalistirServis = new LigCalistirServis();
            //ligCalistirServis.calistirLigTahmin();
            ligCalistirServis.calistirLigTahminWithArgs(args);

        }
        
    }
}
