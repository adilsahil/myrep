using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace UIElements
{
    public class NewButton : Button
    {
         
       #region Property Declaration

       public enum RoundEdge
       {
           None,
           LeftTop,
           LeftBottom,
           Left,
           RightTop,
           RightBottom,
           Right,
           Default

       }

       public static readonly DependencyProperty
                           TitleProperty = DependencyProperty.Register("Title", typeof(string),
                                                                     typeof(NewButton),
                                                                     new PropertyMetadata(string.Empty));
       public static readonly DependencyProperty
                           RoundEdgeProperty = DependencyProperty.Register("RoundEdgeType", typeof(RoundEdge),
                                                                    typeof(NewButton),
                                                                    new PropertyMetadata(RoundEdge.Default));
       #endregion

       #region Constructor
        /// <summary>
            /// Static constructor
            /// </summary>
            static NewButton()
            {
                //override default style
                DefaultStyleKeyProperty.OverrideMetadata(typeof(NewButton), new FrameworkPropertyMetadata(typeof(NewButton)));
            }

            public NewButton()
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

            public RoundEdge RoundEdgeType
            {
                get
                {
                    return (RoundEdge)GetValue(RoundEdgeProperty);
                }
                set
                {
                    SetValue(RoundEdgeProperty,value );
                }
            }

        #endregion
    }
}
