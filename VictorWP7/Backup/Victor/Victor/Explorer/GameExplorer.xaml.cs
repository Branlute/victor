using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;

namespace VictorNamespace
{
    public partial class GameExplorer : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;

        //Objets XNA
        private bool _collision = false;
        private List<InteractionPhoto> lstInteractionsPhotos;
        private List<Image_TexturePhoto> texture_InteractionsPhotos;
        private Victor _victor;
        private bool click_souris = false;
        private Vector2 souris = new Vector2(0, 0);
        private Vector2 _destinationSouris = new Vector2(0, 0);
        //private Image_Texture _retour;

        private Image_TexturePhoto _collisionTexture;

        //Autre
        private Question question;

        //Objets Silverlight
        private BitmapImage img;
        private UIElementRenderer uiRenderer;
        private UIElementRenderer _uiRenderer;
        private int id;
        private bool first = true;
        private DataBaseContext dbc;


        public GameExplorer()
        {
            InitializeComponent();

            // Obtenir le gestionnaire de contenu à partir de l'application
            contentManager = (Application.Current as App).Content;

            // Créez une minuterie pour cette page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;
        }

        private void initGridBlanc()
        {
            txtExplication.Visibility = Visibility.Collapsed;
            txtQuestion.Visibility = Visibility.Collapsed;
            btnYes.Visibility = Visibility.Visible;
            btnNo.Visibility = Visibility.Visible;
            btnQuestion.Visibility = Visibility.Collapsed;
            btnGame.Visibility = Visibility.Collapsed;
            imgTeteVictor.Source = new BitmapImage(new Uri("/Victor;component/Images/Jeu/tete_victor.png", UriKind.Relative));
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            
            base.OnBackKeyPress(e);
            initGridBlanc();

            if (gridBlanc.Width == 720)
            {
                gridBlanc.Margin = new Thickness(900);
                gridBlanc.Width = 0;
                gridBlanc.Height = 0;
                mp3.Stop();
                mp3.IsMuted = true;
                _collision = false;

                this.reposissionnement();
                TouchPanel.EnabledGestures = GestureType.Tap;

                e.Cancel = true;
            }
            else
            {
                NavigationService.Navigate(new Uri("/Explorer/ExplorerMap.xaml", UriKind.Relative));

            }      

            

        }

        private void reposissionnement()
        {

            click_souris = false;

            TouchPanel.EnabledGestures = GestureType.Tap;
            _victor.deplacement(new Vector2(200 - _victor.Position.X, 300 - _victor.Position.Y));
            //_images.ElementAt(0).Position = new Vector2(200, 300);
            _victor.repositionnement();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Définissez le mode de partage de l'appareil graphique pour activer le rendu XNA
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            mp3.IsMuted = true;


            //On parse 
            var data = this.NavigationContext.QueryString;
            id = int.Parse(data["id"]);

            question = new Question();
            gridBlanc.Margin = new Thickness(900);
            gridBlanc.Width = 0;

            // Créer un nouveau SpriteBatch, qui peut être utilisé pour dessiner des textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            // TODO: utilisez ce contenu pour charger ici le contenu de votre jeu
            //On récupère la Photo
            dbc = new DataBaseContext(DataBaseContext.DBConnectionString);
            var photo = from album in dbc.Photos
                        where album.Id == int.Parse(this.NavigationContext.QueryString["id"])
                        select album;

            img = new BitmapImage();

            using (IsolatedStorageFile isoStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isoStorage.FileExists(photo.First().Background))
                {
                    using (IsolatedStorageFileStream fileStream = isoStorage.OpenFile(photo.First().Background, FileMode.Open, FileAccess.Read))
                    {
                        img.SetSource(fileStream);
                        fileStream.Close();
                    }
                }
            }

            imgRendu.Source = img;

            _uiRenderer = new UIElementRenderer(gridImage, SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width, SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height);



            /**
                 * LES OBJETS
                **/
            lstInteractionsPhotos = new List<InteractionPhoto>();

            var images = from image in dbc.InteractionsPhotos
                         where image.PhotoId == int.Parse(this.NavigationContext.QueryString["id"])
                         select image;




            texture_InteractionsPhotos = new List<Image_TexturePhoto>();

