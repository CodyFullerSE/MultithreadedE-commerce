/*Author: Cody Fuller
 *Course: CSE-445*/


using System;
using System.Threading;

namespace ClientApplication
{
    class ComputerMaker
    {
        public static bool isRunning;
        private static Int32 p = 0;
        private static Int32 computerPrice = 1500;
        public static event priceCutEvent priceCut;
        static Random random = new Random();
        ReaderWriterLock rwlock = new ReaderWriterLock();
        private static Int32 previousPrice = 1500;

        // Saves previous price, calculates computer price, gets order, and 
        // sends order to be processed by orderProcessThread.
        public void ComputerMakerFunc()
        {
            isRunning = true;
            // Runs until 10 price cuts are made and then terminates thread.
            while (p < 10)
            {
                Thread.Sleep(500);
                previousPrice = computerPrice;
                PricingModel();
                string order = MultiCellBuffer.GetOneCell();
                EventClass eventClass = MultiCellBuffer.GetOneEventCell();
                if (order != null && order != "")
                {
                    OrderClass orderObj = Decoder(order);
                    OrderProcessing orderProcessing = new OrderProcessing();
                    orderProcessing.SetOrderObj(orderObj);
                    orderProcessing.SetEvent(eventClass);
                    Thread orderProcessThread = new Thread(new ThreadStart(orderProcessing.OrderProcessingFunc));
                    orderProcessThread.Start();
                }
            }
            
            isRunning = false;
        }

        // Gets previous price.
        public Int32 GetPrevPrice()
        {
            // Acquire lock and retrieve the previous computer price.
            rwlock.AcquireReaderLock(300);
            Int32 price = previousPrice;
            rwlock.ReleaseReaderLock();
            return price;
        }

        // Get current price.
        public Int32 GetPrice()
        {
            // Acquire lock and retrieve the current computer price.
            rwlock.AcquireReaderLock(300);
            Int32 price = computerPrice;
            rwlock.ReleaseReaderLock();
            return price;
        }

        // Changes the price.
        public void SetPrice(Int32 price)
        {
            // Acquire lock and set the current computer price.
            rwlock.AcquireWriterLock(300);
            computerPrice = price;
            rwlock.ReleaseWriterLock();
        }

        // Randomly changes the price and notifies subscribers if the price decreases.
        public void PricingModel()
        {
            Int32 price = random.Next(1400, 1600);
            if (price < computerPrice)
            {
                p += 1;
                if (priceCut != null)
                    priceCut(price);
            }
            SetPrice(price);
        }

        // Converts the string containing order object information into an order object.
        public OrderClass Decoder(string orderobj)
        {
            string[] orderInfo = orderobj.Split('/');
            OrderClass OrderObj = new OrderClass();

            OrderObj.setID(orderInfo[0]);
            OrderObj.setCardNum(Int32.Parse(orderInfo[1]));
            OrderObj.setQuantity(Int32.Parse(orderInfo[2]));

            return OrderObj;
        }

        // Returns the counter for price cuts.
        public Int32 GetCounter()
        {
            return p;
        }
    }
}