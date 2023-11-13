using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using weka.core;
using static IddaaWekaTest.SabitDeger;
using Environment = System.Environment;

namespace IddaaWekaTest
{
    class MacSonuOgrenmeServisNew
    {
        SabitDegerler sabitDeger = new SabitDegerler();
        
        public List<BULTEN> getirArsivData(string[] ligler)
        {
            using (var ctx = new IDDAA_Entities())
            {
                HelperServis helper = new HelperServis();

                var arsivIddaaId = ctx.ARSIV.Where(c => ctx.PUAN_DURUMU.Any(d => c.IDDAA_ID == d.IDDAA_ID)
                    && ctx.TOPLAM_GOL_ISTATISTIK.Any(d => c.IDDAA_ID == d.IDDAA_ID)
                    && ctx.MS25_ALT_UST_ISTATISTIK.Any(d => c.IDDAA_ID == d.IDDAA_ID)
                    && ctx.KG_VAR_YOK_ISTATISTIK.Any(d => c.IDDAA_ID == d.IDDAA_ID)
                    && ctx.ILK_YARI_SONU_ISTATISTIK.Any(d => c.IDDAA_ID == d.IDDAA_ID)
                    && ctx.MAC_SONU_ISTATISTIK.Any(d => c.IDDAA_ID == d.IDDAA_ID)
                    && ctx.TAKIM_ISTATISTIK.Any(d => c.IDDAA_ID == d.IDDAA_ID)).Select(c => c.IDDAA_ID);

                var arsiv = ctx.ARSIV.Where(c => arsivIddaaId.Any(d => c.IDDAA_ID == d) 
                                                && c.TARIH >= sabitDeger.macSonuBaslangicTarihi);


                arsiv = arsiv.Where(c => ligler.Contains(c.LIG)).OrderBy(c => c.TARIH);

                return helper.yazArsivToBulten(arsiv);
            }
        }
     
        public OgrenmeClass setMacSonuIstatistik(OgrenmeClass ogrenmeClass)
        {
            using (var ctx = new IDDAA_Entities())
            {
                HelperServis helper = new HelperServis();

                var macSonuIstatistik = ctx.MAC_SONU_ISTATISTIK.FirstOrDefault(c => c.IDDAA_ID == ogrenmeClass.bultenItem.IDDAA_ID);

                ogrenmeClass.MsGenelEvSahibiGalibiyetOran = Convert.ToDecimal(macSonuIstatistik.GENEL_EV_GALIBIYET_ORAN);
                ogrenmeClass.MsGenelEvSahibiBeraberOran = Convert.ToDecimal(macSonuIstatistik.GENEL_EV_BERABER_ORAN);
                ogrenmeClass.MsGenelEvSahibiMaglubiyetOran = Convert.ToDecimal(macSonuIstatistik.GENEL_EV_MAGLUBIYET_ORAN);
                ogrenmeClass.MsGenelDeplasmanGalibiyetOran = Convert.ToDecimal(macSonuIstatistik.GENEL_DEP_GALIBIYET_ORAN);
                ogrenmeClass.MsGenelDeplasmanBeraberOran = Convert.ToDecimal(macSonuIstatistik.GENEL_DEP_BERABER_ORAN);
                ogrenmeClass.MsGenelDeplasmanMaglubiyetOran = Convert.ToDecimal(macSonuIstatistik.GENEL_DEP_MAGLUBIYET_ORAN);

                ogrenmeClass.MsIcDisEvSahibiGalibiyetOran = Convert.ToDecimal(macSonuIstatistik.IC_DIS_EV_GALIBIYET_ORAN);
                ogrenmeClass.MsIcDisEvSahibiBeraberOran = Convert.ToDecimal(macSonuIstatistik.IC_DIS_EV_BERABER_ORAN);
                ogrenmeClass.MsIcDisEvSahibiMaglubiyetOran = Convert.ToDecimal(macSonuIstatistik.IC_DIS_EV_MAGLUBIYET_ORAN);
                ogrenmeClass.MsIcDisDeplasmanGalibiyetOran = Convert.ToDecimal(macSonuIstatistik.IC_DIS_DEP_GALIBIYET_ORAN);
                ogrenmeClass.MsIcDisDeplasmanBeraberOran = Convert.ToDecimal(macSonuIstatistik.IC_DIS_DEP_BERABER_ORAN);
                ogrenmeClass.MsIcDisDeplasmanMaglubiyetOran = Convert.ToDecimal(macSonuIstatistik.IC_DIS_DEP_MAGLUBIYET_ORAN);

                ogrenmeClass.MsAralarindaEvSahibiGalibiyetOran = Convert.ToDecimal(macSonuIstatistik.ARA_EV_GALIBIYET_ORAN);
                ogrenmeClass.MsAralarindaEvSahibiBeraberOran = Convert.ToDecimal(macSonuIstatistik.ARA_EV_BERABER_ORAN);
                ogrenmeClass.MsAralarindaEvSahibiMaglubiyetOran = Convert.ToDecimal(macSonuIstatistik.ARA_EV_MAGLUBIYET_ORAN);
                ogrenmeClass.MsAralarindaDeplasmanGalibiyetOran = Convert.ToDecimal(macSonuIstatistik.ARA_DEP_GALIBIYET_ORAN);
                ogrenmeClass.MsAralarindaDeplasmanBeraberOran = Convert.ToDecimal(macSonuIstatistik.ARA_DEP_BERABER_ORAN);
                ogrenmeClass.MsAralarindaDeplasmanMaglubiyetOran = Convert.ToDecimal(macSonuIstatistik.ARA_DEP_MAGLUBIYET_ORAN);

                return ogrenmeClass;
            }
        }

