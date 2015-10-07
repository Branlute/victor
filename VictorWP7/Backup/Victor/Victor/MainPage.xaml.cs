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
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;
using Microsoft.Xna.Framework;

namespace VictorNamespace
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Popup popup;
        private BackgroundWorker backroungWorker;

        private bool load = true;

        public MainPage()
        {
            InitializeComponent();
            ShowPopup();
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            Game game = new Game();
            game.Exit();          
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
            load = false;
        }


        void backroungWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    //On regarde si c'est le premier lancement
                    var store = IsolatedStorageFile.GetUserStoreForApplication();

                    if (!store.FileExists("firstRunVictor.txt"))
                    {
                        //Création du fichier
                        if (MessageBox.Show("Before to use the game, you must accept the terms of the agreement", "License Agreement", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            //On met à jour l'acceptation des licenses
                            using (var stream = store.OpenFile("firstRunVictor.txt", FileMode.Create))
                            {
                                StreamWriter writer = new StreamWriter(stream);
                                writer.Write(true);
                                writer.Close();
                            }

                            Init.InitBDD();
                        }
                        //On quitte l'application
                        else
                        {
                            Game game = new Game();
                            game.Exit();
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK);
            }
        }

        private void imgVictorDisguise_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Disguise/Disguise.xaml", UriKind.Relative));
        }

        private void imgVictorHome_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Adventure/Home/HomeXNA.xaml?niveau=home&scene=cuisine", UriKind.Relative));
        }

        private void imgVictorExplorer_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Explorer/ExplorerMenu.xaml", UriKind.Relative));
        }

        private void imgVictorRecreation_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Recreation/Recreation.xaml", UriKind.Relative));
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            if (load == false)
            {
                NavigationService.Navigate(new Uri("/Parameters/Parameters.xaml", UriKind.Relative));
            }
        }

        private void imgVictor_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Galerie.xaml", UriKind.Relative));
        }

        private void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Parents/Parents.xaml", UriKind.Relative));
        }
    }
}