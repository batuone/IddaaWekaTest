using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IddaaWekaTest
{
    class BultenServis
    {
        public void silBulten()
        {
            using (var context = new IDDAA_Entities())
            {
                var lstBulten = context.BULTEN;
                context.BULTEN.RemoveRange(lstBulten);
                context.SaveChanges();
            }
        }

        public void ekleBultenList(List<BULTEN> lstBultenData)
        {
            using (var context = new IDDAA_Entities())
            {
                context.BULTEN.AddRange(lstBultenData);
                context.SaveChanges();
            }
        }


    }
}
