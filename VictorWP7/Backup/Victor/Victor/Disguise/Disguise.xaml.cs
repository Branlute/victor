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
using Microsoft.Xna.Framework.Media;
using Microsoft.Phone.Tasks;

namespace VictorNamespace
{
    public partial class Disguise : PhoneApplicationPage
    {
        private Popup popup;
        private BackgroundWorker backroungWorker;
        private DataBaseContext dbc;

        private static List<Accessoire> lstHeads;
        private static List<Accessoire> lstShirts;
        private static List<Accessoire> lstPants;

        private static int indexHead;
        private static int indexShirt;
        private static int indexPants;

        private static bool done = false;
        private bool ok = true;

        public Disguise()
        {
            InitializeComponent();
        }

        protected override void  OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
 	         base.OnNavigatedTo(e);

             if (!done)
             {
                 lstHeads = new List<Accessoire>();
                 lstPants = new List<Accessoire>();
                 lstShirts = new List<Accessoire>();
             }

            //On charge les données
            ShowPopup();
        }

        private void ShowPopup()
        {
            popup = new Popup();
            popup.Child = new PopupSplash();
            popup.IsOpen = true;

            backroungWorker = new BackgroundWorker();
            backroungWorker.DoWork += new DoWorkEventHandler(backroungWorker_DoWork);
            backroungWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backroungWorker_RunWorkerCompleted);