        public OgrenmeClass setIlkYariSonuIstatistik(OgrenmeClass ogrenmeClass)
        {
            using (var ctx = new IDDAA_Entities())
            {
                HelperServis helper = new HelperServis();

                var ilkYariSonuIstatistik = ctx.ILK_YARI_SONU_ISTATISTIK.FirstOrDefault(c => c.IDDAA_ID == ogrenmeClass.bultenItem.IDDAA_ID);

                ogrenmeClass.IyGenelEvSahibiGalibiyetOran = Convert.ToDecimal(ilkYariSonuIstatistik.GENEL_EV_GALIBIYET_ORAN);
                ogrenmeClass.IyGenelEvSahibiBeraberOran = Convert.ToDecimal(ilkYariSonuIstatistik.GENEL_EV_BERABER_ORAN);
                ogrenmeClass.IyGenelEvSahibiMaglubiyetOran = Convert.ToDecimal(ilkYariSonuIstatistik.GENEL_EV_MAGLUBIYET_ORAN);
                ogrenmeClass.IyGenelDeplasmanGalibiyetOran = Convert.ToDecimal(ilkYariSonuIstatistik.GENEL_DEP_GALIBIYET_ORAN);
                ogrenmeClass.IyGenelDeplasmanBeraberOran = Convert.ToDecimal(ilkYariSonuIstatistik.GENEL_DEP_BERABER_ORAN);
                ogrenmeClass.IyGenelDeplasmanMaglubiyetOran = Convert.ToDecimal(ilkYariSonuIstatistik.GENEL_DEP_MAGLUBIYET_ORAN);

                ogrenmeClass.IyIcDisEvSahibiGalibiyetOran = Convert.ToDecimal(ilkYariSonuIstatistik.IC_DIS_EV_GALIBIYET_ORAN);
                ogrenmeClass.IyIcDisEvSahibiBeraberOran = Convert.ToDecimal(ilkYariSonuIstatistik.IC_DIS_EV_BERABER_ORAN);
                ogrenmeClass.IyIcDisEvSahibiMaglubiyetOran = Convert.ToDecimal(ilkYariSonuIstatistik.IC_DIS_EV_MAGLUBIYET_ORAN);
                ogrenmeClass.IyIcDisDeplasmanGalibiyetOran = Convert.ToDecimal(ilkYariSonuIstatistik.IC_DIS_DEP_GALIBIYET_ORAN);
                ogrenmeClass.IyIcDisDeplasmanBeraberOran = Convert.ToDecimal(ilkYariSonuIstatistik.IC_DIS_DEP_BERABER_ORAN);
                ogrenmeClass.IyIcDisDeplasmanMaglubiyetOran = Convert.ToDecimal(ilkYariSonuIstatistik.IC_DIS_DEP_MAGLUBIYET_ORAN);

                ogrenmeClass.IyAralarindaEvSahibiGalibiyetOran = Convert.ToDecimal(ilkYariSonuIstatistik.ARA_EV_GALIBIYET_ORAN);
                ogrenmeClass.IyAralarindaEvSahibiBeraberOran = Convert.ToDecimal(ilkYariSonuIstatistik.ARA_EV_BERABER_ORAN);
                ogrenmeClass.IyAralarindaEvSahibiMaglubiyetOran = Convert.ToDecimal(ilkYariSonuIstatistik.ARA_EV_MAGLUBIYET_ORAN);
                ogrenmeClass.IyAralarindaDeplasmanGalibiyetOran = Convert.ToDecimal(ilkYariSonuIstatistik.ARA_DEP_GALIBIYET_ORAN);
                ogrenmeClass.IyAralarindaDeplasmanBeraberOran = Convert.ToDecimal(ilkYariSonuIstatistik.ARA_DEP_BERABER_ORAN);
                ogrenmeClass.IyAralarindaDeplasmanMaglubiyetOran = Convert.ToDecimal(ilkYariSonuIstatistik.ARA_DEP_MAGLUBIYET_ORAN);

                return ogrenmeClass;
            }
        }

        public OgrenmeClass setKgVarYokIstatistik(OgrenmeClass ogrenmeClass)
        {
            using (var ctx = new IDDAA_Entities())
            {
                var kgVarYokIstatistik = ctx.KG_VAR_YOK_ISTATISTIK.FirstOrDefault(c => c.IDDAA_ID == ogrenmeClass.bultenItem.IDDAA_ID);

                ogrenmeClass.GenelEvSahibiKgVarOran = Convert.ToDecimal(kgVarYokIstatistik.GENEL_EV_KGV_ORAN);
                ogrenmeClass.GenelEvSahibiKgYokOran = Convert.ToDecimal(kgVarYokIstatistik.GENEL_EV_KGY_ORAN);
                ogrenmeClass.IcDisEvSahibiKgVarOran = Convert.ToDecimal(kgVarYokIstatistik.IC_DIS_EV_KGV_ORAN);
                ogrenmeClass.IcDisEvSahibiKgYokOran = Convert.ToDecimal(kgVarYokIstatistik.IC_DIS_EV_KGY_ORAN);
                ogrenmeClass.AralarindaEvSahibiKgVarOran = Convert.ToDecimal(kgVarYokIstatistik.ARA_EV_KGV_ORAN);
                ogrenmeClass.AralarindaEvSahibiKgYokOran = Convert.ToDecimal(kgVarYokIstatistik.ARA_EV_KGY_ORAN);

                ogrenmeClass.GenelDeplasmanKgVarOran = Convert.ToDecimal(kgVarYokIstatistik.GENEL_DEP_KGV_ORAN);
                ogrenmeClass.GenelDeplasmanKgYokOran = Convert.ToDecimal(kgVarYokIstatistik.GENEL_DEP_KGY_ORAN);
                ogrenmeClass.IcDisDeplasmanKgVarOran = Convert.ToDecimal(kgVarYokIstatistik.IC_DIS_DEP_KGV_ORAN);
                ogrenmeClass.IcDisDeplasmanKgYokOran = Convert.ToDecimal(kgVarYokIstatistik.IC_DIS_DEP_KGY_ORAN);
                ogrenmeClass.AralarindaDeplasmanKgVarOran = Convert.ToDecimal(kgVarYokIstatistik.ARA_DEP_KGV_ORAN);
                ogrenmeClass.AralarindaDeplasmanKgYokOran = Convert.ToDecimal(kgVarYokIstatistik.ARA_DEP_KGY_ORAN);

                return ogrenmeClass;
            }
        }

        public OgrenmeClass setMs25AltUstIstatistik(OgrenmeClass ogrenmeClass)
        {
            using (var ctx = new IDDAA_Entities())
            {
                var ms25AltUstIstatistik = ctx.MS25_ALT_UST_ISTATISTIK.FirstOrDefault(c => c.IDDAA_ID == ogrenmeClass.bultenItem.IDDAA_ID);

                ogrenmeClass.GenelEvSahibi25AltOran = Convert.ToDecimal(ms25AltUstIstatistik.GENEL_EV_ALT_ORAN);
                ogrenmeClass.GenelEvSahibi25UstOran = Convert.ToDecimal(ms25AltUstIstatistik.GENEL_EV_UST_ORAN);
                ogrenmeClass.IcDisEvSahibi25AltOran = Convert.ToDecimal(ms25AltUstIstatistik.IC_DIS_EV_ALT_ORAN);
                ogrenmeClass.IcDisEvSahibi25UstOran = Convert.ToDecimal(ms25AltUstIstatistik.IC_DIS_EV_UST_ORAN);
                ogrenmeClass.AralarindaEvSahibi25AltOran = Convert.ToDecimal(ms25AltUstIstatistik.ARA_EV_ALT_ORAN);
                ogrenmeClass.AralarindaEvSahibi25UstOran = Convert.ToDecimal(ms25AltUstIstatistik.ARA_EV_UST_ORAN);

                ogrenmeClass.GenelDeplasman25AltOran = Convert.ToDecimal(ms25AltUstIstatistik.GENEL_DEP_ALT_ORAN);
                ogrenmeClass.GenelDeplasman25UstOran = Convert.ToDecimal(ms25AltUstIstatistik.GENEL_DEP_UST_ORAN);
                ogrenmeClass.IcDisDeplasman25AltOran = Convert.ToDecimal(ms25AltUstIstatistik.IC_DIS_DEP_ALT_ORAN);
                ogrenmeClass.IcDisDeplasman25UstOran = Convert.ToDecimal(ms25AltUstIstatistik.IC_DIS_DEP_UST_ORAN);
                ogrenmeClass.AralarindaDeplasman25AltOran = Convert.ToDecimal(ms25AltUstIstatistik.ARA_DEP_ALT_ORAN);
                ogrenmeClass.AralarindaDeplasman25UstOran = Convert.ToDecimal(ms25AltUstIstatistik.ARA_DEP_UST_ORAN);

                return ogrenmeClass;
            }
        }

