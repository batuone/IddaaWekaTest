using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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
    class DeplasmanTahminWekaServisNew
    {
        SabitDegerler sabitDeger = new SabitDegerler();

        public String calistirTahmin(bool tableInsert, string[] ligler)
        {
            DahilOgrenmeAttribute dahilOgrenme = new DahilOgrenmeAttribute();
            MacSonuOgrenmeServisNew macSonuOgrenmeServisNew = new MacSonuOgrenmeServisNew();
            TahminMLServis tahminMLServis = new TahminMLServis();
            StringBuilder sb = new StringBuilder();
            List<Sonuc> lstSonucGenel = new List<Sonuc>();
            List<OGRENME> lstOgrenme = new List<OGRENME>();
            List<OgrenmeClass> lstOgrenmeButunAttributelar = new List<OgrenmeClass>();
            HelperServis helper = new HelperServis();
            List<String> islemYapilanLigler = new List<string>();
            CalistirTestSonuc calistirTestSonuc = new CalistirTestSonuc();
            Dictionary<string[], string[]> ligPairAttributeMap = new Dictionary<string[], string[]>();

            using (var ctx = new IDDAA_Entities())
            {

                List<BULTEN> lstBulten = macSonuOgrenmeServisNew.getirBultenData(ligler);

                var gununLigleri = lstBulten.Where(c => ligler.Contains(c.LIG) && c.TARIH > DateTime.Now)
                    .GroupBy(c => c.LIG).Select(c => c.Key).ToArray();

                foreach (var lig in gununLigleri)
                {
                    if (islemYapilanLigler.Any(c => c == lig))
                    {
                        continue;
                    }

                    Dictionary<int, string[]> atrributeCountMap = new Dictionary<int, string[]>();

                    //string[] ligPair = getPairLig(lig);
                    string[] ligPair = ligler;

                    islemYapilanLigler.AddRange(ligPair);

                    ////full ogrenme
                    //lstOgrenme = macSonuOgrenmeServisNew.ekleMacSonuOgrenmeFull(ligPair, dahilOgrenme.macSonuDahilAttribute,
                    //    sabitDeger.deplasmanSonuc);
                    lstOgrenmeButunAttributelar = macSonuOgrenmeServisNew.ekleMacSonuOgrenmeButunAttributeler
                        (ligPair, dahilOgrenme.macSonuDahilAttribute, sabitDeger.deplasmanSonuc);
                    lstOgrenme = macSonuOgrenmeServisNew.convertOgrenmeClassToOgrenmeContext(lstOgrenmeButunAttributelar, dahilOgrenme.macSonuDahilAttribute, sabitDeger.deplasmanSonuc);


                    //StringBuilder ss = new StringBuilder();
                    //foreach (var item in lstOgrenme)
                    //{
                    //    ss.Append(item.SONUC);
                    //    ss.Append(System.Environment.NewLine);
                    //}
                    //helper.yazSonucToFile(ss.ToString());


                    //attribute belirle
                    atrributeCountMap = opsiyonelAttributeKumeleri(atrributeCountMap, lstOgrenme);

                    ////test calistir
                    calistirTestSonuc = calisTestParalel(atrributeCountMap, lstOgrenmeButunAttributelar, ligler);
                    if (calistirTestSonuc == null || calistirTestSonuc.kullanilacakAttribute == null)
                    {
                        continue;
                    }

                    // tahmin baslat
                    List<Sonuc> lstSonuc = calistirTahminSonuc(calistirTestSonuc.kullanilacakAttribute, ligPair, lstOgrenmeButunAttributelar);
                    lstSonuc.ForEach(c => { c.testOran = calistirTestSonuc.testYuzdeOran; c.kar = calistirTestSonuc.Kar; });
                    lstSonucGenel.AddRange(lstSonuc);
                }

                // ekle tahmin
                if (tableInsert)
                {
                    tahminMLServis.ekleTahminMLList(lstSonucGenel, ligler, sabitDeger.macSonuSonuc, "2");
                    helper.sendTelegramSecilenMac(lstSonucGenel);
                }

                sb = yazSonuc(sb, lstSonucGenel);
                helper.yazSonucToFile(sb.ToString());
                return sb.ToString();
            }
        }

        private List<Sonuc> calistirTahminSonuc(string[] kullanilacakAttribute, string[] ligPair, 
            List<OgrenmeClass> lstOgrenmeButunAttributelar)
        {
            List<Sonuc> lstSonuc = new List<Sonuc>();
            List<Sonuc> lstSonucHepsi = new List<Sonuc>();
            List<OGRENME> lstOgrenme = new List<OGRENME>();
            List<OgrenmeClass> lstOgrenmeClass = new List<OgrenmeClass>();
            MacSonuOgrenmeServisNew macSonuOgrenmeServisNew = new MacSonuOgrenmeServisNew();
            StringBuilder sb = new StringBuilder();
            HelperServis helper = new HelperServis();
            TahminMLHepsiServis tahminMLHepsiServis = new TahminMLHepsiServis();

            //lstOgrenme = macSonuOgrenmeServisNew.ekleMacSonuOgrenmeFull(ligPair, kullanilacakAttribute, sabitDeger.deplasmanSonuc);

            lstOgrenme = macSonuOgrenmeServisNew.convertOgrenmeClassToOgrenmeContext(lstOgrenmeButunAttributelar, kullanilacakAttribute, sabitDeger.deplasmanSonuc);


            //ogrenme kumesini alir
            List<OGRENME> linesOgrenme = lstOgrenme;
            //test kumesini alir
            List<OGRENME> linesTest = macSonuOgrenmeServisNew.setMacSonuTahmin(kullanilacakAttribute, ligPair, sabitDeger.macSonuSonuc);

            List<OGRENME> lstMergeOgrenme = new List<OGRENME>();
            lstMergeOgrenme.AddRange(linesTest);
            lstMergeOgrenme.AddRange(linesOgrenme);

            lstMergeOgrenme = macSonuOgrenmeServisNew.normalizeOgrenmeKume(lstMergeOgrenme);

            List<String> linesOgrenmeSonuc = lstMergeOgrenme.Where(c => linesOgrenme.Any(d => d.IDDAA_ID == c.IDDAA_ID)).
                Select(c => c.SONUC).ToList();

            List<OGRENME> linesTestOgrenme = lstMergeOgrenme.Where(c => linesTest.Any(d => d.IDDAA_ID == c.IDDAA_ID)).ToList();
            List<String> linesTestSonuc = lstMergeOgrenme.Where(c => linesTest.Any(d => d.IDDAA_ID == c.IDDAA_ID)).
                Select(c => c.SONUC).ToList();

            if (linesTestSonuc.Count() == 0)
            {
                return new List<Sonuc>();
            }

            Instances trainData = helper.convertFromListStringToIntances(linesOgrenmeSonuc);
            string[] lastElementCount = linesOgrenmeSonuc.ElementAt(0).Split(',');
            Instances testData = helper.convertFromListStringToIntances(linesTestSonuc);
            testData.setClassIndex(lastElementCount.Count() - 1);

            Bagging bagging = new Bagging();
            bagging.setClassifier(new DecisionStump());
            LogitBoost ogrenmeLogistic = new LogitBoost();
            ogrenmeLogistic.setNumIterations(10);
            ogrenmeLogistic.setClassifier(bagging);
            ogrenmeLogistic.buildClassifier(trainData);

            double deplasmanOran;
            double depProbability = helper.macProbability(sabitDeger.deplasmanSonuc, ligPair);
            foreach (var mac in linesTestOgrenme)
            {
                int indexIns = linesTestOgrenme.IndexOf(mac);
                double[] probabilities1 = ogrenmeLogistic.distributionForInstance(testData.instance(indexIns));
                double evSahibiProb = probabilities1[0];
                double deplasmanProb = probabilities1[1];

                //Butun tahminler alinir
                lstSonucHepsi.Add(ekleSonuc(mac, deplasmanProb, "D"));

                if (evSahibiProb == 0.5 && deplasmanProb == 0.5)
                {
                    continue;
                }

                double label = ogrenmeLogistic.classifyInstance(testData.instance(indexIns));
                testData.instance(indexIns).setClassValue(label);
                string answer = testData.instance(indexIns).value(lastElementCount.Count() - 1).ToString();
                answer = deplasmanProb > depProbability ? "D" : "E";
                
                if (answer != "D")
                {
                    continue;
                }

                using (var ctx = new IDDAA_Entities())
                {
                    if (ctx.BULTEN.First(c => c.IDDAA_ID == mac.IDDAA_ID).MBS > sabitDeger.macSonuOynanacakMbs)
                    {
                        continue;
                    }
                    deplasmanOran = Convert.ToDouble(ctx.BULTEN.First(c => c.IDDAA_ID == mac.IDDAA_ID).MS_2);
                    if (deplasmanOran < sabitDeger.macSonuTahminMinBahisIddaaOran)
                    {
                        continue;
                    }
                }

                lstSonuc.Add(ekleSonuc(mac, deplasmanProb, answer));
            }

            //butun tahminler insert
            tahminMLHepsiServis.ekleTahminMLList(lstSonucHepsi);

            return lstSonuc;
        }

        private Sonuc ekleSonuc(OGRENME line, Double sistemOran, string answer)
        {
            HelperServis helper = new HelperServis();
            Sonuc sonuc = new Sonuc();
            sonuc.Tahmin = answer == "E" ? "EvSahibi" : (answer == "B" ? "Beraberlik" : (answer == "D" ? "Deplasman" : ""));
            sonuc.Tarih = line.TARIH;
            sonuc.TarihSaat = line.TARIH.ToString("dd/MM - HH:mm");
            sonuc.EvSahibi = line.EV_SAHIBI;
            sonuc.Deplasman = line.DEPLASMAN;
            sonuc.IddaaOran = helper.bulMacSonuOran(line.EV_SAHIBI, line.DEPLASMAN, line.TARIH, answer);
            sonuc.SistemOran = Math.Round(sistemOran, 4) * 100;
            sonuc.lig = line.LIG;
            sonuc.tip = sabitDeger.macSonuSonuc;
            sonuc.deger = "2";
            sonuc.IddaaId = line.IDDAA_ID;
            return sonuc;
        }

        private StringBuilder yazSonuc(StringBuilder sb, List<Sonuc> lstSonuc)
        {
            HelperServis helper = new HelperServis();

            var lstTarih = lstSonuc.GroupBy(x => x.Tarih.ToShortDateString())
                   .Select(grp => new
                   {
                       tarih = grp.Key
                   }).ToArray();

            foreach (var macTarih in lstTarih.OrderBy(c => c.tarih))
            {
                foreach (var item in lstSonuc.Where(c => c.Tarih.ToShortDateString() == macTarih.tarih).OrderBy(c => c.TarihSaat))
                {
                    sb.Append(item.TarihSaat);
                    sb.Append(" --> ");
                    sb.Append(item.EvSahibi);
                    sb.Append(" - ");
                    sb.Append(item.Deplasman);
                    sb.Append(" -- ");
                    sb.Append(item.Tahmin);
                    sb.Append(" -- ");
                    sb.Append(item.IddaaOran);
                    sb.Append(" -- ");
                    sb.Append("%" + item.SistemOran);
                    sb.Append(System.Environment.NewLine);
                }
                sb.Append(System.Environment.NewLine);
            }
            return sb;
        }

        private Dictionary<int, string[]> opsiyonelAttributeKumeleri(Dictionary<int, string[]> atrributeCountMap,
            List<OGRENME> lstOgrenme)
        {
            MacSonuAttributeServisNew macSonuAttributeServisNew = new MacSonuAttributeServisNew();
            WekaAttributePriority attributePriority = macSonuAttributeServisNew.secAttributePerformance(lstOgrenme);
            Dictionary<int, double> priorityPuanMap = new Dictionary<int, double>();

            for (int i = 0; i < attributePriority.CorrelationPriorityMap.Count; i++)
            {
                priorityPuanMap.Add(i, 0);
            }

            for (int i = sabitDeger.deplasmanAttributeCountMin; i <= sabitDeger.deplasmanAttributeCount; i++)
            {
                List<string> secilenAttribute = new List<string>();
                DahilOgrenmeAttribute dahilOgrenme = new DahilOgrenmeAttribute();

                //Dictionary<int, double> priorityMap = macSonuAttributeServisNew.secAttribute(i, lstOgrenme);
                macSonuAttributeServisNew.setPuanMapPerformance(priorityPuanMap, attributePriority.CorrelationPriorityMap, i);
                macSonuAttributeServisNew.setPuanMapPerformance(priorityPuanMap, attributePriority.GainRatioPriorityMap, i);
                macSonuAttributeServisNew.setPuanMapPerformance(priorityPuanMap, attributePriority.InfoGainPriorityMap, i);
                macSonuAttributeServisNew.setPuanMapPerformance(priorityPuanMap, attributePriority.OnePriorityMap, i);
                macSonuAttributeServisNew.setPuanMapPerformance(priorityPuanMap, attributePriority.ReliefPriorityMap, i);
                macSonuAttributeServisNew.setPuanMapPerformance(priorityPuanMap, attributePriority.SymmetricalPriorityMap, i);

                priorityPuanMap = priorityPuanMap.OrderByDescending(c => c.Value).ToDictionary(x => x.Key, x => x.Value);

                for (int x = 0; x < priorityPuanMap.Values.Count(c => c > 0); x++)
                {
                    secilenAttribute.Add(dahilOgrenme.macSonuDahilAttribute[priorityPuanMap.ElementAt(x).Key]);
                }
                atrributeCountMap.Add(i, secilenAttribute.ToArray());
            }

            return atrributeCountMap;
        }

        private CalistirTestSonuc calisTestParalel(Dictionary<int, string[]> atrributeCountMap, List<OgrenmeClass> lstOgrenmeButunAttributelar,
                string[] ligler)
        {
            MacSonuOgrenmeServisNew macSonuOgrenmeServisNew = new MacSonuOgrenmeServisNew();
            MacSonuWekaTestServisNew macSonuWekaTestServisNew = new MacSonuWekaTestServisNew();
            HelperServis helper = new HelperServis();
            decimal kar = 0;
            CalistirTestSonuc calistirTestSonuc = new CalistirTestSonuc();
            TahminTestServis tahminTestServis = new TahminTestServis();


            List<OGRENME> lstOgrenmeParallel = new List<OGRENME>();
            KarTest karTestParallel = new KarTest();
            Dictionary<int, decimal> karMapParallel = new Dictionary<int, decimal>();

            Parallel.For(0, atrributeCountMap.Count(), i => {

                // attributeCount icin ogrenme calisir
                lstOgrenmeParallel = macSonuOgrenmeServisNew.convertOgrenmeClassToOgrenmeContext(lstOgrenmeButunAttributelar, atrributeCountMap.ElementAt(i).Value, sabitDeger.deplasmanSonuc);

                //test calistir
                karTestParallel = macSonuWekaTestServisNew.calistirMacSonuOgrenmeTest(lstOgrenmeParallel, false, sabitDeger.deplasmanSonuc,
                    ligler);

                karMapParallel.Add(atrributeCountMap.ElementAt(i).Key, karTestParallel.kar);
            });

            // en karli olan attribute grubu alinir
            karMapParallel = karMapParallel.OrderByDescending(c => c.Value).ThenByDescending(c => c.Key).ToDictionary(x => x.Key, x => x.Value);

            tahminTestServis.ekleTahminTestList(ligler, sabitDeger.macSonuSonuc, "2", karMapParallel.First().Value, karMapParallel.First().Key,
                karTestParallel.testYuzdeOran);

            if (karMapParallel.First().Value <= sabitDeger.deplasmanKarTutar)
            {
                return null;
            }

            helper.sendTelegramKarTutar(ligler, karMapParallel.First().Value, sabitDeger.deplasmanSonuc, karMapParallel.First().Key);

            string[] kullanilacakAttribute = atrributeCountMap.Where(c => c.Key == karMapParallel.First().Key).First().Value;

            calistirTestSonuc.kullanilacakAttribute = kullanilacakAttribute;
            calistirTestSonuc.Kar = karMapParallel.First().Value;
            calistirTestSonuc.testYuzdeOran = karTestParallel.testYuzdeOran;

            return calistirTestSonuc;
        }

    }
}
