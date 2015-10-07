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
    public partial class Grillepain : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;


        //Background
        private Texture2D _textureBackground;

        //private Texture2D background;
        private Image_Texture _grillepainDerriere;
        private Image_Texture _grillepainDevant;
        private Image_Texture _grillepainManette;
        private Image_Texture _tartine;
        private Vector2 vitesse = new Vector2(1, -35);
        private int manette;
        System.Random generator = new System.Random();
        private bool saut = false;
        private double rotationEnCours;
        private double rotation;
        private bool recreation = false;
        private string scene="";
        private string niveau="";
        private bool _fini;
        private SoundEffect _soundEffect;
        Vector2 deplacement = new Vector2(5,0);
        private bool debut = true;


        private int nbTartine;

        public Grillepain()
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

            saut = false;
            _grillepainDerriere = new Image_Texture(0, "grillepain_MiniJeu", "grille-pain-derriere", new Vector2(140, 380), 200, 87);
            _grillepainDerriere.LoadContent(contentManager);

            _grillepainDevant = new Image_Texture(0, "grillepain_MiniJeu", "grille-pain-devant", new Vector2(140, 380), 200, 87);
            _grillepainDevant.LoadContent(contentManager);

            _grillepainManette = new Image_Texture(0, "grillepainManette_MiniJeu", "grillepainManette", new Vector2(140, 380), 200, 87);
            _grillepainManette.LoadContent(contentManager);

            _tartine = new Image_Texture(0, "tartine_MiniJeu", "tartine", new Vector2(242, 415), 75, 70);
            _tartine.LoadContent(contentManager);

            vitesse = new Vector2(1, -18 - generator.Next(16));
            if (generator.Next(2) == 1)
                vitesse.X = generator.Next(4);
            else
                vitesse.X = -generator.Next(4);


            manette = 40;
            _fini = false;
            
            _soundEffect = contentManager.Load<SoundEffect>(@"grillepainSon");

            //on parse
            var data = this.NavigationContext.QueryString;
            recreation = bool.Parse(data["recreation"]);
            if (!recreation)
            {
                scene = data["scene"];
                niveau = data["niveau"];
            }

            //Background
            _textureBackground = contentManager.Load<Texture2D>(@"Background");

            //Nombre de tartine attrapé
            nbTartine = 0;

            debut = true;

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
            // TODO: ajoutez ici votre logique de mise à jour
            if (_fini == false)
            {
                
                if(_grillepainDerriere.Position.ElementAt(0).X <= 20)
                    deplacement = new Vector2(5,0);
                else
                    if (_grillepainDerriere.Position.ElementAt(0).X + _grillepainDerriere.Rectangle.ElementAt(0).Width >= 780)
                    deplacement = new Vector2(-5,0);
                _grillepainDerriere.Deplacement(deplacement);
                _grillepainDevant.Deplacement(deplacement);
                _grillepainManette.Deplacement(deplacement);
                if (saut)
                {
                    if (manette > 0)
                    {
                        _grillepainManette.Deplacement(new Vector2(2, -6));
                        manette = manette - 6;
                    }
                    _tartine.Deplacement(vitesse);
                    vitesse.Y++;
                    rotationEnCours += rotation;
                    if (_tartine.Rectangle.First().Y > 480)
                    {
                        TouchPanel.EnabledGestures = GestureType.None;
                        _tartine.Replacement(new Vector2(_grillepainDerriere.Position.ElementAt(0).X+102, 415));
                        _grillepainManette.Replacement(new Vector2(_grillepainDerriere.Position.ElementAt(0).X, 380));
                        saut = false;
                        rotationEnCours = 0;
                        vitesse = new Vector2(1, - 18 - generator.Next(16));
                        if (generator.Next(2) == 1)
                            vitesse.X = generator.Next(4);
                        else
                            vitesse.X = -generator.Next(4);
                        manette = 40;
                    }

                    if (TouchPanel.IsGestureAvailable)
                    {
                        GestureSample state = TouchPanel.ReadGesture();
                        if (state.GestureType == GestureType.Tap)
                        {
                            if (_tartine.Rectangle.First().X - 10 <= state.Position.X && _tartine.Rectangle.First().X + _tartine.Rectangle.First().Width + 10 >= state.Position.X && _tartine.Rectangle.First().Y - 50 <= state.Position.Y && _tartine.Rectangle.First().Y + _tartine.Rectangle.First().Height + 50 >= state.Position.Y)
                            {
                                nbTartine++;
                                MessageBox.Show("You caught " + nbTartine + " toast(s)!", "Congratulation !", MessageBoxButton.OK);
                                TouchPanel.EnabledGestures = GestureType.None;
                                if (nbTartine >= 5)
                                {
                                    if (recreation == false)
                                    {
                                        if (niveau == "Explorer")
                                            NavigationService.Navigate(new Uri("/Explorer/GameExplorer.xaml?id=" + scene, UriKind.Relative));

                                        else
                                        {
                                            Session.cuisine_grillepain = true;
                                            NavigationService.Navigate(new Uri("/Adventure/Home/HomeXNA.xaml?niveau=" + niveau + "&scene=" + scene, UriKind.Relative));
                                        }
                                    }
                                    else
                                    {
                                        NavigationService.Navigate(new Uri("/Recreation/Recreation.xaml", UriKind.Relative));
                                    }
                                    _fini = true;
                                }
                                else
                                {
                                    _tartine.Deplacement(new Vector2(0, 500));
                                }
                            }
                        }
                    }

                }
                else
                {
                    _tartine.Deplacement(deplacement);
                    if (generator.Next(40) == 1)
                    {
                        TouchPanel.EnabledGestures = GestureType.Tap;
                        _tartine.Deplacement(vitesse);
                        vitesse.Y++;
                        saut = true;
                        if (generator.Next(2) == 1)
                            rotation = generator.NextDouble() / 8;
                        else
                            rotation = -generator.NextDouble() / 8;
                        rotationEnCours = 0;
                        _grillepainManette.Deplacement(new Vector2(2, -6));
                        manette = manette - 6;
                        _soundEffect.Play();


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
            //spriteBatch.Draw(background, Vector2.Zero, Color.FromNonPremultiplied(255,154,0, 255));
            spriteBatch.Draw(_textureBackground, new Rectangle(0, 0, _textureBackground.Width, _textureBackground.Height), Color.White);
            _grillepainDerriere.Draw(spriteBatch);
            _grillepainManette.Draw(spriteBatch);
            _tartine.DrawRotation(spriteBatch, (float)rotationEnCours, new Vector2(_tartine.Texture.First().Width / 2, _tartine.Texture.First().Height / 2));

            _grillepainDevant.Draw(spriteBatch);
            if (vitesse.Y > 0)
                _tartine.DrawRotation(spriteBatch, (float)rotationEnCours, new Vector2(_tartine.Texture.First().Width / 2, _tartine.Texture.First().Height / 2));
            spriteBatch.End();
            if (debut)
            {
                MessageBox.Show("Catch five toasts taping on the screen.", "How to", MessageBoxButton.OK);
                debut = false;
            }
        }
    }
}