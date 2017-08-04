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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.ComponentModel;
using System.Data.OracleClient;
using System.Configuration;
using UIElements;
using System.Data.SqlClient;
using System.Threading;


namespace Masters
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;
            ResizeMode = ResizeMode.NoResize;
            onload();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string Name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(Name));
            }
        }

        private bool ischek=false;
        public bool Ischek
        {
            get
            {
                return ischek;
            }
            set
            {
                ischek = value;
                OnPropertyChanged("ischek");
            }
        }

        private DataTable contract;
        public DataTable Contract
        {
            get
            {
                return contract;
            }
            set
            {
                contract = value;
                OnPropertyChanged("Contract");
            }
        }

        private string txttype;
        public string Txttype
        {
            get
            {
                return txttype;
            }
            set
            {
                txttype = value;
                OnPropertyChanged("Txttype");
            }
        }

        private DataTable dtdata;
        public DataTable Dtdata
        {
            get
            {
                return dtdata;
            }
            set
            {
                dtdata = value;
                OnPropertyChanged("Dtdata");
            }
        
        }

        private DataRowView dr;
        public DataRowView Dr
        {
            get
            {
                return dr;
            }
            set
            {
                dr = value;
                OnPropertyChanged("Dr");
            }
        }

        private DataRow editRow;
        public DataRow EditRow
        {
            get
            {
                return editRow;
            }
            set
            {
                editRow = value;
                OnPropertyChanged("EditRow");
            }
        }

        private void onload()
        {
            try
            {
                
                if (Contract == null)
                    Contract = new DataTable();
                Contract.Columns.Add("ID", typeof(Int64));
                Contract.Columns.Add("TYPE", typeof(string));
                Contract.Rows.Add(1, "RADIOLOGY");
                Contract.Rows.Add(2, "LABORTORY");

                if (Dtdata == null)
                    Dtdata = new DataTable();
                Dtdata.Columns.Add("SL_NO", typeof(decimal));
                Dtdata.Columns.Add("NAME");
                Dtdata.Columns.Add("CATEGORY");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GOButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate())
                {
                    if (Dtdata != null && Dtdata.Columns.Count > 0)
                    {
                        if (EditRow == null)
                        {
                            DataRow drtemp = Dtdata.NewRow();
                            drtemp["NAME"] = Txttype;
                            drtemp["CATEGORY"] = Dr["TYPE"];
                            Dtdata.Rows.Add(drtemp);
                            serialcount();
                        }
                        else
                        {
                            EditRow["NAME"] = Txttype;
                            EditRow["CATEGORY"] = Dr["TYPE"];
                            serialcount();
                        }
                    }
                }
                
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString());
                e.Handled = true;
                //throw;
            }
        }

        private bool validate()
        {
            try
            {
                bool isvalid = true;
                if (string.IsNullOrEmpty(Txttype))
                {
                    MessageBox.Show("provide type");
                    return false;
                }
                else if (Dr == null)
                {
                    MessageBox.Show("provide contract");
                    return false;
                }
                else
                {
                    return isvalid;
                }
            }
            catch (Exception)
            {
                
                throw;
                
            }
        }

        private void serialcount()
        {
            try
            {
                if (Dtdata != null && Dtdata.Rows.Count > 0)
                {
                    decimal count = 1;
                    foreach (DataRow d in Dtdata.Rows)
                    {
                        d["SL_NO"] = count;
                        count++;
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        UIProgressBar objUIProgressBar = null;
        int TotalCount = 0;
        bool isSuccess = false;
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                if (e.Result != null && e.Result.GetType() == typeof(bool))
                {
                    isSuccess = Convert.ToBoolean(e.Result);
                }

                this.backgroundWorker.DoWork -= new DoWorkEventHandler(backgroundWorker_DoWork);
                this.backgroundWorker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
                this.backgroundWorker.ProgressChanged -= new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
                if (objUIProgressBar != null)
                {
                    objUIProgressBar.CloseWindow();
                    objUIProgressBar = null;

                }
            }
            catch (Exception)
            {
                if (objUIProgressBar != null)
                {
                    objUIProgressBar.CloseWindow();
                    objUIProgressBar = null;

                }
                throw;
            }
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (e.Argument != null)
                    e.Result = save();
            }
            catch (Exception)
            {
                e.Result = false;
                throw;
            }
        }
       
        private void Save_click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region old
                //if (Dtdata != null && Dtdata.Rows.Count > 0)
                //{
                //    //SqlConnection obj = new SqlConnection("Data Source=DB9; User ID=YASASII; Password=FALCON");
                //    //SqlCommand objc = new SqlCommand();
                //    //objc.Connection = obj;
                //    //objc.CommandText = "";
                //    //objc.CommandType = CommandType.StoredProcedure;
                //    //objc.Parameters.Add("", SqlDbType.Int).Direction = ParameterDirection.Output;
                //    //objc.ExecuteNonQuery();
                //    //SqlDataAdapter da = new SqlDataAdapter();
                //    //da.Fill();
                //    Boolean issuccess = false;
                //    using (OracleConnection objConn = new OracleConnection("Data Source=DB9; User ID=YASASII; Password=FALCON"))
                //    {
                //        foreach (DataRow dr in Dtdata.Rows)
                //        {
                //            if (dr.RowState == DataRowState.Added)
                //            {
                //                if (objConn.State != ConnectionState.Open)
                //                {
                //                    objConn.Open();
                //                }
                //                OracleCommand objCmd = new OracleCommand();
                //                objCmd.Connection = objConn;
                //                objCmd.CommandText = "INSERT_WCF_TEMP";
                //                objCmd.CommandType = CommandType.StoredProcedure;
                //                objCmd.Parameters.Add("P_NAME", OracleType.VarChar).Value = dr["NAME"];
                //                objCmd.Parameters.Add("P_TYPE", OracleType.VarChar).Value = dr["CATEGORY"];
                //                //objCmd.Parameters.Add("P_ID", OracleType.Number).Direction = ParameterDirection.Output;
                //                issuccess = objCmd.ExecuteNonQuery() > 0;
                //                //if (issuccess)
                //                //{
                //                //    dr["P_ID"] = objCmd.Parameters["P_ID"].Value;
                //                //}
                //                if (!issuccess)
                //                {
                //                    break;
                //                }
                //            }
                //        }
                //    }
                //    if (issuccess)
                //    {
                //        MessageBox.Show("saved");
                //    }
                //}
                #endregion
                this.backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
                this.backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
                this.backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
                this.backgroundWorker.WorkerReportsProgress = true;
                this.backgroundWorker.WorkerSupportsCancellation = true;
                int TotalCount = 1;
                objUIProgressBar = new UIProgressBar(TotalCount);
                Window win = new Window();
                win.Content = objUIProgressBar;
                win.SizeToContent = SizeToContent.WidthAndHeight;
                win.WindowStyle = WindowStyle.None;
                win.AllowsTransparency = true;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                this.backgroundWorker.RunWorkerAsync(Dtdata);
                win.Owner = Application.Current.MainWindow;
                win.ShowDialog();
                backgroundWorker.Dispose();

                if (isSuccess) 
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("data saved successfully");
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString());
                e.Handled = true;
                //throw;
            }
        }
       
        private bool save()
        {
            try
            {
                bool issavedsuccess = false;
                if (Dtdata != null && Dtdata.Rows.Count > 0)
                {
                    //SqlConnection obj = new SqlConnection("Data Source=DB9; User ID=YASASII; Password=FALCON");
                    //SqlCommand objc = new SqlCommand();
                    //objc.Connection = obj;
                    //objc.CommandText = "";
                    //objc.CommandType = CommandType.StoredProcedure;
                    //objc.Parameters.Add("", SqlDbType.Int).Direction = ParameterDirection.Output;
                    //objc.ExecuteNonQuery();
                    //SqlDataAdapter da = new SqlDataAdapter();
                    //da.Fill();
                    Boolean issuccess = false;
                    int BillCount = 0;
                    TotalCount = Dtdata.Rows.Count;
                    using (OracleConnection objConn = new OracleConnection("Data Source=DB9; User ID=YASASII; Password=FALCON"))
                    {
                        foreach (DataRow dr in Dtdata.Rows)
                        {
                            if (dr.RowState == DataRowState.Added)
                            {
                                BillCount++;
                                if (objUIProgressBar != null)
                                    objUIProgressBar.DisplayCount(TotalCount, BillCount);
                                Thread.Sleep(1000);
                                if (objConn.State != ConnectionState.Open)
                                {
                                    objConn.Open();
                                }
                                OracleCommand objCmd = new OracleCommand();
                                objCmd.Connection = objConn;
                                objCmd.CommandText = "INSERT_WCF_TEMP";
                                objCmd.CommandType = CommandType.StoredProcedure;
                                objCmd.Parameters.Add("P_NAME", OracleType.VarChar).Value = dr["NAME"];
                                objCmd.Parameters.Add("P_TYPE", OracleType.VarChar).Value = dr["CATEGORY"];
                                //objCmd.Parameters.Add("P_ID", OracleType.Number).Direction = ParameterDirection.Output;
                                issuccess = objCmd.ExecuteNonQuery() > 0;
                                //if (issuccess)
                                //{
                                //    dr["P_ID"] = objCmd.Parameters["P_ID"].Value;
                                //}
                                if (!issuccess)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    if (issuccess)
                    {
                        issavedsuccess = true;
                    }
                }
                return issavedsuccess;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        
        private void GETButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txttype) || (Dr != null && Dr["TYPE"] != DBNull.Value))
                {
                    using (OracleConnection objConn = new OracleConnection())
                    {
                        objConn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;
                        if (objConn.State != ConnectionState.Open)
                        {
                            objConn.Open();
                        }
                        OracleCommand objCmd = new OracleCommand();
                        objCmd.Connection = objConn;
                        objCmd.CommandType = CommandType.Text;
                        objCmd.CommandText = "SELECT 1 AS SL_NO,NAME AS NAME,WCF AS CATEGORY FROM WCF_TEMP WHERE NAME='" + Txttype + "' AND WCF='" + Dr["TYPE"] +"'";
                        //AND WCF='" + Dr["TYPE"] + "' ";
                        //objCmd.Parameters.Add("P_NAME", OracleType.VarChar).Value = !string.IsNullOrEmpty(Txttype) ? Txttype : string.Empty;
                        //objCmd.Parameters.Add("P_TYPE", OracleType.VarChar).Value = (Dr != null && Dr["TYPE"] != DBNull.Value) ? Dr["TYPE"] : DBNull.Value;
                        //objCmd.Parameters.Add("P_CURSOR", OracleType.Cursor).Direction = ParameterDirection.Output;
                        DataSet ds = new DataSet();
                        OracleDataAdapter adapter = new OracleDataAdapter(objCmd);
                        adapter.Fill(ds);
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            Dtdata.Merge(ds.Tables["Table"]);
                            serialcount();
                        }

                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString());
                e.Handled = true;
                //throw;
            }

        }

        private void OnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dr = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
                if (dr.Row != null)
                {
                    EditRow = dr.Row;
                    Txttype =Convert.ToString(dr["NAME"]);
                    if (Contract.AsEnumerable().Where(x => Convert.ToString(x["TYPE"]) == Convert.ToString(dr["CATEGORY"])).Select(x => x).Count() > 0)
                    {
                        DataRow row = Contract.AsEnumerable().Where(x => Convert.ToString(x["TYPE"]) == Convert.ToString(dr["CATEGORY"])).Select(x => x).First();
                        if (row != null)
                        {
                            Dr = Contract.DefaultView[Contract.Rows.IndexOf(row)];
                        }
                    }
                }
            }
            catch (Exception Ex)
            {

                MessageBox.Show(Ex.Message.ToString());
                e.Handled = true;
                //throw;
            }
        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {
                
                throw;
            }

        }

        private void Close_click(object sender, RoutedEventArgs e)
        {

            Window.GetWindow(this).Close();
           
        }

        private void PrintClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Dtdata != null && Dtdata.Rows.Count > 0)
                {
                    CrptOrderDetail objCrptOrderDetail = new CrptOrderDetail();
                    DataTable dt = Dtdata.Copy();
                    dt.TableName = "ORDER_DETAIL";
                    objCrptOrderDetail.SetDataSource(dt);
                    objCrptOrderDetail.SetParameterValue("@ReportNAME", "Order Detail");
                    objCrptOrderDetail.SetParameterValue("TitleNAME", "Customer");
                    UcReportViewer objucReportViewer = new UcReportViewer();
                    objucReportViewer.Height = 500;
                    objucReportViewer.Width = 1000;
                    objucReportViewer.ReportSource = objCrptOrderDetail;
                    objucReportViewer.Title = "Order";
                    objucReportViewer.ShowDialog();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString());
                e.Handled = true;
                //throw;
            }
        }

        //private void me()
        //{
        //    int a;
        //    int b = 9;
        //    name(out a, ref b);
           
        //}
        //private void name(out int a, ref int b)
        //{
        //    a = 0;

        //}
    }
}
