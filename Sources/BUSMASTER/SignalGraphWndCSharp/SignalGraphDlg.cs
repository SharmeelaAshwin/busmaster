using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace SignalGraphWndCSharp
{
    using GraphLib;

    public partial class SignalGraphDlg : Form
    {
        private int NumGraphs = 4;
        private String CurExample = "TILED_VERTICAL_AUTO";
        private ColorSchema CurColorSchema = ColorSchema.GRAY;
        private PrecisionTimer.Timer mTimer = null;
        private DateTime lastTimerTick = DateTime.Now;
        private SignalDetails mSignalDetails = new SignalDetails();

         //Create the thread object. This does not start the thread.
        static GraphPlotterThread mPlotterObject = new GraphPlotterThread();
        Thread mPlotterThread = new Thread(mPlotterObject.Plot);

        public SignalDetails SignalDetails
        {
            get { return mSignalDetails; }
            set 
            { 
                mSignalDetails = value;
                CreateDataSourcesFromSignalDetails();
            }
        }

        public SignalGraphDlg()
        {
            InitializeComponent();
            InitializeGraphTypeCombo();
            InitializeColorSchema();
        }

        private void InitializeColorSchema()
        {
            BackGroundColorCombo.Items.Add("BLUE");
            BackGroundColorCombo.Items.Add("WHITE");
            BackGroundColorCombo.Items.Add("GRAY");
            BackGroundColorCombo.Items.Add("LIGHT_BLUE");
            BackGroundColorCombo.Items.Add("BLACK");
            BackGroundColorCombo.Items.Add("RED");
            BackGroundColorCombo.Items.Add("DARK_GREEN");
        }

        private void InitializeGraphTypeCombo()
        {
            GraphTypeCombo.Items.Add("Normal");
            GraphTypeCombo.Items.Add("Normal Autoscaled");
            GraphTypeCombo.Items.Add("Stacked");
            GraphTypeCombo.Items.Add("Vertically Aligned");
            GraphTypeCombo.Items.Add("Vertically Aligned Autoscaled");
            GraphTypeCombo.Items.Add("Tiled Vertical");
            GraphTypeCombo.Items.Add("Tiled Vertical Autoscaled");
            GraphTypeCombo.Items.Add("Tiled Horizontal");
            GraphTypeCombo.Items.Add("Tiled Horizontal Autoscaled");
            
        }

        public SignalGraphDlg(Form parent)
        {
            InitializeComponent();
            MdiParent = parent;
        }

        private void CreateDataSourcesFromSignalDetails()
        {
            this.SuspendLayout();
            GraphCtrl.DataSources.Clear();
            GraphCtrl.SetDisplayRangeX(0, 400);
            NumGraphs = mSignalDetails.NoOfSignals;
            
            for (int j = 0; j < NumGraphs; j++)
            {
                GraphCtrl.DataSources.Add(new DataSource());
                GraphCtrl.DataSources[j].Name = mSignalDetails.Signals[j];
                GraphCtrl.DataSources[j].OnRenderXAxisLabel += RenderXLabel;
            }
            CalcDataGraphs();
            GraphCtrl.Smoothing = System.Drawing.Drawing2D.SmoothingMode.None;
            GraphTypeCombo.SelectedIndex = 6;
            BackGroundColorCombo.SelectedIndex = 1;
            //GraphCtrl.Refresh();

            mTimer = new PrecisionTimer.Timer();
            mTimer.Period = 40;                         // 20 fps
            mTimer.Tick += new EventHandler(OnTimerTick);
            lastTimerTick = DateTime.Now;
            mTimer.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (CurExample == "ANIMATED_AUTO")
            {
                try
                {
                    TimeSpan dt = DateTime.Now - lastTimerTick;

                    for (int j = 0; j < NumGraphs; j++)
                    {

                       // CalcSinusFunction_3(GraphCtrl.DataSources[j], j, (float)dt.TotalMilliseconds);

                    }

                    this.Invoke(new MethodInvoker(RefreshGraph));
                }
                catch (ObjectDisposedException ex)
                {
                    // we get this on closing of form
                }
                catch (Exception ex)
                {
                    Console.Write("exception invoking refreshgraph(): " + ex.Message);
                }


            }
        }
        protected void CalcDataGraphs()
        {

            //this.SuspendLayout();

            //GraphCtrl.DataSources.Clear();
            //GraphCtrl.SetDisplayRangeX(0, 400);

            for (int j = 0; j < NumGraphs; j++)
            {
                //GraphCtrl.DataSources.Add(new DataSource());
                //GraphCtrl.DataSources[j].Name = "Graph " + (j + 1);
                GraphCtrl.DataSources[j].OnRenderXAxisLabel += RenderXLabel;

                switch (CurExample)
                {
                    case "NORMAL":
                        //this.Text = "Normal Graph";
                        GraphCtrl.DataSources[j].Length = 5800;
                        GraphCtrl.PanelLayout = PlotterGraphPaneEx.LayoutMode.NORMAL;
                        GraphCtrl.DataSources[j].AutoScaleY = false;
                        GraphCtrl.DataSources[j].SetDisplayRangeY(0, 300);
                        GraphCtrl.DataSources[j].SetGridDistanceY(50);
                        GraphCtrl.DataSources[j].OnRenderYAxisLabel = RenderYLabel;
                        //CalcSinusFunction_0(GraphCtrl.DataSources[j], j);
                        break;

                    case "NORMAL_AUTO":
                        //this.Text = "Normal Graph Autoscaled";
                        GraphCtrl.DataSources[j].Length = 5800;
                        GraphCtrl.PanelLayout = PlotterGraphPaneEx.LayoutMode.NORMAL;
                        GraphCtrl.DataSources[j].AutoScaleY = true;
                        GraphCtrl.DataSources[j].SetDisplayRangeY(0, 300);
                        GraphCtrl.DataSources[j].SetGridDistanceY(50);
                        GraphCtrl.DataSources[j].OnRenderYAxisLabel = RenderYLabel;
                        //CalcSinusFunction_0(GraphCtrl.DataSources[j], j);
                        break;

                    case "STACKED":
                        //this.Text = "Stacked Graph";
                        GraphCtrl.PanelLayout = PlotterGraphPaneEx.LayoutMode.STACKED;
                        GraphCtrl.DataSources[j].Length = 5800;
                        GraphCtrl.DataSources[j].AutoScaleY = false;
                        GraphCtrl.DataSources[j].SetDisplayRangeY(0, 250);
                        GraphCtrl.DataSources[j].SetGridDistanceY(50);
                        //CalcSinusFunction_1(GraphCtrl.DataSources[j], j);
                        break;

                    case "VERTICAL_ALIGNED":
                        //this.Text = "Vertical aligned Graph";
                        GraphCtrl.PanelLayout = PlotterGraphPaneEx.LayoutMode.VERTICAL_ARRANGED;
                        GraphCtrl.DataSources[j].Length = 5800;
                        GraphCtrl.DataSources[j].AutoScaleY = false;
                        GraphCtrl.DataSources[j].SetDisplayRangeY(0, 300);
                        GraphCtrl.DataSources[j].SetGridDistanceY(50);
                        //CalcSinusFunction_2(GraphCtrl.DataSources[j], j);
                        break;

                    case "VERTICAL_ALIGNED_AUTO":
                        //this.Text = "Vertical aligned Graph autoscaled";
                        GraphCtrl.PanelLayout = PlotterGraphPaneEx.LayoutMode.VERTICAL_ARRANGED;
                        GraphCtrl.DataSources[j].Length = 5800;
                        GraphCtrl.DataSources[j].AutoScaleY = true;
                        GraphCtrl.DataSources[j].SetDisplayRangeY(0, 300);
                        GraphCtrl.DataSources[j].SetGridDistanceY(50);
                        //CalcSinusFunction_2(GraphCtrl.DataSources[j], j);
                        break;

                    case "TILED_VERTICAL":
                        //this.Text = "Tiled Graphs (vertical prefered)";
                        GraphCtrl.PanelLayout = PlotterGraphPaneEx.LayoutMode.TILES_VER;
                        GraphCtrl.DataSources[j].Length = 5800;
                        GraphCtrl.DataSources[j].AutoScaleY = false;
                        GraphCtrl.DataSources[j].SetDisplayRangeY(0, 600);
                        GraphCtrl.DataSources[j].SetGridDistanceY(50);
                        //CalcSinusFunction_2(GraphCtrl.DataSources[j], j);
                        break;

                    case "TILED_VERTICAL_AUTO":
                        //this.Text = "Tiled Graphs (vertical prefered) autoscaled";
                        GraphCtrl.PanelLayout = PlotterGraphPaneEx.LayoutMode.TILES_VER;
                        GraphCtrl.DataSources[j].Length = 5800;
                        GraphCtrl.DataSources[j].AutoScaleY = true;
                        GraphCtrl.DataSources[j].SetDisplayRangeY(0, 300);
                        GraphCtrl.DataSources[j].SetGridDistanceY(50);
                        //CalcSinusFunction_2(GraphCtrl.DataSources[j], j);
                        break;

                    case "TILED_HORIZONTAL":
                        //this.Text = "Tiled Graphs (horizontal prefered)";
                        GraphCtrl.PanelLayout = PlotterGraphPaneEx.LayoutMode.TILES_HOR;
                        GraphCtrl.DataSources[j].Length = 5800;
                        GraphCtrl.DataSources[j].AutoScaleY = false;
                        GraphCtrl.DataSources[j].SetDisplayRangeY(0, 300);
                        GraphCtrl.DataSources[j].SetGridDistanceY(50);
                        //CalcSinusFunction_2(GraphCtrl.DataSources[j], j);
                        break;

                    case "TILED_HORIZONTAL_AUTO":
                        //this.Text = "Tiled Graphs (horizontal prefered) autoscaled";
                        GraphCtrl.PanelLayout = PlotterGraphPaneEx.LayoutMode.TILES_HOR;
                        GraphCtrl.DataSources[j].Length = 5800;
                        GraphCtrl.DataSources[j].AutoScaleY = true;
                        GraphCtrl.DataSources[j].SetDisplayRangeY(0, 300);
                        GraphCtrl.DataSources[j].SetGridDistanceY(50);
                        //CalcSinusFunction_2(GraphCtrl.DataSources[j], j);
                        break;

                    case "ANIMATED_AUTO":

                        //this.Text = "Animated graphs fixed x range";
                        GraphCtrl.PanelLayout = PlotterGraphPaneEx.LayoutMode.TILES_HOR;
                        GraphCtrl.DataSources[j].Length = 402;
                        GraphCtrl.DataSources[j].AutoScaleY = false;
                        GraphCtrl.DataSources[j].AutoScaleX = true;
                        GraphCtrl.DataSources[j].SetDisplayRangeY(0, 300);
                        GraphCtrl.DataSources[j].SetGridDistanceY(50);
                        GraphCtrl.DataSources[j].XAutoScaleOffset = 50;
                        //CalcSinusFunction_3(GraphCtrl.DataSources[j], j, 0);
                        GraphCtrl.DataSources[j].OnRenderYAxisLabel = RenderYLabel;
                        break;
                }
            }

            ApplyColorSchema();

            this.ResumeLayout();
            GraphCtrl.Refresh();

        }

        private String RenderXLabel(DataSource s, int idx)
        {
            if (s.AutoScaleX)
            {
                //if (idx % 2 == 0)
                {
                    int Value = (int)(s.Samples[idx].x);
                    return "" + Value;
                }
                return "";
            }
            else
            {
                String Label = "";
                if (s.Samples[idx].x % 10 == 0)
                {
                    Label = String.Format("{0:0.000}",s.ActualXValuesFromBM[idx] / 10000);
                }
                return Label;
            }
        }

        private String RenderYLabel(DataSource s, float value)
        {
            return String.Format("{0:0.0}", value);
        }

        private void ApplyColorSchema()
        {
            switch (CurColorSchema)
            {
                case ColorSchema.DARK_GREEN:
                    {
                        Color[] cols = { Color.FromArgb(0,255,0), 
                                         Color.FromArgb(0,255,0),
                                         Color.FromArgb(0,255,0), 
                                         Color.FromArgb(0,255,0), 
                                         Color.FromArgb(0,255,0) ,
                                         Color.FromArgb(0,255,0),                              
                                         Color.FromArgb(0,255,0) };

                        for (int j = 0; j < NumGraphs; j++)
                        {
                            GraphCtrl.DataSources[j].GraphColor = cols[j % 7];
                        }

                        GraphCtrl.BackgroundColorTop = Color.FromArgb(0, 64, 0);
                        GraphCtrl.BackgroundColorBot = Color.FromArgb(0, 64, 0);
                        GraphCtrl.SolidGridColor = Color.FromArgb(0, 128, 0);
                        GraphCtrl.DashedGridColor = Color.FromArgb(0, 128, 0);
                    }
                    break;
                case ColorSchema.WHITE:
                    {
                        Color[] cols = { Color.DarkRed, 
                                         Color.DarkSlateGray,
                                         Color.DarkCyan, 
                                         Color.DarkGreen, 
                                         Color.DarkBlue ,
                                         Color.DarkMagenta,                              
                                         Color.DeepPink };

                        for (int j = 0; j < NumGraphs; j++)
                        {
                            GraphCtrl.DataSources[j].GraphColor = cols[j % 7];
                        }

                        GraphCtrl.BackgroundColorTop = Color.White;
                        GraphCtrl.BackgroundColorBot = Color.White;
                        GraphCtrl.SolidGridColor = Color.LightGray;
                        GraphCtrl.DashedGridColor = Color.LightGray;
                    }
                    break;

                case ColorSchema.BLUE:
                    {
                        Color[] cols = { Color.Red, 
                                         Color.Orange,
                                         Color.Yellow, 
                                         Color.LightGreen, 
                                         Color.Blue ,
                                         Color.DarkSalmon,                              
                                         Color.LightPink };

                        for (int j = 0; j < NumGraphs; j++)
                        {
                            GraphCtrl.DataSources[j].GraphColor = cols[j % 7];
                        }

                        GraphCtrl.BackgroundColorTop = Color.Navy;
                        GraphCtrl.BackgroundColorBot = Color.FromArgb(0, 0, 64);
                        GraphCtrl.SolidGridColor = Color.Blue;
                        GraphCtrl.DashedGridColor = Color.Blue;
                    }
                    break;

                case ColorSchema.GRAY:
                    {
                        Color[] cols = { Color.DarkRed, 
                                         Color.DarkSlateGray,
                                         Color.DarkCyan, 
                                         Color.DarkGreen, 
                                         Color.DarkBlue ,
                                         Color.DarkMagenta,                              
                                         Color.DeepPink };

                        for (int j = 0; j < NumGraphs; j++)
                        {
                            GraphCtrl.DataSources[j].GraphColor = cols[j % 7];
                        }

                        GraphCtrl.BackgroundColorTop = Color.White;
                        GraphCtrl.BackgroundColorBot = Color.LightGray;
                        GraphCtrl.SolidGridColor = Color.LightGray;
                        GraphCtrl.DashedGridColor = Color.LightGray;
                    }
                    break;

                case ColorSchema.RED:
                    {
                        Color[] cols = { Color.DarkCyan, 
                                         Color.Yellow,
                                         Color.DarkCyan, 
                                         Color.DarkGreen, 
                                         Color.DarkBlue ,
                                         Color.DarkMagenta,                              
                                         Color.DeepPink };

                        for (int j = 0; j < NumGraphs; j++)
                        {
                            GraphCtrl.DataSources[j].GraphColor = cols[j % 7];
                        }

                        GraphCtrl.BackgroundColorTop = Color.DarkRed;
                        GraphCtrl.BackgroundColorBot = Color.Black;
                        GraphCtrl.SolidGridColor = Color.Red;
                        GraphCtrl.DashedGridColor = Color.Red;
                    }
                    break;

                case ColorSchema.LIGHT_BLUE:
                    {
                        Color[] cols = { Color.DarkRed, 
                                         Color.DarkSlateGray,
                                         Color.DarkCyan, 
                                         Color.DarkGreen, 
                                         Color.DarkBlue ,
                                         Color.DarkMagenta,                              
                                         Color.DeepPink };

                        for (int j = 0; j < NumGraphs; j++)
                        {
                            GraphCtrl.DataSources[j].GraphColor = cols[j % 7];
                        }

                        GraphCtrl.BackgroundColorTop = Color.White;
                        GraphCtrl.BackgroundColorBot = Color.FromArgb(183, 183, 255);
                        GraphCtrl.SolidGridColor = Color.Blue;
                        GraphCtrl.DashedGridColor = Color.Blue;
                    }
                    break;

                case ColorSchema.BLACK:
                    {
                        Color[] cols = { Color.FromArgb(255,0,0), 
                                         Color.FromArgb(0,255,0),
                                         Color.FromArgb(255,255,0), 
                                         Color.FromArgb(64,64,255), 
                                         Color.FromArgb(0,255,255) ,
                                         Color.FromArgb(255,0,255),                              
                                         Color.FromArgb(255,128,0) };

                        for (int j = 0; j < NumGraphs; j++)
                        {
                            GraphCtrl.DataSources[j].GraphColor = cols[j % 7];
                        }

                        GraphCtrl.BackgroundColorTop = Color.Black;
                        GraphCtrl.BackgroundColorBot = Color.Black;
                        GraphCtrl.SolidGridColor = Color.DarkGray;
                        GraphCtrl.DashedGridColor = Color.DarkGray;
                    }
                    break;
            }

        }

        private void RefreshGraph()
        {                             
            GraphCtrl.Refresh();             
        }

        public void AddGraphPlottingValues(double x, int y, int index)
        {
            GraphCtrl.setDataSourceValues(x, y, index);
        }

        public void ClearGraph(short eBusType)
        {
            //Clear graph here.
        }

        public bool StartStopPlotting(bool bStart)
        {
            try
            {
                if (bStart)
                {

                    //mPlotterThread.Start(GraphCtrl);
                    // Loop until worker thread activates.
                    //while (!mPlotterThread.IsAlive) ;
                    GraphCtrl.Start();

                }
                else
                {
                    GraphCtrl.Stop();
                    //if (mPlotterThread.IsAlive)
                    //{
                    //    // Request that the worker thread stop itself:
                    //    mPlotterObject.RequestStop();
                    //}

                }    
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private void GraphTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SuspendLayout();

            //display.DataSources.Clear();
            GraphCtrl.SetDisplayRangeX(0, 400);
            switch (GraphTypeCombo.SelectedIndex)
            {
                case 0:
                    {
                        CurExample = "NORMAL";
                        break;
                    }
                case 1:
                    {
                        CurExample = "NORMAL_AUTO";
                        break;
                    }
                case 2:
                    {
                        CurExample = "STACKED";
                        break;
                    }
                case 3:
                    {
                        CurExample = "VERTICAL_ALIGNED";
                        break;
                    }
                case 4:
                    {
                        CurExample = "VERTICAL_ALIGNED_AUTO";
                        break;
                    }
                case 5:
                    {
                        CurExample = "TILED_VERTICAL";
                        break;
                    }
                case 6:
                    {
                        CurExample = "TILED_VERTICAL_AUTO";
                        break;
                    }
                case 7:
                    {
                        CurExample = "TILED_HORIZONTAL";
                        break;
                    }
                case 8:
                    {
                        CurExample = "TILED_HORIZONTAL_AUTO";
                        break;
                    }
                default:
                    break;
            }
            CalcDataGraphs();
        }

        private void BackGroundColorCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurColorSchema = (ColorSchema)BackGroundColorCombo.SelectedIndex;
            CalcDataGraphs();
        }
    }
}
