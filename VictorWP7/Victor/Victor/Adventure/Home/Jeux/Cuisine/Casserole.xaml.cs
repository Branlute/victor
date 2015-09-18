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
using Microsoft.Xna.Framework.Audio;

namespace VictorNamespace
{
    public partial class Casserole : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;


        //Background
        private Texture2D _textureBackground;


        //private Texture2D background;
        private Image_Texture _casserole;
        private Image_Texture _jauge;
        private Rectangle _jaugeInterieur;
        private List<Image_Texture> _glacons;
        private TimeSpan _elapsedGameTime = TimeSpan.Zero;
        private TimeSpan _tempsRestant = TimeSpan.Zero;
        private int _glaconCapture;
        private SpriteFont _affichageGlaconCapture;
        private SpriteFont _affichageTempsRestant;
        private int _numeroGlacon;
        private Vector2 chute = new Vector2(0, 5);
        System.Random generator = new System.Random();
        private bool _fini;
        private SoundEffect _soundEffect;

        private bool recreation = false;
        private string scene="";
        private string niveau="";

        private bool debut = true;

        public Casserole()
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
            

            _casserole = new Image_Texture(0, "casserole_MiniJeu", "casserole1" , new Vector2(400, 380), 194, 82);
            _casserole.LoadContent(contentManager);

            _jauge = new Image_Texture(0, "jauge_MiniJeu", "jauge", new Vector2(740, 100), 40, 170);
            _jauge.LoadContent(contentManager);
            _jaugeInterieur = new Rectangle(745 , 269 , _jauge.Rectangle.First().Width-10, -2);

            _glacons = new List<Image_Texture>();
            for (int i = 0; i < 10; i++)
            {
                _glacons.Add(new Image_Texture(0, "glacon_MiniJeu", "glacon" , new Vector2(-50,-50), 50,50));
                _glacons.ElementAt(i).LoadContent(contentManager);

            }

            _numeroGlacon = 0;
            _glaconCapture = 0;
            _fini = false;

            TouchPanel.EnabledGestures = GestureType.FreeDrag | GestureType.Tap;

            _affichageGlaconCapture = contentManager.Load<SpriteFont>(@"SpriteFont1");
            _affichageTempsRestant = contentManager.Load<SpriteFont>(@"SpriteFont1");
            _tempsRestant = TimeSpan.Zero;
            _soundEffect = contentManager.Load<SoundEffect>(@"casseroleSon");

            //On parse 
            var data = this.NavigationContext.QueryString;
            recreation = bool.Parse(data["recreation"]);
            if (!recreation)
            {
                scene = data["scene"];
                niveau = data["niveau"];
            }
            //Background
            _textureBackground = contentManager.Load<Texture2D>(@"Background");


            debut = true;

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

