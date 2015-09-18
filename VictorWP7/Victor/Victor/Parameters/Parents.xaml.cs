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
    public partial class Parents : PhoneApplicationPage
    {
        private Popup popup;
        private DataBaseContext dbc;
        List<ParentsCornerItem> logs;
        private BackgroundWorker backroungWorker;

        public Parents()
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
            logs = new List<ParentsCornerItem>();

            backroungWorker = new BackgroundWorker();
            backroungWorker.DoWork += new DoWorkEventHandler(backroungWorker_DoWork);
            backroungWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backroungWorker_RunWorkerCompleted);

            backroungWorker.RunWorkerAsync();
        }

        void backroungWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lstParentsHome.ItemsSource = logs;

            dbc.Dispose();
            popup.IsOpen = false;
        }

        void backroungWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    //On récupère les informations
                    dbc = new DataBaseContext(DataBaseContext.DBConnectionString);

                    //Les logs du niveau home
                    var homelogs = from log in dbc.Logs
                                   where log.Reponse.Question.Scene.Niveau.Nom == "home"
                                   orderby log.Date descending
                                   select log;

                    foreach(var homelog in homelogs)
                    {
                        if (homelog.Reponse.Vrai == false)
                        {

                            logs.Add(new ParentsCornerItem(homelog.Date.ToString(), "Question : " + homelog.Reponse.Question.Intitule, "Answer :" + homelog.Reponse.Intitule, Colors.Red.ToString()));
                        }
                        else
                        {
                            //lstParentsHome.Items.Add(new ParentsCornerItem(homelog.Date.ToString(), homelog.Reponse.Question.Intitule, homelog.Reponse.Intitule));
                            logs.Add(new ParentsCornerItem(homelog.Date.ToString(), "Question : " + homelog.Reponse.Question.Intitule, "Answer : " + homelog.Reponse.Intitule, "#FF0A7C37"));
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK);
            }
        }
    }
}