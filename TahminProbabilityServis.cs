using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IddaaWekaTest
{
    class TahminProbabilityServis
    {
        public void ekleProbability(string deger, double oran, string[] ligPair)
        {
            using (var context = new IDDAA_Entities())
            {
                foreach (var lig in ligPair)
                {
                    if(context.TAHMIN_PROBABILITY.Any(c=> EntityFunctions.TruncateTime(c.TARIH) == DateTime.Today
                        && c.LIG == lig && c.DEGER == deger))
                    {
                        continue;
                    }
                    TAHMIN_PROBABILITY prob = new TAHMIN_PROBABILITY();
                    prob.TARIH = DateTime.Now;
                    prob.DEGER = deger;
                    prob.PROB_ORAN = Convert.ToDecimal(oran);
                    prob.LIG = lig;

                    context.TAHMIN_PROBABILITY.Add(prob);
                    context.SaveChanges();
                }
            }
        }
    }
}
