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
    class Yari2TahminWekaServisNew
    {
        SabitDegerler sabitDeger = new SabitDegerler();

        public void calistirTahmin(string[] ligler)
        {
            MacSonuOgrenmeServisNew macSonuOgrenmeServisNew = new MacSonuOgrenmeServisNew();
            List<OgrenmeClass> lstOgrenmeButunAttributelar = new List<OgrenmeClass>();
            HelperServis helper = new HelperServis();
            List<OGRENME> lstOgrenme = new List<OGRENME>();
            DahilOgrenmeAttribute dahilOgrenme = new DahilOgrenmeAttribute();
            CalistirTestSonuc calistirTestSonuc = new CalistirTestSonuc();
            List<Sonuc> lstSonucGenel = new List<Sonuc>();
            TahminMLServis tahminMLServis = new TahminMLServis();
            StringBuilder sb = new StringBuilder();
            LigCalistirServis ligCalistirServis = new LigCalistirServis();

            List<BULTEN> lstBulten = macSonuOgrenmeServisNew.getirBultenData(ligler);

            Dictionary<int, string[]> atrributeCountMap = new Dictionary<int, string[]>();

            lstOgrenmeButunAttributelar = macSonuOgrenmeServisNew.ekleMacSonuOgrenmeButunAttributeler
                            (ligler, dahilOgrenme.macSonuDahilAttribute, sabitDeger.hangiYariGol);

            lstOgrenme = macSonuOgrenmeServisNew.convertOgrenmeClassToOgrenmeContext(lstOgrenmeButunAttributelar, dahilOgrenme.macSonuDahilAttribute, sabitDeger.hangiYariGol);


            //attribute belirle
            atrributeCountMap = opsiyonelAttributeKumeleri(atrributeCountMap, lstOgrenme);

            Classifier[] classifiers = sabitDeger.classifiers;

            ////test calistir
            List<CalistirTestSonuc> calistirTestSonucList = calisTestParallel(atrributeCountMap, lstOgrenmeButunAttributelar, ligler, classifiers);

            List<CalistirTestSonuc> calistirTestSonucMax = calistirTestSonucList.Where(c => c.Kar == calistirTestSonucList.Max(d => d.Kar)).ToList();


            ligCalistirServis.ekleSiniflandirma(calistirTestSonucMax);

        }

        private Dictionary<int, string[]> opsiyonelAttributeKumeleri(Dictionary<int, string[]> atrributeCountMap,
                List<OGRENME> lstOgrenme)
        {
            MacSonuAttributeServisNew macSonuAttributeServisNew = new MacSonuAttributeServisNew();
            Yari2AttributeServisNew yari2AttributeServisNew = new Yari2AttributeServisNew();
            WekaAttributePriority attributePriority = yari2AttributeServisNew.secAttributePerformance(lstOgrenme);
            Dictionary<int, double> priorityPuanMap = new Dictionary<int, double>();

            for (int i = 0; i < attributePriority.CorrelationPriorityMap.Count; i++)
            {
                priorityPuanMap.Add(i, 0);
            }

            for (int i = sabitDeger.yariAttributeCountMin; i <= sabitDeger.yariAttributeCount; i++)
            {
                List<string> secilenAttribute = new List<string>();
                DahilOgrenmeAttribute dahilOgrenme = new DahilOgrenmeAttribute();

                //Dictionary<int, double> priorityMap = altUstAttributeServisNew.secAttribute(i, lstOgrenme);
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


        private List<CalistirTestSonuc> calisTestParallel(Dictionary<int, string[]> atrributeCountMap, List<OgrenmeClass> lstOgrenmeButunAttributelar,
                string[] ligler, Classifier[] classifiers)
        {
            List<CalistirTestSonuc> calistirTestSonuclist = new List<CalistirTestSonuc>();

            foreach (var item in classifiers)
            {
                try
                {
                    MacSonuOgrenmeServisNew macSonuOgrenmeServisNew = new MacSonuOgrenmeServisNew();
                    Yari2WekaTestServisNew yari2WekaTestServisNew = new Yari2WekaTestServisNew();
                    HelperServis helper = new HelperServis();
                    decimal kar = 0;
                    CalistirTestSonuc calistirTestSonuc = new CalistirTestSonuc();
                    TahminTestServis tahminTestServis = new TahminTestServis();

                    List<OGRENME> lstOgrenmeParallel = new List<OGRENME>();
                    KarTest karTestParallel = new KarTest();
                    Dictionary<int, decimal> karMapParallel = new Dictionary<int, decimal>();

                    Parallel.For(0, atrributeCountMap.Count(), i => {

                        Classifier classifieraa = (Classifier)Activator.CreateInstance(item.GetType());

                        // attributeCount icin ogrenme calisir
                        lstOgrenmeParallel = macSonuOgrenmeServisNew.convertOgrenmeClassToOgrenmeContext(lstOgrenmeButunAttributelar, atrributeCountMap.ElementAt(i).Value, sabitDeger.hangiYariGol);

                        //test calistir
                        karTestParallel = yari2WekaTestServisNew.calistirMacSonuOgrenmeTest(lstOgrenmeParallel, false,
                            sabitDeger.yariTestOran, ligler, classifieraa);

                        karMapParallel.Add(atrributeCountMap.ElementAt(i).Key, karTestParallel.kar);
                    });


                    // en karli olan attribute grubu alinir
                    karMapParallel = karMapParallel.OrderByDescending(c => c.Value).ThenByDescending(c => c.Key).ToDictionary(x => x.Key, x => x.Value);

                    calistirTestSonuc.Kar = karMapParallel.First().Value;
                    calistirTestSonuc.wekaTip = item.GetType().Name;
                    calistirTestSonuc.lig = ligler.First();
                    calistirTestSonuc.macTip = sabitDeger.yari2;

                    calistirTestSonuclist.Add(calistirTestSonuc);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return calistirTestSonuclist;
        }

        private List<Sonuc> calistirTahminSonuc(string[] kullanilacakAttribute, string[] ligPair, List<OgrenmeClass> lstOgrenmeButunAttributelar)
        {
            List<Sonuc> lstSonuc = new List<Sonuc>();
            List<Sonuc> lstSonucHepsi = new List<Sonuc>();
            List<OGRENME> lstOgrenme = new List<OGRENME>();
            MacSonuOgrenmeServisNew macSonuOgrenmeServisNew = new MacSonuOgrenmeServisNew();
            StringBuilder sb = new StringBuilder();
            HelperServis helper = new HelperServis();
            TahminMLHepsiServis tahminMLHepsiServis = new TahminMLHepsiServis();

            lstOgrenme = macSonuOgrenmeServisNew.convertOgrenmeClassToOgrenmeContext(lstOgrenmeButunAttributelar, kullanilacakAttribute, sabitDeger.hangiYariGol);

            //ogrenme kumesini alir
            List<OGRENME> linesOgrenme = lstOgrenme;
            //test kumesini alir
            List<OGRENME> linesTest = macSonuOgrenmeServisNew.setMacSonuTahmin(kullanilacakAttribute, ligPair, sabitDeger.hangiYariGol);

            List<OGRENME> lstMergeOgrenme = new List<OGRENME>();
            lstMergeOgrenme.AddRange(linesTest);
            lstMergeOgrenme.AddRange(linesOgrenme);

            lstMergeOgrenme = macSonuOgrenmeServisNew.normalizeOgrenmeKumeYari(lstMergeOgrenme);

            List<String> linesOgrenmeSonuc = lstMergeOgrenme.Where(c => linesOgrenme.Any(d => d.IDDAA_ID == c.IDDAA_ID)).
                Select(c => c.SONUC).ToList();

            List<OGRENME> linesTestOgrenme = lstMergeOgrenme.Where(c => linesTest.Any(d => d.IDDAA_ID == c.IDDAA_ID)).ToList();
            List<String> linesTestSonuc = lstMergeOgrenme.Where(c => linesTest.Any(d => d.IDDAA_ID == c.IDDAA_ID)).
                Select(c => c.SONUC).ToList();

            if (linesTestSonuc.Count() == 0)
            {
                return new List<Sonuc>();
            }

            Instances trainData = helper.convertFromListStringToIntancesYari(linesOgrenmeSonuc);
            string[] lastElementCount = linesOgrenmeSonuc.ElementAt(0).Split(',');
            Instances testData = helper.convertFromListStringToIntancesYari(linesTestSonuc);
            testData.setClassIndex(lastElementCount.Count() - 1);


            Classifier ogrenmeLogistic = helper.getMlClassifier(ligPair[0], sabitDeger.yari2);
            ogrenmeLogistic.buildClassifier(trainData);


            double yari1Oran; double yari2Oran;
            double yari2Probability = helper.macProbability(sabitDeger.yari2, ligPair);
            foreach (var mac in linesTestOgrenme)
            {
                int indexIns = linesTestOgrenme.IndexOf(mac);
                double label = ogrenmeLogistic.classifyInstance(testData.instance(indexIns));
                testData.instance(indexIns).setClassValue(label);
                string answer = testData.instance(indexIns).value(lastElementCount.Count() - 1).ToString();

                double[] probabilities1 = ogrenmeLogistic.distributionForInstance(testData.instance(indexIns));
                double yari1Prob = probabilities1[0];
                double yari2Prob = probabilities1[1];

                //Butun tahminler alinir
                lstSonucHepsi.Add(ekleSonuc(mac, yari2Prob, "yari2"));

                if (yari1Prob > yari2Prob)
                {
                    answer = yari1Prob > yari2Probability ? "yari1" : "yari2";
                    if (answer != "yari1")
                    {
                        continue;
                    }
                }
                else
                {
                    answer = yari2Prob > yari2Probability ? "yari2" : "yari1";
                    if (answer != "yari2")
                    {
                        continue;
                    }
                }
                //sadece ust icin calissin
                if (answer != "yari2")
                {
                    continue;
                }


                lstSonuc.Add(ekleSonuc(mac, answer == "yari1" ? yari1Prob : yari2Prob, answer));
            }

            //butun tahminler insert
            tahminMLHepsiServis.ekleTahminMLList(lstSonucHepsi);

            return lstSonuc;
        }

        private Sonuc ekleSonuc(OGRENME line, Double sistemOran, string answer)
        {
            HelperServis helper = new HelperServis();
            Sonuc sonuc = new Sonuc();
            sonuc.Tahmin = answer;
            sonuc.Tarih = line.TARIH;
            sonuc.TarihSaat = line.TARIH.ToString("dd/MM - HH:mm");
            sonuc.EvSahibi = line.EV_SAHIBI;
            sonuc.Deplasman = line.DEPLASMAN;
            sonuc.IddaaOran = helper.bulAltUstOran(line.EV_SAHIBI, line.DEPLASMAN, line.TARIH, answer);
            sonuc.SistemOran = Math.Round(sistemOran, 4) * 100;
            sonuc.lig = line.LIG;
            sonuc.tip = sabitDeger.hangiYariGol;
            sonuc.deger = answer;
            sonuc.IddaaId = line.IDDAA_ID;
            return sonuc;
        }
    }
}
