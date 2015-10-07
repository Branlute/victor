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

namespace VictorNamespace
{
    public partial class Bouteille : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;



        //private Texture2D background;
        private Image_Texture _bouteille;
        private Image_Texture _bouchon;
        private Texture2D _bouchonTexture;
        private Rectangle _bouchonRectangleSource;
        private Rectangle _bouchonRectangleDestination;
        private TimeSpan _tempsRestant = TimeSpan.Zero;
        private SpriteFont _affichageTempsRestant;
        private int _deplacement;
        private bool _fini;

        private bool recreation = false;
        private string niveau="";
        private string scene="";
        

        public Bouteille()
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


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Définissez le mode de partage de l'appareil graphique pour activer le rendu XNA
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Créer un nouveau SpriteBatch, qui peut être utilisé pour dessiner des textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            // TODO: utilisez ce contenu pour charger ici le contenu de votre jeu

            //background = contentManager.Load<Texture2D>("");

            //On valide la question

            _bouteille = new Image_Texture(0, "bouteille_MiniJeu", "bouteille", new Vector2(7, 100), 446, 600);
            _bouteille.LoadContent(contentManager);
            _bouchon = new Image_Texture(0, "bouchon_MiniJeu", "bouchon", new Vector2(118, 5), 80, 87);
            _bouchon.LoadContent(contentManager);
            
            
            _bouchonTexture = contentManager.Load<Texture2D>("bouchon1");
            _bouchonRectangleSource = new Rectangle(0, 0, _bouchonTexture.Width / 6, _bouchonTexture.Height);
            _bouchonRectangleDestination = new Rectangle(124, 20, 70,64);

      

            TouchPanel.EnabledGestures = GestureType.HorizontalDrag;

            
            _affichageTempsRestant = contentManager.Load<SpriteFont>(@"SpriteFont1");
            _fini = false;


            //On parse 
            var data = this.NavigationContext.QueryString;
            recreation = bool.Parse(data["recreation"]);
            if (!recreation)
            {
                scene = data["scene"];
                niveau = data["niveau"];
            }

            // Démarrez la minuterie
            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Arrêtez la minuterie
            timer.Stop();

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
            // TODO: ajoutez ici votre logique de mise à jour
            if (_fini == false)
            {
                _tempsRestant += new GameTime(e.TotalTime, e.ElapsedTime).ElapsedGameTime;

                if (_tempsRestant.Seconds == 15)
                {
                    MessageBox.Show("You did not recap the bottle !", "Time up !", MessageBoxButton.OK);

                    if (recreation == false)
                    {
                        if (niveau == "Explorer")
                            NavigationService.Navigate(new Uri("/GameExplorer.xaml?id=" + scene, UriKind.Relative));

                        else
                            NavigationService.Navigate(new Uri("/Adventure/Home/HomeXNA.xaml?niveau=" + niveau + "&scene=" + scene, UriKind.Relative));
                    }
                    else
                    {
                        NavigationService.Navigate(new Uri("/Recreation/Recreation.xaml", UriKind.Relative));
                    }
                    _fini = true;
                }

                if (_bouchon.Rectangle.First().Y > 50)
                {
                    
                    MessageBox.Show("You recaped the bottle !", "Congratulation !", MessageBoxButton.OK);

                    if (recreation == false)
                    {
                        if (niveau == "Explorer")
                            NavigationService.Navigate(new Uri("/GameExplorer.xaml?id=" + scene, UriKind.Relative));

                        else
                        {
                            Session.cuisine_toxique = true;
                            NavigationService.Navigate(new Uri("/Adventure/Home/HomeXNA.xaml?niveau=" + niveau + "&scene=" + scene, UriKind.Relative));
                        }
                    }
                    else
                    {
                        NavigationService.Navigate(new Uri("/Recreation/Recreation.xaml", UriKind.Relative));
                    }
                    _fini = true;
                }

                if (TouchPanel.IsGestureAvailable)
                {



                    GestureSample state = TouchPanel.ReadGesture();

                    if (state.GestureType == GestureType.HorizontalDrag)
                    {
                        if (state.Delta.X < 0)
                        {
                            _deplacement = 50;
                            if (_deplacement % 10 == 0)
                            {
                                _bouchonRectangleDestination.Y += 1;
                                _bouchon.Deplacement(new Vector2(0, 1));
                            }
                            _bouchonRectangleSource.X += 2;
                            if (_bouchonRectangleSource.X + _bouchonRectangleSource.Width >= _bouchonTexture.Width)
                            {
                                _bouchonRectangleSource.X = 0;


                            }
                            

                            _deplacement--;
                        }
                        else
                            if (state.Delta.X > 0)
                            {
                                _deplacement = -50;
                                if (_deplacement % 10 == 0)
                                {
                                    _bouchonRectangleDestination.Y -= 1;
                                    _bouchon.Deplacement(new Vector2(0, -1));
                                }
                                _bouchonRectangleSource.X -= 2;
                                if (_bouchonRectangleSource.X < 0)
                                {
                                    _bouchonRectangleSource.X = _bouchonTexture.Width - _bouchonRectangleSource.Width;


                                }
                                
                                _deplacement++;
                            }
                    }
                    else
                    {
                        if (_deplacement < 0)
                        {

                            if (_deplacement % 10 == 0)
                            {
                                _bouchonRectangleDestination.Y -= 1;
                                _bouchon.Deplacement(new Vector2(0, -1));
                            }
                            _bouchonRectangleSource.X -= 2;
                            if (_bouchonRectangleSource.X < 0)
                            {
                                _bouchonRectangleSource.X = _bouchonTexture.Width - _bouchonRectangleSource.Width;


                            }

                            _deplacement++;
                        }
                        else
                        {
                            if (_deplacement > 0)
                            {
                                if (_deplacement % 10 == 0)
                                {
                                    _bouchonRectangleDestination.Y += 1;
                                    _bouchon.Deplacement(new Vector2(0, 1));
                                }
                                _bouchonRectangleSource.X += 2;
                                if (_bouchonRectangleSource.X + _bouchonRectangleSource.Width >= _bouchonTexture.Width)
                                {
                                    _bouchonRectangleSource.X = 0;


                                }
                                
                                _deplacement--;

                            }
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Permet à la page de se dessiner.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);



            // TODO: ajoutez ici votre code de dessin

            spriteBatch.Begin();
            //spriteBatch.Draw(background, Vector2.Zero, Color.White);



            

            _bouteille.Draw(spriteBatch);
            _bouchon.Draw(spriteBatch);

            spriteBatch.Draw(_bouchonTexture, _bouchonRectangleDestination, _bouchonRectangleSource, Color.White);

            if (_tempsRestant.Seconds > 4)

                spriteBatch.DrawString(_affichageTempsRestant, "Time: " + (14 - _tempsRestant.Seconds) + "." + (1000 - _tempsRestant.Milliseconds), new Vector2(0, 20), Color.Red);
            else
                spriteBatch.DrawString(_affichageTempsRestant, "Time: " + (14 - _tempsRestant.Seconds) + "." + (1000 - _tempsRestant.Milliseconds), new Vector2(0, 20), Color.Black);

            spriteBatch.End();
        }
    }
}