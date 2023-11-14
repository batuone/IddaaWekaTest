using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using Telegram.Bot;
using weka.classifiers.functions;
using weka.core;
using weka.filters.unsupervised.attribute;
using weka.filters.unsupervised.instance;
using static IddaaWekaTest.OgrenmeClass;
using static IddaaWekaTest.SabitDeger;
using Attribute = weka.core.Attribute;

namespace IddaaWekaTest
{
    class HelperServis
    {
        SabitDegerler sabitDeger = new SabitDegerler();

        public List<BULTEN> yazArsivToBulten(IQueryable<ARSIV> lstArsiv)
        {
            List<BULTEN> lstBulten = new List<BULTEN>();

            foreach (var arsivItem in lstArsiv)
            {
                lstBulten.Add(mapBultenToArsiv(arsivItem));
            }
            return lstBulten;
        }

        public BULTEN mapBultenToArsiv(ARSIV arsivItem)
        {
            BULTEN bulten = new BULTEN();
            bulten.UZAKLIK = arsivItem.UZAKLIK;
            bulten.IDDAA_ID = arsivItem.IDDAA_ID;
            bulten.BETRADAR_ID = arsivItem.BETRADAR_ID;
            bulten.MBS = arsivItem.MBS;
            bulten.ALT_1_5 = arsivItem.ALT_1_5;
            bulten.ALT_2_5 = arsivItem.ALT_2_5;
            bulten.ALT_3_5 = arsivItem.ALT_3_5;
            bulten.ALT_IY = arsivItem.ALT_IY;
            bulten.CIFT_1_2 = arsivItem.CIFT_1_2;
            bulten.CIFT_1_X = arsivItem.CIFT_1_X;
            bulten.CIFT_2_X = arsivItem.CIFT_2_X;
            bulten.DEPLASMAN = arsivItem.DEPLASMAN;
            bulten.EV_SAHIBI = arsivItem.EV_SAHIBI;
            bulten.H_1 = arsivItem.H_1;
            bulten.H_2 = arsivItem.H_2;
            bulten.H_X = arsivItem.H_X;
            bulten.IY_1 = arsivItem.IY_1;
            bulten.IY_2 = arsivItem.IY_2;
            bulten.IY_X = arsivItem.IY_X;
            bulten.KGV = arsivItem.KGV;
            bulten.KGY = arsivItem.KGY;
            bulten.LIG = arsivItem.LIG;
            bulten.MS_1 = arsivItem.MS_1;
            bulten.MS_2 = arsivItem.MS_2;
            bulten.MS_X = arsivItem.MS_X;
            bulten.TARIH = arsivItem.TARIH;
            bulten.TG_0_1 = arsivItem.TG_0_1;
            bulten.TG_2_3 = arsivItem.TG_2_3;
            bulten.TG_4_6 = arsivItem.TG_4_6;
            bulten.TG_7_ARTI = arsivItem.TG_7_ARTI;
            bulten.UST_1_5 = arsivItem.UST_1_5;
            bulten.UST_2_5 = arsivItem.UST_2_5;
            bulten.UST_3_5 = arsivItem.UST_3_5;
            bulten.UST_IY = arsivItem.UST_IY;

            return bulten;
        }

        public Decimal getirUzaklik(int uzaklik)
        {
            return Convert.ToDecimal(Math.Round((Convert.ToDouble(uzaklik) / 50), 0) * 50);
        }

