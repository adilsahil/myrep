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
using System.Windows.Media.Animation;
using System.Data;
using System.Windows.Threading;

namespace Masters
{
    /// <summary>
    /// Interaction logic for UIProgressBar.xaml
    /// </summary>
    public partial class UIProgressBar : UserControl
    {
        public bool IsCompleted { get; set; }
        public UIProgressBar()
        {
            InitializeComponent();
            StartBagroundWorker(1);
        }
        public UIProgressBar(int TotalCount)
        {
            InitializeComponent();
            StartBagroundWorker(TotalCount);
        }
        
        //ProgressBar Style
        Storyboard inlineZoomOutAnimation = null;
        void inlineZoomOutAnimation_Completed(object sender, EventArgs e)
        {
            inline.Child = null;
            inlineBorder.Visibility = Visibility.Collapsed;
            dimmer.Visibility = Visibility.Collapsed;
            inlineZoomOutAnimation.Completed -= new EventHandler(inlineZoomOutAnimation_Completed);
        }
        private void Stop_Animation()
        {
            inlineZoomOutAnimation = (Storyboard)this.Resources["InlineZoomOutAnimation"];
            inlineZoomOutAnimation.Completed += new EventHandler(inlineZoomOutAnimation_Completed);
            inlineZoomOutAnimation.Begin(inlineBorder);

        }
        private void StartBagroundWorker(int TotalCount)
        {
            Storyboard _inlineAnimation;
            //lblModuleProgress.Text = "Initializing data for ";// +Enum.GetName(typeof(Module), SelectedModule);

            _inlineAnimation = (Storyboard)Resources["InlineAnimation"];
            inlineBorder.Visibility = Visibility.Visible;

            StackPanel stp = new StackPanel();
            ProgressBar pb = new ProgressBar();
            pb.Style = (Style)this.Resources["LongProcessProgressBar"];
            pb.IsIndeterminate = true;
            pb.Height = 20;
            pb.Margin = new Thickness(5);
            pb.Width = 400;

            stp.Children.Add(pb);

            TextBlock txb = new TextBlock();
            txb.Text = "data saving in progress..........";
            txb.Foreground = new SolidColorBrush(Colors.Aqua);
            txb.FontWeight = FontWeights.Bold;
            txb.HorizontalAlignment = HorizontalAlignment.Center;
            txb.Margin = new Thickness(5);
            txb.FontStyle = FontStyles.Italic;
            txb.FontSize = 20;

            stp.Children.Add(txb);

            txbCount = new TextBlock();
            txbCount.Text = 1 + " " + "Entry out of" + " " + TotalCount;
            txbCount.Foreground = new SolidColorBrush(Colors.Aqua);
            txbCount.FontWeight = FontWeights.Bold;
            txbCount.HorizontalAlignment = HorizontalAlignment.Center;
            txbCount.Margin = new Thickness(5);
            txbCount.FontStyle = FontStyles.Italic;
            txbCount.FontSize = 20;

            stp.Children.Add(txbCount);

            inline.Background = new SolidColorBrush(Colors.WhiteSmoke);
            inline.Child = stp; //new Border() { Background = new SolidColorBrush(Colors.Red), Width = 200, Height = 200 };
            dimmer.Visibility = Visibility.Visible;
            dimmer.IsHitTestVisible = true;
            dimmer.Opacity = 1;
            _inlineAnimation.Begin(inlineBorder);


        }
        TextBlock txbCount = null;
       
      

