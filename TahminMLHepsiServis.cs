using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IddaaWekaTest.OgrenmeClass;
using static IddaaWekaTest.SabitDeger;

namespace IddaaWekaTest
{
    class TahminMLHepsiServis
    {
        SabitDegerler sabitDeger = new SabitDegerler();

        public void ekleTahminMLList(List<Sonuc> lstSonucGenel)
        {
            if (lstSonucGenel.Count() == 0)
            {
                return;
            }
            using (var context = new IDDAA_Entities())
            {
                List<TAHMIN_ML_HEPSI> lstTahmin = new List<TAHMIN_ML_HEPSI>();
                DateTime tarih = DateTime.Now;
                foreach (var sonuc in lstSonucGenel)
                {
                    TAHMIN_ML_HEPSI tahmin = new TAHMIN_ML_HEPSI();
                    tahmin.TARIH = tarih;
                    tahmin.EV_SAHIBI = sonuc.EvSahibi;
                    tahmin.DEPLASMAN = sonuc.Deplasman;
                    tahmin.IDDAA_ID = sonuc.IddaaId;
                    tahmin.LIG = sonuc.lig;
                    tahmin.TIP = sonuc.tip;
                    tahmin.DEGER = sonuc.deger;
                    tahmin.IDDAA_ORAN = sonuc.IddaaOran;
                    tahmin.ML_TAHMIN_YUZDE = Convert.ToDecimal(sonuc.SistemOran);

                    lstTahmin.Add(tahmin);
                }

                context.TAHMIN_ML_HEPSI.AddRange(lstTahmin);
                context.SaveChanges();
            }
        }

    }
}
