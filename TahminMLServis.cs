using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IddaaWekaTest.OgrenmeClass;
using static IddaaWekaTest.SabitDeger;

namespace IddaaWekaTest
{
    class TahminMLServis
    {
        SabitDegerler sabitDeger = new SabitDegerler();

        public void ekleTahminMLList(List<Sonuc> lstSonucGenel, string[] ligler, string tip, string deger)
        {
            silBulten(ligler, tip, deger);
            if (lstSonucGenel.Count() == 0)
            {
                return;
            }
            using (var context = new IDDAA_Entities())
            {
                List<TAHMIN_ML> lstTahminMl = new List<TAHMIN_ML>();
                foreach (var sonuc in lstSonucGenel)
                {
                    TAHMIN_ML tahmin = new TAHMIN_ML();
                    tahmin.TARIH = sonuc.Tarih;
                    tahmin.EV_SAHIBI = sonuc.EvSahibi;
                    tahmin.DEPLASMAN = sonuc.Deplasman;
                    tahmin.IDDAA_ID = sonuc.IddaaId;
                    tahmin.LIG = sonuc.lig;
                    tahmin.TIP = sonuc.tip;
                    tahmin.DEGER = sonuc.deger;
                    tahmin.IDDAA_ORAN = sonuc.IddaaOran;
                    tahmin.ML_TAHMIN_YUZDE = Convert.ToDecimal(sonuc.SistemOran);
                    tahmin.ML_TEST_YUZDE = sonuc.testOran;
                    tahmin.KAR = sonuc.kar;

                    lstTahminMl.Add(tahmin);
                }

                context.TAHMIN_ML.AddRange(lstTahminMl);
                context.SaveChanges();
            }
        }

        public void silBulten(string[] ligler, string tip, string deger)
        {
            using (var context = new IDDAA_Entities())
            {
                var lstTahminMl = context.TAHMIN_ML.Where(c=> ligler.Contains(c.LIG) && c.TIP == tip && c.DEGER == deger);
                context.TAHMIN_ML.RemoveRange(lstTahminMl);

                context.SaveChanges();
            }
        }

        public void ekleTahminMLListForAltUst(List<Sonuc> lstSonucGenel, string[] ligler, string tip)
        {
            silAltUstTahmin(ligler, tip);
            if (lstSonucGenel.Count() == 0)
            {
                return;
            }
            using (var context = new IDDAA_Entities())
            {
                List<TAHMIN_ML> lstTahminMl = new List<TAHMIN_ML>();
                foreach (var sonuc in lstSonucGenel)
                {
                    TAHMIN_ML tahmin = new TAHMIN_ML();
                    tahmin.TARIH = sonuc.Tarih;
                    tahmin.EV_SAHIBI = sonuc.EvSahibi;
                    tahmin.DEPLASMAN = sonuc.Deplasman;
                    tahmin.IDDAA_ID = sonuc.IddaaId;
                    tahmin.LIG = sonuc.lig;
                    tahmin.TIP = sonuc.tip;
                    tahmin.DEGER = sonuc.deger;
                    tahmin.IDDAA_ORAN = sonuc.IddaaOran;
                    tahmin.ML_TAHMIN_YUZDE = Convert.ToDecimal(sonuc.SistemOran);
                    tahmin.ML_TEST_YUZDE = sonuc.testOran;

                    lstTahminMl.Add(tahmin);
                }

                context.TAHMIN_ML.AddRange(lstTahminMl);
                context.SaveChanges();
            }
        }

        public void silAltUstTahmin(string[] ligler, string tip)
        {
            using (var context = new IDDAA_Entities())
            {
                var lstTahminMl = context.TAHMIN_ML.Where(c => ligler.Contains(c.LIG) && c.TIP == tip);
                context.TAHMIN_ML.RemoveRange(lstTahminMl);

                context.SaveChanges();
            }
        }
    }
}
