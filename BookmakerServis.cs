using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IddaaWekaTest
{
    class BookmakerServis
    {

        public bool kontrolBookmakerOddAzaliyor(int iddaaId, string deger)
        {
            HelperServis helper = new HelperServis();

            using (var ctx = new IDDAA_Entities())
            {
                try
                {
                    var lstBookmarkSirali = ctx.BOOKMAKER_ODD.Where(c => c.IDDAA_ID == iddaaId).OrderBy(c => c.TARIH).ToList();
                    if (lstBookmarkSirali.Count == 0)
                    {
                        return false;
                    }

                    var sonBookmaker = lstBookmarkSirali.Last();
                    var ilkBookmakerMs1Ort = lstBookmarkSirali.Take(10).Select(c => c.MS_1).Average();
                    var ilkBookmakerMs2Ort = lstBookmarkSirali.Take(10).Select(c => c.MS_2).Average();
                    var sonBookmakerMs1Ort = lstBookmarkSirali.OrderByDescending(c => c.TARIH).Take(10).Select(c => c.MS_1).Average();
                    var sonBookmakerMs2Ort = lstBookmarkSirali.OrderByDescending(c => c.TARIH).Take(10).Select(c => c.MS_2).Average();

                    if (sonBookmaker.MS_1_IDDAA == 0 && sonBookmaker.MS_X_IDDAA == 0 && sonBookmaker.MS_2_IDDAA == 0)
                    {
                        for (int i = 1; i <= 5; i++)
                        {
                            var sonBookmakerIddaa = lstBookmarkSirali.OrderByDescending(c => c.TARIH).Skip(i).Take(1).First();
                            if (sonBookmaker.MS_1_IDDAA != 0 || sonBookmaker.MS_X_IDDAA != 0 || sonBookmaker.MS_2_IDDAA != 0)
                            {
                                sonBookmaker.MS_1_IDDAA = sonBookmakerIddaa.MS_1_IDDAA;
                                sonBookmaker.MS_X_IDDAA = sonBookmakerIddaa.MS_X_IDDAA;
                                sonBookmaker.MS_2_IDDAA = sonBookmakerIddaa.MS_2_IDDAA;
                                sonBookmaker.ALT_2_5_IDDAA = sonBookmakerIddaa.ALT_2_5_IDDAA;
                                sonBookmaker.UST_2_5_IDDAA = sonBookmakerIddaa.UST_2_5_IDDAA;
                                break;
                            }
                        }
                    }
                    
                    if (deger == "1")
                    {
                        var sonBookmakerMs1 = lstBookmarkSirali.OrderByDescending(c => c.TARIH).Take(30).Select(c => c.MS_1).ToList();
                        if (sonBookmakerMs1.All(c => c.Value == sonBookmaker.MS_1))
                        {
                            return false;
                        }

                        if (ilkBookmakerMs1Ort > sonBookmakerMs1Ort && ilkBookmakerMs2Ort < sonBookmakerMs2Ort)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (deger == "2")
                    {
                        var sonBookmakerMs1 = lstBookmarkSirali.OrderByDescending(c => c.TARIH).Take(30).Select(c => c.MS_2).ToList();
                        if (sonBookmakerMs1.All(c => c.Value == sonBookmaker.MS_2))
                        {
                            return false;
                        }

                        if (ilkBookmakerMs2Ort > sonBookmakerMs2Ort && ilkBookmakerMs1Ort < sonBookmakerMs1Ort)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }


                    var ilkBookmakerAltOrt = lstBookmarkSirali.Take(10).Select(c => c.ALT_2_5).Average();
                    var ilkBookmakerUstOrt = lstBookmarkSirali.Take(10).Select(c => c.UST_2_5).Average();
                    var sonBookmakerAltOrt = lstBookmarkSirali.OrderByDescending(c => c.TARIH).Take(10).Select(c => c.ALT_2_5).Average();
                    var sonBookmakerUstOrt = lstBookmarkSirali.OrderByDescending(c => c.TARIH).Take(10).Select(c => c.UST_2_5).Average();

                    if (deger == "Alt")
                    {
                        var sonBookmakerAlt = lstBookmarkSirali.OrderByDescending(c => c.TARIH).Take(30).Select(c => c.ALT_2_5).ToList();
                        if (sonBookmakerAlt.All(c => c.Value == sonBookmaker.ALT_2_5))
                        {
                            return false;
                        }

                        if (ilkBookmakerAltOrt > sonBookmakerAltOrt && ilkBookmakerUstOrt < sonBookmakerUstOrt)
                        {
                            return true;
                        }
                    }
                    else if (deger == "Ust")
                    {
                        var sonBookmakerUst = lstBookmarkSirali.OrderByDescending(c => c.TARIH).Take(30).Select(c => c.UST_2_5).ToList();
                        if (sonBookmakerUst.All(c => c.Value == sonBookmaker.UST_2_5))
                        {
                            return false;
                        }

                        if (ilkBookmakerUstOrt > sonBookmakerUstOrt && ilkBookmakerAltOrt < sonBookmakerAltOrt)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }   
            }
            return false;
        }

    }
}
