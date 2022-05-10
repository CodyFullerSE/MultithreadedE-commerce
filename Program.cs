/*Author: Cody Fuller
 *Course: CSE-445*/

using System;
using System.Threading;

namespace ClientApplication
{
    public delegate void priceCutEvent(Int32 pr);
    public delegate void orderConfirmationEvent(string name, float pr);
    class Program
    {
        static void Main(string[] args)
        {
            int N = 5;
            ComputerMaker computerMaker = new ComputerMaker();
            Thread computerMakerThread = new Thread(new ThreadStart(computerMaker.ComputerMakerFunc));

            //initialize buffer
            for (int i = 0; i < MultiCellBuffer.GetNumCells(); i++)
            {
                MultiCellBuffer.SetOneCell("", null);
            }

            computerMakerThread.Start();
            Thread[] stores = new Thread[5];
            Store storeThread = new Store();
            ComputerMaker.priceCut += new priceCutEvent(storeThread.LowerPrice);
            for (int i = 0; i < N; i++)
            {
                stores[i] = new Thread(new ThreadStart(storeThread.StoreFunc));
                stores[i].Name = "Store" + (i + 1).ToString();
                stores[i].Start();
            }

            while (computerMakerThread.IsAlive) { }
        }
    }
}