        public OgrenmeClass setToplamGolstatistik(OgrenmeClass ogrenmeClass)
        {
            using (var ctx = new IDDAA_Entities())
            {
                var toplamGolIstatistik = ctx.TOPLAM_GOL_ISTATISTIK.FirstOrDefault(c => c.IDDAA_ID == ogrenmeClass.bultenItem.IDDAA_ID);

                ogrenmeClass.GenelEvSahibiTg01Oran = Convert.ToDecimal(toplamGolIstatistik.GENEL_EV_0_1_ORAN);
                ogrenmeClass.GenelEvSahibiTg23Oran = Convert.ToDecimal(toplamGolIstatistik.GENEL_EV_2_3_ORAN);
                ogrenmeClass.GenelEvSahibiTg46Oran = Convert.ToDecimal(toplamGolIstatistik.GENEL_EV_4_6_ORAN);
                ogrenmeClass.GenelEvSahibiTg7ArtiOran = Convert.ToDecimal(toplamGolIstatistik.GENEL_EV_7_ARTI_ORAN);
                ogrenmeClass.IcDisEvSahibiTg01Oran = Convert.ToDecimal(toplamGolIstatistik.IC_DIS_EV_0_1_ORAN);
                ogrenmeClass.IcDisEvSahibiTg23Oran = Convert.ToDecimal(toplamGolIstatistik.IC_DIS_EV_2_3_ORAN);
                ogrenmeClass.IcDisEvSahibiTg46Oran = Convert.ToDecimal(toplamGolIstatistik.IC_DIS_EV_4_6_ORAN);
                ogrenmeClass.IcDisEvSahibiTg7ArtiOran = Convert.ToDecimal(toplamGolIstatistik.IC_DIS_EV_7_ARTI_ORAN);
                ogrenmeClass.AralarindaEvSahibiTg01Oran = Convert.ToDecimal(toplamGolIstatistik.ARA_EV_0_1_ORAN);
                ogrenmeClass.AralarindaEvSahibiTg23Oran = Convert.ToDecimal(toplamGolIstatistik.ARA_EV_2_3_ORAN);
                ogrenmeClass.AralarindaEvSahibiTg46Oran = Convert.ToDecimal(toplamGolIstatistik.ARA_EV_4_6_ORAN);
                ogrenmeClass.AralarindaEvSahibiTg7ArtiOran = Convert.ToDecimal(toplamGolIstatistik.ARA_EV_7_ARTI_ORAN);

                ogrenmeClass.GenelDeplasmanTg01Oran = Convert.ToDecimal(toplamGolIstatistik.GENEL_DEP_0_1_ORAN);
                ogrenmeClass.GenelDeplasmanTg23Oran = Convert.ToDecimal(toplamGolIstatistik.GENEL_DEP_2_3_ORAN);
                ogrenmeClass.GenelDeplasmanTg46Oran = Convert.ToDecimal(toplamGolIstatistik.GENEL_DEP_4_6_ORAN);
                ogrenmeClass.GenelDeplasmanTg7ArtiOran = Convert.ToDecimal(toplamGolIstatistik.GENEL_DEP_7_ARTI_ORAN);
                ogrenmeClass.IcDisDeplasmanTg01Oran = Convert.ToDecimal(toplamGolIstatistik.IC_DIS_DEP_0_1_ORAN);
                ogrenmeClass.IcDisDeplasmanTg23Oran = Convert.ToDecimal(toplamGolIstatistik.IC_DIS_DEP_2_3_ORAN);
                ogrenmeClass.IcDisDeplasmanTg46Oran = Convert.ToDecimal(toplamGolIstatistik.IC_DIS_DEP_4_6_ORAN);
                ogrenmeClass.IcDisDeplasmanTg7ArtiOran = Convert.ToDecimal(toplamGolIstatistik.IC_DIS_DEP_7_ARTI_ORAN);
                ogrenmeClass.AralarindaDeplasmanTg01Oran = Convert.ToDecimal(toplamGolIstatistik.ARA_DEP_0_1_ORAN);
                ogrenmeClass.AralarindaDeplasmanTg23Oran = Convert.ToDecimal(toplamGolIstatistik.ARA_DEP_2_3_ORAN);
                ogrenmeClass.AralarindaDeplasmanTg46Oran = Convert.ToDecimal(toplamGolIstatistik.ARA_DEP_4_6_ORAN);
                ogrenmeClass.AralarindaDeplasmanTg7ArtiOran = Convert.ToDecimal(toplamGolIstatistik.ARA_DEP_7_ARTI_ORAN);

                return ogrenmeClass;
            }
        }

