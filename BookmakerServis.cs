
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
                    var ilkBookmakerMs1Ort = lstBookmarkSirali.Take(15).Select(c => c.MS_1).Average();
                    var ilkBookmakerMs2Ort = lstBookmarkSirali.Take(15).Select(c => c.MS_2).Average();
                    var sonBookmakerMs1Ort = lstBookmarkSirali.OrderByDescending(c => c.TARIH).Take(15).Select(c => c.MS_1).Average();
                    var sonBookmakerMs2Ort = lstBookmarkSirali.OrderByDescending(c => c.TARIH).Take(15).Select(c => c.MS_2).Average();

                    if (deger == "1")
                    {
                        //var sonBookmakerMs1 = lstBookmarkSirali.Select(c => c.MS_1).ToList().GroupBy(c => c.Value).Count();
                        //if (sonBookmakerMs1 < 3)
                        //{
                        //    return false;
                        //}

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
                        //var sonBookmakerMs1 = lstBookmarkSirali.Select(c => c.MS_2).ToList().GroupBy(c => c.Value).Count();
                        //if (sonBookmakerMs1 < 3)
                        //{
                        //    return false;
                        //}

                        if (ilkBookmakerMs2Ort > sonBookmakerMs2Ort && ilkBookmakerMs1Ort < sonBookmakerMs1Ort)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }


                    var ilkBookmakerAltOrt = lstBookmarkSirali.Take(15).Select(c => c.ALT_2_5).Average();
                    var ilkBookmakerUstOrt = lstBookmarkSirali.Take(15).Select(c => c.UST_2_5).Average();
                    var sonBookmakerAltOrt = lstBookmarkSirali.OrderByDescending(c => c.TARIH).Take(15).Select(c => c.ALT_2_5).Average();
                    var sonBookmakerUstOrt = lstBookmarkSirali.OrderByDescending(c => c.TARIH).Take(15).Select(c => c.UST_2_5).Average();

                    if (deger == "Alt")
                    {
                        //var sonBookmakerAlt = lstBookmarkSirali.Select(c => c.ALT_2_5).ToList().GroupBy(c => c.Value).Count();
                        //if (sonBookmakerAlt < 3)
                        //{
                        //    return false;
                        //}

                        if (ilkBookmakerAltOrt > sonBookmakerAltOrt && ilkBookmakerUstOrt < sonBookmakerUstOrt)
                        {
                            return true;
                        }
                    }
                    else if (deger == "Ust")
                    {
                        //var sonBookmakerUst = lstBookmarkSirali.Select(c => c.UST_2_5).ToList().GroupBy(c => c.Value).Count();
                        //if (sonBookmakerUst < 3)
                        //{
                        //    return false;
                        //}

                        if (ilkBookmakerUstOrt > sonBookmakerUstOrt && ilkBookmakerAltOrt < sonBookmakerAltOrt)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    var ilkBookmakerYari1Ort = lstBookmarkSirali.Take(15).Select(c => c.YARI_1).Average();
                    var ilkBookmakerYari2Ort = lstBookmarkSirali.Take(15).Select(c => c.YARI_2).Average();
                    var sonBookmakerYari1Ort = lstBookmarkSirali.OrderByDescending(c => c.TARIH).Take(15).Select(c => c.YARI_1).Average();
                    var sonBookmakerYari2Ort = lstBookmarkSirali.OrderByDescending(c => c.TARIH).Take(15).Select(c => c.YARI_2).Average();


                    if (deger == "Yari2")
                    {
                        //var sonBookmakerYari2 = lstBookmarkSirali.Select(c => c.YARI_2).ToList().GroupBy(c => c.Value).Count();
                        //if (sonBookmakerYari2 < 2)
                        //{
                        //    return false;
                        //}

                        if (ilkBookmakerYari2Ort > sonBookmakerYari2Ort && ilkBookmakerYari1Ort < sonBookmakerYari1Ort)
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