            backroungWorker.RunWorkerAsync();
        }

        void backroungWorker_DoWork(object sender, DoWorkEventArgs e)
    {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                //On récupère les informations
                dbc = new DataBaseContext(DataBaseContext.DBConnectionString);

                if (!done)
                {
                    //Les T-shirts
                    var shirts = from accessoire in dbc.Accessoires
                                 where accessoire.Modele.Nom == "shirt"
                                 select accessoire;

                    foreach (var shirt in shirts)
                    {
                        lstShirts.Add(shirt);
                    }

                    //Les Pantalons
                    var pants = from accessoire in dbc.Accessoires
                                where accessoire.Modele.Nom == "pants"
                                select accessoire;

                    foreach (var pant in pants)
                    {
                        lstPants.Add(pant);
                    }

                    //La tete
                    var heads = from accessoire in dbc.Accessoires
                                where accessoire.Modele.Nom == "head"
                                select accessoire;

                    foreach (var head in heads)
                    {
                        lstHeads.Add(head);
                    }
                }
                
                //MAJ des index
                //TETE
                var tete = from tetes in dbc.Accessoires
                           where tetes.Porte == true && tetes.Modele.Nom == "head"
                           select tetes;

                if (tete.Count() == 0)
                {
                    indexHead = -1;
                }
                else
                {
                    indexHead = lstHeads.IndexOf(tete.First());
                }

                //PANTALON
                var pantalon = from pantalons in dbc.Accessoires
                               where pantalons.Porte == true && pantalons.Modele.Nom == "pants"
                               select pantalons;

                if (pantalon.Count() == 0)
                {
                    indexPants = -1;
                }
                else
                {
                    indexPants = lstPants.IndexOf(pantalon.First());
                }

                //CHEMISE
                var chemise = from chemises in dbc.Accessoires
                              where chemises.Porte == true && chemises.Modele.Nom == "shirt"
                              select chemises;

                if (chemise.Count() == 0)
                {
                    indexShirt = -1;
                }
                else
                {
                    indexShirt = lstShirts.IndexOf(chemise.First());
                }
            });
        }


        void backroungWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {           
            dbc.Dispose();

            //Tete
            if (indexHead == -1)
            {
                imgPreviousHead.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (indexHead == lstHeads.Count - 1)
                {
                    imgNextHead.Visibility = Visibility.Collapsed;
                }

                imgHead.Source = new BitmapImage(new Uri("/Victor;component/" + lstHeads.ElementAt(indexHead).Url + ".png", UriKind.Relative));
            }

            //Shirt
            if (indexShirt == -1)
            {
                imgPreviousShirt.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (indexShirt == lstShirts.Count - 1)
                {
                    imgNextShirt.Visibility = Visibility.Collapsed;
                }

                imgShirt.Source = new BitmapImage(new Uri("/Victor;component/" + lstShirts.ElementAt(indexShirt).Url + ".png", UriKind.Relative));
            }

            //Pantalon
            if (indexPants == -1)
            {
                imgPreviousPants.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (indexPants == lstPants.Count - 1)
                {
                    imgNextPants.Visibility = Visibility.Collapsed;
                }

                imgPants.Source = new BitmapImage(new Uri("/Victor;component/" + lstPants.ElementAt(indexPants).Url + ".png", UriKind.Relative));
            }

            popup.IsOpen = false;
        }

        private void imgPreviousHead_Tap(object sender, GestureEventArgs e)
        {
            indexHead--;

            if (indexHead == -1)
            {
                imgHead.Source = new BitmapImage(new Uri("", UriKind.Relative));
                imgPreviousHead.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                if (lstHeads.ElementAt(indexHead).Dispo == true)
                {
                    imgHead.Source = new BitmapImage(new Uri("/Victor;component/" + lstHeads.ElementAt(indexHead).Url + ".png", UriKind.Relative));
                }
                else
                {
                    imgHead.Source = new BitmapImage(new Uri("/Victor;component/Images/Accessoires/head_no.png", UriKind.Relative));
                }

                if (indexHead == lstHeads.Count - 2)
                {
                    imgNextHead.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void imgNextHead_Tap(object sender, GestureEventArgs e)
        {
            //Affichage du bouton Previous
            if (indexHead == -1)
            {
                imgPreviousHead.Visibility = System.Windows.Visibility.Visible;
            }

            indexHead++;

            if (lstHeads.ElementAt(indexHead).Dispo == true)
            {
                imgHead.Source = new BitmapImage(new Uri("/Victor;component/" + lstHeads.ElementAt(indexHead).Url + ".png", UriKind.Relative));
            }
            else
            {
                imgHead.Source = new BitmapImage(new Uri("/Victor;component/Images/Accessoires/head_no.png", UriKind.Relative));
            }

            //On cache le bouton Next
            if (indexHead == lstHeads.Count - 1)
            {
                imgNextHead.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void imgPreviousShirt_Tap(object sender, GestureEventArgs e)
        {
            indexShirt--;

            if (indexShirt == -1)
            {
                imgShirt.Source = new BitmapImage(new Uri("", UriKind.Relative));
                imgPreviousShirt.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                if (lstShirts.ElementAt(indexShirt).Dispo == true)
                {
                    imgShirt.Source = new BitmapImage(new Uri("/Victor;component/" + lstShirts.ElementAt(indexShirt).Url + ".png", UriKind.Relative));
                }
                else
                {
                    imgShirt.Source = new BitmapImage(new Uri("/Victor;component/Images/Accessoires/shirt_no.png", UriKind.Relative));
                }

                if (indexShirt == lstShirts.Count - 2)
                {
                    imgNextShirt.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void imgNextShirt_Tap(object sender, GestureEventArgs e)
        {
            //Affichage du bouton Previous
            if (indexShirt == -1)
            {
                imgPreviousShirt.Visibility = System.Windows.Visibility.Visible;
            }

            indexShirt++;

            if (lstShirts.ElementAt(indexShirt).Dispo == true)
            {
                imgShirt.Source = new BitmapImage(new Uri("/Victor;component/" + lstShirts.ElementAt(indexShirt).Url + ".png", UriKind.Relative));
            }
            else
            {
                imgShirt.Source = new BitmapImage(new Uri("/Victor;component/Images/Accessoires/shirt_no.png", UriKind.Relative));
            }

            //On cache le bouton Next
            if (indexShirt == lstShirts.Count - 1)
            {
                imgNextShirt.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void imgPreviousPants_Tap(object sender, GestureEventArgs e)
        {
            indexPants--;

            if (indexPants == -1)
            {
                imgPants.Source = new BitmapImage(new Uri("", UriKind.Relative));
                imgPreviousPants.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                if (lstPants.ElementAt(indexPants).Dispo == true)
                {
                    imgPants.Source = new BitmapImage(new Uri("/Victor;component/" + lstPants.ElementAt(indexPants).Url + ".png", UriKind.Relative));
                }
                else
                {
                    imgPants.Source = new BitmapImage(new Uri("/Victor;component/Images/Accessoires/pants_no.png", UriKind.Relative));
                }

                if (indexPants == lstPants.Count - 2)
                {
                    imgNextPants.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void imgNextPants_Tap(object sender, GestureEventArgs e)
        {
            //Affichage du bouton Previous
            if (indexPants == -1)
            {
                imgPreviousPants.Visibility = System.Windows.Visibility.Visible;
            }

            indexPants++;

            if (lstPants.ElementAt(indexPants).Dispo == true)
            {
                imgPants.Source = new BitmapImage(new Uri("/Victor;component/" + lstPants.ElementAt(indexPants).Url + ".png", UriKind.Relative));
            }
            else
            {
                imgPants.Source = new BitmapImage(new Uri("/Victor;component/Images/Accessoires/pants_no.png", UriKind.Relative));
            }

            //On cache le bouton Next
            if (indexPants == lstPants.Count - 1)
            {
                imgNextPants.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void appBarSav_Click(object sender, EventArgs e)
        {
            //On vérifie si Victor est habillé ou nu
            if(indexHead == -1 && indexPants == -1 && indexShirt == -1)
            {
                ok = false;
                MessageBox.Show("You cannot save a photo of a naked Victor !");
            }
            else {
                if(indexHead != -1  && ok == true)
                {
                    if (lstHeads.ElementAt(indexHead).Dispo == false)
                    {
                        ok = false;
                        MessageBox.Show("You cannot save a photo with a lock clothe !");
                    }
                }

                if (indexShirt != -1 && ok == true)
                {
                    if (lstShirts.ElementAt(indexShirt).Dispo == false)
                    {
                        ok = false;
                        MessageBox.Show("You cannot save a photo with a lock clothe !");
                    }
                }

                if (indexPants != -1 && ok == true)
                {
                    if (lstPants.ElementAt(indexPants).Dispo == false)
                    {
                        ok = false;
                        MessageBox.Show("You cannot save a photo with a lock clothe !");
                    }
                }
            }

            if (ok == true)
            {
                ShowPopupSave();
            }
            else
            {
                ok = true;
            }
        }

        private void ShowPopupSave()
        {
 	        popup = new Popup();
            popup.Child = new PopupSplash();
            popup.IsOpen = true;

            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);

            bgw.RunWorkerAsync();
        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            popup.IsOpen = false;
            MessageBox.Show("The photo can be viewed into the gallery of your phone.", "Operation succeed", MessageBoxButton.OK);
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    //Init
                    WriteableBitmap wb = new WriteableBitmap(400, 340);
                    TranslateTransform t = new TranslateTransform();

                    //Fond orange
                    Rectangle rec = new Rectangle();
                    rec.Width = 399;
                    rec.Height = 330;
                    rec.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 154, 0));
                    wb.Render(rec, new TranslateTransform());

                    //Victor
                    wb.Render(imgVictor, new TranslateTransform());
                    wb.Invalidate();

                    //Tete
                    t.X = 72;
                    t.Y = -11;
                    wb.Render(imgHead, t);
                    wb.Invalidate();

                    //Chemise
                    t.X = 115;
                    t.Y = 146;
                    wb.Render(imgShirt, t);
                    wb.Invalidate();

                    //Pantalon
                    t.X = 248;
                    t.Y = 146;
                    wb.Render(imgPants, t);
                    wb.Invalidate();

                    //Sauvegarde
                    string nom = "Victor " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " " + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".jpg";

                    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        //On copie dans l'isolated storage
                        IsolatedStorageFileStream fileStream = isf.CreateFile(nom);
                        wb.SaveJpeg(fileStream, wb.PixelWidth, wb.PixelHeight, 0, 100);
                        fileStream.Close();

                        //On transfère sur le téléphone
                        using (fileStream = isf.OpenFile(nom, FileMode.Open, FileAccess.Read))
                        {
                            MediaLibrary mediaLibrary = new MediaLibrary();
                            Picture pic = mediaLibrary.SavePicture(nom, fileStream);
                        }

                        fileStream.Close();

                        //On supprime la photo de l'isolated
                        if (isf.FileExists(nom))
                        {
                            isf.DeleteFile(nom);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error occured", MessageBoxButton.OK);
            }
        }

        private void appBarDownload_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This option is not implemented yet ! You will be able to buy exclusive items on our store !", "Coming soon !", MessageBoxButton.OK);
            WebBrowserTask wbTask = new WebBrowserTask();
            wbTask.Uri = new Uri("http://market.victor-app.fr", UriKind.Absolute);
            wbTask.Show();
        }

        private void appBarShare_Click(object sender, EventArgs e)
        {
            bool ok = true;

            if (indexHead != -1)
            {
                if (lstHeads.ElementAt(indexHead).Dispo == false)
                {
                    ok = false;
                }
            }

            if (indexShirt != -1)
            {
                if (lstShirts.ElementAt(indexShirt).Dispo == false)
                {
                    ok = false;
                }
            }

            if (indexPants != -1)
            {
                if (lstPants.ElementAt(indexPants).Dispo == false)
                {
                    ok = false;
                }
            }

            //On verifie que Victor n'est pas habillé avec une vétement non débloqué
            if (!ok)
            {
                MessageBox.Show("You cannot disguise a photo with a lock clothe !");
            }
            else
            {
                ShowPopupShare();
            }
        }

        private void ShowPopupShare()
        {
            popup = new Popup();
            popup.Child = new PopupSplash();
            popup.IsOpen = true;

            BackgroundWorker bgw2 = new BackgroundWorker();
            bgw2.DoWork += new DoWorkEventHandler(bgw2_DoWork);
            bgw2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw2_RunWorkerCompleted);

            bgw2.RunWorkerAsync();
        }

        void bgw2_DoWork(object sender, DoWorkEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                dbc = new DataBaseContext(DataBaseContext.DBConnectionString);

                //On met tout à false
                var alltete = from allHead in dbc.Accessoires
                              where allHead.Modele.Nom == "head"
                              select allHead;

                foreach (var all in alltete)
                {
                    all.Porte = false;
                }

                var allPantalon = from allPants in dbc.Accessoires
                                  where allPants.Modele.Nom == "pants"
                                  select allPants;

                foreach (var all in allPantalon)
                {
                    all.Porte = false;
                }

                var allChemise = from allShirts in dbc.Accessoires
                                 where allShirts.Modele.Nom == "shirt"
                                 select allShirts;

                foreach (var all in allChemise)
                {
                    all.Porte = false;
                }

                dbc.SubmitChanges();

                if (indexHead != -1)
                {
                    var updateTete = dbc.Accessoires.First(x => x.Id == lstHeads.ElementAt(indexHead).Id);
                    updateTete.Porte = true;
                    dbc.SubmitChanges(); 
                }

                if (indexPants != -1)
                {
                    var updatePant = dbc.Accessoires.First(x => x.Id == lstPants.ElementAt(indexPants).Id);
                    updatePant.Porte = true;
                    dbc.SubmitChanges(); 
                }

                if (indexShirt != -1)
                {
                    var updateShirt = dbc.Accessoires.First(x => x.Id == lstShirts.ElementAt(indexShirt).Id);
                    updateShirt.Porte = true;
                    dbc.SubmitChanges(); 
                }
            });
        }

        void bgw2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dbc.Dispose();
            popup.IsOpen = false;

            MessageBox.Show("You can now use your customized Victor into the adventure mode !");
            NavigationService.GoBack();
        }
    }
}