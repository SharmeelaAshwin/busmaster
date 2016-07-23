using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SignalGraphWndCSharp
{
    [Guid("718B078A-F4F3-45F2-818F-1572FD0A0E3E")]
    public interface ISigGraphWnd
    {
        //Create the signal graph dialog.
        long CreateGraphWindow(short eBusType);

        //Checks whether the Signal graph window is visible or not.
        bool IsWindowVisible(short eBusType);

        //Shows or hides the graph window.
        long ShowGraphWindow(short eBusType, bool bShow);


        long PostMessageToSGWnd(short eBusType, uint msg, uint wParam, uint lParam);

        /// <summary>
        /// Clears the graph for a perticular bus type.
        /// </summary>
        /// <param name="eBusType">Type of bus whose graph is to be cleared</param>
        void ClearGraph(short eBusType);

        /// <summary>
        /// Checks if the signal graph window is created for a perticulsr bus type.
        /// </summary>
        /// <param name="eBusType"></param>
        /// <returns></returns>
        bool IsGraphWindowCreated(short eBusType);

        /// <summary>
        /// Update the graph with plotting values.
        /// </summary>
        /// <param name="x">x axis value</param>
        /// <param name="y">y axis value</param>
        /// <param name="eBusType">EBusType</param>
        void AddGraphPlottingValues(double x, int y, short eBusType, int index);

        void SetSignalListDetails(short eBusType, ISignalDetails signalDetails);

        bool StartStopPlotting(short eBusType, bool bStart);
    }

    [Guid("B5D9F4F1-E255-4A8F-AFAA-1EDB5FE41EAA")]
    public interface ISignalDetails
    {
         int NoOfSignals { get; set; }
         void AddSignals(string signal);
    }
}
