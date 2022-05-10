/*Author: Cody Fuller
 *Course: CSE-445*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApplication
{
    class Store
    {
        private OrderClass orderObj = new OrderClass();
        private string timeStamp = "";

        // Evaluates the price, creates an order object and sends it to the encoder to
        // convert into a string, and sends the order object to the buffer.
        public void StoreFunc()
        {
            while(ComputerMaker.isRunning)
            {
                
                DateTime start = DateTime.Now;
                if ((DateTime.Now - start).Seconds > 0.5)
                {
                    Thread.CurrentThread.Priority = ThreadPriority.Normal;
                }

                if ((DateTime.Now - start).Seconds > 1)
                {
                    Thread.CurrentThread.Priority = ThreadPriority.Highest;
                }

                ComputerMaker computerMaker = new ComputerMaker();
                Random random = new Random();
                Thread.Sleep(random.Next(500, 2000));
                Int32 price = computerMaker.GetPrice();
                Console.WriteLine("{0} has computers for a price of ${1}",
                Thread.CurrentThread.Name, price);

                if (computerMaker.GetPrevPrice() - price >= 200) orderObj.setQuantity(2);
                else orderObj.setQuantity(1);

                orderObj.setCardNum(random.Next(1000, 9999));
                orderObj.setID(Thread.CurrentThread.Name);
                string order = Encoder(orderObj);
                timeStamp = DateTime.Now.ToString();

                EventClass eventClass = new EventClass();
                eventClass.confirm += new orderConfirmationEvent(OrderConfirmation);
                MultiCellBuffer.SetOneCell(order, eventClass);
                Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
            }
        }

        // Event handler that is called when a price cut occurs.
        public void LowerPrice(Int32 pr)
        {
            Console.WriteLine("Store{0} has computers on sale for a low price of ${1}",
            Thread.CurrentThread.Name, pr);
        }

        // Creates a string containing order object information.
        public string Encoder(OrderClass OrderObj)
        {
            string orderobj = OrderObj.getID().ToString() + "/" +
            OrderObj.getCardNum().ToString() + "/" +
            OrderObj.getQuantity().ToString();

            return orderobj;
        }

        // Order confirmation event handler.
        public void OrderConfirmation(string name, float price)
        {
            Console.WriteLine("{0} Order confirmed for {1}. Total cost: ${2}", timeStamp,
                name, price);
        }
    }
}
