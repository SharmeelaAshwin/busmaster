using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalGraphWndCSharp
{
    using GraphLib;
    class GraphPlotterThread
    {
        PlotterDisplayEx mGraph = null;

        // This method will be called when the thread is started. 
        public void Plot(object graphCtrl)
        {
             mGraph = (PlotterDisplayEx)graphCtrl;
            if (!_shouldStop)
            {
                mGraph.Start();
            }
            else
            {
                mGraph.Stop();
            }
            Console.WriteLine("worker thread: terminating gracefully.");
        }
        public void RequestStop()
        {
            _shouldStop = true;
            mGraph.Stop();
        }
        // Volatile is used as hint to the compiler that this data 
        // member will be accessed by multiple threads. 
        private volatile bool _shouldStop;


    }
}
