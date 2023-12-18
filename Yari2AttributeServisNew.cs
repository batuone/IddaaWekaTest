using IddaaWekaTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weka.attributeSelection;
using weka.core;
using static IddaaWekaTest.OgrenmeClass;

namespace IddaaWekaV0
{
    class Yari2AttributeServisNew
    {
        public WekaAttributePriority secAttributePerformance(List<OGRENME> lstOgrenme)
        {
            HelperServis helper = new HelperServis();
            Dictionary<int, double> priorityMap = new Dictionary<int, double>();
            Dictionary<int, double> priorityPuanMap = new Dictionary<int, double>();
            WekaAttributePriority attributePriority = new WekaAttributePriority();

            List<OGRENME> linesOgrenmeClass = new List<OGRENME>();
            lstOgrenme.ForEach(c => linesOgrenmeClass.Add(c.ShallowCopy()));
            linesOgrenmeClass = normalizeOgrenmeKumeYari(linesOgrenmeClass);

            List<String> linesOgrenme = linesOgrenmeClass.Select(c => c.SONUC).ToList();

            var attrCount = linesOgrenme.ElementAt(0).Split(',').Count() - 1;
            for (int i = 0; i < attrCount; i++)
            {
                priorityMap.Add(i, 0);
                priorityPuanMap.Add(i, 0);
            }

            Instances trainData = helper.convertFromListStringToIntancesYari(linesOgrenme);


            //CorrelationAttributeEval
            CorrelationAttributeEval correlation = new CorrelationAttributeEval();
            correlation.buildEvaluator(trainData);
            for (int i = 0; i < attrCount; i++)
            {
                var priority = correlation.evaluateAttribute(i);
                priority = Math.Round(priority, 6);

                priorityMap[i] += priority;
            }
            attributePriority.CorrelationPriorityMap = priorityMap.ToDictionary(c => c.Key, c => c.Value);
            setClear(priorityMap);

            //GainRatioAttributeEval
            GainRatioAttributeEval gain = new GainRatioAttributeEval();
            gain.buildEvaluator(trainData);
            for (int i = 0; i < attrCount; i++)
            {
                var priority = gain.evaluateAttribute(i);
                priority = Math.Round(priority, 6);

                priorityMap[i] += priority;
            }
            attributePriority.GainRatioPriorityMap = priorityMap.ToDictionary(c => c.Key, c => c.Value);
            setClear(priorityMap);

            ////InfoGainAttributeEval
            InfoGainAttributeEval infoGain = new InfoGainAttributeEval();
            infoGain.buildEvaluator(trainData);
            for (int i = 0; i < attrCount; i++)
            {
                var priority = infoGain.evaluateAttribute(i);
                priority = Math.Round(priority, 6);

                priorityMap[i] += priority;
            }
            attributePriority.InfoGainPriorityMap = priorityMap.ToDictionary(c => c.Key, c => c.Value);
            setClear(priorityMap);

            ////OneRAttributeEval
            OneRAttributeEval oneR = new OneRAttributeEval();
            oneR.buildEvaluator(trainData);
            for (int i = 0; i < attrCount; i++)
            {
                var priority = oneR.evaluateAttribute(i);
                priority = Math.Round(priority, 6);

                priorityMap[i] += priority;
            }
            attributePriority.OnePriorityMap = priorityMap.ToDictionary(c => c.Key, c => c.Value);
            setClear(priorityMap);

            ////ReliefFAttributeEval
            ReliefFAttributeEval relief = new ReliefFAttributeEval();
            relief.buildEvaluator(trainData);
            for (int i = 0; i < attrCount; i++)
            {
                var priority = relief.evaluateAttribute(i);
                priority = Math.Round(priority, 6);

                priorityMap[i] += priority;
            }
            attributePriority.ReliefPriorityMap = priorityMap.ToDictionary(c => c.Key, c => c.Value);
            setClear(priorityMap);

            ////SymmetricalUncertAttributeEval
            SymmetricalUncertAttributeEval symmetrical = new SymmetricalUncertAttributeEval();
            symmetrical.buildEvaluator(trainData);
            for (int i = 0; i < attrCount; i++)
            {
                var priority = symmetrical.evaluateAttribute(i);
                priority = Math.Round(priority, 6);

                priorityMap[i] += priority;
            }
            attributePriority.SymmetricalPriorityMap = priorityMap.ToDictionary(c => c.Key, c => c.Value);
            setClear(priorityMap);

            return attributePriority;
        }

        public List<OGRENME> normalizeOgrenmeKumeYari(List<OGRENME> lstOgrenmeKume)
        {
            HelperServis helper = new HelperServis();
            Instances inst = helper.convertFromListStringToIntancesYari(lstOgrenmeKume.Select(c => c.SONUC).ToList());
            inst = helper.normalization(inst);
            List<String> lstOgrenmeSonuc = helper.convertFromIntancesToListString(inst);

            int count = 0;
            foreach (var item in lstOgrenmeKume)
            {
                item.SONUC = lstOgrenmeSonuc.ElementAt(count);
                count++;
            }

            return lstOgrenmeKume;
        }

        private Dictionary<int, double> setClear(Dictionary<int, double> priorityMap)
        {
            for (int i = 0; i < priorityMap.Count; i++)
            {
                priorityMap[i] = 0;
            }
            return priorityMap;
        }
    }
}
