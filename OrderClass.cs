/*Author: Cody Fuller
 *Course: CSE-445*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication
{
    class OrderClass
    {
        private string senderID;
        private Int32 cardNo;
        private Int32 quantity;

        // Set the customer ID.
        public void setID(string id)
        {
            senderID = id;
        }

        // Get the customer ID.
        public string getID()
        {
            return senderID;
        }

        // Set the credit card number.
        public void setCardNum(int num)
        {
            cardNo = num;
        }

        // Get the credit card number.
        public Int32 getCardNum()
        {
            return cardNo;
        }

        // Set the quantity of computers to order.
        public void setQuantity(int quant)
        {
            quantity = quant;
        }

        // Get the quantity of computers to order.
        public Int32 getQuantity()
        {
            return quantity;
        }
    }
}