        //public UIProgressBar(DataTable DtAllocateData, ref bool isSuccess, string[] strBillnArray,
        //    DataRow DrvSelectedIPSchemeTemp, DataRowView DrvSelectedIPScheme, CommonSchemeChange objComSchemeChange, long IncoMastSchemeId,
        //    int _sourceCount, long PatFinEncounterId)
        //{
        //    try
        //    {
        //        int TotalCount = 0, BillCount = 1;
        //        if (strBillnArray.Length > 0)
        //        {
        //            TotalCount = strBillnArray.Count();
        //        }
        //        InitializeComponent();
        //        StartBagroundWorker(TotalCount);
        //        string GenPatBillingID = string.Empty;
        //        List<DataSet> ListNewSchem = null;
        //        List<DataSet> ListOldScheme = new List<DataSet>();
        //        DataSet dsOldScheme = new DataSet();
        //        foreach (string stritem in strBillnArray)
        //        {
        //            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
        //              txbCount.Text = BillCount + " " + "Bill out of" + " " + TotalCount));
        //            DataTable DtAllocateDataTemp = DtAllocateData.Copy().Select("BILL_NO ='" + stritem + "'").CopyToDataTable();
        //            string[] GenPatBillingIDArray = (from sl in DtAllocateDataTemp.AsEnumerable()
        //                                             select Convert.ToString(sl["GEN_PAT_BILLING_ID"])).Distinct().ToArray();
        //            if (GenPatBillingIDArray.Length > 0)
        //            {
        //                GenPatBillingID = String.Join(",", GenPatBillingIDArray);
        //            }
        //            ListNewSchem = objComSchemeChange.FilterBillBasedOnServiceType(DtAllocateDataTemp, Convert.ToString(DrvSelectedIPSchemeTemp["INCO_MAST_SCHEME_ID"]), Convert.ToString(DrvSelectedIPSchemeTemp["INCO_DTLS_SCHEME_ID"]));

        //            if (!dsOldScheme.Tables.Contains("GEN_APPLICATION_SETTING"))
        //            {
        //                DataTable dtemp = new DataTable();
        //                dtemp = CommonData.dsGlobal.Tables["GEN_APPLICATION_SETTING"];
        //                dtemp.TableName = "GEN_APPLICATION_SETTING";
        //                dsOldScheme.Tables.Add(dtemp.Copy());
        //                ListOldScheme.Add(dsOldScheme.Copy());
        //            }

        //            if (BillCount == TotalCount)
        //            {
        //                ListOldScheme = new List<DataSet>();
        //                DataTable dtOldScheme = new DataTable("PATIENT_VISIT_ENCOUNTER");
        //                dtOldScheme.Columns.Add("PAT_PATIENT_VISIT_ENCOUNTER_ID", typeof(decimal));
        //                dtOldScheme.Columns.Add("PAT_FIN_ENCOUNTER_ID", typeof(decimal));
        //                dtOldScheme.Columns.Add("INCO_PATIENT_SCHEME_ID", typeof(decimal));
        //                dtOldScheme.Columns.Add("GEN_AUDIT_ID", typeof(decimal));
        //                dtOldScheme.Columns.Add("BILLED_STATUS", typeof(decimal));
        //                dtOldScheme.Columns.Add("MODE", typeof(decimal));
        //                DataTable dtEncounter = new DataTable();
        //                dtEncounter.Columns.Add("PAT_FIN_ENCOUNTER_ID", typeof(decimal));
        //                foreach (DataSet dsData in ListNewSchem)
        //                {
        //                    DataRow dr = dtEncounter.NewRow();
        //                    dr["PAT_FIN_ENCOUNTER_ID"] = dsData.Tables["GEN_PAT_BILLING_OLD"].Rows[0]["PAT_FIN_ENCOUNTER_ID"];
        //                    dtEncounter.Rows.Add(dr);
        //                }
        //                DataTable dtGroupsPatFinEncounter = dtEncounter.DefaultView.ToTable(true, new string[] { "PAT_FIN_ENCOUNTER_ID" });
        //                foreach (DataRow drTemp in dtGroupsPatFinEncounter.Rows)
        //                {
        //                    DataRow drOldScheme = dtOldScheme.NewRow();
        //                    drOldScheme["PAT_PATIENT_VISIT_ENCOUNTER_ID"] = -1;
        //                    drOldScheme["PAT_FIN_ENCOUNTER_ID"] = drTemp["PAT_FIN_ENCOUNTER_ID"];
        //                    drOldScheme["INCO_PATIENT_SCHEME_ID"] = DrvSelectedIPSchemeTemp["INCO_PATIENT_SCHEME_ID"];
        //                    drOldScheme["BILLED_STATUS"] = 0;
        //                    drOldScheme["MODE"] = 1;
        //                    dtOldScheme.Rows.Add(drOldScheme);
        //                }
        //                if (_sourceCount == 0 && IncoMastSchemeId != Convert.ToInt64(DrvSelectedIPScheme["INCO_MAST_SCHEME_ID"]) &&
        //                    ListNewSchem.Any() && ListNewSchem[0].Tables.Contains("BILL_COMMON_DETAILS") && ListNewSchem[0].Tables["BILL_COMMON_DETAILS"].Columns.Contains("VISITMODE"))
        //                {
        //                    DataTable dtPatFinEounterUpdate = new DataTable();
        //                    dtPatFinEounterUpdate.Columns.Add("INCO_MAST_SCHEME_ID", typeof(long));
        //                    dtPatFinEounterUpdate.Columns.Add("ENCOUNTER_DEFAULT_SCHEME_ID", typeof(long));
        //                    dtPatFinEounterUpdate.Columns.Add("MODE", typeof(int));
        //                    dtPatFinEounterUpdate.Columns.Add("PAT_FIN_ENCOUNTER_ID", typeof(long));
        //                    dtPatFinEounterUpdate.Columns.Add("VISIT_MODE", typeof(int));
        //                    dtPatFinEounterUpdate.Columns.Add("LAST_UPDATED_BY", typeof(string));
        //                    dtPatFinEounterUpdate.Rows.Add(Convert.ToInt64(DrvSelectedIPScheme["INCO_MAST_SCHEME_ID"]), Convert.ToInt64(DrvSelectedIPScheme["INCO_DTLS_SCHEME_ID"]), 17, PatFinEncounterId,
        //                        ListNewSchem[0].Tables["BILL_COMMON_DETAILS"].Rows[0]["VISITMODE"], CommonData.LoggedInUser);
        //                    dtPatFinEounterUpdate.TableName = "UPDATE_PAT_FIN_ENCOUNTER";
        //                    foreach (DataSet dsSet in ListNewSchem)
        //                    {
        //                        dsSet.Tables.Add(dtPatFinEounterUpdate.Copy());
        //                    }

