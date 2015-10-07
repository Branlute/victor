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
    public partial class Parameters : PhoneApplicationPage
    {
        private String category;
        private Popup popup;
        private BackgroundWorker backroungWorker;

        public Parameters()
        {
            InitializeComponent();
        }

        private void ShowPopup()
        {
            popup = new Popup();
            popup.Child = new PopupSplash();
            popup.IsOpen = true;

            StartLoadingData();
        }

        private void StartLoadingData()
        {
            backroungWorker = new BackgroundWorker();
            backroungWorker.DoWork += new DoWorkEventHandler(backroungWorker_DoWork);
            backroungWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backroungWorker_WorkCompleted);

            backroungWorker.RunWorkerAsync();
        }

        void backroungWorker_WorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            popup.IsOpen = false;

            MessageBox.Show("The operation is now finished !", "Operation succeed", MessageBoxButton.OK);
        }

        void backroungWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {

                    if (category.Equals("reset"))
                    {
                        Init.InitBDD();
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error occured", MessageBoxButton.OK);
            }
        }

        private void gridSave_Tap(object sender, GestureEventArgs e)
        {
            MessageBox.Show("This option is not implemented yet ! You will be able to save your data on our servers !", "Coming soon !", MessageBoxButton.OK);
        }

        private void gridDownload_Tap(object sender, GestureEventArgs e)
        {
            MessageBox.Show("This option is not implemented yet ! You will be able to download your data from our servers !", "Coming soon !", MessageBoxButton.OK);
        }

        private void gridReset_Tap(object sender, GestureEventArgs e)
        {
            if (MessageBox.Show("This action will restart all your data. Are you sure you want to continue ?", "Warning !", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                category = "reset";
                ShowPopup();
            }
        }
    }
}