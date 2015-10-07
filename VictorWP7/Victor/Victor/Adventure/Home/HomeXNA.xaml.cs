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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Windows.Media.Imaging;

namespace VictorNamespace
{
    public partial class HomeXNA : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;

        //Objets XNA
        private Texture2D background;
        private Vector2 positionBackground;
        private string mouseStatus = "";
        private Vector2 souris = new Vector2(0, 0);
        private Vector2 _destinationSouris = new Vector2(0,0);
        private bool click_souris = false;
        private Victor _victor;
        private UIElementRenderer _uiRenderer;
        private bool _collision = false;
        private Image_Texture _collisionTexture;
        private Texture2D _puzzleTexture;
        private Rectangle _puzzle;
        private int _puzzleApparition;
        private List<Image_Texture> texture_Interactions;
        //private Image_Texture _retour;


        //lampe
        private Image_Texture _lampe;
        private float _rotationLampe;
        private int _deltaRotationLampe;
        private bool _directionLampe;
        private TimeSpan _elapsedGameTimeLampe = TimeSpan.Zero;

        //Aiguilles
        private Image_Texture _petiteAiguille;
        private float _rotationPetiteAiguille;
        private TimeSpan _elapsedGameTimePetiteAiguille = TimeSpan.Zero;
        private Image_Texture _grandeAiguille;
        private float _rotationGrandeAiguille;


        //Flammes
        //private Image_Texture _flammes;
        private BouteilleCours _flammesSprite;
        //private float _rotationGrandeAiguille;





        

        

        //Autre
        private UIElementRenderer uiRenderer;
        private Question question;

        //Objets Debug
        private SpriteFont font;

        //Objets Silverlight
        private List<Interaction> lstImage_Interactions;
        private DataBaseContext dbc;
        private string niveau;
        private string lvl;
        private bool first = true;

        private static bool sonPremiere = true;

        public HomeXNA()
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

