using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalGraphWndCSharp
{
    using GraphLib;
    class UpdateGraphValuesThread
    {
        // This method will be called when the thread is started. 
        public void UpdateGraph(DataSource ds, int x, int y, int graphID)
        {
            while (_shouldStop == false)
            {
                if (ds.Samples != null)
	            {
                    int cnt = ds.Samples.Length;
                    ds.Samples[cnt].x = x;
                    ds.Samples[cnt].y = y;
	            }
               
            }
            Console.WriteLine("worker thread: terminating gracefully.");
        }
        public void RequestStop()
        {
            _shouldStop = true;
        }
        // Volatile is used as hint to the compiler that this data 
        // member will be accessed by multiple threads. 
        private volatile bool _shouldStop;
    }
}
