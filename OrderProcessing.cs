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
    class OrderProcessing
    {
        private OrderClass orderObj;
        private EventClass eventClass;

        // Validates credit card and calculates total cost and sends 
        // confirmation to store.
        public void OrderProcessingFunc()
        {
            ComputerMaker computerMaker = new ComputerMaker();

            bool validCardNum = false;
            Int32 cardNum = orderObj.getCardNum();
            Int32 quantity = orderObj.getQuantity();

            if (cardNum >= 0000 && cardNum < 9999) validCardNum = true;
            if (!validCardNum) Console.WriteLine("Invalid credit card number.");
            if (validCardNum)
            {
                float totalCharge = (float)(((computerMaker.GetPrice() * quantity)
                    * 6.5) + 29.99);
                string name = orderObj.getID();
                eventClass.SendConfirmation(name, totalCharge);
            }
            Thread.CurrentThread.Abort();
        }

        // Used to get order object when decoding string to order object.
        public void SetOrderObj(OrderClass OrderObjIn)
        {
            orderObj = OrderObjIn;
        }

        // Set event to callback store thread from OrderProcessing later.
        public void SetEvent(EventClass evc)
        {
            eventClass = evc;
        }
    }
}
