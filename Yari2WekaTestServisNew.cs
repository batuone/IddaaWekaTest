using IddaaWekaTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weka.classifiers;
using weka.core;
using static IddaaWekaTest.OgrenmeClass;
using static IddaaWekaTest.SabitDeger;

namespace IddaaWekaV0
{
    class Yari2WekaTestServisNew
    {
        SabitDegerler sabitDeger = new SabitDegerler();

        public KarTest calistirMacSonuOgrenmeTest(List<OGRENME> lstOgrenme, bool isYazdir, int yariTestOran, 
            string[] ligPair, Classifier ogrenmeLogistic)
        {
            HelperServis helper = new HelperServis();
            List<Sonuc> lstProbs = new List<Sonuc>();
            KarSonuc karSonuc = new KarSonuc();
            decimal kar = 0;
            StringBuilder sb = new StringBuilder();
            MacSonuOgrenmeServisNew macSonuOgrenmeServis = new MacSonuOgrenmeServisNew();
            KarTest karTest = new KarTest();
            //ogrenme ve test kumelerini alir
            OgrenmeTestKume ogrenmeTestKume = getirOgrenmeVeTestKume(lstOgrenme, yariTestOran);

            List<OGRENME> lstMergeOgrenme = new List<OGRENME>();
            List<OGRENME> linesOgrenmeClass = ogrenmeTestKume.lstOgrenmeKume;
            List<OGRENME> linesTestClass = ogrenmeTestKume.lstTestKume;
            lstMergeOgrenme.AddRange(linesOgrenmeClass);
            lstMergeOgrenme.AddRange(linesTestClass);

            lstMergeOgrenme = macSonuOgrenmeServis.normalizeOgrenmeKumeYari(lstMergeOgrenme);

            List<String> linesOgrenme = lstMergeOgrenme.Where(c => linesOgrenmeClass.Any(d => d.IDDAA_ID == c.IDDAA_ID)).
                    Select(c => c.SONUC).ToList();

            List<String> linesTest = lstMergeOgrenme.Where(c => linesTestClass.Any(d => d.IDDAA_ID == c.IDDAA_ID)).
                    Select(c => c.SONUC).ToList();

            if (linesOgrenme.Count() == 0 || linesTest.Count() == 0)
            {
                karTest.kar = 0;
                return karTest;
            }

            Instances trainData = helper.convertFromListStringToIntancesYari(linesOgrenme);
            Instances testData = helper.convertFromListStringToIntancesYari(linesTest);

            ogrenmeLogistic.buildClassifier(trainData);


            double yari1Oran; double yari2Oran; string evSahibi; string deplasman; string lig; DateTime tarih;
            string[] lastElementCount = linesOgrenme.ElementAt(0).Split(',');

            double yari2Probability = helper.macProbability(sabitDeger.yari2, ligPair);
            for (int i = 0; i < ogrenmeTestKume.lstTestKume.Count(); i++)
            {
                OGRENME mac = ogrenmeTestKume.lstTestKume.ElementAt(i);

                double[] probabilities1 = ogrenmeLogistic.distributionForInstance(testData.instance(i));
                double yari1Prob = probabilities1[0];
                double yari2Prob = probabilities1[1];

                double answerDouble = ogrenmeLogistic.classifyInstance(testData.instance(i));
                double macSonucDouble = testData.instance(i).value(lastElementCount.Count() - 1);

                string answer = "";
                string macSonuc = macSonucDouble == 0 ? "Yari1" : "Yari2";

                if (yari1Prob > yari2Prob)
                {
                    answer = yari1Prob > yari2Probability ? "Yari1" : "Yari2";
                    if (answer != "Yari1")
                    {
                        continue;
                    }
                }
                else
                {
                    answer = yari2Prob > yari2Probability ? "Yari2" : "Yari1";
                    if (answer != "Yari2")
                    {
                        continue;
                    }
                }
                //sadece ust icin calissin
                if (answer != "Yari2")
                {
                    continue;
                }

                using (var ctx = new IDDAA_Entities())
                {
                    evSahibi = ctx.ARSIV.First(c => c.IDDAA_ID == mac.IDDAA_ID).EV_SAHIBI;
                    deplasman = ctx.ARSIV.First(c => c.IDDAA_ID == mac.IDDAA_ID).DEPLASMAN;
                    lig = ctx.ARSIV.First(c => c.IDDAA_ID == mac.IDDAA_ID).LIG;
                    tarih = ctx.ARSIV.First(c => c.IDDAA_ID == mac.IDDAA_ID).TARIH;

                    if (ctx.ARSIV.First(c => c.IDDAA_ID == mac.IDDAA_ID).MBS > sabitDeger.macSonuOynanacakMbs)
                    {
                        continue;
                    }
                }

                Sonuc orans = new Sonuc();
                orans.SistemOran = Convert.ToDouble(answer == "Yari1" ? yari1Prob : yari2Prob);
                orans.EvSahibi = evSahibi;
                orans.Deplasman = deplasman;
                orans.Tahmin = answer;
                orans.Tarih = tarih;
                orans.isBasari = answer == macSonuc ? true : false;
                orans.IddaaOran = Convert.ToDecimal(answer == "Yari1" ? 1.80 : 1.80);
                orans.lig = lig;

                lstProbs.Add(orans);
            }

            int countTrue = 0; int countFalse = 0;
            foreach (var item in lstProbs.OrderByDescending(c => c.SistemOran))
            {
                if (item.isBasari)
                {
                    countTrue++;
                    kar += (item.IddaaOran - 1);
                }
                else
                {
                    countFalse++;
                    kar += -1;
                }
            }
            if (countTrue + countFalse == 0)
            {
                karSonuc.Sonuc = Convert.ToString(-1);
                karTest.kar = 0;
                return karTest;
            }
            decimal sonuc = Convert.ToDecimal(countTrue) / Convert.ToDecimal(countTrue + countFalse);
            string snc = "%" + Math.Round(sonuc, 2) * 100 + " - " + countTrue + "/" + countFalse;
            karSonuc.Sonuc = snc.ToString();
            karSonuc.Kar = kar;

            sb.Append(karSonuc.Sonuc);
            sb.Append(System.Environment.NewLine);
            sb.Append("KAR: ");
            sb.Append(kar * 100);

            if (isYazdir)
            {
                helper.yazSonucToFile(sb.ToString());
            }

            karTest.kar = kar;
            karTest.testYuzdeOran = ogrenmeTestKume.testYuzdeOran;

            return karTest;
        }