            foreach (var interaction in images)
            {
                texture_InteractionsPhotos.Add(new Image_TexturePhoto(interaction.QuestionId, interaction.Question.Nom, new Vector2((float)interaction.X/512 * 800, (float)interaction.Y/307 * 480), (int)((float)interaction.Longueur/512 * 800), (int)((float)interaction.Hauteur/307 * 480)));
                  

                //Créé fonction qui permet d'ajouter une image dans la classe image_interaction
                /*if (i != interaction.Id)
                {
                    if (interaction.Id != images.First().Id)
                    {
                        textureTmp.LoadContent((Application.Current as App).Content);
                        texture_InteractionsPhotos.Add(textureTmp);
                    }
                    textureTmp = new Image_Texture(interaction.QuestionId, interaction.Question.Nom, interaction.Url, new Vector2(interaction.X, interaction.Y), interaction.Longueur, interaction.Hauteur);

                }
                else
                {*/
                //texture_InteractionsPhotos.Add(AjoutTexture(interaction.Url, new Vector2(interaction.X, interaction.Y), interaction.Longueur, interaction.Hauteur, (Application.Current as App).Content);
                    //Ajouter une image a l'objet Image_interaction crée précédement.
                //}
            }
            foreach (Image_TexturePhoto interaction in texture_InteractionsPhotos)
            {
                interaction.LoadContent(contentManager);
            }

            dbc.Dispose();


            _victor = new Victor("marche", new Vector2(100,300));
            _victor.LoadContent(contentManager);




            _collision = false;
            click_souris = false;


            /*_retour = new Image_Texture(-1, "retour", "previous", new Vector2(0, 0), 50, 50);
            _retour.LoadContent(contentManager);
            */


            if(!first)
            {
                TouchPanel.EnabledGestures = GestureType.Tap;
                souris.X = 200;
                souris.Y = 300;


                    Vector2 tmpVector = new Vector2(200, 300);
                    _victor.Position = tmpVector;
                    _victor.repositionnement();

            }
            /**
             * LES INTERACTIONS
            **/
            TouchPanel.EnabledGestures = GestureType.Tap;



            // Démarrez la minuterie
            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            initGridBlanc();
            // Arrêtez la minuterie
            timer.Stop();


            // On stop la detection de l'écran tactile
            TouchPanel.EnabledGestures = GestureType.None;

