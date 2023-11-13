using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weka.classifiers.functions;
using weka.classifiers.meta;
using weka.classifiers.trees;
using weka.core;
using static IddaaWekaTest.OgrenmeClass;
using static IddaaWekaTest.SabitDeger;

namespace IddaaWekaTest
{
    class MacSonuWekaTestServisNew
    {
        SabitDegerler sabitDeger = new SabitDegerler();

        public KarTest calistirMacSonuOgrenmeTest(List<OGRENME> lstOgrenme, bool isYazdir, string ogrenmeTip, 
            string[] ligPair)
        {
            HelperServis helper = new HelperServis();
            List<Sonuc> lstProbs = new List<Sonuc>();
            KarSonuc karSonuc = new KarSonuc();
            decimal kar = 0;
            StringBuilder sb = new StringBuilder();
            MacSonuOgrenmeServisNew macSonuOgrenmeServis = new MacSonuOgrenmeServisNew();
            KarTest karTest = new KarTest();
            //ogrenme ve test kumelerini alir
            OgrenmeTestKume ogrenmeTestKume = getirOgrenmeVeTestKume(lstOgrenme);

            List<OGRENME> lstMergeOgrenme = new List<OGRENME>();
            List<OGRENME> linesOgrenmeClass = ogrenmeTestKume.lstOgrenmeKume;
            List<OGRENME> linesTestClass = ogrenmeTestKume.lstTestKume;
            lstMergeOgrenme.AddRange(linesOgrenmeClass);
            lstMergeOgrenme.AddRange(linesTestClass);

            lstMergeOgrenme = macSonuOgrenmeServis.normalizeOgrenmeKume(lstMergeOgrenme);

            List<String> linesOgrenme = lstMergeOgrenme.Where(c => linesOgrenmeClass.Any(d => d.IDDAA_ID == c.IDDAA_ID)).
                    Select(c => c.SONUC).ToList();

            List<String> linesTest = lstMergeOgrenme.Where(c => linesTestClass.Any(d => d.IDDAA_ID == c.IDDAA_ID)).
                    Select(c => c.SONUC).ToList();

            if (linesOgrenme.Count() == 0 || linesTest.Count() == 0)
            {
                karTest.kar = 0;
                karTest.testYuzdeOran = ogrenmeTestKume.testYuzdeOran;
                return karTest;
            }

            Instances trainData = helper.convertFromListStringToIntances(linesOgrenme);
            Instances testData = helper.convertFromListStringToIntances(linesTest);

            Bagging bagging = new Bagging();
            bagging.setClassifier(new DecisionStump());
            LogitBoost ogrenmeLogistic = new LogitBoost();
            ogrenmeLogistic.setNumIterations(10);
            ogrenmeLogistic.setClassifier(bagging);
            ogrenmeLogistic.buildClassifier(trainData);

            double evSahibiOran; double deplasmanOran; double beraberOran; string evSahibi; string deplasman; string lig; DateTime tarih;
            string[] lastElementCount = linesOgrenme.ElementAt(0).Split(',');

            double evProbability = helper.macProbability(sabitDeger.evSahibiSonuc, ligPair);
            double depProbability = helper.macProbability(sabitDeger.deplasmanSonuc, ligPair);

            for (int i = 0; i < ogrenmeTestKume.lstTestKume.Count(); i++)
            {
                OGRENME mac = ogrenmeTestKume.lstTestKume.ElementAt(i);
                double evSahibiProb = 0; double deplasmanProb = 0;

                double[] probabilities1 = ogrenmeLogistic.distributionForInstance(testData.instance(i));
                evSahibiProb = probabilities1[0];
                deplasmanProb = probabilities1[1];

                if (evSahibiProb == 0.5 && deplasmanProb == 0.5)
                {
                    continue;
                }
                               
                double macSonucDouble = testData.instance(i).value(lastElementCount.Count() - 1);

                string answer="";
                string macSonuc = macSonucDouble == 0 ? "E" : "D";

                if (ogrenmeTip == sabitDeger.evSahibiSonuc)
                {
                    answer = evSahibiProb > evProbability ? "E" : "D";
                    if (answer != "E")
                    {
                        continue;
                    }
                }
                else if(ogrenmeTip == sabitDeger.deplasmanSonuc)
                {
                    answer = deplasmanProb > depProbability ? "D" : "E";
                    if (answer != "D")
                    {
                        continue;
                    }
                }

                using (var ctx = new IDDAA_Entities())
                {
                    evSahibiOran = Convert.ToDouble(ctx.ARSIV.First(c => c.IDDAA_ID == mac.IDDAA_ID).MS_1);
                    deplasmanOran = Convert.ToDouble(ctx.ARSIV.First(c => c.IDDAA_ID == mac.IDDAA_ID).MS_2);
                    beraberOran = Convert.ToDouble(ctx.ARSIV.First(c => c.IDDAA_ID == mac.IDDAA_ID).MS_X);
                    evSahibi = ctx.ARSIV.First(c => c.IDDAA_ID == mac.IDDAA_ID).EV_SAHIBI + "-" + evSahibiOran;
                    deplasman = ctx.ARSIV.First(c => c.IDDAA_ID == mac.IDDAA_ID).DEPLASMAN + "-" + deplasmanOran;
                    lig = ctx.ARSIV.First(c => c.IDDAA_ID == mac.IDDAA_ID).LIG;
                    tarih = ctx.ARSIV.First(c => c.IDDAA_ID == mac.IDDAA_ID).TARIH;

                    if (ctx.ARSIV.First(c => c.IDDAA_ID == mac.IDDAA_ID).MBS > sabitDeger.macSonuOynanacakMbs)
                    {
                        continue;
                    }
                    if (ogrenmeTip == sabitDeger.evSahibiSonuc && evSahibiProb < 0.50)
                    {
                        continue;
                    }
                    if (ogrenmeTip == sabitDeger.evSahibiSonuc && evSahibiOran < sabitDeger.macSonuMinBahisIddaaOran)
                    {
                        continue;
                    }
                    if (ogrenmeTip == sabitDeger.deplasmanSonuc && deplasmanOran < sabitDeger.macSonuMinBahisIddaaOran)
                    {
                        continue;
                    }
                }

                Sonuc orans = new Sonuc();
                orans.SistemOran = Convert.ToDouble(answer == "E" ? evSahibiProb : deplasmanProb);
                orans.EvSahibi = evSahibi;
                orans.Deplasman = deplasman;
                orans.Tahmin = answer;
                orans.Tarih = tarih;
                orans.isBasari = answer == macSonuc ? true : false;
                orans.IddaaOran = Convert.ToDecimal(answer == "E" ? evSahibiOran : deplasmanOran);
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
                karTest.testYuzdeOran = ogrenmeTestKume.testYuzdeOran;
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

            if(isYazdir)
            {
                helper.yazSonucToFile(sb.ToString());
            }

            karTest.kar = kar;
            karTest.testYuzdeOran = ogrenmeTestKume.testYuzdeOran;

            return karTest;
        }

        private OgrenmeTestKume getirOgrenmeVeTestKume(List<OGRENME> lstOgrenme)
        {
            OgrenmeTestKume ogrenmeTestKume = new OgrenmeTestKume();
            using (var ctx = new IDDAA_Entities())
            {
                var ogrenme = lstOgrenme.OrderByDescending(c => c.TARIH).ThenByDescending(c => c.IDDAA_ID).ToList();
                int ogrenmeCount = ogrenme.Count();
                int oran = 10;
                if (ogrenmeCount < 800)
                {
                    oran = sabitDeger.macSonu800TestOran;
                }
                else if(ogrenmeCount < 1000)
                {
                    oran = sabitDeger.macSonu1000TestOran;
                }
                else if (ogrenmeCount < 1200)
                {
                    oran = sabitDeger.macSonu1200TestOran;
                }
                else
                {
                    oran = sabitDeger.macSonuFullTestOran;
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