        private OgrenmeTestKume getirOgrenmeVeTestKume(List<OGRENME> lstOgrenme, int oran)
        {
            OgrenmeTestKume ogrenmeTestKume = new OgrenmeTestKume();
            using (var ctx = new IDDAA_Entities())
            {
                var ogrenme = lstOgrenme.OrderByDescending(c => c.TARIH).ThenByDescending(c => c.IDDAA_ID).ToList();
                int ogrenmeCount = ogrenme.Count();

                if (ogrenmeCount < 800)
                {
                    oran = sabitDeger.yari800TestOran;
                }
                else if (ogrenmeCount < 1000)
                {
                    oran = sabitDeger.yari1000TestOran;
                }
                else if (ogrenmeCount < 1200)
                {
                    oran = sabitDeger.yari1200TestOran;
                }
                else
                {
                    oran = sabitDeger.yariFullTestOran;
                }

                int ogrenmeOranCount = ogrenmeCount * oran / 100;
                int ogrenmeKalanCount = ogrenmeCount - ogrenmeOranCount;
                ogrenmeTestKume.lstTestKume = ogrenme.Take(ogrenmeOranCount).ToList();
                ogrenmeTestKume.lstOgrenmeKume = ogrenme.Skip(ogrenmeOranCount).Take(ogrenmeKalanCount).ToList();
                ogrenmeTestKume.testYuzdeOran = oran;
            }
            return ogrenmeTestKume;
        }
    }
}
