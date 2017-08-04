using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace UIElements
{
    public class NewComboBox : ComboBox
    {
         #region Property Declaration

            public static readonly DependencyProperty
                           TitleProperty = DependencyProperty.Register("Title", typeof(string),
                                                                     typeof(NewComboBox),
                                                                     new PropertyMetadata(string.Empty));

            private static DependencyProperty
                   IsMandatoryProperty = DependencyProperty.Register("IsMandatory", typeof(bool),
                                                           typeof(NewComboBox),
                                                           new PropertyMetadata(false));
            // Using a DependencyProperty as the backing store for IsValueSelected.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty 
                            IsValueSelectedProperty = DependencyProperty.Register("IsValueSelected", 
                                                                                    typeof(Boolean), 
                                                                                    typeof(NewComboBox), 
                                                                                    new UIPropertyMetadata(false));
            public static readonly DependencyProperty
                           InformationProperty = DependencyProperty.Register("Information", typeof(string),
                                                                             typeof(NewComboBox),
                                                                             new PropertyMetadata(string.Empty));

            private static readonly DependencyPropertyKey
                            HasTextPropertyKey = DependencyProperty.RegisterReadOnly("HasText", typeof(bool),
                                                                                     typeof(NewComboBox),
                                                                                     new FrameworkPropertyMetadata(false));
            public static readonly DependencyProperty
                                   HasTextProperty = HasTextPropertyKey.DependencyProperty;


        #endregion

        #region Constructor
        /// <summary>
            /// Static constructor
            /// </summary>
            static NewComboBox()
            {
                //override default style
               DefaultStyleKeyProperty.OverrideMetadata(typeof(NewComboBox), new FrameworkPropertyMetadata(typeof(NewComboBox)));
               
            }

            public NewComboBox()
            {
            }

            public override void OnApplyTemplate()
            {
                base.OnApplyTemplate();
                if (IsEditable == true)
                {
                    TextBox txt = this.GetTemplateChild("PART_EditableTextBox") as TextBox;
                    txt.TextChanged += new TextChangedEventHandler(txt_TextChanged);
                }
            }

            void txt_TextChanged(object sender, TextChangedEventArgs e)
            {
                TextBox text = (TextBox)sender;
                if (text.Text.ToString().Length != 0)
                {
                    this.SetValue(HasTextPropertyKey, true);
                }
                else
                {
                    this.SetValue(HasTextPropertyKey, false);
                }
            }

            void txt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
            {
              
            }
        #endregion

        #region Public Property

            /// <summary>
            /// This will return if the Text has any value
            /// </summary>
            public bool HasText
            {
                get
                {
                    return System.Convert.ToBoolean(GetValue(HasTextProperty));
                }
            }

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

            /// <summary>
            /// This will return the field is Mandatory or Not
            /// </summary>
            public bool  IsMandatory
            {
                get
                {
                    return (bool)(GetValue(IsMandatoryProperty));
                }
                set
                {
                    SetValue(IsMandatoryProperty, value);
                }
            }

            /// <summary>
            /// This will set the Value Selected Property
            /// </summary>
            public Boolean IsValueSelected
            {
                get 
                { 
                    return (Boolean)GetValue(IsValueSelectedProperty); 
                }
                set 
                { 
                    SetValue(IsValueSelectedProperty, value); 
                }
            }

            protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e)
            {
                base.OnPreviewTextInput(e);
            }
            /// <summary>
            /// This will assign Information property value to the Textbox in Italic 
            /// </summary>
            public string Information
            {
                get
                {
                    return System.Convert.ToString(GetValue(InformationProperty));
                }
                set
                {
                    SetValue(InformationProperty, value);
                }
            }
        #endregion

        #region protected Methods
            protected override void OnSelectionChanged(SelectionChangedEventArgs e)
            {
                base.OnSelectionChanged(e);
                IsValueSelected = Text.Length != 0;

                if (Text.Length != 0)
                {
                    this.SetValue(HasTextPropertyKey, true);
                }
                else
                {
                    if (IsEditable == false )
                    {
                        this.SetValue(HasTextPropertyKey, false);
                    }
                }
            }

       
        #endregion
    }
}