        //                }
        //                dsOldScheme.Tables.Add(dtOldScheme.Copy());
        //                // DataTable dtOldData = DtViewBillDtls.Select("GEN_PAT_BILLING_ID IN (" + GenPatBillingID + ") ").CopyToDataTable();
        //                // List<DataSet> ListOldSchem = FilterBillBasedOnServiceType(dtOldData, Convert.ToString(CommonData.DefaultScheme), Convert.ToString(CommonData.DefaultScheme));
        //                if (!dsOldScheme.Tables.Contains("GEN_APPLICATION_SETTING"))
        //                {
        //                    DataTable dtemp = new DataTable();
        //                    dtemp = CommonData.dsGlobal.Tables["GEN_APPLICATION_SETTING"];
        //                    dtemp.TableName = "GEN_APPLICATION_SETTING";
        //                    dsOldScheme.Tables.Add(dtemp.Copy());
        //                    ListOldScheme.Add(dsOldScheme.Copy());
        //                }
        //                ListOldScheme.Add(dsOldScheme);

        //            }
        //            else
        //            {
        //                BillCount++;
        //            }
        //            MainFinalBilling objMainFb = new MainFinalBilling();
        //            isSuccess = objMainFb.SaveIPAllocation(ListNewSchem, ListOldScheme, GenPatBillingID); // Common save for Scheme Change.
        //            if (isSuccess)
        //            {
        //                foreach (DataSet dsSet in ListNewSchem)
        //                {
        //                    if (dsSet.Tables.Contains("CON_PAT_BILLING"))
        //                    {
        //                        {
        //                            DataTable DtCriteria = new DataTable();
        //                            DtCriteria.Columns.Add("MODE");
        //                            DtCriteria.Columns.Add("PAT_FIN_ENCOUNTER_ID");
        //                            DtCriteria.Rows.Add(2, dsSet.Tables["CON_PAT_BILLING"].Rows[0]["PAT_FIN_ENCOUNTER_ID"]);
        //                            MainCommon objMainCommon = new MainCommon();
        //                            DataTable DtEncounter = objMainCommon.SelectFinancialEncounter(DtCriteria);
        //                            if (DtEncounter != null && DtEncounter.Rows.Count > 0 && DtEncounter.Rows[0]["PAT_FIN_EPISODE_ID"] != DBNull.Value)
        //                            {
        //                                DtCriteria = new DataTable();
        //                                DtCriteria.Columns.Add("MODE");
        //                                DtCriteria.Columns.Add("PAT_FIN_EPISODE_ID");
        //                                DtCriteria.Rows.Add(5, DtEncounter.Rows[0]["PAT_FIN_EPISODE_ID"]);
        //                                DataTable DtEpisode = objMainCommon.FetchFinancialEpisodes(DtCriteria);
        //                                if (DtEpisode != null && DtEpisode.Rows.Count > 0 && Convert.ToInt16(DtEpisode.Rows[0]["EPISODE_MODE"]) != 0)
        //                                {
        //                                    MainFinalBilling objMainFinalBilling = new MainFinalBilling();
        //                                    if (DtEncounter.Rows[0]["ISFREE"] != DBNull.Value && Convert.ToInt16(DtEncounter.Rows[0]["ISFREE"]) == 1)
        //                                    {
        //                                    }
        //                                    else
        //                                    {
        //                                        DataTable DtBillCommonDetails = new DataTable();
        //                                        DtBillCommonDetails.Columns.Add("FREE_DAYS");
        //                                        DtBillCommonDetails.Columns.Add("FREE_VISIT_ELIGIBLE");
        //                                        DtBillCommonDetails.Columns.Add("EPISODE_END_DATE");
        //                                        DtBillCommonDetails.Rows.Add();
        //                                        MainMasterSettings objMainMaster = new MainMasterSettings();
        //                                        DtCriteria = new DataTable();
        //                                        DtCriteria.Columns.Add("INCO_MAST_SCHEME_ID");
        //                                        DtCriteria.Columns.Add("INV_MAST_SERVICE_ID");
        //                                        DtCriteria.Columns.Add("MODE");
        //                                        DtCriteria.Rows.Add(DtEncounter.Rows[0]["INCO_MAST_SCHEME_ID"], dsSet.Tables["CON_PAT_BILLING"].Rows[0]["INV_MAST_SERVICE_ID"], 1);
        //                                        DataTable dtEpisodeDuration = objMainMaster.FetchEpisodeDuration(DtCriteria);
        //                                        if (dtEpisodeDuration.Rows[0]["EPISODE_DAYS"] != DBNull.Value)
        //                                        {
        //                                            DtBillCommonDetails.Rows[0]["FREE_DAYS"] = dtEpisodeDuration.Rows[0]["FREE_DAYS"] != DBNull.Value ? Convert.ToInt32(dtEpisodeDuration.Rows[0]["FREE_DAYS"]) : 0;
        //                                            DtBillCommonDetails.Rows[0]["FREE_VISIT_ELIGIBLE"] = dtEpisodeDuration.Rows[0]["FREE_VISIT"] != DBNull.Value ? Convert.ToInt32(dtEpisodeDuration.Rows[0]["FREE_VISIT"]) : 0;
        //                                        }
        //                                        else
        //                                        {
        //                                            DataSet DsConTypes = objMainMaster.FetchProviderConTypes(Convert.ToString(DtEncounter.Rows[0]["PROVIDER_ID"]));
        //                                            if (DsConTypes.Tables.Contains("FETCH_CON_MAST_TYPE_RULE"))
        //                                            {
        //                                                {
        //                                                    DtCriteria = new DataTable();
        //                                                    DtCriteria.Columns.Add("PAT_FIN_ENCOUNTER_ID");
        //                                                    DtCriteria.Columns.Add("MODE");
        //                                                    DtCriteria.Rows.Add(DtEncounter.Rows[0]["PAT_FIN_ENCOUNTER_ID"], 12);
        //                                                    DataTable DtConPatBilling = objMainFinalBilling.FetchConPatBilling(DtCriteria);
        //                                                    DataTable DtConTypes = DsConTypes.Tables["FETCH_CON_MAST_TYPE_RULE"].Copy();
        //                                                    var resPorvider = DtConTypes.AsEnumerable().Where(row => row["CON_TYPE_ID"] != DBNull.Value && Convert.ToInt64(row["CON_TYPE_ID"]) == Convert.ToInt64(DtConPatBilling.Rows[0]["CON_TYPE_ID"]));
        //                                                    if (resPorvider.Count() > 0)
        //                                                    {
        //                                                        DataTable dt = resPorvider.CopyToDataTable();
        //                                                        DtBillCommonDetails.Rows[0]["FREE_DAYS"] = dt.Rows[0]["FREE_DAYS"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["FREE_DAYS"]) : 0;
        //                                                        DtBillCommonDetails.Rows[0]["FREE_VISIT_ELIGIBLE"] = dt.Rows[0]["FREE_VISIT_NO"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["FREE_VISIT_NO"]) : 0;
        //                                                    }
        //                                                }
        //                                            }
        //                                            else
        //                                            {
        //                                                DataSet DsDeptConTypes = objMainMaster.FetchDepartmentConTypes(Convert.ToString(DtEncounter.Rows[0]["GEN_DEPARTMENT_ID"]), Convert.ToString(DtEncounter.Rows[0]["PROVIDER_ID"]));
        //                                                if (DsDeptConTypes.Tables.Contains("FETCH_CON_MAST_TYPE_RULE"))
        //                                                {
        //                                                    DtCriteria = new DataTable();
        //                                                    DtCriteria.Columns.Add("PAT_FIN_ENCOUNTER_ID");
        //                                                    DtCriteria.Columns.Add("MODE");
        //                                                    DtCriteria.Rows.Add(DtEncounter.Rows[0]["PAT_FIN_ENCOUNTER_ID"], 12);
        //                                                    DataTable DtConPatBilling = objMainFinalBilling.FetchConPatBilling(DtCriteria);
        //                                                    DataTable DtConTypes = DsDeptConTypes.Tables["FETCH_CON_MAST_TYPE_RULE"].Copy();
        //                                                    var resPorvider = DtConTypes.AsEnumerable().Where(row => row["CON_TYPE_ID"] != DBNull.Value && Convert.ToInt32(row["CON_TYPE_ID"]) == Convert.ToInt64(DtConPatBilling.Rows[0]["CON_TYPE_ID"]));
        //                                                    if (resPorvider.Count() > 0)
        //                                                    {
        //                                                        DataTable dt = resPorvider.CopyToDataTable();
        //                                                        DtBillCommonDetails.Rows[0]["FREE_DAYS"] = dt.Rows[0]["FREE_DAYS"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["FREE_DAYS"]) : 0;
        //                                                        DtBillCommonDetails.Rows[0]["FREE_VISIT_ELIGIBLE"] = dt.Rows[0]["FREE_VISIT_NO"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["FREE_VISIT_NO"]) : 0;
        //                                                    }
        //                                                }
        //                                            }
        //                                        }
        //                                        if (DtBillCommonDetails != null && DtBillCommonDetails.Rows.Count > 0
        //                                            && DtBillCommonDetails.Rows[0]["FREE_DAYS"] != DBNull.Value &&
        //                                             DtBillCommonDetails.Rows[0]["FREE_VISIT_ELIGIBLE"] != DBNull.Value)
        //                                        {
        //                                            DataTable Dt = objMainCommon.SelectFinDtlsEpisode(DtEncounter);
        //                                            DtBillCommonDetails.Columns.Add("PAT_FIN_DTLS_EPISODE_ID");
        //                                            DtBillCommonDetails.Columns.Add("MODE");
        //                                            DtBillCommonDetails.Rows[0]["PAT_FIN_DTLS_EPISODE_ID"] = Dt.Rows[0]["PAT_FIN_DTLS_EPISODE_ID"];
        //                                            DtBillCommonDetails.Rows[0]["MODE"] = 2;
        //                                            objMainCommon.UpdateFinancialDtlsEpisode(DtBillCommonDetails);
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        IsCompleted = isSuccess;

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public void DisplayCount(int TotalCount, int BillCount)
        {
            try
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                     txbCount.Text = BillCount + " " + "Entry out of" + " " + TotalCount));
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void CloseWindow()
        {
            try
            {
                Stop_Animation();
                Window.GetWindow(this).Close();
            }
            catch (Exception)
            {

                throw;
            }
        }
      
    }
}
