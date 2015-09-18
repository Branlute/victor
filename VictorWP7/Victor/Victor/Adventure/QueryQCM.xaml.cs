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
using System.Windows.Media.Imaging;

namespace VictorNamespace
{
    public partial class QueryQCM : PhoneApplicationPage
    {
        private Popup popup;
        private DataBaseContext dbc;
        private BackgroundWorker backroungWorker;

        private int questionId;
        private string niveau;
        private string scene;

        private Question q;
        private List<Reponse> lstRep;

        public QueryQCM()
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

        void backroungWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //On ferme la connexion à la BDD
            dbc.Dispose();

            //Mise à jour de l'UI
            imgQuestion.Source = new BitmapImage(new Uri("/Victor;component" + q.Url, UriKind.Relative));
            txtQuestion.Text = q.Intitule;

            for (int i = 0; i < lstRep.Count; i++)
            {
                TextBlock txtB = new TextBlock();
                txtB.Text = lstRep.ElementAt(i).Intitule;
                txtB.TextWrapping = TextWrapping.Wrap;


                RadioButton rb = new RadioButton();
                rb.Content = txtB;
                rb.Name = lstRep.ElementAt(i).Vrai.ToString() + ";" + i.ToString() + ";" + lstRep.ElementAt(i).Id;
                rb.Foreground = new SolidColorBrush(Color.FromArgb(255, 19, 123, 55));

                rb.Checked += new RoutedEventHandler(rb_Checked);

                lbReponses.Items.Add(rb);
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

                    //On récupère les réponses à la question
                    var reponses = from reponse in dbc.Reponses
                                   where reponse.QuestionId == questionId
                                   select reponse;

                    lstRep = new List<Reponse>();

                    foreach (var reponse in reponses)
                    {
                        lstRep.Add(reponse);
                    }
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

            questionId = int.Parse(data["id"]);
            niveau = data["niveau"];
            scene = data["scene"];

            ShowPopup();

            base.OnNavigatedTo(e);
        }

        private void btnValider_Click(object sender, RoutedEventArgs e)
        {

            if (lbReponses.SelectedIndex == -1)
            {
                MessageBox.Show("You have to select an answer !");
            }
            else
            {
                //On enregistre la réponse dans les logs
                dbc = new DataBaseContext(DataBaseContext.DBConnectionString);
                dbc.Logs.InsertOnSubmit(new Log() { Date = DateTime.Now, ReponseId = int.Parse(((RadioButton)lbReponses.SelectedItem).Name.Split(';').ElementAt(2)) });
                dbc.SubmitChanges();
                dbc.Dispose();

                //On affiche l'écran qui va bien
                if (bool.Parse(((RadioButton)lbReponses.SelectedItem).Name.Split(';').ElementAt(0)) == true)
                {
                    //On appelle l'update en fonction du niveau
                    /*if (niveau == "home")
                    {
                      Session.updateHome(scene, q.Nom);
                    }*/

                    NavigationService.Navigate(new Uri("/Adventure/QCMResultsOK.xaml?id=" + questionId + "&objet=" + q.Nom + "&niveau=" + niveau + "&scene=" + scene, UriKind.Relative));

                    /*if (q.Nom == "casserole")
                    {
                        NavigationService.Navigate(new Uri("/Adventure/Home/Jeux/Cuisine/Casserole.xaml", UriKind.Relative));
                    }*/

                    //On vérifie la disponibilité de la pièce du puzzle
                    if (Session.cuisine_puzzle)
                    {
                        MessageBox.Show("Congratulation you have found the first puzzle piece !");
                    }
                }
                else
                {
                    NavigationService.Navigate(new Uri("/Adventure/QCMResultsNotOK.xaml?id=" + questionId + "&niveau=" + niveau + "&scene=" + scene, UriKind.Relative));
                }
            }
        }

        private void rb_Checked(object sender, RoutedEventArgs e)
        {
            //On split le nom pour récupérer l'index
            lbReponses.SelectedIndex = int.Parse((sender as RadioButton).Name.Split(';').ElementAt(1));
        }

        private void imgRepeat_Tap(object sender, GestureEventArgs e)
        {
            mp3.Position = new TimeSpan(0);
            mp3.Play();
        }
    }
}