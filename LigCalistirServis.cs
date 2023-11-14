using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IddaaWekaTest
{
    class LigCalistirServis
    {
        HelperServis helper = new HelperServis();

        public void calistirLigTahminWithArgs(string[] ligler)
        {
            EvSahibiTahminWekaServisNew evSahibiTahminWekaServisNew = new EvSahibiTahminWekaServisNew();
            evSahibiTahminWekaServisNew.calistirTahmin(ligler);

            DeplasmanTahminWekaServisNew deplasmanTahminWekaServisNew = new DeplasmanTahminWekaServisNew();
            deplasmanTahminWekaServisNew.calistirTahmin(ligler);

            AltUstTahminWekaServisNew altUstTahminWekaServisNew = new AltUstTahminWekaServisNew();
            altUstTahminWekaServisNew.calistirTahmin(ligler);
        }

    }
}