        public List<OgrenmeClass> cevirDegerMinMaxOto(List<OgrenmeClass> lstGenelMacDetay, string[] dahilAttribute)
        {
            double reachMin = 0; double reachMax = 0; int yuzdeYaklasimOran = Convert.ToInt32(lstGenelMacDetay.Count * 0.005); int tempCount = 0;

            var classItems = lstGenelMacDetay.ToList().Select(c => c.GetType().GetProperties()).First().OrderBy(c => c.Name);

            foreach (var item in classItems)
            {
                if (!dahilAttribute.Any(c => c == item.Name))
                {
                    continue;
                }

                reachMin = 0; reachMax = 0; tempCount = 0;

                var lstOran = lstGenelMacDetay.ToList().Select(c => c.GetType().GetProperty(item.Name).GetValue(c)).ToList();

                var numberGroups = lstOran.GroupBy(x => x)
                    .Select(grp => new
                    {
                        oran = grp.Key,
                        adet = grp.Count()
                    }).ToArray();

                foreach (var keyValue in numberGroups.OrderByDescending(c => c.oran))
                {
                    tempCount += keyValue.adet;
                    if (tempCount >= yuzdeYaklasimOran)
                    {
                        reachMax = Convert.ToDouble(keyValue.oran);
                        break;
                    }
                }

                tempCount = 0;
                foreach (var keyValue in numberGroups.OrderBy(c => c.oran))
                {
                    tempCount += keyValue.adet;
                    if (tempCount >= yuzdeYaklasimOran)
                    {
                        reachMin = Convert.ToDouble(keyValue.oran);
                        break;
                    }
                }

                foreach (var mac in lstGenelMacDetay)
                {
                    if (Convert.ToDouble(mac.GetType().GetProperty(item.Name).GetValue(mac)) >= reachMax)
                    {
                        mac.GetType().GetProperty(item.Name).SetValue(mac, Convert.ChangeType(reachMax, mac.GetType().GetProperty(item.Name).PropertyType));
                    }
                    else if (Convert.ToDouble(mac.GetType().GetProperty(item.Name).GetValue(mac)) <= reachMin)
                    {
                        mac.GetType().GetProperty(item.Name).SetValue(mac, Convert.ChangeType(reachMin, mac.GetType().GetProperty(item.Name).PropertyType));
                    }
                }
            }
            return lstGenelMacDetay;
        }

        public String yazMacSonuFeature(OgrenmeClass mac, string[] dahilAttribute)
        {
            StringBuilder sb = new StringBuilder();

            int count = 0;
            double normalizeSonuc = 0;
            var classItems = mac.GetType().GetProperties().OrderBy(c => c.Name);
            foreach (var item in classItems)
            {
                if (!dahilAttribute.Any(c => c == item.Name))
                {
                    continue;
                }

                normalizeSonuc = Convert.ToDouble(mac.GetType().GetProperty(item.Name).GetValue(mac));

                if (sb.Length != 0)
                {
                    sb.Append("," + normalizeSonuc);
                }
                else
                {
                    sb.Append(normalizeSonuc);
                }
                count++;
            }

            return sb.ToString();
        }

        public String convertMacSonucForEvSahibi(OgrenmeClass mac)
        {
            using (var ctx = new IDDAA_Entities())
            {
                string macSonucu = ctx.ARSIV.First(c => c.IDDAA_ID == mac.bultenItem.IDDAA_ID).SKOR_MS;
                string[] macSkoru = macSonucu.Split('-');
                int evSahibiGolSayisi = Convert.ToInt32(macSkoru.First());
                int deplasmanGolSayisi = Convert.ToInt32(macSkoru.Last());

                if (evSahibiGolSayisi > deplasmanGolSayisi)
                {
                    return "E";
                }
                else
                {
                    return "D";
                }
            }
        }
        
        public String convertMacSonucForDeplasman(OgrenmeClass mac)
        {
            using (var ctx = new IDDAA_Entities())
            {
                string macSonucu = ctx.ARSIV.First(c => c.IDDAA_ID == mac.bultenItem.IDDAA_ID).SKOR_MS;
                string[] macSkoru = macSonucu.Split('-');
                int evSahibiGolSayisi = Convert.ToInt32(macSkoru.First());
                int deplasmanGolSayisi = Convert.ToInt32(macSkoru.Last());

                if (deplasmanGolSayisi > evSahibiGolSayisi)
                {
                    return "D";
                }
                else
                {
                    return "E";
                }
            }
        }

        public String convertMacSonucForAltUst(OgrenmeClass mac)
        {
            using (var ctx = new IDDAA_Entities())
            {
                string macSonucu = ctx.ARSIV.First(c => c.IDDAA_ID == mac.bultenItem.IDDAA_ID).SKOR_MS;
                string[] macSkoru = macSonucu.Split('-');
                int evSahibiGolSayisi = Convert.ToInt32(macSkoru.First());
                int deplasmanGolSayisi = Convert.ToInt32(macSkoru.Last());

                if ((deplasmanGolSayisi + evSahibiGolSayisi) > 2)
                {
                    return "Ust";
                }
                else
                {
                    return "Alt";
                }
            }
        }

