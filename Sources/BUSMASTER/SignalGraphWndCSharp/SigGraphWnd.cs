using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace SignalGraphWndCSharp
{
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("8FAD63D9-E8B3-42A5-B589-71AB5148D6DD")]
    public class SigGraphWnd: ISigGraphWnd
    {
        uint AVAILABLE_PROTOCOLS   =      5;
        SignalGraphDlg[] m_SGWnd = new SignalGraphDlg[5];
        public long CreateGraphWindow(short eBusType)
        {
            if (m_SGWnd[eBusType] == null)
            {
                m_SGWnd[eBusType] = new SignalGraphDlg();
                
            }
            m_SGWnd[eBusType].Show();  
            return 1;
        }

        public bool IsWindowVisible(short eBusType)
        {
            return false;
        }

        public long ShowGraphWindow(short eBusType, bool bShow)
        {
            if (m_SGWnd[eBusType] != null)
            {
                m_SGWnd[eBusType].Show();
            }
            return 1;
        }

        public long PostMessageToSGWnd(short eBusType, uint msg, uint wParam, uint lParam)
        {
            throw new NotImplementedException();
        }

        public void ClearGraph(short eBusType)
        {
            m_SGWnd[eBusType].ClearGraph(eBusType);
        }

        public bool IsGraphWindowCreated(short eBusType)
        {
            if (eBusType >= m_SGWnd.Length)
            {
                return false;
            }
            if (m_SGWnd[eBusType] != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Update the graph with plotting values.
        /// </summary>
        /// <param name="x">x axis value</param>
        /// <param name="y">y axis value</param>
        /// <param name="eBusType">EBusType</param>
        public void AddGraphPlottingValues(double x, int y, short eBusType, int index)
        {
            m_SGWnd[eBusType].AddGraphPlottingValues(x, y, index);
        }

        /// <summary>
        /// Sets the signal detials to the SignalGraph dialog.
        /// </summary>
        /// <param name="eBusType"></param>
        /// <param name="signalDetails"></param>
        public void SetSignalListDetails(short eBusType, ISignalDetails signalDetails)
        {
            m_SGWnd[eBusType].SignalDetails = (SignalDetails)signalDetails;
        }

        public bool StartStopPlotting(short eBusType, bool bStart)
        {
            return m_SGWnd[eBusType].StartStopPlotting(bStart);
        }
    }

    [ClassInterface(ClassInterfaceType.None)]
    [Guid("0333CD15-F3B1-4B54-940D-463602FDF3AD")]
    public class SignalDetails : ISignalDetails
    {
        int noOfSignals = 0;
        List<string> signals;
        public int NoOfSignals
        {
            get { return noOfSignals; }
            set { noOfSignals = value; }
        }
        
        public List<string> Signals
        {
            get { return signals; }
        }

        public void AddSignals(string signal)
        {
            if (signals == null)
            {
                signals = new List<string>();
            }
            signals.Add(signal);
        }
    }
}
