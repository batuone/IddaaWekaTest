using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IddaaWekaTest
{
    class OgrenmeClass
    {
        #region
        public BULTEN bultenItem { get; set; }

        public decimal Uzaklik { get; set; }

        public decimal MsGenelEvSahibiGalibiyetOran { get; set; }
        public decimal MsGenelEvSahibiBeraberOran { get; set; }
        public decimal MsGenelEvSahibiMaglubiyetOran { get; set; }
        public decimal MsGenelDeplasmanGalibiyetOran { get; set; }
        public decimal MsGenelDeplasmanBeraberOran { get; set; }
        public decimal MsGenelDeplasmanMaglubiyetOran { get; set; }
        public decimal MsIcDisEvSahibiGalibiyetOran { get; set; }
        public decimal MsIcDisEvSahibiBeraberOran { get; set; }
        public decimal MsIcDisEvSahibiMaglubiyetOran { get; set; }
        public decimal MsIcDisDeplasmanGalibiyetOran { get; set; }
        public decimal MsIcDisDeplasmanBeraberOran { get; set; }
        public decimal MsIcDisDeplasmanMaglubiyetOran { get; set; }
        public decimal MsAralarindaEvSahibiGalibiyetOran { get; set; }
        public decimal MsAralarindaEvSahibiBeraberOran { get; set; }
        public decimal MsAralarindaEvSahibiMaglubiyetOran { get; set; }
        public decimal MsAralarindaDeplasmanGalibiyetOran { get; set; }
        public decimal MsAralarindaDeplasmanBeraberOran { get; set; }
        public decimal MsAralarindaDeplasmanMaglubiyetOran { get; set; }

        public decimal IyGenelEvSahibiGalibiyetOran { get; set; }
        public decimal IyGenelEvSahibiBeraberOran { get; set; }
        public decimal IyGenelEvSahibiMaglubiyetOran { get; set; }
        public decimal IyGenelDeplasmanGalibiyetOran { get; set; }
        public decimal IyGenelDeplasmanBeraberOran { get; set; }
        public decimal IyGenelDeplasmanMaglubiyetOran { get; set; }
        public decimal IyIcDisEvSahibiGalibiyetOran { get; set; }
        public decimal IyIcDisEvSahibiBeraberOran { get; set; }
        public decimal IyIcDisEvSahibiMaglubiyetOran { get; set; }
        public decimal IyIcDisDeplasmanGalibiyetOran { get; set; }
        public decimal IyIcDisDeplasmanBeraberOran { get; set; }
        public decimal IyIcDisDeplasmanMaglubiyetOran { get; set; }
        public decimal IyAralarindaEvSahibiGalibiyetOran { get; set; }
        public decimal IyAralarindaEvSahibiBeraberOran { get; set; }
        public decimal IyAralarindaEvSahibiMaglubiyetOran { get; set; }
        public decimal IyAralarindaDeplasmanGalibiyetOran { get; set; }
        public decimal IyAralarindaDeplasmanBeraberOran { get; set; }
        public decimal IyAralarindaDeplasmanMaglubiyetOran { get; set; }

        public decimal GenelEvSahibiKgVarOran { get; set; }
        public decimal GenelEvSahibiKgYokOran { get; set; }
        public decimal IcDisEvSahibiKgVarOran { get; set; }
        public decimal IcDisEvSahibiKgYokOran { get; set; }
        public decimal AralarindaEvSahibiKgVarOran { get; set; }
        public decimal AralarindaEvSahibiKgYokOran { get; set; }
        public decimal GenelDeplasmanKgVarOran { get; set; }
        public decimal GenelDeplasmanKgYokOran { get; set; }
        public decimal IcDisDeplasmanKgVarOran { get; set; }
        public decimal IcDisDeplasmanKgYokOran { get; set; }
        public decimal AralarindaDeplasmanKgVarOran { get; set; }
        public decimal AralarindaDeplasmanKgYokOran { get; set; }

        public decimal GenelEvSahibi25AltOran { get; set; }
        public decimal GenelEvSahibi25UstOran { get; set; }
        public decimal IcDisEvSahibi25AltOran { get; set; }
        public decimal IcDisEvSahibi25UstOran { get; set; }
        public decimal AralarindaEvSahibi25AltOran { get; set; }
        public decimal AralarindaEvSahibi25UstOran { get; set; }
        public decimal GenelDeplasman25AltOran { get; set; }
        public decimal GenelDeplasman25UstOran { get; set; }
        public decimal IcDisDeplasman25AltOran { get; set; }
        public decimal IcDisDeplasman25UstOran { get; set; }
        public decimal AralarindaDeplasman25AltOran { get; set; }
        public decimal AralarindaDeplasman25UstOran { get; set; }

        public decimal GenelEvSahibiTg01Oran { get; set; }
        public decimal GenelEvSahibiTg23Oran { get; set; }
        public decimal GenelEvSahibiTg46Oran { get; set; }
        public decimal GenelEvSahibiTg7ArtiOran { get; set; }
        public decimal IcDisEvSahibiTg01Oran { get; set; }
        public decimal IcDisEvSahibiTg23Oran { get; set; }
        public decimal IcDisEvSahibiTg46Oran { get; set; }
        public decimal IcDisEvSahibiTg7ArtiOran { get; set; }
        public decimal AralarindaEvSahibiTg01Oran { get; set; }
        public decimal AralarindaEvSahibiTg23Oran { get; set; }
        public decimal AralarindaEvSahibiTg46Oran { get; set; }
        public decimal AralarindaEvSahibiTg7ArtiOran { get; set; }
        public decimal GenelDeplasmanTg01Oran { get; set; }
        public decimal GenelDeplasmanTg23Oran { get; set; }
        public decimal GenelDeplasmanTg46Oran { get; set; }
        public decimal GenelDeplasmanTg7ArtiOran { get; set; }
        public decimal IcDisDeplasmanTg01Oran { get; set; }
        public decimal IcDisDeplasmanTg23Oran { get; set; }
        public decimal IcDisDeplasmanTg46Oran { get; set; }
        public decimal IcDisDeplasmanTg7ArtiOran { get; set; }
        public decimal AralarindaDeplasmanTg01Oran { get; set; }
        public decimal AralarindaDeplasmanTg23Oran { get; set; }
        public decimal AralarindaDeplasmanTg46Oran { get; set; }
        public decimal AralarindaDeplasmanTg7ArtiOran { get; set; }

        public decimal EvSahibiGenelSira { get; set; }
        public decimal EvSahibiGenelGalibiyetOran { get; set; }
        public decimal EvSahibiGenelBeraberlikOran { get; set; }
        public decimal EvSahibiGenelMaglubiyetOran { get; set; }
        public decimal EvSahibiGenelAttigiGolOran { get; set; }
        public decimal EvSahibiGenelYedigiGolOran { get; set; }
        public decimal EvSahibiGenelPuanOran { get; set; }
        public decimal EvSahibiIcerdeSira { get; set; }
        public decimal EvSahibiIcerdeGalibiyetOran { get; set; }
        public decimal EvSahibiIcerdeBeraberlikOran { get; set; }
        public decimal EvSahibiIcerdeMaglubiyetOran { get; set; }
        public decimal EvSahibiIcerdeAttigiGolOran { get; set; }
        public decimal EvSahibiIcerdeYedigiGolOran { get; set; }
        public decimal EvSahibiIcerdePuanOran { get; set; }
        public decimal EvSahibiDisardaSira { get; set; }
        public decimal EvSahibiDisardaGalibiyetOran { get; set; }
        public decimal EvSahibiDisardaBeraberlikOran { get; set; }
        public decimal EvSahibiDisardaMaglubiyetOran { get; set; }
        public decimal EvSahibiDisardaAttigiGolOran { get; set; }
        public decimal EvSahibiDisardaYedigiGolOran { get; set; }
        public decimal EvSahibiDisardaPuanOran { get; set; }

        public decimal DeplasmanGenelSira { get; set; }
        public decimal DeplasmanGenelGalibiyetOran { get; set; }
        public decimal DeplasmanGenelBeraberlikOran { get; set; }
        public decimal DeplasmanGenelMaglubiyetOran { get; set; }
        public decimal DeplasmanGenelAttigiGolOran { get; set; }
        public decimal DeplasmanGenelYedigiGolOran { get; set; }
        public decimal DeplasmanGenelPuanOran { get; set; }
        public decimal DeplasmanIcerdeSira { get; set; }
        public decimal DeplasmanIcerdeGalibiyetOran { get; set; }
        public decimal DeplasmanIcerdeBeraberlikOran { get; set; }
        public decimal DeplasmanIcerdeMaglubiyetOran { get; set; }
        public decimal DeplasmanIcerdeAttigiGolOran { get; set; }
        public decimal DeplasmanIcerdeYedigiGolOran { get; set; }
        public decimal DeplasmanIcerdePuanOran { get; set; }
        public decimal DeplasmanDisardaSira { get; set; }
        public decimal DeplasmanDisardaGalibiyetOran { get; set; }
        public decimal DeplasmanDisardaBeraberlikOran { get; set; }
        public decimal DeplasmanDisardaMaglubiyetOran { get; set; }
        public decimal DeplasmanDisardaAttigiGolOran { get; set; }
        public decimal DeplasmanDisardaYedigiGolOran { get; set; }
        public decimal DeplasmanDisardaPuanOran { get; set; }

        public decimal EvSahibiGolDenemesi { get; set; }
        public decimal EvSahibiSutKaleyiBulan { get; set; }
        public decimal EvSahibiSutKaleyiBulmayan { get; set; }
        public decimal EvSahibiKorner { get; set; }
        public decimal EvSahibiTopaSahipOlma { get; set; }
        public decimal EvSahibiSutEngelleme { get; set; }
        public decimal EvSahibiToplamKart { get; set; }
        public decimal EvSahibiFrikik { get; set; }
        public decimal EvSahibiSariKart { get; set; }
        public decimal EvSahibiKirmiziKart { get; set; }
        public decimal EvSahibiSutEtkisi { get; set; }
        public decimal DeplasmanGolDenemesi { get; set; }
        public decimal DeplasmanSutKaleyiBulan { get; set; }
        public decimal DeplasmanSutKaleyiBulmayan { get; set; }
        public decimal DeplasmanKorner { get; set; }
        public decimal DeplasmanTopaSahipOlma { get; set; }
        public decimal DeplasmanSutEngelleme { get; set; }
        public decimal DeplasmanToplamKart { get; set; }
        public decimal DeplasmanFrikik { get; set; }
        public decimal DeplasmanSariKart { get; set; }
        public decimal DeplasmanKirmiziKart { get; set; }
        public decimal DeplasmanSutEtkisi { get; set; }

        public decimal EvSahibiSakatCezaliOran { get; set; }
        public decimal DeplasmanSakatCezaliOran { get; set; }

        public decimal EvSahibiSonMacGalibiyetOran { get; set; }
        public decimal EvSahibiSonMacBeraberOran { get; set; }
        public decimal EvSahibiSonMacMaglubiyetOran { get; set; }
        public decimal DeplasmanSonMacGalibiyetOran { get; set; }
        public decimal DeplasmanSonMacBeraberOran { get; set; }
        public decimal DeplasmanSonMacMaglubiyetOran { get; set; }
        #endregion

        public class DahilOgrenmeAttribute
        {
            public string[] macSonuDahilAttribute = new string[]
            {
                               
                "Uzaklik",
                "MsGenelEvSahibiGalibiyetOran",
                "MsGenelEvSahibiBeraberOran",
                "MsGenelEvSahibiMaglubiyetOran",
                "MsGenelDeplasmanGalibiyetOran",
                "MsGenelDeplasmanBeraberOran",
                "MsGenelDeplasmanMaglubiyetOran",
                "MsIcDisEvSahibiGalibiyetOran",
                "MsIcDisEvSahibiBeraberOran",
                "MsIcDisEvSahibiMaglubiyetOran",
                "MsIcDisDeplasmanGalibiyetOran",
                "MsIcDisDeplasmanBeraberOran",
                "MsIcDisDeplasmanMaglubiyetOran",
                "MsAralarindaEvSahibiGalibiyetOran",
                "MsAralarindaEvSahibiBeraberOran",
                "MsAralarindaEvSahibiMaglubiyetOran",
                "MsAralarindaDeplasmanGalibiyetOran",
                "MsAralarindaDeplasmanBeraberOran",
                "MsAralarindaDeplasmanMaglubiyetOran",
                "IyGenelEvSahibiGalibiyetOran",
                "IyGenelEvSahibiBeraberOran",
                "IyGenelEvSahibiMaglubiyetOran",
                "IyGenelDeplasmanGalibiyetOran",
                "IyGenelDeplasmanBeraberOran",
                "IyGenelDeplasmanMaglubiyetOran",
                "IyIcDisEvSahibiGalibiyetOran",
                "IyIcDisEvSahibiBeraberOran",
                "IyIcDisEvSahibiMaglubiyetOran",
                "IyIcDisDeplasmanGalibiyetOran",
                "IyIcDisDeplasmanBeraberOran",
                "IyIcDisDeplasmanMaglubiyetOran",
                "IyAralarindaEvSahibiGalibiyetOran",
                "IyAralarindaEvSahibiBeraberOran",
                "IyAralarindaEvSahibiMaglubiyetOran",
                "IyAralarindaDeplasmanGalibiyetOran",
                "IyAralarindaDeplasmanBeraberOran",
                "IyAralarindaDeplasmanMaglubiyetOran",
                "GenelEvSahibiKgVarOran",
                "GenelEvSahibiKgYokOran",
                "IcDisEvSahibiKgVarOran",
                "IcDisEvSahibiKgYokOran",
                "AralarindaEvSahibiKgVarOran",
                "AralarindaEvSahibiKgYokOran",
                "GenelDeplasmanKgVarOran",
                "GenelDeplasmanKgYokOran",
                "IcDisDeplasmanKgVarOran",
                "IcDisDeplasmanKgYokOran",
                "AralarindaDeplasmanKgVarOran",
                "AralarindaDeplasmanKgYokOran",
                "GenelEvSahibi25AltOran",
                "GenelEvSahibi25UstOran",
                "IcDisEvSahibi25AltOran",
                "IcDisEvSahibi25UstOran",
                "AralarindaEvSahibi25AltOran",
                "AralarindaEvSahibi25UstOran",
                "GenelDeplasman25AltOran",
                "GenelDeplasman25UstOran",
                "IcDisDeplasman25AltOran",
                "IcDisDeplasman25UstOran",
                "AralarindaDeplasman25AltOran",
                "AralarindaDeplasman25UstOran",
                "GenelEvSahibiTg01Oran",
                "GenelEvSahibiTg23Oran",
                "IcDisEvSahibiTg01Oran",
                "IcDisEvSahibiTg23Oran",
                "AralarindaEvSahibiTg01Oran",
                "AralarindaEvSahibiTg23Oran",
                "AralarindaEvSahibiTg46Oran",
                "AralarindaEvSahibiTg7ArtiOran",
                "GenelDeplasmanTg01Oran",
                "GenelDeplasmanTg23Oran",
                "IcDisDeplasmanTg01Oran",
                "IcDisDeplasmanTg23Oran",
                "AralarindaDeplasmanTg01Oran",
                "AralarindaDeplasmanTg23Oran",
                "AralarindaDeplasmanTg46Oran",
                "AralarindaDeplasmanTg7ArtiOran",
                "EvSahibiGenelSira",
                "EvSahibiGenelGalibiyetOran",
                "EvSahibiGenelBeraberlikOran",
                "EvSahibiGenelMaglubiyetOran",
                "EvSahibiGenelAttigiGolOran",
                "EvSahibiGenelYedigiGolOran",
                "EvSahibiGenelPuanOran",
                "EvSahibiIcerdeSira",
                "EvSahibiIcerdeGalibiyetOran",
                "EvSahibiIcerdeBeraberlikOran",
                "EvSahibiIcerdeMaglubiyetOran",
                "EvSahibiIcerdeAttigiGolOran",
                "EvSahibiIcerdeYedigiGolOran",
                "EvSahibiIcerdePuanOran",
                "EvSahibiDisardaSira",
                "EvSahibiDisardaGalibiyetOran",
                "EvSahibiDisardaBeraberlikOran",
                "EvSahibiDisardaMaglubiyetOran",
                "EvSahibiDisardaAttigiGolOran",
                "EvSahibiDisardaYedigiGolOran",
                "EvSahibiDisardaPuanOran",
                "DeplasmanGenelSira",
                "DeplasmanGenelGalibiyetOran",
                "DeplasmanGenelBeraberlikOran",
                "DeplasmanGenelMaglubiyetOran",
                "DeplasmanGenelAttigiGolOran",
                "DeplasmanGenelYedigiGolOran",
                "DeplasmanGenelPuanOran",
                "DeplasmanIcerdeSira",
                "DeplasmanIcerdeGalibiyetOran",
                "DeplasmanIcerdeBeraberlikOran",
                "DeplasmanIcerdeMaglubiyetOran",
                "DeplasmanIcerdeAttigiGolOran",
                "DeplasmanIcerdeYedigiGolOran",
                "DeplasmanIcerdePuanOran",
                "DeplasmanDisardaSira",
                "DeplasmanDisardaGalibiyetOran",
                "DeplasmanDisardaBeraberlikOran",
                "DeplasmanDisardaMaglubiyetOran",
                "DeplasmanDisardaAttigiGolOran",
                "DeplasmanDisardaYedigiGolOran",
                "DeplasmanDisardaPuanOran",
                "EvSahibiGolDenemesi",
                "EvSahibiSutKaleyiBulan",
                "EvSahibiSutKaleyiBulmayan",
                "EvSahibiKorner",
                "EvSahibiTopaSahipOlma",
                "EvSahibiSutEngelleme",
                "EvSahibiToplamKart",
                "EvSahibiFrikik",
                "EvSahibiSariKart",
                "EvSahibiKirmiziKart",
                "DeplasmanGolDenemesi",
                "DeplasmanSutKaleyiBulan",
                "DeplasmanSutKaleyiBulmayan",
                "DeplasmanKorner",
                "DeplasmanTopaSahipOlma",
                "DeplasmanSutEngelleme",
                "DeplasmanToplamKart",
                "DeplasmanFrikik",
                "DeplasmanSariKart",
                "DeplasmanKirmiziKart",
                "EvSahibiSakatCezaliOran",
                "DeplasmanSakatCezaliOran",
                "EvSahibiSonMacGalibiyetOran",
                "EvSahibiSonMacBeraberOran",
                "EvSahibiSonMacMaglubiyetOran",
                "DeplasmanSonMacGalibiyetOran",
                "DeplasmanSonMacBeraberOran",
                "DeplasmanSonMacMaglubiyetOran"




            };
            
        }

        public string[] DahilOgrenmeAttributeSetter(string[] aaa)
        {
            return aaa;
        }

        public class OgrenmeTestKume
        {
            public List<OGRENME> lstOgrenmeKume { get; set; }
            public List<OGRENME> lstTestKume { get; set; }

            public int testYuzdeOran { get; set; }
        }

        public class Sonuc
        {
            public String Tahmin { get; set; }
            public DateTime Tarih { get; set; }
            public String TarihSaat { get; set; }
            public String EvSahibi { get; set; }
            public String Deplasman { get; set; }
            public Decimal IddaaOran { get; set; }
            public Double SistemOran { get; set; }
            public Boolean isBasari { get; set; }
            public String lig { get; set; }
            public String tip { get; set; }
            public String deger { get; set; }
            public int testOran { get; set; }
            public int IddaaId { get; set; }
            public decimal kar { get; set; }
        }

        public class KarSonuc
        {
            public String Sonuc { get; set; }
            public Decimal Kar { get; set; }

            public List<string> OynananLigler { get; set; }
        }

        public class KarTest
        {
            public int testYuzdeOran { get; set; }
            public decimal kar { get; set; }

        }

        public class CalistirTestSonuc
        {
            public int testYuzdeOran { get; set; }
            public decimal Kar { get; set; }

            public string[] kullanilacakAttribute;

        }

        public class WekaAttributePriority
        {
            public Dictionary<int, double> CorrelationPriorityMap { get; set; }

            public Dictionary<int, double> GainRatioPriorityMap { get; set; }

            public Dictionary<int, double> InfoGainPriorityMap { get; set; }

            public Dictionary<int, double> OnePriorityMap { get; set; }

            public Dictionary<int, double> ReliefPriorityMap { get; set; }

            public Dictionary<int, double> SymmetricalPriorityMap { get; set; }

        }

        public class MacHesaplamaYuzde
        {
            public string msSkor { get; set; }
            public int iddaaId { get; set; }
            public decimal iddaaOran { get; set; }
            public decimal mlTahminYuzde { get; set; }

        }

    }
}
