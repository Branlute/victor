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
using System.Windows.Controls.Primitives;
using System.ComponentModel;

namespace VictorNamespace
{
    public partial class Recreation : PhoneApplicationPage
    {
        private Popup popup;
        private BackgroundWorker backroungWorker;

        public Recreation()
        {
            InitializeComponent();
            ShowPopup();
        }

        private void ShowPopup()
        {
            this.popup = new Popup();
            this.popup.Child = new PopupSplash();
            this.popup.IsOpen = true;

            StartLoadingData();
        }

        private void StartLoadingData()
        {
            backroungWorker = new BackgroundWorker();
            backroungWorker.DoWork += new DoWorkEventHandler(backroungWorker_DoWork);
            backroungWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backroungWorker_RunWorkerCompleted);

            backroungWorker.RunWorkerAsync();
        }

        void backroungWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            popup.IsOpen = false;
        }


        void backroungWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    //Traitement                   
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK);
            }
        }
        
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));


        }

        private void image1_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Adventure/Home/Jeux/Cuisine/Mario.xaml?recreation=true", UriKind.Relative));
        }

        private void image2_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Adventure/Home/Jeux/Cuisine/Casserole.xaml?recreation=true", UriKind.Relative));
        }

        private void image4_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Adventure/Home/Jeux/Cuisine/Couteau.xaml?recreation=true", UriKind.Relative));
        }

        private void image3_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Adventure/Home/Jeux/Cuisine/Four.xaml?recreation=true", UriKind.Relative));
        }

        private void image5_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Adventure/Home/Jeux/Cuisine/GrillePain.xaml?recreation=true", UriKind.Relative));
        }

    }
}