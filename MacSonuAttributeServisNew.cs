using System;
using System.Collections.Generic;
using System.Linq;
using weka.classifiers.functions;
using weka.core;
using static IddaaWekaTest.OgrenmeClass;
using static IddaaWekaTest.SabitDeger;
using weka.attributeSelection;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace IddaaWekaTest
{
    class MacSonuAttributeServisNew
    {
        SabitDegerler sabitDeger = new SabitDegerler();

        public Dictionary<int, double> secAttribute(int puanCount, List<OGRENME> lstOgrenme)
        {
            HelperServis helper = new HelperServis();
            Dictionary<int, double> priorityMap = new Dictionary<int, double>();
            Dictionary<int, double> priorityPuanMap = new Dictionary<int, double>();

            List<OGRENME> linesOgrenmeClass = new List<OGRENME>();
            lstOgrenme.ForEach(c => linesOgrenmeClass.Add(c.ShallowCopy()));
            linesOgrenmeClass = normalizeOgrenmeKume(linesOgrenmeClass);

            List<String> linesOgrenme = linesOgrenmeClass.Select(c => c.SONUC).ToList();

            var attrCount = linesOgrenme.ElementAt(0).Split(',').Count() - 1;
            for (int i = 0; i < attrCount; i++)
            {
                priorityMap.Add(i, 0);
                priorityPuanMap.Add(i, 0);
            }

            Instances trainData = helper.convertFromListStringToIntances(linesOgrenme);


            //CorrelationAttributeEval
            CorrelationAttributeEval correlation = new CorrelationAttributeEval();
            correlation.buildEvaluator(trainData);
            for (int i = 0; i < attrCount; i++)
            {
                var priority = correlation.evaluateAttribute(i);
                priority = Math.Round(priority, 6);

                priorityMap[i] += priority;
            }
            priorityMap = setPuanMap(priorityPuanMap, priorityMap, puanCount);

            //GainRatioAttributeEval
            GainRatioAttributeEval gain = new GainRatioAttributeEval();
            gain.buildEvaluator(trainData);
            for (int i = 0; i < attrCount; i++)
            {
                var priority = gain.evaluateAttribute(i);
                priority = Math.Round(priority, 6);

                priorityMap[i] += priority;
            }
            priorityMap = setPuanMap(priorityPuanMap, priorityMap, puanCount);

            ////InfoGainAttributeEval
            InfoGainAttributeEval infoGain = new InfoGainAttributeEval();
            infoGain.buildEvaluator(trainData);
            for (int i = 0; i < attrCount; i++)
            {
                var priority = infoGain.evaluateAttribute(i);
                priority = Math.Round(priority, 6);

                priorityMap[i] += priority;
            }
            priorityMap = setPuanMap(priorityPuanMap, priorityMap, puanCount);

            ////OneRAttributeEval
            OneRAttributeEval oneR = new OneRAttributeEval();
            oneR.buildEvaluator(trainData);
            for (int i = 0; i < attrCount; i++)
            {
                var priority = oneR.evaluateAttribute(i);
                priority = Math.Round(priority, 6);

                priorityMap[i] += priority;
            }
            priorityMap = setPuanMap(priorityPuanMap, priorityMap, puanCount);

            ////ReliefFAttributeEval
            ReliefFAttributeEval relief = new ReliefFAttributeEval();
            relief.buildEvaluator(trainData);
            for (int i = 0; i < attrCount; i++)
            {

                var priority = relief.evaluateAttribute(i);
                priority = Math.Round(priority, 6);

                priorityMap[i] += priority;
            }
            priorityMap = setPuanMap(priorityPuanMap, priorityMap, puanCount);

            ////SymmetricalUncertAttributeEval
            SymmetricalUncertAttributeEval symmetrical = new SymmetricalUncertAttributeEval();
            symmetrical.buildEvaluator(trainData);
            for (int i = 0; i < attrCount; i++)
            {
                var priority = symmetrical.evaluateAttribute(i);
                priority = Math.Round(priority, 6);

                priorityMap[i] += priority;
            }
            priorityMap = setPuanMap(priorityPuanMap, priorityMap, puanCount);


            priorityPuanMap = priorityPuanMap.OrderByDescending(c => c.Value).ToDictionary(x => x.Key, x => x.Value);

            return priorityPuanMap;
        }

        private Dictionary<int, double> setClear(Dictionary<int, double> priorityMap)
        {
            for (int i = 0; i < priorityMap.Count; i++)
            {
                priorityMap[i] = 0;
            }
            return priorityMap;
        }

        private Dictionary<int, double> setPuanMap(Dictionary<int, double> priorityPuanMap, Dictionary<int, double> priorityMap,
            int puanCount)
        {
            priorityMap = priorityMap.OrderByDescending(c => c.Value).ToDictionary(x => x.Key, x => x.Value);
            double sira = 1;
            foreach (var item in priorityMap)
            {
                if (puanCount == 0 || item.Value <= 0)
                {
                    break;
                }
                priorityPuanMap[item.Key] += 1;
                puanCount--;
                sira++;
            }
            return setClear(priorityMap);
        }

        public List<OGRENME> normalizeOgrenmeKume(List<OGRENME> lstOgrenmeKume)
        {
            HelperServis helper = new HelperServis();
            Instances inst = helper.convertFromListStringToIntances(lstOgrenmeKume.Select(c => c.SONUC).ToList());
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

        public List<OGRENME> normalizeOgrenmeKumeAltUst(List<OGRENME> lstOgrenmeKume)
        {
            HelperServis helper = new HelperServis();
            Instances inst = helper.convertFromListStringToIntancesAltUst(lstOgrenmeKume.Select(c => c.SONUC).ToList());
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
        
        public WekaAttributePriority secAttributePerformance(List<OGRENME> lstOgrenme)
        {
            HelperServis helper = new HelperServis();
            Dictionary<int, double> priorityMap = new Dictionary<int, double>();
            Dictionary<int, double> priorityPuanMap = new Dictionary<int, double>();
            WekaAttributePriority attributePriority = new WekaAttributePriority();

            List<OGRENME> linesOgrenmeClass = new List<OGRENME>();
            lstOgrenme.ForEach(c => linesOgrenmeClass.Add(c.ShallowCopy()));
            linesOgrenmeClass = normalizeOgrenmeKume(linesOgrenmeClass);

            List<String> linesOgrenme = linesOgrenmeClass.Select(c => c.SONUC).ToList();

            var attrCount = linesOgrenme.ElementAt(0).Split(',').Count() - 1;
            for (int i = 0; i < attrCount; i++)
            {
                priorityMap.Add(i, 0);
                priorityPuanMap.Add(i, 0);
            }

            Instances trainData = helper.convertFromListStringToIntances(linesOgrenme);


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

        public Dictionary<int, double> setPuanMapPerformance(Dictionary<int, double> priorityPuanMap, 
            Dictionary<int, double> priorityMap, int puanCount)
        {
            priorityMap = priorityMap.OrderByDescending(c => c.Value).ToDictionary(x => x.Key, x => x.Value);
            double sira = 1;
            foreach (var item in priorityMap)
            {
                if (puanCount == 0 || item.Value <= 0)
                {
                    break;
                }
                priorityPuanMap[item.Key] += 1;
                puanCount--;
                sira++;
            }
            return priorityPuanMap;
        }
    }
}
