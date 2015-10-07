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

namespace VictorNamespace.Adventure
{
    public partial class QCMResultsOk2 : PhoneApplicationPage
    {
        private Popup popup;
        private DataBaseContext dbc;
        private BackgroundWorker backroungWorker;

        private Explication exp;

        private int questionId;
        private string scene;
        private string niveau;

        public QCMResultsOk2()
        {
            InitializeComponent();

            ShowPopup();
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
            backroungWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backroungWorker_RunWorkerCompleted);

            backroungWorker.RunWorkerAsync();
        }

        void backroungWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //On ferme la connexion à la BDD
            dbc.Dispose();

            //Mise à jour de l'UI
            txtExplication.Text = exp.Pourquoi;

            //Mise à jour audio
            mp3.Source = new Uri(exp.Mp3, UriKind.Relative);
            mp3.Position = new TimeSpan(0);
            mp3.Volume = 1;

            //On affiche l'écran
            popup.IsOpen = false;

            //On lit la question
            mp3.Play();
        }

        void backroungWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    //On récupère les informations de la question
                    dbc = new DataBaseContext(DataBaseContext.DBConnectionString);

                    var explications = from explication in dbc.Explications
                                        where explication.QuestionId == questionId
                                       select explication;

                    exp = explications.First();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK);
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var data = this.NavigationContext.QueryString;

            Session.cuisine_tasse = true;

            questionId = int.Parse(data["id"]);
            scene = data["scene"];
            niveau = data["niveau"];

            base.OnNavigatedTo(e);
        }

        private void imgRepeat_Tap(object sender, GestureEventArgs e)
        {
            mp3.Position = new TimeSpan(0);
            mp3.Play();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (niveau == "Explorer")
            {
                NavigationService.Navigate(new Uri("/GameExplorer.xaml?scene=" + scene, UriKind.Relative));
            }
            else
            NavigationService.Navigate(new Uri("/Adventure/Home/HomeXNA.xaml?niveau=" + niveau + "&scene=" + scene, UriKind.Relative));
        }
    }
}