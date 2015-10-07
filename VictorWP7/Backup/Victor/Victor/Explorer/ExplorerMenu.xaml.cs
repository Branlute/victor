using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace VictorNamespace
{
    public partial class ExplorerMenu : PhoneApplicationPage
    {
        public ExplorerMenu()
        {
            InitializeComponent();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void gridAdd_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Explorer/Explorer.xaml", UriKind.Relative));
        }

        private void gridPlay_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Explorer/ExplorerMap.xaml", UriKind.Relative));
        }

        private void gridDelete_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Explorer/ExplorerManage.xaml", UriKind.Relative));
        }
    }
}