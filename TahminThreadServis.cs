using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IddaaWekaTest
{
    class TahminThreadServis
    {
        public void calistirTahmin(bool tableInsert, string[] ligler)
        {
            EvSahibiTahminWekaServisNew evSahibiTahminWekaServisNew = new EvSahibiTahminWekaServisNew();
            DeplasmanTahminWekaServisNew deplasmanTahminWekaServisNew = new DeplasmanTahminWekaServisNew();
            AltUstTahminWekaServisNew altUstTahminWekaServisNew = new AltUstTahminWekaServisNew();

            Thread myNewThread;
            List<Thread> machineThreads = new List<Thread>();

            myNewThread = new Thread(() => evSahibiTahminWekaServisNew.calistirTahmin(tableInsert, ligler));
            machineThreads.Add(myNewThread);
            myNewThread.Start();
            Thread.Sleep(3000);
            
            myNewThread = new Thread(() => deplasmanTahminWekaServisNew.calistirTahmin(tableInsert, ligler));
            machineThreads.Add(myNewThread);
            myNewThread.Start();
            Thread.Sleep(3000);

            myNewThread = new Thread(() => altUstTahminWekaServisNew.calistirTahmin(tableInsert, ligler));
            machineThreads.Add(myNewThread);
            myNewThread.Start();
            Thread.Sleep(3000);

            foreach (Thread machineThread in machineThreads)
            {
                machineThread.Join();
            }
        }
    }
}
