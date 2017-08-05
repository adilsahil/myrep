using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Infologics.Medilogics.CommonClient.Controls.CommonWindow;
using Infologics.Medilogics.Enumerators.General;
using System.ComponentModel;
using Infologics.Medilogics.CommonSharedUI.CPOEMedication;
using Infologics.Medilogics.CommonSharedUI.CPOEMedication.CPOEMedicineAdministrationView;
using Infologics.Medilogics.CommonClient.Controls.StaticData;
using Infologics.Medilogics.General.Control;

namespace Infologics.Medilogics.Billing.UIControls.Billing
{
    /// <summary>
    /// Interaction logic for UIServiceDeliveryDescription.xaml
    /// </summary>
    public partial class UIServiceDeliveryDescription : UserControl, INotifyPropertyChanged
    {
        public UIServiceDeliveryDescription()
        {
            InitializeComponent();
            DataTable dtGenApp = CommonData.GetDefaultSettings(0, "IS_VISIBILE_LOCATION");
            if (dtGenApp != null && dtGenApp.Rows.Count > 0&&Convert.ToInt16(dtGenApp.Rows[0]["VALUE"])==1)
            {
                IsLocationVisible = true;
            }
            DataTable dtGenSetting = CommonData.GetDefaultSettings(0, "IS_VERIFICATION_REQUIRED");
            if (dtGenSetting.KIIsNotNullAndRowCount() && Convert.ToInt16(dtGenSetting.Rows[0]["VALUE"]) == 1)
            {
                EnableVerificationSettingBased = true;
            }
            else
            {
                EnableVerificationSettingBased = false;
            }
            HasChangeToZeroCharge = CommonData.CheckPrivilege("FO_BILLING_CHANGE_TO_ZERO_CHARGE");
            IsFromPharmacy = CommonData.SelectedModule == Module.Pharmacy ? true:false;
        }

        public DataTable dtMedicationdtls { get; set; }



        /// <summary>
        /// 
        /// </summary>
        private bool hasChangeToZeroCharge;
        public bool HasChangeToZeroCharge
        {
            get
            {
                return hasChangeToZeroCharge;
            }
            set
            {
                hasChangeToZeroCharge = value;
                OnPropertyChanged("HasChangeToZeroCharge");
            }
        }

        private bool isCheckCommonSale;
        public bool IsCheckCommonSale
        {
            get
            {
                return isCheckCommonSale;
            }
            set
            {
                isCheckCommonSale = value;
                OnPropertyChanged("IsCheckCommonSale");
            }
        }

        /// <summary>
        /// EnableVerification Property
        /// </summary>
        private bool enableVerificationSettingBased;
        public bool EnableVerificationSettingBased
        {
            get { return enableVerificationSettingBased; }
            set
            {
                enableVerificationSettingBased = value;
                OnPropertyChanged("EnableVerificationSettingBased");
                if (!EnableVerificationSettingBased)
                {
                    if (grdDeliveryDescription.Columns.Contains(clnVerification))
                    {
                        grdDeliveryDescription.Columns.Remove(clnVerification);
                    }
                }
            }
        }

        /// <summary>
        /// IsFromPharmacy Property
        /// </summary>
        private bool isFromPharmacy;
        public bool IsFromPharmacy
        {
            get { return isFromPharmacy; }
            set
            {
                isFromPharmacy = value;
                OnPropertyChanged("IsFromPharmacy");
                if (IsFromPharmacy)
                {
                    if (grdDeliveryDescription.Columns.Contains(clnLocation))
                    {
                        grdDeliveryDescription.Columns.Remove(clnLocation);
                    }
                }
            }
        }

        /// <summary>
        /// IsLocationVisible Property
        /// </summary>
        private bool isLocationVisible;
        public bool IsLocationVisible
        {
            get { return isLocationVisible; }
            set
            {
                isLocationVisible = value;
                OnPropertyChanged("IsLocationVisible");
            }
        }

