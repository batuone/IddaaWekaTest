using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IddaaWekaTest
{
    class TahminTestServis
    {

        public void ekleTahminTestList(string[] ligler, string tip, string deger, decimal kar, int karKey, int testOranYuzde)
        {
            using (var context = new IDDAA_Entities())
            {
                List<TAHMIN_TEST> lstTahminTest = new List<TAHMIN_TEST>();
                foreach (var lig in ligler)
                {
                    TAHMIN_TEST tahmin = new TAHMIN_TEST();
                    tahmin.TARIH = DateTime.Now;
                    tahmin.LIG = lig;
                    tahmin.TIP = tip;
                    tahmin.DEGER = deger;
                    tahmin.ML_TEST_YUZDE = testOranYuzde;
                    tahmin.KAR = kar;
                    tahmin.KAR_KEY = karKey;

                    lstTahminTest.Add(tahmin);
                }

                context.TAHMIN_TEST.AddRange(lstTahminTest);
                context.SaveChanges();
            }
        }


    }
}
