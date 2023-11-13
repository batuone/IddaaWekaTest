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

        public void calistirLigTahmin()
        {
            using (var ctx = new IDDAA_Entities())
            {
                int ligSayisi = ctx.LIG_ISLEME.Where(c => c.TARIH == DateTime.Today).Count();
                for (int i = 0; i < ligSayisi; i++)
                {
                    string lig = getirSiradakiLig();
                    if(lig == "")
                    {
                        continue;
                    }
                    baslatLig(lig);
                    calistirLig(lig);
                    bitirLig(lig);

                    kontrolLiglerBitti();
                }
            }

        }

        private void calistirLig(string lig)
        {            
            string[] ligler = new string[] { lig };

            helper.sendTelegramLigBasladi(ligler);

            EvSahibiTahminWekaServisNew evSahibiTahminWekaServisNew = new EvSahibiTahminWekaServisNew();
            evSahibiTahminWekaServisNew.calistirTahmin(true, ligler);

            DeplasmanTahminWekaServisNew deplasmanTahminWekaServisNew = new DeplasmanTahminWekaServisNew();
            deplasmanTahminWekaServisNew.calistirTahmin(true, ligler);

            AltUstTahminWekaServisNew altUstTahminWekaServisNew = new AltUstTahminWekaServisNew();
            altUstTahminWekaServisNew.calistirTahmin(true, ligler);

            helper.sendTelegramLigBitti(ligler);
        }

        private string getirSiradakiLig()
        {
            using (var ctx = new IDDAA_Entities())
            {
                var ligler = ctx.LIG_ISLEME.Where(c => c.TARIH == DateTime.Today && c.BASLADI == "0");
                if(ligler.Count() == 0)
                {
                    return "";
                }

                return ligler.OrderBy(c => c.SIRA).First().LIG;
            }
        }

        private void baslatLig(string lig)
        {
            using (var ctx = new IDDAA_Entities())
            {
                var ligIsleme = ctx.LIG_ISLEME.First(c => c.TARIH == DateTime.Today && c.LIG == lig);
                ligIsleme.BASLADI = "1";
                ctx.SaveChanges();
            }
        }

        private void bitirLig(string lig)
        {
            using (var ctx = new IDDAA_Entities())
            {
                var ligIsleme = ctx.LIG_ISLEME.First(c => c.TARIH == DateTime.Today && c.LIG == lig);
                ligIsleme.BITTI = "1";
                ctx.SaveChanges();
            }
        }

        private void kontrolLiglerBitti()
        {
            using (var ctx = new IDDAA_Entities())
            {
                int kalanLigSayisi = ctx.LIG_ISLEME.Where(c => c.TARIH == DateTime.Today && c.BITTI == "0").Count();
                if (kalanLigSayisi == 0)
                {
                    helper.sendTelegramLigBitti(new string[] { "Ligler" } );
                }
            }
        }

        public void calistirLigTahminWithArgs(string[] input)
        {
            string[] lig = new string[] { input[0] };
            string tip = input[1];

            helper.sendTelegramLigBasladi(input);

            if(tip == "EvSahibi")
            {
                EvSahibiTahminWekaServisNew evSahibiTahminWekaServisNew = new EvSahibiTahminWekaServisNew();
                evSahibiTahminWekaServisNew.calistirTahmin(true, lig);
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
