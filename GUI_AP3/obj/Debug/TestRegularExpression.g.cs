﻿#pragma checksum "..\..\TestRegularExpression.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "19C1A22C16F8130E0A6F13B7212BE1D47FF4480F"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace GUI_AP3 {
    
    
    /// <summary>
    /// TestRegularExpression
    /// </summary>
    public partial class TestRegularExpression : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\TestRegularExpression.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtfasta;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\TestRegularExpression.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button fasta_browse;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\TestRegularExpression.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtRegularExpression;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\TestRegularExpression.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Test_button;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\TestRegularExpression.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Parse_OK_button;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\TestRegularExpression.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label fasta_status;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\TestRegularExpression.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid fasta_grid;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\TestRegularExpression.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid examples;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GUI_AP3;component/testregularexpression.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\TestRegularExpression.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.txtfasta = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.fasta_browse = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\TestRegularExpression.xaml"
            this.fasta_browse.Click += new System.Windows.RoutedEventHandler(this.fasta_browse_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtRegularExpression = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.Test_button = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\TestRegularExpression.xaml"
            this.Test_button.Click += new System.Windows.RoutedEventHandler(this.Test_button_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Parse_OK_button = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\TestRegularExpression.xaml"
            this.Parse_OK_button.Click += new System.Windows.RoutedEventHandler(this.Parse_OK_button_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.fasta_status = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.fasta_grid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 8:
            this.examples = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