        /// <summary>
        /// IsServiceSettingEnable Property
        /// </summary>
        private bool isServiceSettingEnable;
        public bool IsServiceSettingEnable
        {
            get { return isServiceSettingEnable; }
            set
            {
                isServiceSettingEnable = value;
                OnPropertyChanged("IsServiceSettingEnable");
                if (!IsServiceSettingEnable)
                {
                    //if (grdDeliveryDescription.Columns.Contains(clnAdjustment))
                    //{
                    //    grdDeliveryDescription.Columns.Remove(clnAdjustment);
                    //}
                    if (grdDeliveryDescription.Columns.Contains(clnNet))
                    {
                        grdDeliveryDescription.Columns.Remove(clnNet);
                    }
                    if (grdDeliveryDescription.Columns.Contains(clnPatientShare))
                    {
                        grdDeliveryDescription.Columns.Remove(clnPatientShare);
                    }
                    if (grdDeliveryDescription.Columns.Contains(clnSponsor))
                    {
                        grdDeliveryDescription.Columns.Remove(clnSponsor);
                    }
                }
            }
        }

        /// <summary>
        /// IsPhamracySettingEnable Property
        /// </summary>
        private bool isPhamracySettingEnable;
        public bool IsPhamracySettingEnable
        {
            get { return isPhamracySettingEnable; }
            set
            {
                isPhamracySettingEnable = value;
                OnPropertyChanged("IsPhamracySettingEnable");
                if (!IsPhamracySettingEnable)
                {
                    //if (grdDeliveryDescription.Columns.Contains(clnAdjustment))
                    //{
                    //    grdDeliveryDescription.Columns.Remove(clnAdjustment);
                    //}
                    if (grdDeliveryDescription.Columns.Contains(clnNet))
                    {
                        grdDeliveryDescription.Columns.Remove(clnNet);
                    }
                    if (grdDeliveryDescription.Columns.Contains(clnPatientShare))
                    {
                        grdDeliveryDescription.Columns.Remove(clnPatientShare);
                    }
                    if (grdDeliveryDescription.Columns.Contains(clnSponsor))
                    {
                        grdDeliveryDescription.Columns.Remove(clnSponsor);
                    }
                }
            }
        }

        private void btnQty_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox textbox = (TextBox)sender;
                DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
                Int32 qty = 0;
                bool isallow = true;
                if (textbox.Text != string.Empty)
                {
                    qty = Convert.ToInt32(textbox.Text);
                }
                if (drSelected != null)
                {
                    if (textbox.Text != string.Empty && Convert.ToInt32(drSelected["QTY"]) != qty)
                    {
                        drSelected["QTY"] = textbox.Text;
                    }
                    else if (textbox.Text == string.Empty)
                    {
                        drSelected["QTY"] = 1;
                    }
                    else
                    {
                        isallow = false;
                    }
                }
                if (isallow)
                {
                    CommandBinding objCommand = new CommandBinding();
                    objCommand.Command = UIControls.BillingNew.Command.BillingCommands.QtySelectionChange;
                    objCommand.Command.Execute(drSelected);
                }
                if (drSelected != null && drSelected.IsEdit==true)
                {
                    string tempBatchNo = Convert.ToString(drSelected["BATCHNO"]);
                    string tempUnitofQty = Convert.ToString(drSelected["PH_UNIT_SALES_CONVERSION_ID"]);
                    drSelected.EndEdit();
                    drSelected["BATCHNO"] = tempBatchNo;
                    drSelected["PH_UNIT_SALES_CONVERSION_ID"] = tempUnitofQty;
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }

