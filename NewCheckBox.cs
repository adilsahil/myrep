using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace UIElements
{
    public class NewCheckBox : CheckBox
    {
        #region Property Declaration

        public static readonly DependencyProperty
                       TitleProperty = DependencyProperty.Register("Title", typeof(string),
                                                                 typeof(NewCheckBox),
                                                                 new PropertyMetadata(string.Empty));

        //private static DependencyProperty
        //       IsMandatoryProperty = DependencyProperty.Register("IsMandatory", typeof(bool),
        //                                               typeof(giCheckBox),
        //                                               new PropertyMetadata(false));

        #endregion

        #region Constructor
        /// <summary>
        /// Static constructor
        /// </summary>
        static NewCheckBox()
        {
            //override default style
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NewCheckBox), new FrameworkPropertyMetadata(typeof(NewCheckBox)));
        }

        public NewCheckBox()
        {
        }
        #endregion

        #region Public Property
        /// <summary>
        /// Sets the Title 
        /// </summary>
        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        ///// <summary>
        ///// This will return the field is Mandatory or Not
        ///// </summary>
        //public bool  IsMandatory
        //{
        //    get
        //    {
        //        return (bool)(GetValue(IsMandatoryProperty));
        //    }
        //    set
        //    {
        //        SetValue(IsMandatoryProperty, value);
        //    }
        //}

        #endregion
    }
}
