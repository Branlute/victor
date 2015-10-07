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
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.IO.IsolatedStorage;
using System.Collections.ObjectModel;

namespace VictorNamespace
{
    public partial class Editor : PhoneApplicationPage
    {
        private Popup popup;
        private BackgroundWorker bgw;
        private BitmapImage img;

        private DataBaseContext dbc;

        //Enregistrement des clics
        private bool clic1 = false;
        private bool clic2 = false;
        private bool objet = false;

        private List<InteractionPhoto> lstInteractions;
        private InteractionPhoto tmp;

        public Editor()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ShowPopup();
        }

        private void ShowPopup()
        {
            popup = new Popup();
            popup.Child = new PopupSplash();
            popup.IsOpen = true;

            bgw = new BackgroundWorker();
            bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);

            bgw.RunWorkerAsync();
        }

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                //Init
                lstInteractions = new List<InteractionPhoto>();

                //On charge l'image
                img = new BitmapImage();
                img = App.Image;
            });
        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            imgResult.Source = img;
            popup.IsOpen = false;
        }

        private void imgResult_Tap(object sender, GestureEventArgs e)
        {
            //Si premier clic, on enrgistre le X et le Y
            if (clic1)
            {
                tmp.X = Convert.ToInt32(e.GetPosition(imgResult).X);
                tmp.Y = Convert.ToInt32(e.GetPosition(imgResult).Y);

                //On affiche le premier point
                e1.Margin = new Thickness(tmp.X, tmp.Y, 0, 0);
                e1.Visibility = Visibility.Visible;

                clic1 = false;
                clic2 = true;

                return;
            }

            //Si second clic, on enregistre longeur et largeur
            if (clic2)
            {
                //Longueur
                int x = Convert.ToInt32(e.GetPosition(imgResult).X);

                if (tmp.X > x)
                {
                    MessageBox.Show("This point should be the corner on the down right", "Error", MessageBoxButton.OK);
                }
                else
                {
                    tmp.Longueur = x - tmp.X;
                    int y = Convert.ToInt32(e.GetPosition(imgResult).Y);

                    if (tmp.Y > y)
                    {
                        MessageBox.Show("This point should be the corner on the down right", "Error", MessageBoxButton.OK);
                    }
                    else
                    {
                        tmp.Hauteur = y - tmp.Y;
                        clic2 = false;

                        //On enlève l'ellipse
                        e1.Visibility = Visibility.Collapsed;

                        //On affiche le rectable
                        r1.Margin = new Thickness(e1.Margin.Left + 10, e1.Margin.Top +6,0,0);
                        r1.Width = tmp.Longueur;
                        r1.Height = tmp.Hauteur;
                        r1.Visibility = Visibility.Visible;

                        //On choisit l'objet
                        objet = true;
                        ContentPanelItem.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void appBarAdd_Click(object sender, EventArgs e)
        {
            if (!clic1 && !clic2 && !objet)
            {
                MessageBox.Show("Indicate where are the dangers by doing rectangles. Indicate the corner on the top left and then the corner on the down right");
                tmp = new InteractionPhoto();
                r1.Visibility = Visibility.Collapsed;
                clic1 = true;
            }
        }

        private void appBarSave_Click(object sender, EventArgs e)
        {
            //On regarde s'il y a des interaction
            if (lstInteractions.Count != 0)
            {
                //On sauvegarde l'image dans l'isolated storage
                string nom = "Victor " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " " + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".jpg";
                WriteableBitmap wb = new WriteableBitmap(img);

                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    //On copie dans l'isolated storage
                    IsolatedStorageFileStream fileStream = isf.CreateFile(nom);
                    wb.SaveJpeg(fileStream, wb.PixelWidth, wb.PixelHeight, 0, 100);
                    fileStream.Close();
                }

                //On sauvegarde la photo dans la BDD
                dbc = new DataBaseContext(DataBaseContext.DBConnectionString);

                Photo photo = new Photo() { Background = nom };
                dbc.Photos.InsertOnSubmit(photo);
                dbc.SubmitChanges();

                //On sauvegarde le nom de la photo sur toutes les interactions
                foreach (var q in lstInteractions)
                {
                    q.PhotoId = photo.Id;

                    dbc.InteractionsPhotos.InsertOnSubmit(q);
                }

                dbc.SubmitChanges();
                dbc.Dispose();

                NavigationService.Navigate(new Uri("/Explorer/ExplorerMenu.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("You cannot save a photo without any danger", "Error", MessageBoxButton.OK);
            }
        }

        private void imgCasserole_Tap(object sender, GestureEventArgs e)
        {
            dbc = new DataBaseContext(DataBaseContext.DBConnectionString);
            var q = dbc.Questions.First(x => x.Nom == "casserole");
            tmp.QuestionId = q.Id;
            lstInteractions.Add(tmp);
            dbc.Dispose();

            objet = false;
            ContentPanelItem.Visibility = Visibility.Collapsed;
        }

        private void imgCouteau_Tap(object sender, GestureEventArgs e)
        {
            dbc = new DataBaseContext(DataBaseContext.DBConnectionString);
            var q = dbc.Questions.First(x => x.Nom == "couteau");
            tmp.QuestionId = q.Id;
            lstInteractions.Add(tmp);
            dbc.Dispose();

            objet = false;
            ContentPanelItem.Visibility = Visibility.Collapsed;
        }

        private void imgFour_Tap(object sender, GestureEventArgs e)
        {
            dbc = new DataBaseContext(DataBaseContext.DBConnectionString);
            var q = dbc.Questions.First(x => x.Nom == "four");
            tmp.QuestionId = q.Id;
            lstInteractions.Add(tmp);
            dbc.Dispose();

            objet = false;
            ContentPanelItem.Visibility = Visibility.Collapsed;
        }

        private void imgGrille_Tap(object sender, GestureEventArgs e)
        {
            dbc = new DataBaseContext(DataBaseContext.DBConnectionString);
            var q = dbc.Questions.First(x => x.Nom == "grillepain");
            tmp.QuestionId = q.Id;
            lstInteractions.Add(tmp);
            dbc.Dispose();

            objet = false;
            ContentPanelItem.Visibility = Visibility.Collapsed;
        }

        private void imgToxique_Tap(object sender, GestureEventArgs e)
        {
            dbc = new DataBaseContext(DataBaseContext.DBConnectionString);
            var q = dbc.Questions.First(x => x.Nom == "toxique");
            tmp.QuestionId = q.Id;
            lstInteractions.Add(tmp);
            dbc.Dispose();

            objet = false;
            ContentPanelItem.Visibility = Visibility.Collapsed;
        }
    }
}