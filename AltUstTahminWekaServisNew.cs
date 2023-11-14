using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weka.classifiers;
using weka.classifiers.functions;
using weka.classifiers.meta;
using weka.classifiers.trees;
using weka.core;
using static IddaaWekaTest.OgrenmeClass;
using static IddaaWekaTest.SabitDeger;
using Environment = System.Environment;

namespace IddaaWekaTest
{
    class AltUstTahminWekaServisNew
    {

        SabitDegerler sabitDeger = new SabitDegerler();

        public void calistirTahmin(string[] ligler)
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
                Dictionary<int, string[]> atrributeCountMap = new Dictionary<int, string[]>();

                lstOgrenmeButunAttributelar = macSonuOgrenmeServisNew.ekleMacSonuOgrenmeButunAttributeler
                    (ligler, dahilOgrenme.macSonuDahilAttribute, sabitDeger.altUstSonuc);

                lstOgrenme = macSonuOgrenmeServisNew.convertOgrenmeClassToOgrenmeContext(lstOgrenmeButunAttributelar, dahilOgrenme.macSonuDahilAttribute, sabitDeger.altUstSonuc);

                //attribute belirle
                atrributeCountMap = opsiyonelAttributeKumeleri(atrributeCountMap, lstOgrenme);

                Classifier[] classifiers = sabitDeger.classifiers;

                ////test calistir
                List<CalistirTestSonuc> calistirTestSonucList = calisTestParallel(atrributeCountMap, lstOgrenmeButunAttributelar, 
                    ligler, classifiers);

                List<CalistirTestSonuc> calistirTestSonucMax = calistirTestSonucList.Where(c => c.Kar == calistirTestSonucList.Max(d => d.Kar)).ToList();

                sb = helper.yazSonuc(sb, calistirTestSonucMax);
                helper.yazSonucWekaTestToFile(sb.ToString());
            }
        }

        private Dictionary<int, string[]> opsiyonelAttributeKumeleri(Dictionary<int, string[]> atrributeCountMap,
            List<OGRENME> lstOgrenme)
        {
            MacSonuAttributeServisNew macSonuAttributeServisNew = new MacSonuAttributeServisNew();
            AltUstAttributeServisNew altUstAttributeServisNew = new AltUstAttributeServisNew();
            WekaAttributePriority attributePriority = altUstAttributeServisNew.secAttributePerformance(lstOgrenme);
            Dictionary<int, double> priorityPuanMap = new Dictionary<int, double>();

            for (int i = 0; i < attributePriority.CorrelationPriorityMap.Count; i++)
            {
                priorityPuanMap.Add(i, 0);
            }

            for (int i = sabitDeger.altUstAttributeCountMin; i <= sabitDeger.altUstAttributeCount; i++)
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
                MacSonuOgrenmeServisNew macSonuOgrenmeServisNew = new MacSonuOgrenmeServisNew();
                AltUstWekaTestServisNew altUstWekaTestServisNew = new AltUstWekaTestServisNew();
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
                    lstOgrenmeParallel = macSonuOgrenmeServisNew.convertOgrenmeClassToOgrenmeContext(lstOgrenmeButunAttributelar, atrributeCountMap.ElementAt(i).Value, sabitDeger.altUstSonuc);

                    //test calistir
                    karTestParallel = altUstWekaTestServisNew.calistirMacSonuOgrenmeTest(lstOgrenmeParallel, false,
                        sabitDeger.altUstTestOran, ligler, classifieraa);

                    karMapParallel.Add(atrributeCountMap.ElementAt(i).Key, karTestParallel.kar);
                });


                // en karli olan attribute grubu alinir
                karMapParallel = karMapParallel.OrderByDescending(c => c.Value).ThenByDescending(c => c.Key).ToDictionary(x => x.Key, x => x.Value);

                calistirTestSonuc.Kar = karMapParallel.First().Value;
                calistirTestSonuc.wekaTip = item.GetType().Name;
                calistirTestSonuc.lig = ligler.First();
                calistirTestSonuc.macTip = sabitDeger.altUstSonuc;

                calistirTestSonuclist.Add(calistirTestSonuc);
            }

            return calistirTestSonuclist;
        }
    }
}
