﻿#pragma checksum "C:\Users\Ludovic\Documents\Victor\Victor\Victor\Victor\Parameters\ParentsCornerItem.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4E3346F2B28BAB2C4E1F7D2FC8BF18E4"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.530
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace VictorNamespace {
    
    
    public partial class ParentsCornerItem : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock txtDate;
        
        internal System.Windows.Controls.TextBlock txtQuestion;
        
        internal System.Windows.Controls.TextBlock txtAnswer;
        
        internal System.Windows.Shapes.Rectangle separateur;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Victor;component/Parameters/ParentsCornerItem.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtDate = ((System.Windows.Controls.TextBlock)(this.FindName("txtDate")));
            this.txtQuestion = ((System.Windows.Controls.TextBlock)(this.FindName("txtQuestion")));
            this.txtAnswer = ((System.Windows.Controls.TextBlock)(this.FindName("txtAnswer")));
            this.separateur = ((System.Windows.Shapes.Rectangle)(this.FindName("separateur")));
        }
    }
}