        public OgrenmeClass setPuanDurumuEvSahibi(OgrenmeClass ogrenmeClass)
        {
            using (var ctx = new IDDAA_Entities())
            {
                HelperServis helper = new HelperServis();

                var evSahibiPuanDurumu = ctx.PUAN_DURUMU.FirstOrDefault(c => c.IDDAA_ID == ogrenmeClass.bultenItem.IDDAA_ID
                    && c.TAKIM_ADI == ogrenmeClass.bultenItem.EV_SAHIBI);

                if(evSahibiPuanDurumu == null)
                {
                    return ogrenmeClass;
                }

                int genelMacSayisi = Convert.ToInt32(evSahibiPuanDurumu.GENEL_MAC_SAYISI);
                if (genelMacSayisi != 0)
                {
                    ogrenmeClass.EvSahibiGenelSira = Convert.ToInt32(evSahibiPuanDurumu.GENEL_SIRA);
                    ogrenmeClass.EvSahibiGenelGalibiyetOran = Convert.ToDecimal(evSahibiPuanDurumu.GENEL_GALIBIYET) / genelMacSayisi;
                    ogrenmeClass.EvSahibiGenelBeraberlikOran = Convert.ToDecimal(evSahibiPuanDurumu.GENEL_BERABERLIK) / genelMacSayisi;
                    ogrenmeClass.EvSahibiGenelMaglubiyetOran = Convert.ToDecimal(evSahibiPuanDurumu.GENEL_MAGLUBIYET) / genelMacSayisi;
                    ogrenmeClass.EvSahibiGenelAttigiGolOran = Convert.ToDecimal(evSahibiPuanDurumu.GENEL_ATTIGI_GOL) / genelMacSayisi;
                    ogrenmeClass.EvSahibiGenelYedigiGolOran = Convert.ToDecimal(evSahibiPuanDurumu.GENEL_YEDIGI_GOL) / genelMacSayisi;
                    ogrenmeClass.EvSahibiGenelPuanOran = Convert.ToDecimal(evSahibiPuanDurumu.GENEL_PUAN) / genelMacSayisi;
                }

                int icerdeMacSayisi = Convert.ToInt32(evSahibiPuanDurumu.EV_SAHIBI_MAC_SAYISI);
                if (icerdeMacSayisi != 0)
                {
                    ogrenmeClass.EvSahibiIcerdeSira = Convert.ToInt32(evSahibiPuanDurumu.EV_SAHIBI_SIRA);
                    ogrenmeClass.EvSahibiIcerdeGalibiyetOran = Convert.ToDecimal(evSahibiPuanDurumu.EV_SAHIBI_GALIBIYET) / icerdeMacSayisi;
                    ogrenmeClass.EvSahibiIcerdeBeraberlikOran = Convert.ToDecimal(evSahibiPuanDurumu.EV_SAHIBI_BERABERLIK) / icerdeMacSayisi;
                    ogrenmeClass.EvSahibiIcerdeMaglubiyetOran = Convert.ToDecimal(evSahibiPuanDurumu.EV_SAHIBI_MAGLUBIYET) / icerdeMacSayisi;
                    ogrenmeClass.EvSahibiIcerdeAttigiGolOran = Convert.ToDecimal(evSahibiPuanDurumu.EV_SAHIBI_ATTIGI_GOL) / icerdeMacSayisi;
                    ogrenmeClass.EvSahibiIcerdeYedigiGolOran = Convert.ToDecimal(evSahibiPuanDurumu.EV_SAHIBI_YEDIGI_GOL) / icerdeMacSayisi;
                    ogrenmeClass.EvSahibiIcerdePuanOran = Convert.ToDecimal(evSahibiPuanDurumu.EV_SAHIBI_PUAN) / icerdeMacSayisi;
                }

                int disardaMacSayisi = Convert.ToInt32(evSahibiPuanDurumu.DEPLASMAN_MAC_SAYISI);
                if (disardaMacSayisi != 0)
                {
                    ogrenmeClass.EvSahibiDisardaSira = Convert.ToInt32(evSahibiPuanDurumu.DEPLASMAN_SIRA);
                    ogrenmeClass.EvSahibiDisardaGalibiyetOran = Convert.ToDecimal(evSahibiPuanDurumu.DEPLASMAN_GALIBIYET) / disardaMacSayisi;
                    ogrenmeClass.EvSahibiDisardaBeraberlikOran = Convert.ToDecimal(evSahibiPuanDurumu.DEPLASMAN_BERABERLIK) / disardaMacSayisi;
                    ogrenmeClass.EvSahibiDisardaMaglubiyetOran = Convert.ToDecimal(evSahibiPuanDurumu.DEPLASMAN_MAGLUBIYET) / disardaMacSayisi;
                    ogrenmeClass.EvSahibiDisardaAttigiGolOran = Convert.ToDecimal(evSahibiPuanDurumu.DEPLASMAN_ATTIGI_GOL) / disardaMacSayisi;
                    ogrenmeClass.EvSahibiDisardaYedigiGolOran = Convert.ToDecimal(evSahibiPuanDurumu.DEPLASMAN_YEDIGI_GOL) / disardaMacSayisi;
                    ogrenmeClass.EvSahibiDisardaPuanOran = Convert.ToDecimal(evSahibiPuanDurumu.DEPLASMAN_PUAN) / disardaMacSayisi;
                }

                return ogrenmeClass;
            }
        }

        public OgrenmeClass setPuanDurumuDeplasman(OgrenmeClass ogrenmeClass)
        {
            using (var ctx = new IDDAA_Entities())
            {
                HelperServis helper = new HelperServis();

                var deplasmanPuanDurumu = ctx.PUAN_DURUMU.FirstOrDefault(c => c.IDDAA_ID == ogrenmeClass.bultenItem.IDDAA_ID
                    && c.TAKIM_ADI == ogrenmeClass.bultenItem.DEPLASMAN);

                if (deplasmanPuanDurumu == null)
                {
                    return ogrenmeClass;
                }

                int genelMacSayisi = Convert.ToInt32(deplasmanPuanDurumu.GENEL_MAC_SAYISI);
                if (genelMacSayisi != 0)
                {
                    ogrenmeClass.DeplasmanGenelSira = Convert.ToInt32(deplasmanPuanDurumu.GENEL_SIRA);
                    ogrenmeClass.DeplasmanGenelGalibiyetOran = Convert.ToDecimal(deplasmanPuanDurumu.GENEL_GALIBIYET) / genelMacSayisi;
                    ogrenmeClass.DeplasmanGenelBeraberlikOran = Convert.ToDecimal(deplasmanPuanDurumu.GENEL_BERABERLIK) / genelMacSayisi;
                    ogrenmeClass.DeplasmanGenelMaglubiyetOran = Convert.ToDecimal(deplasmanPuanDurumu.GENEL_MAGLUBIYET) / genelMacSayisi;
                    ogrenmeClass.DeplasmanGenelAttigiGolOran = Convert.ToDecimal(deplasmanPuanDurumu.GENEL_ATTIGI_GOL) / genelMacSayisi;
                    ogrenmeClass.DeplasmanGenelYedigiGolOran = Convert.ToDecimal(deplasmanPuanDurumu.GENEL_YEDIGI_GOL) / genelMacSayisi;
                    ogrenmeClass.DeplasmanGenelPuanOran = Convert.ToDecimal(deplasmanPuanDurumu.GENEL_PUAN) / genelMacSayisi;
                }

                int icerdeMacSayisi = Convert.ToInt32(deplasmanPuanDurumu.EV_SAHIBI_MAC_SAYISI);
                if (icerdeMacSayisi != 0)
                {
                    ogrenmeClass.DeplasmanIcerdeSira = Convert.ToInt32(deplasmanPuanDurumu.EV_SAHIBI_SIRA);
                    ogrenmeClass.DeplasmanIcerdeGalibiyetOran = Convert.ToDecimal(deplasmanPuanDurumu.EV_SAHIBI_GALIBIYET) / icerdeMacSayisi;
                    ogrenmeClass.DeplasmanIcerdeBeraberlikOran = Convert.ToDecimal(deplasmanPuanDurumu.EV_SAHIBI_BERABERLIK) / icerdeMacSayisi;
                    ogrenmeClass.DeplasmanIcerdeMaglubiyetOran = Convert.ToDecimal(deplasmanPuanDurumu.EV_SAHIBI_MAGLUBIYET) / icerdeMacSayisi;
                    ogrenmeClass.DeplasmanIcerdeAttigiGolOran = Convert.ToDecimal(deplasmanPuanDurumu.EV_SAHIBI_ATTIGI_GOL) / icerdeMacSayisi;
                    ogrenmeClass.DeplasmanIcerdeYedigiGolOran = Convert.ToDecimal(deplasmanPuanDurumu.EV_SAHIBI_YEDIGI_GOL) / icerdeMacSayisi;
                    ogrenmeClass.DeplasmanIcerdePuanOran = Convert.ToDecimal(deplasmanPuanDurumu.EV_SAHIBI_PUAN) / icerdeMacSayisi;
                }

                int disardaMacSayisi = Convert.ToInt32(deplasmanPuanDurumu.DEPLASMAN_MAC_SAYISI);
                if (disardaMacSayisi != 0)
                {
                    ogrenmeClass.DeplasmanDisardaSira = Convert.ToInt32(deplasmanPuanDurumu.DEPLASMAN_SIRA);
                    ogrenmeClass.DeplasmanDisardaGalibiyetOran = Convert.ToDecimal(deplasmanPuanDurumu.DEPLASMAN_GALIBIYET) / disardaMacSayisi;
                    ogrenmeClass.DeplasmanDisardaBeraberlikOran = Convert.ToDecimal(deplasmanPuanDurumu.DEPLASMAN_BERABERLIK) / disardaMacSayisi;
                    ogrenmeClass.DeplasmanDisardaMaglubiyetOran = Convert.ToDecimal(deplasmanPuanDurumu.DEPLASMAN_MAGLUBIYET) / disardaMacSayisi;
                    ogrenmeClass.DeplasmanDisardaAttigiGolOran = Convert.ToDecimal(deplasmanPuanDurumu.DEPLASMAN_ATTIGI_GOL) / disardaMacSayisi;
                    ogrenmeClass.DeplasmanDisardaYedigiGolOran = Convert.ToDecimal(deplasmanPuanDurumu.DEPLASMAN_YEDIGI_GOL) / disardaMacSayisi;
                    ogrenmeClass.DeplasmanDisardaPuanOran = Convert.ToDecimal(deplasmanPuanDurumu.DEPLASMAN_PUAN) / disardaMacSayisi;
                }

                return ogrenmeClass;
            }
        }

