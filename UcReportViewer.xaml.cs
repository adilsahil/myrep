using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Masters
{
    /// <summary>
    /// Interaction logic for UcReportViewer.xaml
    /// </summary>
    public partial class UcReportViewer : Window,INotifyPropertyChanged
    {
        public UcReportViewer()
        {
            InitializeComponent();
            SetReportSource();
        }
        public CrystalDecisions.Windows.Forms.ToolPanelViewType ToolPanelView
        {
            get { return MyReportViewer.ToolPanelView; }
            set { MyReportViewer.ToolPanelView = value; }
        }

        private object reportSource;
        public object ReportSource
        {
            get
            {
                return reportSource;
            }
            set
            {
                reportSource = value;
                OnPropertyChanged("ReportSource");
                SetReportSource();
            }
        }
        private void SetReportSource()
        {
            MyReportViewer.ReportSource = ReportSource;
            MyReportViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            MyReportViewer.ShowParameterPanelButton = false;
            MyReportViewer.ShowRefreshButton = false;
            MyReportViewer.EnableDrillDown = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
