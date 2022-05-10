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
    class MultiCellBuffer
    {
        private static Int32 n = 2;
        private static Int32 items = 0;
        private static Semaphore _sem = new Semaphore(n, n);
        private static ReaderWriterLock rwlock = new ReaderWriterLock();
        private static string[] buffer = new string[n];
        private static EventClass[] eventBuffer = new EventClass[n];

        // Get the number of cells for Main to use to initialize buffer.
        public static int GetNumCells()
        {
            return n;
        }

        // Change the value of a cell in the buffer and change event in evbuffer.
        public static void SetOneCell(string value, EventClass evc)
        {
            Random random = new Random();
            _sem.WaitOne();

            // Sleep if buffer is full.
            if (items == 2) Thread.Sleep(random.Next(200, 1000));
            rwlock.AcquireWriterLock(Timeout.Infinite);
            try
            {
                for (int i = 0; i < n; i++)
                {
                    if (buffer[i] == "")
                    {
                        buffer[i] = value;
                        if (i == 0) eventBuffer[i] = evc;
                        if (i == 1) eventBuffer[i] = evc;
                        items += 1;
                        break;
                    }
                }
            }
            finally
            {
                rwlock.ReleaseWriterLock();
            }
            _sem.Release();
        }

        // Get and return the value of a cell in the buffer.
        public static string GetOneCell()
        {
            Random random = new Random();
            string value = "";
            _sem.WaitOne();

            // Sleep if buffer is empty.
            if (items == 0) Thread.Sleep(random.Next(1, 2000));
            rwlock.AcquireReaderLock(300);
            try
            {
                for (int i = 0; i < n; i++)
                {
                    if (buffer[i] != "")
                    {
                        value = buffer[i];
                        buffer[i] = "";
                        items += 1;
                        break;
                    }
                }
            }
            finally
            {
                rwlock.ReleaseReaderLock();
            }
            _sem.Release();
            return value;
        }

        // Get the event so OrderProcessing can notify store.
        public static EventClass GetOneEventCell()
        {
            Random random = new Random();
            EventClass eventClass = new EventClass();
            //sleep if buffer is empty
            if (items == 0) Thread.Sleep(random.Next(1, 2000));
            _sem.WaitOne();
            rwlock.AcquireReaderLock(300);
            try
            {
                for (int i = 0; i < n; i++)
                {
                    if (eventBuffer[i] != null)
                    {
                        eventClass = eventBuffer[i];
                        eventBuffer[i] = null;
                        items += 1;
                        break;
                    }
                }
            }
            finally
            {
                rwlock.ReleaseReaderLock();
            }
            _sem.Release();
            return eventClass;
        }
    }
}