        public OgrenmeClass setTakimIstatistikEvSahibi(OgrenmeClass ogrenmeClass)
        {
            using (var ctx = new IDDAA_Entities())
            {
                HelperServis helper = new HelperServis();

                var evSahibiTakimIstatistik = ctx.TAKIM_ISTATISTIK.FirstOrDefault(c => c.IDDAA_ID == ogrenmeClass.bultenItem.IDDAA_ID
                    && c.TAKIM_ADI == ogrenmeClass.bultenItem.EV_SAHIBI);

                if(evSahibiTakimIstatistik == null)
                {
                    return ogrenmeClass;
                }

                ogrenmeClass.EvSahibiGolDenemesi = Math.Round(Convert.ToDecimal(evSahibiTakimIstatistik.GOL_DENEMESI), 0);
                ogrenmeClass.EvSahibiSutKaleyiBulan = Math.Round(Convert.ToDecimal(evSahibiTakimIstatistik.SUT_KALEYI_BULAN), 0);
                ogrenmeClass.EvSahibiSutKaleyiBulmayan = Math.Round(Convert.ToDecimal(evSahibiTakimIstatistik.SUT_KALEYI_BULMAYAN), 0);
                ogrenmeClass.EvSahibiKorner = Math.Round(Convert.ToDecimal(evSahibiTakimIstatistik.KORNER), 1);
                ogrenmeClass.EvSahibiTopaSahipOlma = Math.Round(Convert.ToDecimal(evSahibiTakimIstatistik.TOPA_SAHIP_OLMA), 0);
                ogrenmeClass.EvSahibiSutEngelleme = Math.Round(Convert.ToDecimal(evSahibiTakimIstatistik.SUT_ENGELLEME), 0);
                ogrenmeClass.EvSahibiToplamKart = Math.Round(Convert.ToDecimal(evSahibiTakimIstatistik.TOPLAM_KART), 1);
                ogrenmeClass.EvSahibiFrikik = Math.Round(Convert.ToDecimal(evSahibiTakimIstatistik.FRIKIK), 0);
                ogrenmeClass.EvSahibiSariKart = Math.Round(Convert.ToDecimal(evSahibiTakimIstatistik.SARI_KART), 1);
                ogrenmeClass.EvSahibiKirmiziKart = Math.Round(Convert.ToDecimal(evSahibiTakimIstatistik.KIRMIZI_KART), 1);
                ogrenmeClass.EvSahibiSutEtkisi = Convert.ToDecimal(evSahibiTakimIstatistik.SUT_ETKISI);

                return ogrenmeClass;
            }
        }

        public OgrenmeClass setTakimIstatistikDeplasman(OgrenmeClass ogrenmeClass)
        {
            using (var ctx = new IDDAA_Entities())
            {
                HelperServis helper = new HelperServis();

                var deplasmanTakimIstatistik = ctx.TAKIM_ISTATISTIK.FirstOrDefault(c => c.IDDAA_ID == ogrenmeClass.bultenItem.IDDAA_ID
                    && c.TAKIM_ADI == ogrenmeClass.bultenItem.DEPLASMAN);

                if (deplasmanTakimIstatistik == null)
                {
                    return ogrenmeClass;
                }

                ogrenmeClass.DeplasmanGolDenemesi = Math.Round(Convert.ToDecimal(deplasmanTakimIstatistik.GOL_DENEMESI), 0);
                ogrenmeClass.DeplasmanSutKaleyiBulan = Math.Round(Convert.ToDecimal(deplasmanTakimIstatistik.SUT_KALEYI_BULAN), 0);
                ogrenmeClass.DeplasmanSutKaleyiBulmayan = Math.Round(Convert.ToDecimal(deplasmanTakimIstatistik.SUT_KALEYI_BULMAYAN), 0);
                ogrenmeClass.DeplasmanKorner = Math.Round(Convert.ToDecimal(deplasmanTakimIstatistik.KORNER), 1);
                ogrenmeClass.DeplasmanTopaSahipOlma = Math.Round(Convert.ToDecimal(deplasmanTakimIstatistik.TOPA_SAHIP_OLMA), 0);
                ogrenmeClass.DeplasmanSutEngelleme = Math.Round(Convert.ToDecimal(deplasmanTakimIstatistik.SUT_ENGELLEME), 0);
                ogrenmeClass.DeplasmanToplamKart = Math.Round(Convert.ToDecimal(deplasmanTakimIstatistik.TOPLAM_KART), 1);
                ogrenmeClass.DeplasmanFrikik = Math.Round(Convert.ToDecimal(deplasmanTakimIstatistik.FRIKIK), 0);
                ogrenmeClass.DeplasmanSariKart = Math.Round(Convert.ToDecimal(deplasmanTakimIstatistik.SARI_KART), 1);
                ogrenmeClass.DeplasmanKirmiziKart = Math.Round(Convert.ToDecimal(deplasmanTakimIstatistik.KIRMIZI_KART), 1);
                ogrenmeClass.DeplasmanSutEtkisi = Convert.ToDecimal(deplasmanTakimIstatistik.SUT_ETKISI);

                return ogrenmeClass;
            }
        }