                if (_tempsRestant.Seconds == 30)
                {
                    MessageBox.Show("You did not catch 10 ice cubes!", "Time up !", MessageBoxButton.OK);

                    if (recreation == false)
                    {
                        if(niveau=="Explorer")
                            NavigationService.Navigate(new Uri("/Explorer/GameExplorer.xaml?id=" + scene, UriKind.Relative));
 
                        else
                            NavigationService.Navigate(new Uri("/Adventure/Home/HomeXNA.xaml?niveau=" + niveau + "&scene=" + scene, UriKind.Relative));
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

                    if (state.GestureType == GestureType.FreeDrag || state.GestureType == GestureType.Tap)
                    {
                        _casserole.Deplacement(new Vector2(state.Position.X - (_casserole.Rectangle.First().X + _casserole.Rectangle.First().Width / 2), 0));
                    }
                }

                foreach (var glacon in _glacons)
                {
                    if (glacon.Rectangle.First().Y + glacon.Rectangle.First().Height >= _casserole.Rectangle.First().Y + _casserole.Rectangle.First().Height / 4 && glacon.Rectangle.First().Y + glacon.Rectangle.First().Height < _casserole.Rectangle.First().Y + _casserole.Rectangle.First().Height)
                    {
                        if (glacon.Rectangle.First().X >= _casserole.Rectangle.First().X && glacon.Rectangle.First().X + glacon.Rectangle.First().Width <= _casserole.Rectangle.First().X + _casserole.Rectangle.First().Width/5*3)
                        {
                            glacon.Deplacement(new Vector2(0, 300));
                            _glaconCapture++;
                            _jaugeInterieur.Y -= (_jauge.Rectangle.First().Height) / 10;
                            _jaugeInterieur.Height += _jauge.Rectangle.First().Height / 10;
                            _soundEffect.Play();

                            if (_glaconCapture == 10)
                            {
                                MessageBox.Show("You caught 10 ice cubes!", "Congratulation !", MessageBoxButton.OK);

                                if (recreation == false)
                                {
                                    if (niveau == "Explorer")
                                        NavigationService.Navigate(new Uri("/Explorer/GameExplorer.xaml?id=" + scene, UriKind.Relative));

                                    else
                                    {
                                        Session.cuisine_casserole = true;
                                        NavigationService.Navigate(new Uri("/Adventure/Home/HomeXNA.xaml?niveau=" + niveau + "&scene=" + scene, UriKind.Relative));
                                    }

                                }
                                else
                                {
                                    NavigationService.Navigate(new Uri("/Recreation/Recreation.xaml", UriKind.Relative));
                                }
                                _fini = true;
                            }
                        }
                    }

                }


                List<Image_Texture> tmpGlacons = new List<Image_Texture>();
                int cpt = 0;
                foreach (var glacon in _glacons)
                {
                    if (glacon.Rectangle.First().Y > 480)
                    {
                        List<Rectangle> tmpRectangle = new List<Rectangle>();
                        Image_Texture tmpImage = _glacons.ElementAt(cpt);

                        tmpRectangle.Add(new Rectangle(-50, -50, tmpImage.Rectangle.First().Width, tmpImage.Rectangle.First().Height));
                        tmpImage.Rectangle = tmpRectangle;

                        tmpGlacons.Add(tmpImage);

                    }
                    else
                    {
                        if (glacon.Rectangle.First().Y >= 0)
                        {
                            glacon.Deplacement(chute);
                        }

                        tmpGlacons.Add(glacon);
                    }

                    cpt++;
                }
                _glacons = tmpGlacons;
                _elapsedGameTime += new GameTime(e.TotalTime, e.ElapsedTime).ElapsedGameTime;

                if (_elapsedGameTime.TotalMilliseconds >= 200)
                {
                    _elapsedGameTime = TimeSpan.Zero;
                    if (generator.Next(5) == 1)
                    {
                        List<Rectangle> tmpRectangle = new List<Rectangle>();
                        Image_Texture tmpImage = _glacons.ElementAt(_numeroGlacon);

                        tmpRectangle.Add(new Rectangle(generator.Next(795), 0, tmpImage.Rectangle.First().Width, tmpImage.Rectangle.First().Height));
                        tmpImage.Rectangle = tmpRectangle;

                        _glacons.RemoveAt(_numeroGlacon);
                        _glacons.Insert(_numeroGlacon, tmpImage);

                        _numeroGlacon++;
                    }

                    if (_numeroGlacon > 9)
                    {
                        _numeroGlacon = 0;
                    }


                }
            }
            
        }

        /// <summary>
        /// Permet à la page de se dessiner.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {

            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.FromNonPremultiplied(255, 154, 0, 255));

            // TODO: ajoutez ici votre code de dessin
            spriteBatch.Begin();
            //spriteBatch.Draw(background, Vector2.Zero, Color.FromNonPremultiplied(255,154,0, 255));
            spriteBatch.Draw(_textureBackground, new Rectangle(0, 0, _textureBackground.Width, _textureBackground.Height), Color.White);
            _casserole.Draw(spriteBatch);
 

            foreach (var texture in _glacons)
            {
                texture.Draw(spriteBatch);
            }

            spriteBatch.Draw(contentManager.Load<Texture2D>("rouge"), _jaugeInterieur, Color.White);
            _jauge.Draw(spriteBatch);

            //spriteBatch.DrawString(_affichageGlaconCapture, "Blocks of ice obtained: " + _glaconCapture + "/10", Vector2.Zero, Color.Black);
            if (_tempsRestant.Seconds > 20)

                spriteBatch.DrawString(_affichageTempsRestant, "Time: " + (30 - _tempsRestant.Seconds) + "." + (1000 - _tempsRestant.Milliseconds), new Vector2(0, 20), Color.Red);
            else
                spriteBatch.DrawString(_affichageTempsRestant, "Time: " + (30 - _tempsRestant.Seconds) + "." + (1000 - _tempsRestant.Milliseconds), new Vector2(0, 20), Color.Black);

            spriteBatch.End();
            if (debut)
            {
                MessageBox.Show("Cool the pan by moving it and dropping ice cubes in!", "How to", MessageBoxButton.OK);
                debut = false;
            }
        }
    }
}