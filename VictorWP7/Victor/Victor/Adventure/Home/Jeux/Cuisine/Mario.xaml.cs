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
using Microsoft.Xna.Framework.GamerServices;
using VictorNamespace.Adventure.Home.Jeux.Cuisine;
using System.Windows.Controls.Primitives;

namespace VictorNamespace
{
    public partial class Mario : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        GameTime gameTime;
        SpriteBatch spriteBatch;
        private Map map1;
        private List<Obstacle> _obstacle;
        private Vector2 vitesse = new Vector2(5, 0);
        private Victor _victorCourt;
        private BouteilleCours _bouteilleCourt;
        private int saut=0;
        private TimeSpan _elapsedGameTime = TimeSpan.Zero;
        private TimeSpan _elapsedGameTime2 = TimeSpan.Zero;
        private bool _collision = false;
        private int chute = 0;
        

        //Silverlight
        bool recreation;
        string scene="";
        string niveau="";
        int returned = -2;
        bool fin = false;
        bool debut = true;

        public Mario()
        {
            InitializeComponent();

            // Obtenir le gestionnaire de contenu à partir de l'application
            contentManager = (Application.Current as App).Content;
            map1 = new Map("Images/Mario/backgroundMario");
            _obstacle = new List<Obstacle>();

            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(1400,350,80,80)));
            
            //_obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(1460, 350, 80, 80)));
            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(1840, 350, 80, 80)));
            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(1920, 350, 80, 80)));
            //_obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(1920, 300, 80, 80)));
            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(2600, 350, 80, 80)));
            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(2680, 350, 80, 80)));
            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(2760, 350, 80, 80)));
            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(2840, 350, 80, 80)));
            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(2920, 350, 80, 80)));
            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(3000, 270, 80, 80)));
            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(3080, 270, 80, 80)));
            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(3160, 270, 80, 80)));
            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(3240, 270, 80, 80)));
            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(3400, 190, 80, 80)));
            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(3480, 190, 80, 80)));
            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(3560, 190, 80, 80)));
            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(4400, 350, 80, 80)));
            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(5000, 190, 80, 80)));
            _obstacle.Add(new Obstacle("Images/Mario/bloc", new Rectangle(5500, 350, 80, 80)));

            // Créez une minuterie pour cette page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(133333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;
            _victorCourt = new Victor("marche", new Vector2(500, 280));
            _bouteilleCourt = new BouteilleCours("Images/Mario/bidon", new Vector2(30, 140), 6);

        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            if (recreation == false)
            {
                if (niveau == "Explorer")
                    NavigationService.Navigate(new Uri("/GameExplorer.xaml?id=" + scene, UriKind.Relative));

                else
                {
                    NavigationService.Navigate(new Uri("/Adventure/Home/HomeXNA.xaml?niveau=" + niveau + "&scene=" + scene, UriKind.Relative));
                }
            }
            else
            {
                NavigationService.Navigate(new Uri("/Recreation/Recreation.xaml", UriKind.Relative));
            }
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Définissez le mode de partage de l'appareil graphique pour activer le rendu XNA
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Créer un nouveau SpriteBatch, qui peut être utilisé pour dessiner des textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);
            map1.LoadContent(contentManager);
            foreach (Obstacle objet in _obstacle)
                objet.LoadContent(contentManager);
            _victorCourt.LoadContent(contentManager);
            _bouteilleCourt.LoadContent(contentManager);
            // TODO: utilisez ce contenu pour charger ici le contenu de votre jeu
            TouchPanel.EnabledGestures = GestureType.Tap;


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
            
            if(!fin)
            if (_bouteilleCourt.Position.X + _bouteilleCourt.Rectangle.Width < 0)
            {
                fin = true;
                MessageBox.Show("You escaped to the bottle!", "Congratulation !", MessageBoxButton.OK);
                Session.cuisine_toxique = true;
                if (recreation == false)
                {
                    if (niveau == "Explorer")
                        NavigationService.Navigate(new Uri("/Explorer/GameExplorer.xaml?id=" + scene, UriKind.Relative));

                    else
                    {
                        NavigationService.Navigate(new Uri("/Adventure/Home/HomeXNA.xaml?niveau=" + niveau + "&scene=" + scene, UriKind.Relative));
                    }
                }
                else
                {
                    NavigationService.Navigate(new Uri("/Recreation/Recreation.xaml", UriKind.Relative));
                }

            }
            else
            if (returned > -2)
            {


                if (returned == 0)
                {
                    
                    NavigationService.Navigate(new Uri("/Adventure/Home/Jeux/Cuisine/Mario.xaml?recreation=" + recreation + "&niveau=" + niveau + "&scene=" + scene + "&id=" + Guid.NewGuid(), UriKind.Relative));


                }
                else if (returned == 1)
                {
                    if (recreation == false)
                    {
                        if (niveau == "Explorer")
                            NavigationService.Navigate(new Uri("/Explorer/GameExplorer.xaml?id=" + scene, UriKind.Relative));

                        else
                        {
                            
                            NavigationService.Navigate(new Uri("/Adventure/Home/HomeXNA.xaml?niveau=" + niveau + "&scene=" + scene, UriKind.Relative));
                        }
                    }
                    else
                    {
                        NavigationService.Navigate(new Uri("/Recreation/Recreation.xaml", UriKind.Relative));
                    }
                }

            }
            else
            if (_collision && _victorCourt.Position.X < _bouteilleCourt.Position.X)
            {
                

                if (Guide.IsVisible == false)
                {
                    returned = -1;
                    
                        
                        
                        Guide.BeginShowMessageBox("You lose", "You lose", new List<string> { "Try again", "Quit" }, 0, MessageBoxIcon.Error,
                            asyncResult =>
                            {
                                try
                                {
                                        returned = (int)Guide.EndShowMessageBox(asyncResult);
                                }
                                catch (InvalidOperationException)
                                {
                                    TouchPanel.EnabledGestures = GestureType.None;

   
                                }
                            }, null);
                    
                    
                }

            }
            else
            {
                if (_collision)
                {

                    _victorCourt.deplacement(new Vector2(-5, 0));


                }
                else
                {
                    foreach (Obstacle objet in _obstacle)
                    {
                        if (objet.DestinationRectangle.X < 800 && objet.DestinationRectangle.X+ objet.DestinationRectangle.Width > 0 && objet.Collision(_victorCourt))
                            if (_victorCourt.Position.Y + _victorCourt.Texture.Height > objet.DestinationRectangle.Y + 20)
                            {
                                if (_victorCourt.Position.X - _victorCourt.Texture.Width / 8 < objet.DestinationRectangle.X)
                                    _collision = true;
                                else
                                {
                                    if (saut >= 20)
                                        saut = 0;
                                    TouchPanel.EnabledGestures = GestureType.Tap;
                                }
                            }
                            else
                            {
                                if (saut >= 20)
                                {
                                    saut = 0;
                                    TouchPanel.EnabledGestures = GestureType.Tap;
                                }
                            }
                    }



                    if (_victorCourt.Position.Y < 280 && saut == 0 && chute == 0)
                    {
                        foreach (Obstacle objet in _obstacle)
                        {
                            if (objet.AuDessus(_victorCourt))
                            {
                                chute = 0;
                                break;
                            }
                            else
                            {
                                chute = 1;

                            }

                        }
                    }

                }
                if (_victorCourt.Position.Y >= 280 && chute != 0)
                {
                    chute = 0;
                    TouchPanel.EnabledGestures = GestureType.Tap;
                }
                gameTime = new GameTime(e.TotalTime, e.ElapsedTime);
                _elapsedGameTime2 += gameTime.ElapsedGameTime;
                _bouteilleCourt.UpdateFixe(gameTime);
                if (_elapsedGameTime2.TotalMilliseconds >= 100)
                {
                    _elapsedGameTime2 = TimeSpan.Zero;
                    _bouteilleCourt.deplacement(new Vector2(-2, 0));

                }
                // TODO: ajoutez ici votre logique de mise à jour

                if (saut >= 40 && _victorCourt.Position.Y >= 280)
                {
                    saut = 0;
                    TouchPanel.EnabledGestures = GestureType.Tap;
                }
                map1.deplacement(vitesse);
                int cpt = 0;
                bool suppression = false;
                foreach (Obstacle objet in _obstacle)
                {
                    objet.deplacement(vitesse);
                    if (objet.DestinationRectangle.X + objet.DestinationRectangle.Width < 0)
                    {
                        suppression = true;
                        break;
                    }
                    cpt++;
                }
                if (suppression)
                    _obstacle.RemoveAt(cpt);


                if (saut != 0)
                {
                    _elapsedGameTime += gameTime.ElapsedGameTime;



                    if (_elapsedGameTime.TotalMilliseconds >= 14)
                    {
                        _elapsedGameTime = TimeSpan.Zero;

                        _victorCourt.deplacement(new Vector2(0, -(20 - saut)));
                        if (_victorCourt.Position.Y > 280)
                            _victorCourt.deplacement(new Vector2(0, 280 - _victorCourt.Position.Y));


                        saut++;
                    }
                }
                else
                {
                    if (chute != 0)
                    {

                        _elapsedGameTime += gameTime.ElapsedGameTime;



                        if (_elapsedGameTime.TotalMilliseconds >= 20)
                        {
                            _elapsedGameTime = TimeSpan.Zero;
                            _victorCourt.deplacement(new Vector2(0, chute));
                            if (_victorCourt.Position.Y > 280)
                                _victorCourt.deplacement(new Vector2(0, 280 - _victorCourt.Position.Y));
                            _victorCourt.UpdateFixe(gameTime);
                            chute++;
                        }
                    }
                    else if (TouchPanel.IsGestureAvailable)
                    {
                        //MouseState state = Mouse.GetState();
                        GestureSample state = TouchPanel.ReadGesture();

                        if (state.GestureType == GestureType.Tap)
                        {
                            saut = 1;
                            TouchPanel.EnabledGestures = GestureType.None;
                        }
                        else
                        {
                            _victorCourt.UpdateFixe(gameTime);
                        }
                    }
                    else
                    {

                        _victorCourt.UpdateFixe(gameTime);
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
            spriteBatch.Begin();
            map1.OnDraw(spriteBatch);
            _victorCourt.Draw(spriteBatch);
            
            foreach (Obstacle objet in _obstacle)
            {
                objet.OnDraw(spriteBatch);
            }
            _bouteilleCourt.Draw(spriteBatch);
            spriteBatch.End();
            if (debut)
            {
                MessageBox.Show("Touch the screen to jump and escape to this wicked bottle!", "How to", MessageBoxButton.OK);
                debut = false;
            }

            // TODO: ajoutez ici votre code de dessin
        }
    }
}