        public OgrenmeClass setSakatCezaliEvSahibi(OgrenmeClass ogrenmeClass)
        {
            using (var ctx = new IDDAA_Entities())
            {
                HelperServis helper = new HelperServis();

                var evSahibiSakatCezali = ctx.SAKAT_CEZALI.Where(c => c.IDDAA_ID == ogrenmeClass.bultenItem.IDDAA_ID
                    && c.TAKIM_ADI == ogrenmeClass.bultenItem.EV_SAHIBI);

                ogrenmeClass.EvSahibiSakatCezaliOran = bulSakatCezaliOran(evSahibiSakatCezali);

                return ogrenmeClass;
            }
        }

        public OgrenmeClass setSakatCezaliDeplasman(OgrenmeClass ogrenmeClass)
        {
            using (var ctx = new IDDAA_Entities())
            {
                HelperServis helper = new HelperServis();

                var deplasmanSakatCezali = ctx.SAKAT_CEZALI.Where(c => c.IDDAA_ID == ogrenmeClass.bultenItem.IDDAA_ID
                    && c.TAKIM_ADI == ogrenmeClass.bultenItem.DEPLASMAN);

                ogrenmeClass.DeplasmanSakatCezaliOran = bulSakatCezaliOran(deplasmanSakatCezali);

                return ogrenmeClass;
            }
        }

        public decimal bulSakatCezaliOran(IQueryable<SAKAT_CEZALI> lstSakatCezali)
        {
            decimal oran;
            decimal toplamOran = 0;

            foreach (var item in lstSakatCezali)
            {
                oran = Convert.ToDecimal((item.ON_BIR_MAC_SAYISI * 0.2) + ((item.MAC_SAYISI - item.ON_BIR_MAC_SAYISI) * 0.1) +
                    (item.GOL_SAYISI * 0.3));
                toplamOran += oran;
            }

            return toplamOran;
        }

        public OgrenmeClass setSonMaclarEvSahibi(OgrenmeClass ogrenmeClass)
        {
            using (var ctx = new IDDAA_Entities())
            {
                HelperServis helper = new HelperServis();

                var evSahibiSonMaclar = ctx.EV_SAHIBI_SON_MAC.Where(c => c.IDDAA_ID == ogrenmeClass.bultenItem.IDDAA_ID
                    && c.LIG == ogrenmeClass.bultenItem.LIG && c.TARIH < ogrenmeClass.bultenItem.TARIH).OrderByDescending(c => c.TARIH)
                    .Take(5);

                int galibiyetSayi = evSahibiSonMaclar.Where(c => c.SONUC == "WON").Count();
                int beraberSayi = evSahibiSonMaclar.Where(c => c.SONUC == "DRAW").Count();
                int maglubiyetSayi = evSahibiSonMaclar.Where(c => c.SONUC == "LOST").Count();
                int toplamMacSayisi = galibiyetSayi + beraberSayi + maglubiyetSayi;

                if (toplamMacSayisi != 0)
                {
                    ogrenmeClass.EvSahibiSonMacGalibiyetOran = Convert.ToDecimal(galibiyetSayi) / toplamMacSayisi;
                    ogrenmeClass.EvSahibiSonMacBeraberOran = Convert.ToDecimal(beraberSayi) / toplamMacSayisi;
                    ogrenmeClass.EvSahibiSonMacMaglubiyetOran = Convert.ToDecimal(maglubiyetSayi) / toplamMacSayisi;
                }
                else
                {
                    ogrenmeClass.EvSahibiSonMacGalibiyetOran = 0;
                    ogrenmeClass.EvSahibiSonMacBeraberOran = 0;
                    ogrenmeClass.EvSahibiSonMacMaglubiyetOran = 0;
                }

                return ogrenmeClass;
            }
        }

        public OgrenmeClass setSonMaclarDeplasman(OgrenmeClass ogrenmeClass)
        {
            using (var ctx = new IDDAA_Entities())
            {
                HelperServis helper = new HelperServis();

                var deplasmanSonMaclar = ctx.DEPLASMAN_SON_MAC.Where(c => c.IDDAA_ID == ogrenmeClass.bultenItem.IDDAA_ID
                    && c.LIG == ogrenmeClass.bultenItem.LIG && c.TARIH < ogrenmeClass.bultenItem.TARIH).OrderByDescending(c => c.TARIH)
                    .Take(5);

                int galibiyetSayi = deplasmanSonMaclar.Where(c => c.SONUC == "WON").Count();
                int beraberSayi = deplasmanSonMaclar.Where(c => c.SONUC == "DRAW").Count();
                int maglubiyetSayi = deplasmanSonMaclar.Where(c => c.SONUC == "LOST").Count();
                int toplamMacSayisi = galibiyetSayi + beraberSayi + maglubiyetSayi;

                if (toplamMacSayisi != 0)
                {
                    ogrenmeClass.DeplasmanSonMacGalibiyetOran = Convert.ToDecimal(galibiyetSayi) / toplamMacSayisi;
                    ogrenmeClass.DeplasmanSonMacBeraberOran = Convert.ToDecimal(beraberSayi) / toplamMacSayisi;
                    ogrenmeClass.DeplasmanSonMacMaglubiyetOran = Convert.ToDecimal(maglubiyetSayi) / toplamMacSayisi;
                }
                else
                {
                    ogrenmeClass.DeplasmanSonMacGalibiyetOran = 0;
                    ogrenmeClass.DeplasmanSonMacBeraberOran = 0;
                    ogrenmeClass.DeplasmanSonMacMaglubiyetOran = 0;
                }

                return ogrenmeClass;
            }
        }