        //Initialise le cadres blanc où sont affiché les questions
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
            //Si une question est en train d'être posé
            if(gridBlanc.Width == 720)
            {
                gridBlanc.Margin = new Thickness(900);
                gridBlanc.Width = 0;
                gridBlanc.Height = 0;
                mp3.IsMuted = true;;
				mp3.Stop();
                _collision = false;

                this.reposissionnement();
                TouchPanel.EnabledGestures = GestureType.Tap;

                e.Cancel = true;
            }
            else //Sinon
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }      
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Définissez le mode de partage de l'appareil graphique pour activer le rendu XNA
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            if (sonPremiere)
            {
                mp3.IsMuted = false;
                mp3.Source = new Uri("/MP3/Home/Menu/HomeMenu.mp3", UriKind.Relative);
                mp3.Position = new TimeSpan(0);
                mp3.Volume = 1;
                mp3.Play();
                sonPremiere = false;
            }
            else
            {
                mp3.IsMuted = true;
            }
            // Créer un nouveau SpriteBatch, qui peut être utilisé pour dessiner des textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            question = new Question();
            gridBlanc.Margin = new Thickness(900);
            gridBlanc.Width = 0;

            if (first == true)
            {
                // TODO: utilisez ce contenu pour charger ici le contenu de votre jeu

                //On récupère le niveau et la scène
                var data = this.NavigationContext.QueryString;

                niveau = data["niveau"];
                lvl = data["scene"];

                //Connexion à la BDD
                dbc = new DataBaseContext(DataBaseContext.DBConnectionString);

                /**
                 * BACKGROUND
                **/
                //On récupère l'arrière plan de la scène
                var bg = from scene in dbc.Scenes
                         where scene.Nom == lvl
                         select scene;

                //On affecte
                background = (Application.Current as App).Content.Load<Texture2D>(bg.First().Background);
                positionBackground = Vector2.Zero;

                //Debug, indication des coordonnées du pointeur
                font = (Application.Current as App).Content.Load<SpriteFont>(@"SpriteFont1");

                /**
                 * LES OBJETS
                **/
                lstImage_Interactions = new List<Interaction>();

                var images = from image in dbc.Interactions
                                   where image.Scene.Nom == lvl
                                   select image;               

                /**
                 * RENDU
                **/

                int i = -1;
                Image_Texture textureTmp = new Image_Texture();
                texture_Interactions = new List<Image_Texture>();

                foreach (var interaction in images)
                {
                    //Si l'interaction n'a pas encore d'image 
                    //Créé fonction qui permet d'ajouter une image dans la classe image_interaction
                    if(i != interaction.Id)
                    {
                        if(interaction.Id != images.First().Id)//Si il y a eu une interaction avant, on lance le loader
                        {
                            textureTmp.LoadContent(contentManager);
                            texture_Interactions.Add(textureTmp);
                        }
                        textureTmp = new Image_Texture(interaction.QuestionId, interaction.Question.Nom, interaction.Url, new Vector2(interaction.X, interaction.Y), interaction.Longueur, interaction.Hauteur);
                        
                    }
                    else//Si l'interaction à deja une image ou plusieurs, on rajoute une image
                    {
                        textureTmp.AjoutTexture(interaction.Url, new Vector2(interaction.X, interaction.Y), interaction.Longueur, interaction.Hauteur, (Application.Current as App).Content);
                        //Ajouter une image a l'objet Image_interaction crée précédement.
                    }
                    i = interaction.Id;

                }
                if (images.Count() != 0)//Si il y a des interactions, il faut lancer le content pour la dernière interaction et l'ajouter a la liste
                {
                    textureTmp.LoadContent(contentManager);
                    texture_Interactions.Add(textureTmp);
                }

                //On s'occupe de la lampe
                _lampe = new Image_Texture(-1, "lampe_cuisine", "lampe_cuisine", new Vector2(1250, 0), 130, 258);
                _lampe.LoadContent(contentManager);

                _rotationLampe = 0;
                _deltaRotationLampe = 10;
                _directionLampe = true;


                //On s'occupe des aiguilles
                _petiteAiguille = new Image_Texture(-1, "petite_aiguille", "aiguille-p", new Vector2(379, 43), 3, 12);
                _petiteAiguille.LoadContent(contentManager);
                _rotationGrandeAiguille = 180;

                _grandeAiguille = new Image_Texture(-1, "grande_aiguille", "aiguille-g", new Vector2(379, 43), 3, 23);
                _grandeAiguille.LoadContent(contentManager);
                _rotationPetiteAiguille = 180;



                //On s'occupe de la flamme
                _flammesSprite = new BouteilleCours("FlammesSprites", new Vector2(860, 290), 4);
                _flammesSprite.LoadContent(contentManager);

                /**
                 * FERMETURE BDD
                **/
                dbc.Dispose();

                _uiRenderer = new UIElementRenderer(gridLayout, SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width, SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height);

                /**
                 * LES INTERACTIONS
                **/
                TouchPanel.EnabledGestures = GestureType.Tap;

                /**
                 * AUTRES
                **/

                _victor = new Victor("marche", new Vector2(200, 300));


                    _victor.LoadContent(contentManager);
                

                first = false;
            }
            //On replace victor au milieu de l'écran
            else
            {
                TouchPanel.EnabledGestures = GestureType.Tap;
                souris.X = 200;
                souris.Y = 300;
                _destinationSouris.X = 200;
                _destinationSouris.Y = 300;


                    Vector2 tmpVector = new Vector2(200, 300);
                    _victor.Position = tmpVector;
                    _victor.repositionnement();
            }

            _puzzleTexture = contentManager.Load<Texture2D>("puzzle1");
            _puzzle = new Rectangle(900, 900, 0, 0);
            Session.isPuzzleCuisineAvailable();
            _puzzleApparition = 50;
            _collision = false;
            //_retour = new Image_Texture(-1, "retour", "previous", new Vector2(0, 0), 50, 50);
            //_retour.LoadContent(contentManager);

            // Démarrez la minuterie
            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            initGridBlanc();

            // Arrêtez la minuterie
			mp3.IsMuted = true;
			mp3.Stop();
            timer.Stop();

            TouchPanel.EnabledGestures = GestureType.None;

            // Définissez le mode de partage de l'appareil graphique pour désactiver le rendu XNA
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }




        private void reposissionnement()
        {

            click_souris = false;

            TouchPanel.EnabledGestures = GestureType.Tap;
            _victor.deplacement(new Vector2(200 - _victor.Position.X, 300 - _victor.Position.Y));
            //_images.ElementAt(0).Position = new Vector2(200, 300);
            _victor.repositionnement();

            foreach (Image_Texture intera in texture_Interactions)
            {
                intera.Deplacement(new Vector2(-positionBackground.X, 0));

            }
            _lampe.Deplacement(new Vector2(-positionBackground.X, 0));
            _grandeAiguille.Deplacement(new Vector2(-positionBackground.X, 0));
            _petiteAiguille.Deplacement(new Vector2(-positionBackground.X, 0));
            _flammesSprite.deplacement(new Vector2(-positionBackground.X, 0));

            positionBackground.X = 0;

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
                    if (Session.cuisine_puzzle == true)
                    {

                        if (_puzzleApparition > 0)
                        {
                            _puzzle.Width += 5;
                            _puzzle.Height += 4;

                            _puzzleApparition--;
                        }
                        _puzzle.X = 400 - _puzzle.Width / 2;
                        _puzzle.Y = 230 - _puzzle.Height / 2;

                    }
                    if (TouchPanel.IsGestureAvailable)
                    {
                        //MouseState state = Mouse.GetState();
                        GestureSample state = TouchPanel.ReadGesture();

                        if (state.GestureType == GestureType.Tap)
                        {
                            mouseStatus = string.Format("{0} ; {1}", state.Position.X, state.Position.Y);
                            souris.X = state.Position.X;
                            souris.Y = state.Position.Y;
                            _destinationSouris.X = state.Position.X;
                            if (state.Position.Y < 245)
                            {
                                _destinationSouris.Y = 245;
                            }
                            else if (state.Position.Y > 320)
                            {
                                _destinationSouris.Y = 320;
                            }
                            else
                            {
                                _destinationSouris.Y = state.Position.Y;
                            }

                            click_souris = true;
                        }
                        /*if (_retour.Rectangle.First().X <= state.Position.X && _retour.Rectangle.First().X + _retour.Rectangle.First().Width >= state.Position.X && _retour.Rectangle.First().Y <= state.Position.Y && _retour.Rectangle.First().Y + _retour.Rectangle.First().Height >= state.Position.Y)
                        {
                            NavigationService.Navigate(new Uri("/Adventure/Home/HomeMenu.xaml", UriKind.Relative));
                        }
                        else*/
                        if (Session.cuisine_puzzle == true && _puzzle.X <= state.Position.X && _puzzle.X + _puzzle.Width >= state.Position.X && _puzzle.Y <= state.Position.Y && _puzzle.Y + _puzzle.Height >= state.Position.Y)
                        {
                            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                        }
                    }


                        if (click_souris)
                        {
                            _victor.Update(new GameTime(e.TotalTime, e.ElapsedTime), SharedGraphicsDeviceManager.Current, _destinationSouris);
                            foreach (var texture in texture_Interactions)
                            {
                                int cpt = 0;
                                foreach (var position in texture.Position)
                                {
                                    if (souris.X >= position.X && souris.X <= position.X + texture.Rectangle.ElementAt(cpt).Width && souris.Y >= position.Y && souris.Y <= position.Y + texture.Rectangle.ElementAt(cpt).Height)
                                    {
                                        _collision = texture.Collision(_victor);// && texture.CollisionParPixel(image);
                                        if (_collision)
                                        {
                                            mouseStatus = "collision";

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
                        }
                        else
                        {
                            _victor.Update(new GameTime(e.TotalTime, e.ElapsedTime), SharedGraphicsDeviceManager.Current);
                        }
                    
                }


                if (_victor.Position.X > 600 && positionBackground.X + background.Width > 800)
                {
                    if (positionBackground.X - _victor.Position.X - 200 < -background.Width)
                    {

                        _victor.deplacement(new Vector2(positionBackground.X - _victor.Position.X - 200 + background.Width, 0));


                    }
                    foreach (Image_Texture intera in texture_Interactions)
                    {
                        intera.Deplacement(new Vector2(600 - _victor.Position.X, 0));

                    }
                    _lampe.Deplacement(new Vector2(600 - _victor.Position.X, 0));
                    _grandeAiguille.Deplacement(new Vector2(600 - _victor.Position.X, 0));
                    _petiteAiguille.Deplacement(new Vector2(600 - _victor.Position.X, 0));
                    _flammesSprite.deplacement(new Vector2(600 - _victor.Position.X, 0));
                    souris.X += 600 - _victor.Position.X;
                    _destinationSouris.X += 600 - _victor.Position.X;
                    positionBackground.X += 600 - _victor.Position.X;
                    _victor.deplacement(new Vector2(600 - _victor.Position.X, 0));

                }
                else if (_victor.Position.X < 200 && positionBackground.X < 0)
                {
                    if (positionBackground.X - (_victor.Position.X - 200) > 0)
                    {

                        _victor.deplacement(new Vector2((positionBackground.X - (_victor.Position.X - 200)), 0));


                    }
                    foreach (Image_Texture intera in texture_Interactions)
                    {
                        intera.Deplacement(new Vector2(200 - _victor.Position.X, 0));
                    }
                    _lampe.Deplacement(new Vector2(200 - _victor.Position.X, 0));
                    _grandeAiguille.Deplacement(new Vector2(200 - _victor.Position.X, 0));
                    _petiteAiguille.Deplacement(new Vector2(200 - _victor.Position.X, 0));
                    _flammesSprite.deplacement(new Vector2(200 - _victor.Position.X, 0));
                    souris.X += 200 - _victor.Position.X;
                    _destinationSouris.X += 200 - _victor.Position.X;
                    positionBackground.X += 200 - _victor.Position.X;
                    _victor.deplacement(new Vector2(200 - _victor.Position.X, 0));
                }


                //On fait bouger la flamme
                _flammesSprite.UpdateFixe(new GameTime(e.TotalTime, e.ElapsedTime));

                //On fait bouger la lampe
                 _elapsedGameTimeLampe += new GameTime(e.TotalTime, e.ElapsedTime).ElapsedGameTime;

                 if (_elapsedGameTimeLampe.TotalMilliseconds >= 60)
                 {

                     _elapsedGameTimeLampe = TimeSpan.Zero;

                     if (_rotationLampe >= 0)
                     {
                         if (!_directionLampe)
                         {
                             _deltaRotationLampe = 10;
                             _directionLampe = true;
                         }

                         _deltaRotationLampe -= 1;
                     }
                     else
                     {
                         if (_directionLampe)
                         {
                             _deltaRotationLampe = -10;
                             _directionLampe = false;
                         }

                         _deltaRotationLampe += 1;
                     }
                     _rotationLampe += _deltaRotationLampe;
                 }
                _elapsedGameTimePetiteAiguille += new GameTime(e.TotalTime, e.ElapsedTime).ElapsedGameTime;

                if (_elapsedGameTimePetiteAiguille.TotalMilliseconds >= 60)
                {

                    _elapsedGameTimePetiteAiguille = TimeSpan.Zero;
                    _rotationPetiteAiguille += 1;
                    if (_rotationPetiteAiguille >= 360)
                        _rotationPetiteAiguille -= 360;
                }

                
                    _rotationGrandeAiguille += 6;
                    if (_rotationGrandeAiguille >= 360)
                        _rotationGrandeAiguille -= 360;
 

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
            spriteBatch.Draw(background, positionBackground, Color.White);
             
            //spriteBatch.Draw(_uiRenderer.Texture, Vector2.Zero, Color.White);
            _grandeAiguille.DrawRotation(spriteBatch, MathHelper.ToRadians(_rotationGrandeAiguille), new Vector2(_grandeAiguille.Rectangle.ElementAt(0).Width / 2, 0));
            _petiteAiguille.DrawRotation(spriteBatch, MathHelper.ToRadians(_rotationPetiteAiguille), new Vector2(_petiteAiguille.Rectangle.ElementAt(0).Width / 2, 0));
            _flammesSprite.Draw(spriteBatch);
            foreach (var texture in texture_Interactions)
            {
                texture.Draw(spriteBatch);
            }


            _victor.Draw(spriteBatch);
            _lampe.DrawRotation(spriteBatch, MathHelper.ToRadians(_rotationLampe), new Vector2(_lampe.Rectangle.ElementAt(0).Width / 2, 0));
            
            
            
            if(Session.cuisine_puzzle == true)
            spriteBatch.Draw(_puzzleTexture, _puzzle, Color.White);
            //_retour.Draw(spriteBatch);
            //spriteBatch.DrawString(font, mouseStatus, Vector2.Zero, Color.Black);
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
            question = dbc.Questions.First(x => x.Id == int.Parse((sender as Image_Texture).IdQuestion.ToString()));
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
                NavigationService.Navigate(new Uri("/Adventure/Home/Jeux/Cuisine/Casserole.xaml?recreation=false&niveau=" + niveau  +"&scene=" + lvl, UriKind.Relative));
            }
            else if (nom == "toxique")
            {
                NavigationService.Navigate(new Uri("/Adventure/Home/Jeux/Cuisine/Mario.xaml?recreation=false&niveau=" + niveau + "&scene=" + lvl, UriKind.Relative));
            }
            else if (nom == "grillepain")
            {
                NavigationService.Navigate(new Uri("/Adventure/Home/Jeux/Cuisine/Grillepain.xaml?recreation=false&niveau=" + niveau + "&scene=" + lvl, UriKind.Relative));
            }
            else if (nom == "couteau")
            {
                NavigationService.Navigate(new Uri("/Adventure/Home/Jeux/Cuisine/Couteau.xaml?recreation=false&niveau=" + niveau + "&scene=" + lvl, UriKind.Relative));
            }
            else if (nom == "four")
            {
                NavigationService.Navigate(new Uri("/Adventure/Home/Jeux/Cuisine/Four.xaml?recreation=false&niveau=" + niveau + "&scene=" + lvl, UriKind.Relative));
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