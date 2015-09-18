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
    public partial class QCMDragAndDrop : PhoneApplicationPage
    {
        private Popup popup;
        private DataBaseContext dbc;
        private BackgroundWorker backroungWorker;

        private int questionId;
        private string niveau;
        private string scene;

        private Question q;

        private bool recreation = false;

        public QCMDragAndDrop()
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


        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            NavigationService.Navigate(new Uri("/Adventure/Home/HomeXNA.xaml?niveau=home&scene=cuisine", UriKind.Relative));
        }

        private void StartLoadingData()
        {
            backroungWorker = new BackgroundWorker();
            backroungWorker.DoWork += new DoWorkEventHandler(backroungWorker_DoWork);
            backroungWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backroungWorker_RunWorkerCompleted);

            backroungWorker.RunWorkerAsync();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var data = this.NavigationContext.QueryString;

            questionId = int.Parse(data["id"]);
            niveau = data["niveau"];
            scene = data["scene"];

            base.OnNavigatedTo(e);
        }

        void backroungWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    //On récupère les informations de la question
                    dbc = new DataBaseContext(DataBaseContext.DBConnectionString);

                    var questions = from question in dbc.Questions
                                    where question.Id == questionId
                                    select question;

                    q = questions.First();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK);
            }
        }

        void backroungWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //On ferme la connexion à la BDD
            dbc.Dispose();

            //Mise à jour de l'UI
            txtQuestion.Text = q.Intitule;

            //Les réponses
            for (int i = 0; i < 4; i++)
            {
                reorderListBox.Items.Add(new QCMDragAndDropItem(i));
            }

            //Mise à jour audio
            mp3.Source = new Uri(q.Mp3, UriKind.Relative);
            mp3.Position = new TimeSpan(0);
            mp3.Volume = 1;

            //On affiche l'écran
            popup.IsOpen = false;

            //On lit la question
            mp3.Play();
        }

        private void btnValider_Click(object sender, RoutedEventArgs e)
        {
            bool rep;

            //On regarde si c'est dans le bon ordre
            if ((reorderListBox.Items.ElementAt(0) as QCMDragAndDropItem).txtNum.Text == "0")
            {
                if ((reorderListBox.Items.ElementAt(1) as QCMDragAndDropItem).txtNum.Text == "1")
                {
                    if ((reorderListBox.Items.ElementAt(2) as QCMDragAndDropItem).txtNum.Text == "2")
                    {
                        if ((reorderListBox.Items.ElementAt(3) as QCMDragAndDropItem).txtNum.Text == "3")
                        {
                            rep = true;
                        }
                        else
                        {
                            rep = false;
                        }
                    }
                    else
                    {
                        rep = false;
                    }
                }
                else
                {
                    rep = false;
                }
            }
            else
            {
                rep = false;
            }


            if (recreation == false)
            {
                //On affiche l'écran qui va bien
                if (rep == true)
                {
                    //On appelle l'update en fonction du niveau
                    /*if (niveau == "home")
                    {
                        Session.updateHome(scene, q.Nom);
                    }*/

                    NavigationService.Navigate(new Uri("/Adventure/QCMResultsOK2.xaml?id=" + questionId + "&niveau=" + niveau + "&scene=" + scene, UriKind.Relative));
                }
                else
                {
                    NavigationService.Navigate(new Uri("/Adventure/QCMResultsNotOK2.xaml?id=" + questionId + "&niveau=" + niveau + "&scene=" + scene, UriKind.Relative));
                }
            }
            else
            {
                NavigationService.Navigate(new Uri("/Recreation/Recreation.xaml", UriKind.Relative));
            }
        }

        private void imgRepeat_Tap(object sender, GestureEventArgs e)
        {
            mp3.Position = new TimeSpan(0);
            mp3.Play();
        }
    }
}