        private void btnDelete_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btnDelete = (Button)sender;
                DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
                CommandBinding objCommand = new CommandBinding();
                objCommand.Command = UIControls.BillingNew.Command.BillingCommands.ServiceDelete;
                objCommand.Command.Execute(drSelected);
            }
            catch (Exception)
            {                
                throw;
            }            
        }

        private void btnIsverification_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox chkVerificationClick = (CheckBox)sender;
                DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
                CommandBinding objCommand = new CommandBinding();
                objCommand.Command = UIControls.BillingNew.Command.BillingCommands.IsVerificationChecked;
                if (chkVerificationClick.IsChecked == true)
                    drSelected["ISVERIFICATION"] = true;
                else
                    drSelected["ISVERIFICATION"] = false;
                objCommand.Command.Execute(drSelected);
            }
            catch (Exception)
            {                
                throw;
            }            
        }

        private void btnSale_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox chkSale = (CheckBox)sender;
                DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
                CommandBinding objCommand = new CommandBinding();
                objCommand.Command = UIControls.BillingNew.Command.BillingCommands.IsSaveChecked;
                if (chkSale.IsChecked == true)
                    drSelected["ISSALE"] = true;
                else
                    drSelected["ISSALE"] = false;
                IsCheckCommonSale = false;
                objCommand.Command.Execute(drSelected);
            }
            catch (Exception)
            {                
                throw;
            }            
        }

        private void chkCommonSale_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox chkSale = (CheckBox)sender;
                DataRowView drSelected = CreateDataRowView(Convert.ToBoolean(chkSale.IsChecked));
                CommandBinding objCommand = new CommandBinding();
                objCommand.Command = UIControls.BillingNew.Command.BillingCommands.CommonSaleClick;
                objCommand.Command.Execute(drSelected);
            }
            catch (Exception)
            {                
                throw;
            }            
        }

        private void chkVerification_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox chkVerification = (CheckBox)sender;
                DataRowView drSelected = CreateDataRowView(Convert.ToBoolean(chkVerification.IsChecked));
                CommandBinding objCommand = new CommandBinding();
                objCommand.Command = UIControls.BillingNew.Command.BillingCommands.CommonVerificationClick;
                objCommand.Command.Execute(drSelected);
            }
            catch (Exception)
            {                
                throw;
            }            
        }

        private DataRowView CreateDataRowView(bool IsChecked)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ISCHECK");
                dt.Rows.Add(IsChecked);
                DataRowView drv = dt.DefaultView[dt.Rows.IndexOf(dt.Rows[0])];
                return drv;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void mnuOptions_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void mnuOptions_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void menuViewDrugInfo(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.menuViewDrugInfo;
            objCommand.Command.Execute(drSelected);
        }

        private void menuOrderInfo(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.menuOrderInfo;
            objCommand.Command.Execute(drSelected);
        }

        private void menuMARView(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.menuMARView;
            objCommand.Command.Execute(drSelected);
        }

        private void EditPharmacy_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.EditPharmacy_Click;
            objCommand.Command.Execute(drSelected);
        }

        private void menuIntervention(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.menuIntervention;
            objCommand.Command.Execute(drSelected);
        }

        private void menuHold(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.menuHold;
            objCommand.Command.Execute(drSelected);
        }

        private void menuPharmacyEducation(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.menuPharmacyEducation;
            objCommand.Command.Execute(drSelected);
        }

        private void MenuIssueRemark(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.MenuIssueRemark;
            objCommand.Command.Execute(drSelected);
        }

        private void Pharmacy_Note(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.PharmacyNote;
            objCommand.Command.Execute(drSelected);
        }


        /// <summary>
        /// SelectedPharmacyUnitType Property
        /// </summary>
        private DataRowView selectedPharmacyUnitType;
        public DataRowView SelectedPharmacyUnitType
        {
            get { return selectedPharmacyUnitType; }
            set
            {
                selectedPharmacyUnitType = value;
                OnPropertyChanged("SelectedPharmacyUnitType");
            }
        }

        private bool isArrangeServiceOrderColumn;
        public bool IsArrangeServiceOrderColumn
        {
            get
            {
                return isArrangeServiceOrderColumn;
            }
            set
            {
                isArrangeServiceOrderColumn = value;
                OnPropertyChanged("IsArrangeServiceOrderColumn");
                OnHideColumn();
            }
        }

        private bool isfromPharmacyPanel = false;
        public bool IsFromPharmacyPanel
        {
            get
            {
                return isfromPharmacyPanel;
            }
            set
            {
                isfromPharmacyPanel = value;
                OnPropertyChanged("IsFromPharmacyPanel");
            }
        }


       /// <summary>
       /// Hiding Column
       /// </summary>
        public void OnHideColumn()
        {
            try
            {
                //UIServiceDeliveryDescription control = source as UIServiceDeliveryDescription;
                int ColumnIndex = 0;
               
                if (IsArrangeServiceOrderColumn)
                {
                    //foreach (var item in ((DataGrid)control.grdDeliveryDescription).Columns)
                    //{
                    //ColumnIndex++;
                    //if ((control.IsDonor && ColumnIndex == 2)
                    //    || (control.IsDonor && ColumnIndex == 3)
                    //    || (control.IsDonor && ColumnIndex == 4)
                    //    || (!control.IsDonor && ColumnIndex == 3)
                    //    || (!control.IsDonor && ColumnIndex == 8)
                    //    || (!control.IsDonor && ColumnIndex == 9)
                    //    || (!control.IsDonor && ColumnIndex == 10)
                    //    || (!control.IsDonor && ColumnIndex == 11)
                    //    || (!control.IsDonor && ColumnIndex == 13)
                    //    || (!control.IsDonor && ColumnIndex == 16))
                    //{
                    //    item.MinWidth = 0;
                    //    item.Width = 0;
                    //}
                    //}
                    //if (grdDeliveryDescription.Columns.Contains(clnMenu))
                    //{
                    //    grdDeliveryDescription.Columns.Remove(clnMenu);
                    //}
                    if (grdDeliveryDescription.Columns.Contains(clnQtyOD))
                    {
                        grdDeliveryDescription.Columns.Remove(clnQtyOD);
                    }
                    //if (grdDeliveryDescription.Columns.Contains(clnOrderDate))
                    //{
                    //    grdDeliveryDescription.Columns.Remove(clnOrderDate);
                    //}
                    //commented for enabling the batch in pharmacy service group billing
                    //if (grdDeliveryDescription.Columns.Contains(clnBatch))
                    //{
                    //    grdDeliveryDescription.Columns.Remove(clnBatch);
                    //}
                    //if (grdDeliveryDescription.Columns.Contains(clnUnitofQty))
                    //{
                    //    grdDeliveryDescription.Columns.Remove(clnUnitofQty);
                    //}

                    if (grdDeliveryDescription.Columns.Contains(clnFrequency))
                    {
                        grdDeliveryDescription.Columns.Remove(clnFrequency);
                    }
                   
                    if (grdDeliveryDescription.Columns.Contains(clnVerification))
                    {
                        grdDeliveryDescription.Columns.Remove(clnVerification);
                    }
                    if (grdDeliveryDescription.Columns.Contains(clnduration))
                    {
                        grdDeliveryDescription.Columns.Remove(clnduration);
                    }
                    if (grdDeliveryDescription.Columns.Contains(clnDose))
                    {
                        grdDeliveryDescription.Columns.Remove(clnDose);
                    }
                    if (grdDeliveryDescription.Columns.Contains(clnAllergy))
                    {
                        grdDeliveryDescription.Columns.Remove(clnAllergy);
                    }
                    if (grdDeliveryDescription.Columns.Contains(clnLabel))
                    {
                        grdDeliveryDescription.Columns.Remove(clnLabel);
                    }
                    if (grdDeliveryDescription.Columns.Contains(clnLocation) && !IsLocationVisible)
                    {
                        grdDeliveryDescription.Columns.Remove(clnLocation);
                    }
                    if (grdDeliveryDescription.Columns.Contains(clnExpDate))
                    {
                        grdDeliveryDescription.Columns.Remove(clnExpDate);
                    }
                }
                else
                {
                    //foreach (var item in ((DataGrid)grdDeliveryDescription).Columns)
                    //{
                    //    ColumnIndex++;
                    //    if (!IsDonor && ColumnIndex == 7)
                    //    {
                    //        item.MinWidth = 0;
                    //        item.Width = 0;
                    //    }
                    //}

                    if (grdDeliveryDescription.Columns.Contains(clnLocation))
                    {
                        grdDeliveryDescription.Columns.Remove(clnLocation);
                    }
                    if (grdDeliveryDescription.Columns.Contains(clnPropDate))
                    {
                        grdDeliveryDescription.Columns.Remove(clnPropDate);
                    }
                    if (!IsFromPharmacyPanel)
                    {
                        if (grdDeliveryDescription.Columns.Contains(clnLabel))
                        {
                            grdDeliveryDescription.Columns.Remove(clnLabel);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion


        public bool IsDonor
        {
            get { return (bool)GetValue(IsDonorProperty); }
            set { SetValue(IsDonorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDonor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDonorProperty =
            DependencyProperty.Register("IsDonor", typeof(bool), typeof(UIServiceDeliveryDescription), new UIPropertyMetadata(false));



        private void cbxSelection_Changed(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBox cbxVerification = (ComboBox)sender;

                //string str = Convert.ToString(cbxVerification.SelectedValue);
                DataRowView drSelectedRow = (DataRowView)cbxVerification.SelectedItem;
                DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
                if (drSelected != null && drSelected.Row.Table.Columns.Contains("IS_CONSUMPTION") &&
                    Convert.ToInt16(drSelected["SERVICE_TYPE"]) == 4 && Convert.ToInt16(drSelected["IS_CONSUMPTION"]) != 0)
                {
                    return;
                }
                else
                {
                    bool isallow = true;
                    if (drSelected != null && drSelected.Row["BATCHNO"] != DBNull.Value && cbxVerification.Text != string.Empty &&
                        Convert.ToString(drSelected.Row["BATCHNO"]) == Convert.ToString(cbxVerification.Text))
                    {
                        isallow = false;
                    }
                    if (drSelectedRow != null && drSelectedRow.Row["BATCHNO"] != string.Empty && drSelectedRow.Row["BATCHNO"] != "" && isallow)
                    {
                        drSelected.Row["BATCHNO"] = drSelectedRow.Row["BATCHNO"];
                        int s = (int)cbxVerification.SelectedIndex;
                        CommandBinding objCommand = new CommandBinding();
                        objCommand.Command = UIControls.BillingNew.Command.BillingCommands.GridBatchSelectionChanged;
                        objCommand.Command.Execute(drSelected);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }            
        }

        private void MenuDrugDetail(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.SerivceDetails;
            objCommand.Command.Execute(drSelected);
        }

        private void btn_ServiceScanClick(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.btnServiceScan_click;
            objCommand.Command.Execute(drSelected);
        }

        private void cbxUnitType_Changed(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBox cbxVerification = (ComboBox)sender;
                //   string str = Convert.ToString(cbxVerification.SelectedValue);
                DataRowView drSelectedRow = (DataRowView)cbxVerification.SelectedItem;
                DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
                // drSelected.Row["SALES_UNIT_NAME"] = str;
                if (drSelected != null && drSelected.Row.Table.Columns.Contains("IS_CONSUMPTION") &&
                    Convert.ToInt16(drSelected["SERVICE_TYPE"]) == 4 && Convert.ToInt16(drSelected["IS_CONSUMPTION"]) != 0)
                {
                    return;
                }
                else
                {
                    bool isallow = true;
                    if (drSelected != null && drSelectedRow != null && drSelected.Row["SALES_UNIT_NAME"] != DBNull.Value &&
                        Convert.ToString(drSelected.Row["SALES_UNIT_NAME"]) == Convert.ToString(drSelectedRow.Row["TO_UNIT_NAME"]))
                    {
                        isallow = false;
                    }
                    if (drSelectedRow != null)
                    {
                        drSelected.Row["SALES_UNIT_NAME"] = drSelectedRow.Row["TO_UNIT_NAME"];
                    }
                    if (isallow)
                    {
                        CommandBinding objCommand = new CommandBinding();
                        objCommand.Command = UIControls.BillingNew.Command.BillingCommands.PharmacyUnitSalesChaneged;
                        objCommand.Command.Execute(drSelected);
                    }
                    if (drSelected != null && drSelected.IsEdit == true)
                    {
                        string tempBatchNo = Convert.ToString(drSelected["BATCHNO"]);
                        string tempUnitofQty = Convert.ToString(drSelected["PH_UNIT_SALES_CONVERSION_ID"]);
                        drSelected.EndEdit();
                        drSelected["BATCHNO"] = tempBatchNo;
                        drSelected["PH_UNIT_SALES_CONVERSION_ID"] = tempUnitofQty;
                    }
                }
            }
            catch (Exception)
            {                
                throw;
            }            
        }

        private void txt_click(object sender, MouseButtonEventArgs e)
        {
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.OrderView;
            objCommand.Command.Execute(drSelected);
        }

        private void AllergyReason(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.AllergyReason;
            objCommand.Command.Execute(drSelected);
        }

        private void ExcltoIncl(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = (DataRowView)(e.OriginalSource as FrameworkElement).DataContext;
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.ExcltoIncl;
            objCommand.Command.Execute(drSelected);
        }

        private void IncltoExcl(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = (DataRowView)(e.OriginalSource as FrameworkElement).DataContext;
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.IncltoExcl;
            objCommand.Command.Execute(drSelected);
        }

        private void ApprToSelf(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = (DataRowView)(e.OriginalSource as FrameworkElement).DataContext;
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.Needtoself;
            objCommand.Command.Execute(drSelected);
        }

        private void WaitToSelf(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = (DataRowView)(e.OriginalSource as FrameworkElement).DataContext;
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.WaitingtoSelf;
            objCommand.Command.Execute(drSelected);
        }
       

        private void WaitToIncl(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = (DataRowView)(e.OriginalSource as FrameworkElement).DataContext;
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.WaitToIncl;
            objCommand.Command.Execute(drSelected);
        }

        private void NeedApprToIncl(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = (DataRowView)(e.OriginalSource as FrameworkElement).DataContext;
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.NeedToIncl;
            objCommand.Command.Execute(drSelected);
        }

        private void btnLabel_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chkSale = (CheckBox)sender;
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.LabelClick;
            if (chkSale.IsChecked == true)
                drSelected["ISLABEL"] = true;
            else
                drSelected["ISLABEL"] = false;
            IsCheckCommonSale = false;
            objCommand.Command.Execute(drSelected);
        }

        private void BarCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textbox = (TextBox)sender;
                DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
                if (drSelected != null)
                {
                    if (textbox.Text != string.Empty)
                    {
                        drSelected["BARCODE"] = textbox.Text;
                    }
                }
                CommandBinding objCommand = new CommandBinding();
                objCommand.Command = UIControls.BillingNew.Command.BillingCommands.EMRBarcodeWiseSearch;
                objCommand.Command.Execute(drSelected);
            }
        }

        private void MenuChangeZeroCharge(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = (DataRowView)(e.OriginalSource as FrameworkElement).DataContext;
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.ChangetoZeroCharge;
            objCommand.Command.Execute(drSelected);
        }

        private void btnAllDelete_clicked(object sender, RoutedEventArgs e)
        {
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.AllServiceDelete;
            objCommand.Command.Execute("");
        }

        private void btnRemovServiceClick(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.btnRemoveService;
            objCommand.Command.Execute(drSelected);

        }

        private void btnAddServiceClick(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.btnAddRemovedService;
            objCommand.Command.Execute(drSelected);
        }

        private void LabelPrint_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
        }

        private void LabelPrint_Clicks(object sender, RoutedEventArgs e)
        {
            DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
            CommandBinding objCommand = new CommandBinding();
            objCommand.Command = UIControls.BillingNew.Command.BillingCommands.PharmacyLabelPrinting;
            objCommand.Command.Execute(drSelected);
        }

        private void Pharmacy_RemarksOrComments(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
                CommandBinding objCommand = new CommandBinding();
                objCommand.Command = UIControls.BillingNew.Command.BillingCommands.PharmacistRemarkorComment;
                objCommand.Command.Execute(drSelected);
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private void Pharmacy_VerificationCancel(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
                CommandBinding objCommand = new CommandBinding();
                objCommand.Command = UIControls.BillingNew.Command.BillingCommands.VerificationCancel;
                objCommand.Command.Execute(drSelected);
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private void ChngSublocation(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);
                CommandBinding objCommand = new CommandBinding();
                objCommand.Command = UIControls.BillingNew.Command.BillingCommands.ChangeSublocation;
                objCommand.Command.Execute(drSelected);
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private void cboCostCenter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBox cbxCostCenter = (ComboBox)sender;
                //string str = Convert.ToString(cbxVerification.SelectedValue);
                DataRowView drSelectedRow = (DataRowView)cbxCostCenter.SelectedItem;
                DataRowView drSelected = ((DataRowView)(e.OriginalSource as FrameworkElement).DataContext);              
                    
                    if (drSelectedRow != null && drSelectedRow.Row["H_ADMIN_DEPT_DEPTID"].KIIsNotNullOrEmpty())
                    {
                        //drSelected.BeginEdit();
                        //drSelected.Row["COST_CENTER_ID"] = drSelectedRow.Row["H_ADMIN_DEPT_DEPTID"];
                        drSelected.Row["H_ADMIN_DEPT_DNAME"] = drSelectedRow.Row["H_ADMIN_DEPT_DNAME"];
                        //drSelected.EndEdit();
                    }
              
            }
            catch (Exception)
            {
                
                throw;
            }
        }

       

        //public void ClearBillDescription()
        //{
        //    chkCommonSale.IsChecked = false;
        //}
    }
}