            // Définissez le mode de partage de l'appareil graphique pour désactiver le rendu XNA
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Permet à la page d'exécuter la logique, comme par exemple mettre à jour le monde,
        /// contrôler les collisions, collecter les données d'entrée, et lire le son.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            if (_collision)
            {

                if (gridBlanc.Width == 720)
                {
                    if (txtQuestion.Visibility == Visibility.Collapsed)
                    {
                        //Audio
                        mp3.IsMuted = false;
                        mp3.Play();

                        txtQuestion.Visibility = Visibility.Visible;
                    }


                }
                else
                {
                    gridBlanc.Width += 36;
                    gridBlanc.Height += 20;
                    gridBlanc.Margin = new Thickness(gridBlanc.Margin.Left - 18, gridBlanc.Margin.Top - 10, 0, 0);

                }
            }
            else
            {
                if (!_collision)
                {
                    if (TouchPanel.IsGestureAvailable)
                    {
                        //MouseState state = Mouse.GetState();
                        GestureSample state = TouchPanel.ReadGesture();

                        if (state.GestureType == GestureType.Tap)
                        {
                            //mouseStatus = string.Format("{0} ; {1}", state.Position.X, state.Position.Y);
                            souris.X = state.Position.X;

                            /*if (state.Position.Y < 215)
                            {
                                souris.Y = 215;
                            }
                            else
                            {*/
                            souris.Y = state.Position.Y;
                            //}

                            click_souris = true;
                        }
                        /*if (_retour.Rectangle.First().X <= state.Position.X && _retour.Rectangle.First().X + _retour.Rectangle.First().Width >= state.Position.X && _retour.Rectangle.First().Y <= state.Position.Y && _retour.Rectangle.First().Y + _retour.Rectangle.First().Height >= state.Position.Y)
                        {
                            NavigationService.Navigate(new Uri("/galerie.xaml", UriKind.Relative));
                        }*/
                    }


                    if (click_souris)
                    {
                        _victor.Update(new GameTime(e.TotalTime, e.ElapsedTime), SharedGraphicsDeviceManager.Current, souris);
                        foreach (var texture in texture_InteractionsPhotos)
                        {
                            int cpt = 0;

                            if (souris.X >= texture.Position.X && souris.X <= texture.Position.X + texture.Rectangle.Width && souris.Y >= texture.Position.Y && souris.Y <= texture.Position.Y + texture.Rectangle.Height)
                            {
                                _collision = texture.Collision(_victor);// && texture.CollisionParPixel(image);
                                if (_collision)
                                {
                                    //mouseStatus = "collision";
                                    TouchPanel.EnabledGestures = GestureType.None;
                                    _collisionTexture = texture;
                                    img_Tap(texture);
                                    //timer.Stop();
                                    //image.retour();
                                    return;
                                }
                            }
                            cpt++;

                        }
                    }
                    else
                    {
                        _victor.Update(new GameTime(e.TotalTime, e.ElapsedTime), SharedGraphicsDeviceManager.Current);
                    }

                }


                /*if (_victor.Position.X > 700 && background.Width - positionBackground.X > 800)
                {
                    foreach (Image_Texture intera in texture_Interactions)
                    {
                        intera.Deplacement(new Vector2(700 - _victor.ElementAt(0).Position.X, 0));

                    }
                    souris.X += 700 - _victor.ElementAt(0).Position.X;
                    positionBackground.X += 700 - _victor.ElementAt(0).Position.X;
                    _victor.ElementAt(0).deplacement(new Vector2(700 - _victor.ElementAt(0).Position.X, 0));

                }
                else if (_victor.ElementAt(0).Position.X < 100 && positionBackground.X > 0)
                {
                    foreach (Image_Texture intera in texture_Interactions)
                    {
                        intera.Deplacement(new Vector2(100 - _victor.ElementAt(0).Position.X, 0));
                    }
                    souris.X += 100 - _victor.ElementAt(0).Position.X;
                    positionBackground.X += 100 - _victor.ElementAt(0).Position.X;
                    _victor.ElementAt(0).deplacement(new Vector2(100 - _victor.ElementAt(0).Position.X, 0));
                }*/
            }
        }

        /// <summary>
        /// Permet à la page de se dessiner.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);

            _uiRenderer.Render();
            spriteBatch.Begin();
            spriteBatch.Draw(_uiRenderer.Texture, Vector2.Zero, Color.White);
            foreach (Image_TexturePhoto interaction in texture_InteractionsPhotos)
            {
                interaction.Draw(spriteBatch);
            }
            _victor.Draw(spriteBatch);
            //_retour.Draw(spriteBatch);
            if (_collision)
            {
                uiRenderer.Render();
                spriteBatch.Draw(uiRenderer.Texture, Vector2.Zero, Color.White);
            }
            spriteBatch.End();
        }



        private void img_Tap(object sender)
        {
            gridBlanc.Width = 0;
            gridBlanc.Height = 0;
            gridBlanc.Margin = new Thickness(400, 240, 0, 0);

            //On cherche la bonne question à afficher
            dbc = new DataBaseContext(DataBaseContext.DBConnectionString);
            question = dbc.Questions.First(x => x.Id == int.Parse((sender as Image_TexturePhoto).IdQuestion.ToString()));
            dbc.Dispose();

            txtQuestion.Text = question.Intitule;

            //Audio
            mp3.IsMuted = false;
            mp3.Source = new Uri(question.Mp3, UriKind.Relative);
            mp3.Position = new TimeSpan(0);
            mp3.Volume = 1;

            uiRenderer = new UIElementRenderer(gridLayout, SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width, SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height);
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            txtExplication.Text = getExplication();

            //Bonne réponse
            if (question.Vrai)
            {
                //On affiche l'explication
                txtExplication.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
                txtExplication.Visibility = Visibility.Visible;

                if (question.Qcm)
                {
                    saveLog(true);
                }

                //On change les boutons
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnGame.Visibility = Visibility.Visible;
            }
            //Mauvaise réponse
            else
            {
                txtExplication.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                txtExplication.Visibility = Visibility.Visible;

                if (question.Qcm)
                {
                    saveLog(false);
                }

                //on change la tete de victor
                imgTeteVictor.Source = new BitmapImage(new Uri("/Victor;component/Images/Jeu/tete_victor_pleure.png", UriKind.Relative));

                //On change les boutons
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnQuestion.Visibility = Visibility.Visible;

            }
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            txtExplication.Text = getExplication();

            //Bonne réponse
            if (!question.Vrai)
            {
                txtExplication.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
                txtExplication.Visibility = Visibility.Visible;

                if (question.Qcm)
                {
                    saveLog(true);
                }

                //On change les boutons
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnGame.Visibility = Visibility.Visible;
            }
            //Mauvaise réponse
            else
            {
                txtExplication.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                txtExplication.Visibility = Visibility.Visible;

                if (question.Qcm)
                {
                    saveLog(false);
                }

                //On vhange la tete de victor
                imgTeteVictor.Source = new BitmapImage(new Uri("/Victor;component/Images/Jeu/tete_victor_pleure.png", UriKind.Relative));

                //On change les boutons
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                btnQuestion.Visibility = Visibility.Visible;
            }
        }

        private void saveLog(bool vrai)
        {
            //On enregistre la réponse dans les logs
            dbc = new DataBaseContext(DataBaseContext.DBConnectionString);
            var reponse = from rep in dbc.Reponses
                          where rep.Vrai == vrai && rep.QuestionId == question.Id
                          select rep;

            dbc.Logs.InsertOnSubmit(new Log() { Date = DateTime.Now, ReponseId = reponse.First().Id });
            dbc.SubmitChanges();
            dbc.Dispose();
        }

        private string getExplication()
        {
            dbc = new DataBaseContext(DataBaseContext.DBConnectionString);
            Explication explication = dbc.Explications.First(x => x.QuestionId == question.Id);
            dbc.Dispose();

            //Audio
            mp3.IsMuted = false;
            mp3.Source = new Uri(explication.Mp3, UriKind.Relative);
            mp3.Position = new TimeSpan(0);
            mp3.Play();

            return explication.Pourquoi;
        }

        private void btnQuestion_Click(object sender, RoutedEventArgs e)
        {
            txtExplication.Visibility = Visibility.Collapsed;

            //On change les boutons
            btnYes.Visibility = Visibility.Visible;
            btnNo.Visibility = Visibility.Visible;
            btnQuestion.Visibility = Visibility.Collapsed;

            //Audio
            mp3.IsMuted = false;
            mp3.Source = new Uri(question.Mp3, UriKind.Relative);
            mp3.Position = new TimeSpan(0);
            mp3.Play();


            //On change l'image
            imgTeteVictor.Source = new BitmapImage(new Uri("/Victor;component/Images/Jeu/tete_victor.png", UriKind.Relative));
        }

        private void btnGame_Click(object sender, RoutedEventArgs e)
        {
            mp3.IsMuted = true;
            mp3.Stop();
            string nom = _collisionTexture.Name;
            if (nom == "casserole")
            {
                NavigationService.Navigate(new Uri("/Adventure/Home/Jeux/Cuisine/Casserole.xaml?recreation=false&niveau=Explorer&scene=" + id, UriKind.Relative));
            }
            else if (nom == "toxique")
            {
                NavigationService.Navigate(new Uri("/Adventure/Home/Jeux/Cuisine/Mario.xaml?recreation=false&niveau=Explorer&scene=" + id, UriKind.Relative));
            }
            else if (nom == "grillepain")
            {
                NavigationService.Navigate(new Uri("/Adventure/Home/Jeux/Cuisine/Grillepain.xaml?recreation=false&niveau=Explorer&scene=" + id, UriKind.Relative));
            }
            else if (nom == "couteau")
            {
                NavigationService.Navigate(new Uri("/Adventure/Home/Jeux/Cuisine/Couteau.xaml?recreation=false&niveau=Explorer&scene=" + id, UriKind.Relative));
            }
            else if (nom == "four")
            {
                NavigationService.Navigate(new Uri("/Adventure/Home/Jeux/Cuisine/Four.xaml?recreation=false&niveau=Explorer&scene=" + id, UriKind.Relative));
            }
        }

        private void imgRepeat_Tap(object sender, GestureEventArgs e)
        {
            mp3.IsMuted = false;
            mp3.Position = new TimeSpan(0);
            mp3.Play();
        }
    }
}