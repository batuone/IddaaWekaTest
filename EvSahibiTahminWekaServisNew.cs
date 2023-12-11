using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading;
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
    class EvSahibiTahminWekaServisNew
    {
        SabitDegerler sabitDeger = new SabitDegerler();

        public void calistirTahmin(string[] ligler)
        {
            DahilOgrenmeAttribute dahilOgrenme = new DahilOgrenmeAttribute();
            MacSonuOgrenmeServisNew macSonuOgrenmeServisNew = new MacSonuOgrenmeServisNew();
            StringBuilder sb = new StringBuilder();
            HelperServis helper = new HelperServis();
            LigCalistirServis ligCalistirServis = new LigCalistirServis();

            Dictionary<int, string[]> atrributeCountMap = new Dictionary<int, string[]>();

            //full ogrenme
            List<OgrenmeClass> lstOgrenmeButunAttributelar = macSonuOgrenmeServisNew.ekleMacSonuOgrenmeButunAttributeler
                (ligler, dahilOgrenme.macSonuDahilAttribute, sabitDeger.evSahibiSonuc);

            List<OGRENME> lstOgrenme = macSonuOgrenmeServisNew.convertOgrenmeClassToOgrenmeContext(lstOgrenmeButunAttributelar, 
                dahilOgrenme.macSonuDahilAttribute, sabitDeger.evSahibiSonuc);

            //attribute belirle
            atrributeCountMap = opsiyonelAttributeKumeleri(atrributeCountMap, lstOgrenme);

            Classifier[] classifiers = sabitDeger.classifiers;
            ////test calistir
            List<CalistirTestSonuc> calistirTestSonucList = calisTestParallel(atrributeCountMap, lstOgrenmeButunAttributelar, 
                ligler, classifiers);

            List<CalistirTestSonuc> calistirTestSonucMax = calistirTestSonucList.Where(c => c.Kar == calistirTestSonucList.Max(d => d.Kar)).ToList();

            //sb = helper.yazSonuc(sb, calistirTestSonucMax);
            //helper.yazSonucWekaTestToFile(sb.ToString());

            //helper.sendTelegramMesaj(sb.ToString());

            ligCalistirServis.ekleSiniflandirma(calistirTestSonucMax);

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

            for (int i = sabitDeger.macSonuAttributeCountMin; i <= sabitDeger.macSonuAttributeCount; i++)
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

        private List<CalistirTestSonuc> calisTestParallel(Dictionary<int, string[]> atrributeCountMap, List<OgrenmeClass> lstOgrenmeButunAttributelar,
            string[] ligler, Classifier[] classifiers)
        {
            List<CalistirTestSonuc> calistirTestSonuclist = new List<CalistirTestSonuc>();
            
            foreach (var item in classifiers)
            {
                try
                { 
                    MacSonuOgrenmeServisNew macSonuOgrenmeServisNew = new MacSonuOgrenmeServisNew();
                    MacSonuWekaTestServisNew macSonuWekaTestServisNew = new MacSonuWekaTestServisNew();
                    HelperServis helper = new HelperServis();
                    decimal kar = 0;                
                    TahminTestServis tahminTestServis = new TahminTestServis();
                    List<OGRENME> lstOgrenmeParallel = new List<OGRENME>();
                    KarTest karTestParallel = new KarTest();
                    Dictionary<int, decimal> karMapParallel = new Dictionary<int, decimal>();

                    CalistirTestSonuc calistirTestSonuc = new CalistirTestSonuc();

                    Parallel.For(0, atrributeCountMap.Count(), i => {

                        Classifier classifieraa = (Classifier)Activator.CreateInstance(item.GetType());

                        lstOgrenmeParallel = macSonuOgrenmeServisNew.convertOgrenmeClassToOgrenmeContext(lstOgrenmeButunAttributelar, atrributeCountMap.ElementAt(i).Value, sabitDeger.evSahibiSonuc);

                        //test calistir
                        karTestParallel = macSonuWekaTestServisNew.calistirMacSonuOgrenmeTest(lstOgrenmeParallel, false, sabitDeger.evSahibiSonuc,
                            ligler, classifieraa);

                        karMapParallel.Add(atrributeCountMap.ElementAt(i).Key, karTestParallel.kar);
                    });

                    // en karli olan attribute grubu alinir
                    karMapParallel = karMapParallel.OrderByDescending(c => c.Value).ThenByDescending(c => c.Key).ToDictionary(x => x.Key, x => x.Value);

                    calistirTestSonuc.Kar = karMapParallel.First().Value;
                    calistirTestSonuc.wekaTip = item.GetType().Name;
                    calistirTestSonuc.lig = ligler.First();
                    calistirTestSonuc.macTip = sabitDeger.evSahibiSonuc;
                               
                    calistirTestSonuclist.Add(calistirTestSonuc);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            return calistirTestSonuclist;
        }



    }
}
