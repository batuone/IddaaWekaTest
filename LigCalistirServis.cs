using IddaaWekaV0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IddaaWekaTest.OgrenmeClass;

namespace IddaaWekaTest
{
    class LigCalistirServis
    {
        HelperServis helper = new HelperServis();

        public void calistirLigTahminWithArgs(SINIFLANDIRMA_TEST item)
        {
            string[] ligler = new string[] { item.LIG };
            guncelleSiniflandirmaTestBaslangicTarih(item.LIG, item.TIP);

            if (item.TIP == "EvSahibi")
            {
                EvSahibiTahminWekaServisNew evSahibiTahminWekaServisNew = new EvSahibiTahminWekaServisNew();
                evSahibiTahminWekaServisNew.calistirTahmin(ligler);
            }

            if (item.TIP == "Deplasman")
            {
                DeplasmanTahminWekaServisNew deplasmanTahminWekaServisNew = new DeplasmanTahminWekaServisNew();
                deplasmanTahminWekaServisNew.calistirTahmin(ligler);
            }
            
            if (item.TIP == "Ust")
            {
                AltUstTahminWekaServisNew altUstTahminWekaServisNew = new AltUstTahminWekaServisNew();
                altUstTahminWekaServisNew.calistirTahmin(ligler);
            }

            if (item.TIP == "Yari2")
            {
                Yari2TahminWekaServisNew yari2TahminWekaServisNew = new Yari2TahminWekaServisNew();
                yari2TahminWekaServisNew.calistirTahmin(ligler);
            }

            guncelleSiniflandirmaTestIslendi(item.LIG, item.TIP);
            sendTelegramMesaj();
        }

        private void guncelleSiniflandirmaTestBaslangicTarih(string lig, string tip)
        {
            using (var ctx = new IDDAA_Entities())
            {
                var satir = ctx.SINIFLANDIRMA_TEST.First(c => c.LIG == lig && c.TIP == tip);
                satir.BASLAMA_TARIH = DateTime.Now;
                ctx.SaveChanges();
            }
        }

        private void guncelleSiniflandirmaTestIslendi(string lig, string tip)
        {
            using (var ctx = new IDDAA_Entities())
            {
                var satir = ctx.SINIFLANDIRMA_TEST.First(c => c.LIG == lig && c.TIP == tip);
                satir.BITIS_TARIH = DateTime.Now;
                satir.ISLENDI = 1;
                ctx.SaveChanges();
            }
        }

        public void ekleSiniflandirma(List<CalistirTestSonuc> calistirTestSonucMax)
        {
            List<SINIFLANDIRMA_SONUC> lstSiniflandirma = new List<SINIFLANDIRMA_SONUC>();
            foreach (var item in calistirTestSonucMax)
            {
                SINIFLANDIRMA_SONUC sonuc = new SINIFLANDIRMA_SONUC();
                sonuc.LIG = item.lig;
                sonuc.TIP = item.macTip;
                sonuc.WEKA_TIP = item.wekaTip;
                sonuc.KAR = item.Kar;
                sonuc.TARIH = DateTime.Today;

                lstSiniflandirma.Add(sonuc);
            }
            
            using (var context = new IDDAA_Entities())
            {
                context.SINIFLANDIRMA_SONUC.AddRange(lstSiniflandirma);
                context.SaveChanges();
            }
        }

        private void sendTelegramMesaj()
        {
            if (DateTime.Now.Hour % 6 == 0)
            {
                using (var ctx = new IDDAA_Entities())
                {
                    var islenenKumeSayi = ctx.SINIFLANDIRMA_TEST.Count(c=> c.ISLENDI == 1);

                    helper.sendTelegramMesaj("Sınıflandırma islenen sayı: " + islenenKumeSayi);
                }
            }
        }

    }
}