        public DataTable ToDataTable<T>(List<T> items, string[] dahilAttribute)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            int count = 0;
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                if (!dahilAttribute.Any(c => c == prop.Name))
                {
                    continue;
                }
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[dahilAttribute.Length];
                count = 0;
                foreach (var prop in Props)
                {
                    if (!dahilAttribute.Any(c => c == prop.Name))
                    {
                        continue;
                    }
                    values[count] = prop.GetValue(item, null);
                    count++;
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public List<String> useWekaStrategy(List<String> linesOgrenme)
        {
            Instances inst = convertFromListStringToIntances(linesOgrenme);
            inst = normalization(inst);
            inst = removeMisclassify(inst);
            List<String> vvv =  convertFromIntancesToListString(inst);
            return vvv;
        }

        public Instances convertFromListStringToIntances(List<String> linesOgrenme)
        {
            string[] kolon = linesOgrenme.First().Split(',');
            
            //set attributes
            java.util.ArrayList lstAttribute = new java.util.ArrayList();            
            for (int i = 0; i < kolon.Count()-1; i++)
            {
                Attribute attr = new Attribute(i.ToString());
                lstAttribute.add(attr);
            }

            Attribute sonuc = sabitDeger.attributeSonuc();
            lstAttribute.add(sonuc);
            Instances dataset = new Instances("column", lstAttribute, 0);
            
            //set instance
            for (int i = 0; i < linesOgrenme.Count; i++)
            {
                string[] data = linesOgrenme.ElementAt(i).Split(',');

                Instance inst = new DenseInstance(data.Count());
                for (int x = 0; x < data.Count()-1; x++)
                {
                    inst.setValue(x, Convert.ToDouble(data[x]));
                }
                if(data.Last() != "?")
                {
                    inst.setValue(sonuc, data.Last());
                }
                
                dataset.add(inst);
            }
            
            dataset.setClassIndex(dataset.numAttributes() - 1);
            return dataset;
        }

        public Instances convertFromListStringToIntancesAltUst(List<String> linesOgrenme)
        {
            string[] kolon = linesOgrenme.First().Split(',');

            //set attributes
            java.util.ArrayList lstAttribute = new java.util.ArrayList();
            for (int i = 0; i < kolon.Count() - 1; i++)
            {
                Attribute attr = new Attribute(i.ToString());
                lstAttribute.add(attr);
            }

            Attribute sonuc = sabitDeger.attributeSonucAltUst();
            lstAttribute.add(sonuc);
            Instances dataset = new Instances("column", lstAttribute, 0);

            //set instance
            for (int i = 0; i < linesOgrenme.Count; i++)
            {
                string[] data = linesOgrenme.ElementAt(i).Split(',');

                Instance inst = new DenseInstance(data.Count());
                for (int x = 0; x < data.Count() - 1; x++)
                {
                    inst.setValue(x, Convert.ToDouble(data[x]));
                }
                if (data.Last() != "?")
                {
                    inst.setValue(sonuc, data.Last());
                }

                dataset.add(inst);
            }

            dataset.setClassIndex(dataset.numAttributes() - 1);
            return dataset;
        }

        public Instances normalization(Instances dataset)
        {
            Normalize normalize = new Normalize();

            normalize.setScale(1.0);
            normalize.setTranslation(0.0);

            normalize.setInputFormat(dataset);
            return weka.filters.Filter.useFilter(dataset, normalize);
        }

        public Instances removeMisclassify(Instances dataset)
        {
            RemoveMisclassified removeMisclassify = new RemoveMisclassified();
            Logistic logistic = new Logistic();
            logistic.setMaxIts(-1);
            logistic.setRidge(1.0E-8);
            logistic.setUseConjugateGradientDescent(false);
            logistic.setDebug(false);

            removeMisclassify.setClassIndex(-1);
            removeMisclassify.setInvert(false);
            removeMisclassify.setMaxIterations(0);
            removeMisclassify.setNumFolds(0);
            removeMisclassify.setThreshold(0.1);
            removeMisclassify.setClassifier(logistic);
            
            removeMisclassify.setInputFormat(dataset);
            return weka.filters.Filter.useFilter(dataset, removeMisclassify);
        }
        
        public Instances discretize(Instances dataset)
        {
            Discretize discretize = new Discretize();
            discretize.setInputFormat(dataset);

            return weka.filters.Filter.useFilter(dataset, discretize);
        }

        public List<String> convertFromIntancesToListString(Instances dataset)
        {
            List<string> linesOgrenme = new List<string>();
            for (int i = 0; i < dataset.numInstances(); i++)
            {
                linesOgrenme.Add(dataset.instance(i).ToString());
            }
            return linesOgrenme;
        }

        public StringBuilder yazSonuc(StringBuilder sb, List<CalistirTestSonuc> calistirTestSonuc)
        {
            for (int i = 0; i < calistirTestSonuc.Count; i++)
            {
                sb.Append(calistirTestSonuc[i].lig);
                sb.Append(";");
                sb.Append(calistirTestSonuc[i].macTip);
                sb.Append(";");
                sb.Append(calistirTestSonuc[i].wekaTip);
                sb.Append(";");
                sb.Append(calistirTestSonuc[i].Kar);
                if(i != (calistirTestSonuc.Count - 1))
                {
                    sb.Append(System.Environment.NewLine);
                }
            }
            return sb;
        }

        public void yazSonucToFile(String not)
        {
            //string dosya_yolu = @"C:\\Users\\batuh\\Desktop\\Iddaa\\versiyon3\\wekaML.txt";
            //System.IO.FileStream fs = new FileStream(dosya_yolu, FileMode.Truncate, FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs);
            //sw.WriteLine(not);
            //sw.Flush();
            //sw.Close();
            //fs.Close();
        }

        public void yazSonucWekaTestToFile(String not)
        {
            // C:\\Users\\Administrator\\Desktop\\wekaML.txt
            // C:\\Users\\batuh\\Desktop\\wekaML.txt

            string dosya_yolu = @"C:\\Users\\Administrator\\Desktop\\wekaML.txt"; 
            System.IO.FileStream fs = new FileStream(dosya_yolu, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(not);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public decimal bulMacSonuOran(String evSahibi, String deplasman, DateTime tarih, String predict)
        {
            using (var ctx = new IDDAA_Entities())
            {
                var bultenDusukOran = ctx.BULTEN.FirstOrDefault(c => c.EV_SAHIBI == evSahibi && c.DEPLASMAN == deplasman && c.TARIH == tarih);
                return Convert.ToDecimal(predict == "E" ? bultenDusukOran.MS_1 : (predict == "B" ? bultenDusukOran.MS_X : (predict == "D" ? bultenDusukOran.MS_2 : 0)));
            }
        }

        public decimal bulAltUstOran(String evSahibi, String deplasman, DateTime tarih, String predict)
        {
            using (var ctx = new IDDAA_Entities())
            {
                var bultenDusukOran = ctx.BULTEN.FirstOrDefault(c => c.EV_SAHIBI == evSahibi && c.DEPLASMAN == deplasman && c.TARIH == tarih);
                return Convert.ToDecimal(predict == "Alt" ? bultenDusukOran.ALT_2_5 : bultenDusukOran.UST_2_5);
            }
        }
        
        public void sendMail(string sonucMsEvSahibi, string[] lig)
        {
            try
            {
                SmtpClient sc = new SmtpClient();
                sc.Port = 587;
                sc.Host = "smtp.gmail.com";
                sc.EnableSsl = true;
                sc.Credentials = new NetworkCredential("iddaakuponbatuhan@gmail.com", "iddaakupon+64+64");
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("iddaakuponbatuhan@gmail.com", "iddaakuponbatuhan@gmail.com");
                mail.To.Add("batuhanergen@yahoo.com");
                //mail.To.Add("aliburakergen@gmail.com");
                mail.Subject = lig[0] + " - Iddaa Kupon";
                mail.IsBodyHtml = false;

                StringBuilder sb = new StringBuilder();
                sb.Append(sonucMsEvSahibi);
                //sb.Append(System.Environment.NewLine);
                //sb.Append(System.Environment.NewLine);
                //sb.Append(sonucMs25Ust);
                //sb.Append(System.Environment.NewLine);
                //sb.Append(System.Environment.NewLine);
                string mailBody = sb.ToString();
                if (mailBody.Length > 25)
                {
                    mail.Body = mailBody;
                }
                else
                {
                    mail.Body = "Mac Bulunamadi..";
                }
                sc.Send(mail);
            }
            catch (Exception ep)
            {

            }
        }
        
        public void sendTelegramMesaj(string message)
        {
            if (message.Length == 0 || message.Replace(System.Environment.NewLine, "").Length == 0)
            {
                return;
            }
            //string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}";
            string apiToken = "5184555832:AAHros8ojcNrmlHdzwTZsJnW--YTFIUgleg";
            string chatId = "-713281114";
            string text = message;
            
            TelegramBotClient bot = new TelegramBotClient(apiToken);
            bot.SendTextMessageAsync(chatId, text);
            Thread.Sleep(2000);
        }
    
        public void sendTelegramKarTutar(string[] ligler, decimal karTutar, string tip, int attributeKey)
        {
            string message = "";
            if (ligler.Count() > 1)
            {
                message = ligler[0] + "-" + ligler[1] + " - " + tip + " Kar: " + karTutar + " Key: " + attributeKey;
            }
            else
            {
                message = ligler[0] + " - " + tip + " Kar: " + karTutar + " Key: " + attributeKey;
            }            

            sendTelegramMesaj(message);
        }

        public void sendTelegramLigBitti(string[] ligler)
        {
            string mesaj = "Bitti";
            string message = "";
            if (ligler.Count() > 1)
            {
                message = ligler[0] + "-" + ligler[1] + " - " + mesaj;
            }
            else
            {
                message = ligler[0] + " - " + mesaj;
            }

            sendTelegramMesaj(message);
        }

        public void sendTelegramLigBasladi(string[] ligler)
        {
            string mesaj = "Basladı";
            string message = "";
            if (ligler.Count() > 1)
            {
                message = ligler[0] + "-" + ligler[1] + " - " + mesaj;
            }
            else
            {
                message = ligler[0] + " - " + mesaj;
            }

            sendTelegramMesaj(message);

        }

        public void sendTelegramSecilenMac(List<Sonuc> lstSonucGenel)
        {
            try
            {
                if (lstSonucGenel.Count == 0)
                {
                    return;
                }
                StringBuilder sb = new StringBuilder();
                string deger;
                foreach (var item in lstSonucGenel)
                {
                    if (item.tip == "MacSonuSonuc")
                    {
                        deger = item.deger == "1" ? "EvSahibi" : "Deplasman";
                    }
                    else
                    {
                        deger = item.deger;
                    }
                    sb.Append(item.Tarih.ToString("HH:mm") + " - ");
                    sb.Append(item.EvSahibi + "-" + item.Deplasman + " - " + deger + " - " + item.lig);
                    sb.Append(System.Environment.NewLine);
                }

                sendTelegramMesaj(sb.ToString());
            }
            catch (Exception)
            {
            }
        }

        public bool evSahibiMacSonuBasarili(string macSonucu)
        {
            string[] macSkoru = macSonucu.Split('-');
            int evSahibiGolSayisi = Convert.ToInt32(macSkoru.First());
            int deplasmanGolSayisi = Convert.ToInt32(macSkoru.Last());
            if (evSahibiGolSayisi > deplasmanGolSayisi)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool deplasmanMacSonuBasarili(string macSonucu)
        {
            string[] macSkoru = macSonucu.Split('-');
            int evSahibiGolSayisi = Convert.ToInt32(macSkoru.First());
            int deplasmanGolSayisi = Convert.ToInt32(macSkoru.Last());
            if (evSahibiGolSayisi < deplasmanGolSayisi)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ustMacSonuBasarili(string macSonucu)
        {
            string[] macSkoru = macSonucu.Split('-');
            int evSahibiGolSayisi = Convert.ToInt32(macSkoru.First());
            int deplasmanGolSayisi = Convert.ToInt32(macSkoru.Last());
            if (evSahibiGolSayisi + deplasmanGolSayisi > 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public double macProbability2(string macTuru, string[] ligPair)
        {
            using (var ctx = new IDDAA_Entities())
            {
                if (macTuru == "EvSahibi")
                {
                    macTuru = "1";
                }
                else if ((macTuru == "Deplasman"))
                {
                    macTuru = "2";
                }

                PARAMETRE param;
                List<double> oran = new List<double>();
                foreach (var lig in ligPair)
                {
                    if (ctx.PARAMETRE.Any(c => c.AD_EK == macTuru && c.AD == lig))
                    {
                        param = ctx.PARAMETRE.First(c => c.AD_EK == macTuru && c.AD == lig);
                    }
                    else
                    {
                        param = ctx.PARAMETRE.First(c => c.AD_EK == macTuru && c.AD == "DEFAULT");
                    }
                    oran.Add(Convert.ToDouble(param.DEGER));
                }
                return oran.Max() / 100;
            }
        }

        public double macProbability(string macTuru, string[] ligPair)
        {
            BookmakerServis bookmakerServis = new BookmakerServis();
            TahminProbabilityServis probabilityServis = new TahminProbabilityServis();

            if (macTuru == "EvSahibi")
            {
                macTuru = "1";
            }
            else if((macTuru == "Deplasman"))
            {
                macTuru = "2";
            }

            using (var ctx = new IDDAA_Entities())
            {
                List<MacHesaplamaYuzde> macYuzdeList = new List<MacHesaplamaYuzde>();

                var yuzdeList = ctx.TAHMIN_ML_HEPSI.
                    Where(c => c.DEGER == macTuru && ligPair.Contains(c.LIG) && c.ML_TAHMIN_YUZDE > 50).ToList();
                yuzdeList = yuzdeList.GroupBy(c=> c.IDDAA_ID).Select(c=>c.First()).ToList();

                foreach (var mac in yuzdeList)
                {
                    if (!ctx.ARSIV.Any(c => c.IDDAA_ID == mac.IDDAA_ID))
                    {
                        continue;
                    }

                    bool bookmakerKontrol = bookmakerServis.kontrolBookmakerOddAzaliyor(mac.IDDAA_ID, mac.DEGER);
                    if (!bookmakerKontrol)
                    {
                        continue;
                    }

                    MacHesaplamaYuzde yuzdeItem = new MacHesaplamaYuzde();
                    yuzdeItem.iddaaId = mac.IDDAA_ID;
                    yuzdeItem.msSkor = ctx.ARSIV.Where(c => c.IDDAA_ID == mac.IDDAA_ID).First().SKOR_MS;
                    yuzdeItem.mlTahminYuzde = mac.ML_TAHMIN_YUZDE;
                    yuzdeItem.iddaaOran = mac.IDDAA_ORAN;

                    macYuzdeList.Add(yuzdeItem);
                }

                decimal minMlYuzde = bulMinMlYuzde(macYuzdeList, macTuru);
                double oran = hesaplaKarOran(macYuzdeList, macTuru, minMlYuzde) / 100;
                
                probabilityServis.ekleProbability(macTuru, oran, ligPair);
                
                return oran;
            }
        }
    
        private double hesaplaKarOran(List<MacHesaplamaYuzde> macYuzdeList, string macTuru, decimal minMlYuzde)
        {
            macYuzdeList = macYuzdeList.Where(c => c.mlTahminYuzde > minMlYuzde).OrderByDescending(c => c.mlTahminYuzde).ToList();

            using (var ctx = new IDDAA_Entities())
            {
                if (macYuzdeList.Count() < 3)
                {
                    var param = ctx.PARAMETRE.First(c => c.AD_EK == macTuru && c.AD == "DEFAULT");
                    return Convert.ToDouble(param.DEGER);
                }
            }
            Dictionary<int, decimal> yuzdeKarMap = new Dictionary<int, decimal>();
            decimal basari; decimal basarisiz; decimal oran;            

            foreach (var item in macYuzdeList)
            {
                var lstHesapKume = macYuzdeList.Where(c => c.mlTahminYuzde >= item.mlTahminYuzde);
                basari = 0; basarisiz = 0;
                foreach (var mac in lstHesapKume.OrderByDescending(c => c.mlTahminYuzde))
                {
                    if ((macTuru == "1" && evSahibiMacSonuBasarili(mac.msSkor)) ||
                        (macTuru == "2" && deplasmanMacSonuBasarili(mac.msSkor)) ||
                        (macTuru == "Ust" && ustMacSonuBasarili(mac.msSkor)))
                    {
                        basari++;
                    }
                    else
                    {
                        basarisiz++;
                    }
                }

                oran = Math.Round((basari / (basari + basarisiz)), 4);
                yuzdeKarMap.Add(item.iddaaId, oran);
            }

            var maxOran = yuzdeKarMap.OrderByDescending(c => c.Value).ToDictionary(x => x.Key, x => x.Value).First().Value;
            if(maxOran < Convert.ToDecimal(0.75))
            {
                //return Convert.ToDouble(macYuzdeList.First().mlTahminYuzde);//default deger paramtreden
                using (var ctx = new IDDAA_Entities())
                {
                    var param = ctx.PARAMETRE.First(c => c.AD_EK == macTuru && c.AD == "DEFAULT");
                    double paramOran = Convert.ToDouble(param.DEGER);
                    double maxMlYuzde = Convert.ToDouble(macYuzdeList.First().mlTahminYuzde);

                    return paramOran > maxMlYuzde ? paramOran : maxMlYuzde;
                }
            }

            decimal resultIddaaId = 0;
            for (int i = yuzdeKarMap.Count()-1; i >= 0; i--)
            {
                if (yuzdeKarMap.ElementAt(i).Value >= Convert.ToDecimal(0.75))
                {
                    resultIddaaId = yuzdeKarMap.ElementAt(i).Key;
                    break;
                }
            }
            
            if(resultIddaaId == 0)
            {
                using (var ctx = new IDDAA_Entities())
                {
                    var param = ctx.PARAMETRE.First(c => c.AD_EK == macTuru && c.AD == "DEFAULT");
                    return Convert.ToDouble(param.DEGER);
                }
            }

            string skorMac = macYuzdeList.First(c => c.iddaaId == resultIddaaId).msSkor;
            decimal mlTahminYuzde = macYuzdeList.First(c => c.iddaaId == resultIddaaId).mlTahminYuzde;

            if ((macTuru == "1" && evSahibiMacSonuBasarili(skorMac)) ||
            (macTuru == "2" && deplasmanMacSonuBasarili(skorMac)) ||
            (macTuru == "Ust" && ustMacSonuBasarili(skorMac)))
            {
                if (macYuzdeList.Any(c => c.mlTahminYuzde < mlTahminYuzde))
                {
                    return Convert.ToDouble(macYuzdeList.Where(c => c.mlTahminYuzde < mlTahminYuzde)
                        .OrderByDescending(c => c.mlTahminYuzde).First().mlTahminYuzde);
                }
                else
                {
                    return Convert.ToDouble(mlTahminYuzde);
                }
            }
            else
            {
                return Convert.ToDouble(mlTahminYuzde);
            }
        }

        private decimal bulMinMlYuzde(List<MacHesaplamaYuzde> macYuzdeList, string macTuru)
        {
            Dictionary<int, decimal> yuzdeKarMap = new Dictionary<int, decimal>();
            decimal basari =0; decimal basarisiz =0; bool basarisizMacVar = false; decimal oran;
            decimal minMlYuzde = 0;

            macYuzdeList = macYuzdeList.OrderBy(c => c.mlTahminYuzde).ToList();

            for (int i = 0; i < macYuzdeList.Count; i++)
            {
                if ((macTuru == "1" && evSahibiMacSonuBasarili(macYuzdeList[i].msSkor)) ||
                    (macTuru == "2" && deplasmanMacSonuBasarili(macYuzdeList[i].msSkor)) ||
                    (macTuru == "Ust" && ustMacSonuBasarili(macYuzdeList[i].msSkor)))
                {
                    if (basarisizMacVar)
                    {
                        basarisizMacVar = false;
                        oran = basari / (basari + basarisiz);
                        if (oran < Convert.ToDecimal(0.70))
                        {
                            minMlYuzde = macYuzdeList[i-1].mlTahminYuzde;
                        }
                        else
                        {
                            break;
                        }
                        basari = 0; basarisiz = 0;
                    }
                    
                    basari++;
                }
                else
                {
                    basarisizMacVar = true;
                    basarisiz++;
                }
            }

            return minMlYuzde;
        }
    }
}
