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

        public void calistirLigTahminWithArgs(string[] input)
        {
            string[] lig = new string[] { input[0] };
            string tip = input[1];

            helper.sendTelegramLigBasladi(input);

            if(tip == "EvSahibi")
            {
                EvSahibiTahminWekaServisNew evSahibiTahminWekaServisNew = new EvSahibiTahminWekaServisNew();
                evSahibiTahminWekaServisNew.calistirTahmin(lig);
            }

            if (tip == "Deplasman")
            {
                DeplasmanTahminWekaServisNew deplasmanTahminWekaServisNew = new DeplasmanTahminWekaServisNew();
                deplasmanTahminWekaServisNew.calistirTahmin(true, lig);
            }

            if (tip == "Ust")
            {
                AltUstTahminWekaServisNew altUstTahminWekaServisNew = new AltUstTahminWekaServisNew();
                altUstTahminWekaServisNew.calistirTahmin(true, lig);
            }

            helper.sendTelegramLigBitti(input);

            using (var ctx = new IDDAA_Entities())
            {
                var sonLig = ctx.LIG_ISLEME.ToList().OrderBy(c => c.SIRA).Last();
                if(sonLig.LIG == lig.First() && sonLig.TIP == tip)
                {
                    helper.sendTelegramLigBitti(new string[] { "Ligler" } );
                }
            }
        }

    }
}
