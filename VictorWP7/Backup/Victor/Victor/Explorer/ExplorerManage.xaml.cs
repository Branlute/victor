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
using System.IO.IsolatedStorage;
using System.IO;

namespace VictorNamespace
{
    public partial class ExplorerManage : PhoneApplicationPage
    {
        private Popup popup;
        private DataBaseContext dbc;
        private Photo photo;
        private List<Rectangle> lstRectangles;

        public ExplorerManage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            lstRectangles = new List<Rectangle>();
            refreshList(); 
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            ContentPhoto.Visibility = Visibility.Collapsed;
            ContentLst.Visibility = Visibility.Visible;
        }


        private void refreshList()
        {
            popup = new Popup();
            popup.Child = new PopupSplash();
            popup.IsOpen = true;

            dbc = new DataBaseContext(DataBaseContext.DBConnectionString);

            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);

            bgw.RunWorkerAsync();
        }

        private void bgw_DoWork(object sender, DoWorkEventArgs e) {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var q = from lst in dbc.Photos
                        select lst;

                lstMaps.ItemsSource = q;
            });
        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            dbc.Dispose();
            popup.IsOpen = false;
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //On sélectionne l'item
            if (lstMaps.SelectedIndex != -1)
            {
                photo = (Photo)lstMaps.SelectedItem;

                BitmapImage tmp = new BitmapImage();

                using (IsolatedStorageFile isoStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (isoStorage.FileExists(photo.Background))
                    {
                        using (IsolatedStorageFileStream fileStream = isoStorage.OpenFile(photo.Background, FileMode.Open, FileAccess.Read))
                        {
                            tmp.SetSource(fileStream);
                            fileStream.Close();
                        }
                    }
                }

                img.Source = tmp;

                //On affiche les rectangles
                dbc = new DataBaseContext(DataBaseContext.DBConnectionString);
                var objects = from interactions in dbc.InteractionsPhotos
                              where interactions.PhotoId == photo.Id
                              select interactions;

                foreach (InteractionPhoto ip in objects)
                {
                    Rectangle r = new Rectangle();
                    r.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    r.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    r.Fill = new SolidColorBrush(Color.FromArgb(150, 250, 0, 0));
                    r.Fill.Opacity = 0.5;
                    r.Height = (ip.Hauteur*235)/307;
                    r.Width = (ip.Longueur*392)/512;
                    r.Margin = new Thickness(((ip.X * 392) / 512) + (776 - 392) / 2, ((ip.Y * 235) / 307), 0, 0);

                    ContentPhoto.Children.Add(r);
                    lstRectangles.Add(r);
                }

                ContentPhoto.Visibility = Visibility.Visible;
                ContentLst.Visibility = Visibility.Collapsed;

                lstMaps.SelectedIndex = -1;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure that you want to delete this photo ?", "Sure ?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                ContentPhoto.Visibility = Visibility.Collapsed;
                ContentLst.Visibility = Visibility.Visible;

                //On supprime physiquement
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    //On supprime la photo de l'isolated
                    if (isf.FileExists(photo.Background))
                    {
                        isf.DeleteFile(photo.Background);
                    }
                }

                dbc = new DataBaseContext(DataBaseContext.DBConnectionString);

                dbc.Photos.Attach(photo);
                dbc.Photos.DeleteOnSubmit(photo);
                dbc.SubmitChanges();

                foreach (InteractionPhoto ip in dbc.InteractionsPhotos)
                {
                    if (ip.PhotoId == photo.Id)
                    {
                        //dbc.InteractionsPhotos.Attach(ip);
                        dbc.InteractionsPhotos.DeleteOnSubmit(ip);
                        dbc.SubmitChanges();
                    }
                }

                foreach (Rectangle r in lstRectangles)
                {
                    ContentPhoto.Children.Remove(r);
                }

                lstRectangles.Clear();

                refreshList();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ContentPhoto.Visibility = Visibility.Collapsed;
            ContentLst.Visibility = Visibility.Visible;

            foreach (Rectangle r in lstRectangles)
            {
                ContentPhoto.Children.Remove(r);
            }

            lstRectangles.Clear();
        }
    }
}