        public List<OGRENME> convertOgrenmeClassToOgrenmeContext(List<OgrenmeClass> lstGenelMacDetay, string[] macSonuDahilAttribute,
            string ogrenmeTip)
        {
            ConcurrentBag<OGRENME> lstUstOgrenmeSatirGenelParalel = new ConcurrentBag<OGRENME>();
            
            Parallel.ForEach(lstGenelMacDetay, mac =>
            {
                convertOgrenmeClassToOgrenmeContextParalel(mac, lstUstOgrenmeSatirGenelParalel, macSonuDahilAttribute, ogrenmeTip);
            });

            return lstUstOgrenmeSatirGenelParalel.OrderByDescending(c => c.TARIH).ThenByDescending(c => c.IDDAA_ID).ToList();
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
               
        public List<OGRENME> setMacSonuTahmin(string[] macSonuDahilAttribute, string[] ligler, string ogrenmeTip)
        {
            HelperServis helper = new HelperServis();
            List<OgrenmeClass> lstMacDetay = new List<OgrenmeClass>();
            List<OgrenmeClass> lstGenelMacDetay = new List<OgrenmeClass>();
            OgrenmeClass ogrenmeClass;
            List<OGRENME> lstOgrenme = new List<OGRENME>();

            using (var ctx = new IDDAA_Entities())
            {
                var bulten = getirBultenData(ligler);
                foreach (var bultenItem in bulten)
                {
                    ogrenmeClass = new OgrenmeClass();
                    ogrenmeClass.bultenItem = bultenItem;
                    ogrenmeClass.Uzaklik = helper.getirUzaklik(Convert.ToInt32(bultenItem.UZAKLIK));

                    setMacSonuIstatistik(ogrenmeClass);
                    setIlkYariSonuIstatistik(ogrenmeClass);
                    setKgVarYokIstatistik(ogrenmeClass);
                    setMs25AltUstIstatistik(ogrenmeClass);
                    setToplamGolstatistik(ogrenmeClass);
                    setPuanDurumuEvSahibi(ogrenmeClass);
                    setPuanDurumuDeplasman(ogrenmeClass);
                    setTakimIstatistikEvSahibi(ogrenmeClass);
                    setTakimIstatistikDeplasman(ogrenmeClass);
                    setSakatCezaliEvSahibi(ogrenmeClass);
                    setSakatCezaliDeplasman(ogrenmeClass);
                    setSonMaclarEvSahibi(ogrenmeClass);
                    setSonMaclarDeplasman(ogrenmeClass);

                    // normalize ederken lstAltUstMac listesinin tamamina ihtiyac oldugundan ayri ayri db'ye yazma. onemli...
                    lstMacDetay.Add(ogrenmeClass);
                    if (lstMacDetay.Count == 500)
                    {
                        lstGenelMacDetay.AddRange(lstMacDetay);
                        lstMacDetay.Clear();
                    }
                }
                lstGenelMacDetay.AddRange(lstMacDetay);
                lstOgrenme.AddRange(convertOgrenmeClassToOgrenmeTahmin(lstGenelMacDetay, macSonuDahilAttribute, ogrenmeTip));

                return lstOgrenme;
            }
        }

        public List<BULTEN> getirBultenData(string[] ligler)
        {
            using (var ctx = new IDDAA_Entities())
            {
                HelperServis helper = new HelperServis();
                BultenServis bultenServis = new BultenServis();

                var bultenIddaaId = ctx.BULTEN.Where(c => ctx.PUAN_DURUMU.Any(d => c.IDDAA_ID == d.IDDAA_ID)
                    && ctx.TOPLAM_GOL_ISTATISTIK.Any(d => c.IDDAA_ID == d.IDDAA_ID)
                    && ctx.MS25_ALT_UST_ISTATISTIK.Any(d => c.IDDAA_ID == d.IDDAA_ID)
                    && ctx.KG_VAR_YOK_ISTATISTIK.Any(d => c.IDDAA_ID == d.IDDAA_ID)
                    && ctx.ILK_YARI_SONU_ISTATISTIK.Any(d => c.IDDAA_ID == d.IDDAA_ID)
                    && ctx.MAC_SONU_ISTATISTIK.Any(d => c.IDDAA_ID == d.IDDAA_ID)
                    && ctx.TAKIM_ISTATISTIK.Any(d => c.IDDAA_ID == d.IDDAA_ID)).Select(c => c.IDDAA_ID);

                var bulten = ctx.BULTEN.Where(c => bultenIddaaId.Any(d => c.IDDAA_ID == d)).ToList();

                bulten = bulten.Where(c => ligler.Contains(c.LIG) && c.TARIH > DateTime.Now).OrderBy(c => c.TARIH).ToList();

                return bulten;
            }
        }

        private List<OGRENME> convertOgrenmeClassToOgrenmeTahmin(List<OgrenmeClass> lstGenelMacDetay, string[] macSonuDahilAttribute,
            string ogrenmeTip)
        {
            List<OGRENME> lstUstOgrenmeSatir = new List<OGRENME>();
            List<OGRENME> lstUstOgrenmeSatirGenel = new List<OGRENME>();
            HelperServis helper = new HelperServis();
            StringBuilder sb = new StringBuilder();
            OGRENME ogrenme;

            foreach (var mac in lstGenelMacDetay)
            {
                ogrenme = new OGRENME();
                sb.Clear();
                sb.Append(helper.yazMacSonuFeature(mac, macSonuDahilAttribute));
                sb.Append(",");
                sb.Append("?");
                ogrenme.SONUC = sb.ToString();
                ogrenme.IDDAA_ID = Convert.ToInt32(mac.bultenItem.IDDAA_ID);
                ogrenme.TARIH = Convert.ToDateTime(mac.bultenItem.TARIH);
                ogrenme.EV_SAHIBI = mac.bultenItem.EV_SAHIBI;
                ogrenme.DEPLASMAN = mac.bultenItem.DEPLASMAN;
                ogrenme.TIPI = ogrenmeTip;
                ogrenme.LIG = mac.bultenItem.LIG;
                lstUstOgrenmeSatir.Add(ogrenme);
                if (lstUstOgrenmeSatir.Count == 500)
                {
                    lstUstOgrenmeSatirGenel.AddRange(lstUstOgrenmeSatir);
                    lstUstOgrenmeSatir.Clear();
                }
            }
            lstUstOgrenmeSatirGenel.AddRange(lstUstOgrenmeSatir);
            return lstUstOgrenmeSatirGenel;
        }

        public List<OgrenmeClass> ekleMacSonuOgrenmeButunAttributeler(string[] ligler, string[] macSonuDahilAttribute, string ogrenmeTip)
        {
            List<BULTEN> bulten = getirArsivData(ligler);

            List<OgrenmeClass> lstOgrenmeClass = setMacSonuOgrenmeKumeButunAttributeler(bulten, macSonuDahilAttribute, ogrenmeTip);

            return lstOgrenmeClass.OrderByDescending(c => c.bultenItem.TARIH).ThenByDescending(c => c.bultenItem.IDDAA_ID).ToList();
            
        }

        public List<OgrenmeClass> setMacSonuOgrenmeKumeButunAttributeler(List<BULTEN> bulten, string[] macSonuDahilAttribute, 
            string ogrenmeTip)
        {
            //HelperServis helper = new HelperServis();
            //List<OgrenmeClass> lstMacDetay = new List<OgrenmeClass>();
            //List<OgrenmeClass> lstGenelMacDetay = new List<OgrenmeClass>();
            //OgrenmeClass ogrenmeClass;
            //List<OGRENME> lstOgrenme = new List<OGRENME>();
            ConcurrentBag<OgrenmeClass> lstMacDetayParalel = new ConcurrentBag<OgrenmeClass>();

            Parallel.ForEach(bulten, bultenItem =>
            {                
                setMacSonuOgrenmeKumeButunAttributelerParalel(bultenItem, lstMacDetayParalel);
            });

            //using (var ctx = new IDDAA_Entities())
            //{
                //foreach (var bultenItem in bulten)
                //{
                //    ogrenmeClass = new OgrenmeClass();
                //    ogrenmeClass.bultenItem = bultenItem;
                //    ogrenmeClass.Uzaklik = helper.getirUzaklik(Convert.ToInt32(bultenItem.UZAKLIK));

                //    setMacSonuIstatistik(ogrenmeClass);
                //    setIlkYariSonuIstatistik(ogrenmeClass);
                //    setKgVarYokIstatistik(ogrenmeClass);
                //    setMs25AltUstIstatistik(ogrenmeClass);
                //    setToplamGolstatistik(ogrenmeClass);
                //    setPuanDurumuEvSahibi(ogrenmeClass);
                //    if (ogrenmeClass.EvSahibiGenelGalibiyetOran == 0 && ogrenmeClass.EvSahibiGenelBeraberlikOran == 0 &&
                //        ogrenmeClass.EvSahibiGenelMaglubiyetOran == 0)
                //    {
                //        continue;
                //    }
                //    setPuanDurumuDeplasman(ogrenmeClass);
                //    if (ogrenmeClass.DeplasmanGenelGalibiyetOran == 0 && ogrenmeClass.DeplasmanGenelBeraberlikOran == 0 &&
                //        ogrenmeClass.DeplasmanGenelMaglubiyetOran == 0)
                //    {
                //        continue;
                //    }
                //    setTakimIstatistikEvSahibi(ogrenmeClass);
                //    setTakimIstatistikDeplasman(ogrenmeClass);
                //    setSakatCezaliEvSahibi(ogrenmeClass);
                //    setSakatCezaliDeplasman(ogrenmeClass);
                //    setSonMaclarEvSahibi(ogrenmeClass);
                //    setSonMaclarDeplasman(ogrenmeClass);

                //    // normalize ederken lstAltUstMac listesinin tamamina ihtiyac oldugundan ayri ayri db'ye yazma. onemli...
                //    lstMacDetay.Add(ogrenmeClass);
                //    if (lstMacDetay.Count == 500)
                //    {
                //        lstGenelMacDetay.AddRange(lstMacDetay);
                //        lstMacDetay.Clear();
                //    }
                //}
                //lstGenelMacDetay.AddRange(lstMacDetay);


                //foreach (var item in lstMacDetayParalel)
                //{
                //    if (!lstGenelMacDetay.Any(c => c.bultenItem.IDDAA_ID == item.bultenItem.IDDAA_ID))
                //    {

                //    }
                //    var tt = lstGenelMacDetay.First(c => c.bultenItem.IDDAA_ID == item.bultenItem.IDDAA_ID).EvSahibiGenelAttigiGolOran;
                //    if (tt != item.EvSahibiGenelAttigiGolOran)
                //    {

                //    }
                //    var ttt = lstGenelMacDetay.First(c => c.bultenItem.IDDAA_ID == item.bultenItem.IDDAA_ID).DeplasmanIcerdeMaglubiyetOran;
                //    if (ttt != item.DeplasmanIcerdeMaglubiyetOran)
                //    {

                //    }
                //    var t = lstGenelMacDetay.First(c => c.bultenItem.IDDAA_ID == item.bultenItem.IDDAA_ID).IyAralarindaEvSahibiBeraberOran;
                //    if (t != item.IyAralarindaEvSahibiBeraberOran)
                //    {

                //    }
                //}

                return lstMacDetayParalel.ToList();
            //}
        }
        
        private void convertOgrenmeClassToOgrenmeContextParalel(OgrenmeClass mac, ConcurrentBag<OGRENME> lstUstOgrenmeSatirGenelParalel, string[] macSonuDahilAttribute,
            string ogrenmeTip)
        {
            OGRENME ogrenme = new OGRENME();
            HelperServis helper = new HelperServis();
            StringBuilder sb1 = new StringBuilder();

            sb1.Append(helper.yazMacSonuFeature(mac, macSonuDahilAttribute));
            sb1.Append(",");
            if (ogrenmeTip == sabitDeger.evSahibiSonuc)
            {
                sb1.Append(helper.convertMacSonucForEvSahibi(mac));
            }
            else if (ogrenmeTip == sabitDeger.deplasmanSonuc)
            {
                sb1.Append(helper.convertMacSonucForDeplasman(mac));
            }
            else if (ogrenmeTip == sabitDeger.altUstSonuc)
            {
                sb1.Append(helper.convertMacSonucForAltUst(mac));
            }

            ogrenme.SONUC = sb1.ToString();
            ogrenme.SONUC_REAL = sb1.ToString();

            ogrenme.IDDAA_ID = Convert.ToInt32(mac.bultenItem.IDDAA_ID);
            ogrenme.TARIH = Convert.ToDateTime(mac.bultenItem.TARIH);
            ogrenme.EV_SAHIBI = mac.bultenItem.EV_SAHIBI;
            ogrenme.DEPLASMAN = mac.bultenItem.DEPLASMAN;
            if (ogrenmeTip == sabitDeger.evSahibiSonuc || ogrenmeTip == sabitDeger.deplasmanSonuc)
            {
                ogrenme.TIPI = sabitDeger.macSonuSonuc;
            }
            else if (ogrenmeTip == sabitDeger.altUstSonuc)
            {
                ogrenme.TIPI = sabitDeger.altUstSonuc;
            }
            ogrenme.LIG = mac.bultenItem.LIG;

            lstUstOgrenmeSatirGenelParalel.Add(ogrenme);
        }

        public void setMacSonuOgrenmeKumeButunAttributelerParalel(BULTEN bultenItem, 
            ConcurrentBag<OgrenmeClass> lstMacDetayParalel)
        {
            HelperServis helper = new HelperServis();
            OgrenmeClass ogrenmeClass = new OgrenmeClass();
            ogrenmeClass.bultenItem = bultenItem;
            ogrenmeClass.Uzaklik = helper.getirUzaklik(Convert.ToInt32(bultenItem.UZAKLIK));

            setMacSonuIstatistik(ogrenmeClass);
            setIlkYariSonuIstatistik(ogrenmeClass);
            setKgVarYokIstatistik(ogrenmeClass);
            setMs25AltUstIstatistik(ogrenmeClass);
            setToplamGolstatistik(ogrenmeClass);
            setPuanDurumuEvSahibi(ogrenmeClass);
            if (ogrenmeClass.EvSahibiGenelGalibiyetOran == 0 && ogrenmeClass.EvSahibiGenelBeraberlikOran == 0 &&
                ogrenmeClass.EvSahibiGenelMaglubiyetOran == 0)
            {
                return;
            }
            setPuanDurumuDeplasman(ogrenmeClass);
            if (ogrenmeClass.DeplasmanGenelGalibiyetOran == 0 && ogrenmeClass.DeplasmanGenelBeraberlikOran == 0 &&
                ogrenmeClass.DeplasmanGenelMaglubiyetOran == 0)
            {
                return;
            }
            setTakimIstatistikEvSahibi(ogrenmeClass);
            setTakimIstatistikDeplasman(ogrenmeClass);
            setSakatCezaliEvSahibi(ogrenmeClass);
            setSakatCezaliDeplasman(ogrenmeClass);
            setSonMaclarEvSahibi(ogrenmeClass);
            setSonMaclarDeplasman(ogrenmeClass);

            lstMacDetayParalel.Add(ogrenmeClass);

        }
    }
}
