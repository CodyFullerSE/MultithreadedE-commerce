/*Author: Cody Fuller
 *Course: CSE-445*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication
{
    class EventClass
    {
        public event orderConfirmationEvent confirm;

        // Send confirmation to store.
        public void SendConfirmation(string name, float price)
        {
            if (confirm != null) confirm(name, price);
        }
    }
}
