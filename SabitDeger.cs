using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weka.classifiers;
using weka.classifiers.bayes;
using weka.classifiers.functions;
using weka.classifiers.meta;
using weka.classifiers.rules;
using weka.classifiers.trees;
using weka.core;
using Attribute = weka.core.Attribute;

namespace IddaaWekaTest
{
    class SabitDeger
    {
        public class SabitDegerler
        {
            public string macSonuSonuc { get; set; } = "MacSonuSonuc";

            public string evSahibiSonuc { get; set; } = "EvSahibi";

            public string deplasmanSonuc { get; set; } = "Deplasman";

            public decimal evSahibiKarTutar { get; set; } = 0;

            public decimal deplasmanKarTutar { get; set; } = 0;            

            public double macSonuMinBahisIddaaOran { get; set; } = 1.00;

            public double macSonuTahminMinBahisIddaaOran { get; set; } = 1.00;

            public int macSonuOynanacakMbs { get; set; } = 3;
            
            public int macSonuAttributeCount { get; set; } = 75;

            public int macSonuAttributeCountMin { get; set; } = 5;

            public int deplasmanAttributeCount { get; set; } = 75;

            public int deplasmanAttributeCountMin { get; set; } = 5;

            public DateTime macSonuBaslangicTarihi { get; set; } = new DateTime(2020, 10, 01);
            
            public int macSonu800TestOran { get; set; } = 10;

            public int macSonu1000TestOran { get; set; } = 15;

            public int macSonu1200TestOran { get; set; } = 20;

            public int macSonuFullTestOran { get; set; } = 33;
            
            public Attribute attributeSonuc()
            {
                FastVector my_nominal_values = new FastVector(2);
                my_nominal_values.addElement("E");
                my_nominal_values.addElement("D");
                return new Attribute("sonuc", my_nominal_values);
            }


            public string altUstSonuc { get; set; } = "AltUstSonuc";

            public string altUst { get; set; } = "AltUst";

            public string ust { get; set; } = "Ust";

            public decimal altUstKarTutar { get; set; } = 0;

            public int altUstAttributeCount { get; set; } = 75;

            public int altUstAttributeCountMin { get; set; } = 5;            

            public int altUstTestOran { get; set; } = 15;
            
            public int altUst800TestOran { get; set; } = 10;

            public int altUst1000TestOran { get; set; } = 15;

            public int altUst1200TestOran { get; set; } = 20;

            public int altUstFullTestOran { get; set; } = 33;

            public Attribute attributeSonucAltUst()
            {
                FastVector my_nominal_values = new FastVector(2);
                my_nominal_values.addElement("Alt");
                my_nominal_values.addElement("Ust");
                return new Attribute("sonuc", my_nominal_values);
            }


            public Classifier[] classifiers = { new BayesNet(), new NaiveBayes(), new NaiveBayesMultinomial(),
             new Logistic(), new MultilayerPerceptron(), new SGD(), new SimpleLogistic(), new SMO(), new VotedPerceptron(), 
             new AdaBoostM1(), new Bagging(), new LogitBoost(), new MultiClassClassifier(), new RandomSubSpace(), new Stacking(),
             new DecisionTable(), new DecisionStump(), new J48(), new LMT(), new RandomForest(), new RandomTree(), new REPTree()};

                       
            public string[] testLigler = new string[] { "AVUS", "JAP", "JAP2", "GKOR", "GKOR2", "ÇINSL", "ÇEK2", "TSL", "T1L",
            "RUS","NOR","NOR1","ÇEK","SIRP","ITB","AL1","AL2","AVU", "DAN","DAN1","ISV","ROM","YUN","MAC","IS1","IS2",
            "BEL","HOL","HOL2","INP","INCL","ITA","FR1","FR2","POR","POL","POL1","BEL2","EST1","FIN","LITA","UKR",
            "POR2","IBSL","GAL","SLVN","IRL","IN1","IN2","IKP","ISÇ","BLR","HIR","ISV2","SUUD","AU2","AL3","GAFPSL",
            "ISÇ2","RUS1","URU","SIL","MEK","PER","EKV1","KOL","BR1","ARJ","ABD","PAR"};

                       
        }
    }
}
