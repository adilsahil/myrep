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
using System.Collections.ObjectModel;

namespace Masters
{
    /// <summary>
    /// Interaction logic for UcControls.xaml
    /// </summary>
    public partial class UcControls : UserControl
    {
        public UcControls()
        {
            InitializeComponent();
            this.DataContext = this;
            SetValue(ObserverPropertyKey, new ObservableCollection<Button>());
            MyCustom = "abc";
        }
        private string txtname;
        public string _Txtname
        {
            get
            {
                return txtname;
            }
            set
            {
                txtname = value;
            }
        }
        private bool _ISchk;
        public bool ISchk
        {
            get
            {
                return _ISchk;
            }
            set
            {
                _ISchk = value;
                if (_ISchk)
                {
                    using (OracleConnection objconn = new OracleConnection("Data Source=DB9; User ID=YASASII; Password=FALCON"))
                    {
                        if (objconn.State != ConnectionState.Open)
                        {
                            objconn.Open();
                            OracleCommand cmd2 = new OracleCommand();
                            cmd2.Connection = objconn;
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.CommandText = "SELECT_WCF_TEMP";
                            cmd2.Parameters.Add("P_NAME", OracleType.VarChar).Value = _Txtname.ToString();
                            cmd2.Parameters.Add("P_TYPE", OracleType.VarChar).Value = _Txtname.ToString();
                            cmd2.Parameters.Add("P_CURSOR", OracleType.Cursor).Direction = ParameterDirection.Output;
                            DataSet ds = new DataSet();
                            OracleDataAdapter adapter = new OracleDataAdapter(cmd2);
                            adapter.Fill(ds);
                        }
                    }
                }
            }
        }

        private void BtnGOclick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (OracleConnection objcon = new OracleConnection("Data Source=DB9; User ID=YASASII; Password=FALCON"))
                {
                    if (objcon.State != ConnectionState.Open)
                    {
                        objcon.Open();
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = objcon;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "INSERT_WCF_TEMP";
                        cmd.Parameters.Add("P_NAME", OracleType.VarChar).Value = _Txtname.ToString();
                        cmd.Parameters.Add("P_TYPE", OracleType.VarChar).Value = _Txtname.ToString();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("saved");
                        }

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        static FrameworkPropertyMetadata propertymetadata = new FrameworkPropertyMetadata("Comes as Default", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
            FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(MyCustom_PropertyChanged), new CoerceValueCallback(MyCustom_CoerceValue), false, UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty MyCustomProperty =
        DependencyProperty.Register("MyCustom", typeof(string), typeof(UcControls), propertymetadata, new ValidateValueCallback(MyCustom_Validate));

        private static void MyCustom_PropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            //To be called whenever the DP is changed.
            MessageBox.Show(string.Format(
               "Property changed is fired : OldValue {0} NewValue : {1}", e.OldValue, e.NewValue));
        }

        private static object MyCustom_CoerceValue(DependencyObject dobj, object Value)
        {
            //called whenever dependency property value is reevaluated. The return value is the
            //latest value set to the dependency property
            MessageBox.Show(string.Format("CoerceValue is fired : Value {0}", Value));
            return Value;
        }

        private static bool MyCustom_Validate(object Value)
        {
            //Custom validation block which takes in the value of DP
            //Returns true / false based on success / failure of the validation
            MessageBox.Show(string.Format("DataValidation is Fired : Value {0}", Value));
            return true;
        }

        public string MyCustom
        {
            get
            {
                return this.GetValue(MyCustomProperty) as string;
            }
            set
            {
                this.SetValue(MyCustomProperty, value);
            }
        }

        public static readonly DependencyPropertyKey ObserverPropertyKey = DependencyProperty.RegisterReadOnly("Observer", typeof(ObservableCollection<Button>), typeof(UcControls), new FrameworkPropertyMetadata(new ObservableCollection<Button>()));
        public static readonly DependencyProperty ObserverProperty = ObserverPropertyKey.DependencyProperty;
        public ObservableCollection<Button> Observer
        {
            get
            {
                return (ObservableCollection<Button>)GetValue(ObserverProperty);
            }
        }


    }
}
