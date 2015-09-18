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
    public partial class Four : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;


        //private Texture2D background;

        //Background
        private Texture2D _textureBackground;
        private Rectangle _rectangle1Background;
        private Rectangle _rectangle2Background;
        private Rectangle _rectangle3Background;


        private Image_Texture _fond1;
        private Image_Texture _fond2;
        private Image_Texture _fond3;
        private Image_Texture _jauge;
        private Rectangle _jaugeInterieur;
        private List<Image_Texture> _fourFroid;
        private List<int> _fourFroidMonte;
        private List<Image_Texture> _fourChaud;
        private List<int> _fourChaudMonte;
        private TimeSpan _tempsRestant = TimeSpan.Zero;
        private int _fourTape;
        private SpriteFont _affichageTempsRestant;
        private List<short> _trouOccupe;
        private Vector2 chute = new Vector2(0, 5);
        System.Random generator = new System.Random();
        private bool recreation = false;
        private string scene="";
        private string niveau="";
        private bool _fini;
        private SoundEffect _soundEffect;

        private bool debut = true;

        public Four()
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


            _fond1 = new Image_Texture(0, "fond1_MiniJeu", "fond1", new Vector2(3, 279), 794, 155);
            _fond2 = new Image_Texture(0, "fond2_MiniJeu", "fond2", new Vector2(61, 110), 678, 170);
            _fond3 = new Image_Texture(0, "fond3_MiniJeu", "fond3", new Vector2(143, 20), 514, 90);
            _fond1.LoadContent(contentManager); 
            _fond2.LoadContent(contentManager);
            _fond3.LoadContent(contentManager);

            _jauge = new Image_Texture(0, "jauge_MiniJeu", "jauge", new Vector2(650, 440), 40, 500);
            _jauge.LoadContent(contentManager);
            _jaugeInterieur = new Rectangle(159, 443, -2 , 30);

            _fourFroid = new List<Image_Texture>();
            for (int i = 0; i < 6; i++)
            {
                _fourFroid.Add(new Image_Texture(0, "fourFroid_MiniJeu", "tete_victor", new Vector2(900, 500), 151, 200));
                _fourFroid.ElementAt(i).LoadContent(contentManager);
            }

            _fourChaud = new List<Image_Texture>();
            for (int i = 0; i < 6; i++)
            {
                _fourChaud.Add(new Image_Texture(0, "fourFroid_MiniJeu", "fourChaud", new Vector2(900, 500), 200, 200));
                _fourChaud.ElementAt(i).LoadContent(contentManager);

            }
            _trouOccupe = new List<short>();
            for (int i = 0; i < 6; i++)
            {
                _trouOccupe.Add(-1);

            }
            _fourFroidMonte = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                _fourFroidMonte.Add(0);

            }
            _fourChaudMonte = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                _fourChaudMonte.Add(0);

            }

            _fourTape = 0;

            TouchPanel.EnabledGestures = GestureType.Tap;

            
            _affichageTempsRestant = contentManager.Load<SpriteFont>(@"SpriteFont1");
            _tempsRestant = TimeSpan.Zero;
            _fini = false;
            _soundEffect = contentManager.Load<SoundEffect>(@"fourSon");

            //on parse
            var data = this.NavigationContext.QueryString;
            recreation = bool.Parse(data["recreation"]);
            if (!recreation)
            {
                scene = data["scene"];
                niveau = data["niveau"];
            }


            //On s'occupe du fond
            _textureBackground = contentManager.Load<Texture2D>(@"background");
            _rectangle1Background = new Rectangle(0, 0, 800, 230);
            _rectangle2Background = new Rectangle(0, 230, 800, 200);
            _rectangle3Background = new Rectangle(0, 430, 800, 50);


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
                int cpt;
                Rectangle tmpRectangle;

                if (generator.Next(50) == 1)
                {
                    int trou = generator.Next(6);
                    if (_trouOccupe.ElementAt(trou) == -1)
                    {
                        if (generator.Next(2) == 1)
                        {
                            cpt = 0;
                            foreach (var four in _fourFroid)
                            {
                                if (four.Rectangle.First().X == 900)
                                {
                                    _fourFroidMonte.RemoveAt(cpt);
                                    _fourFroidMonte.Insert(cpt, 1);
                                    switch (trou)
                                    {
                                        case 0:
                                            four.Replacement(new Vector2(115, 290));
                                            four.IdQuestion = 1;
                                            tmpRectangle = four.Rectangle.First();
                                            tmpRectangle.Width = 150;
                                            tmpRectangle.Height = 200;
                                            four.Rectangle.Clear();
                                            four.Rectangle.Add(tmpRectangle);
                                            break;
                                        case 1:
                                            four.Replacement(new Vector2(325, 290));
                                            four.IdQuestion = 1;
                                            tmpRectangle = four.Rectangle.First();
                                            tmpRectangle.Width = 150;
                                            tmpRectangle.Height = 200;
                                            four.Rectangle.Clear();
                                            four.Rectangle.Add(tmpRectangle);
                                            break;
                                        case 2:
                                            four.Replacement(new Vector2(535, 290));
                                            four.IdQuestion = 1;
                                            tmpRectangle = four.Rectangle.First();
                                            tmpRectangle.Width = 150;
                                            tmpRectangle.Height = 200;
                                            four.Rectangle.Clear();
                                            four.Rectangle.Add(tmpRectangle);
                                            break;
                                        case 3:
                                            four.Replacement(new Vector2(180, 160));
                                            four.IdQuestion = 2;
                                            tmpRectangle = four.Rectangle.First();
                                            tmpRectangle.Width = 115;
                                            tmpRectangle.Height = 150;
                                            four.Rectangle.Clear();
                                            four.Rectangle.Add(tmpRectangle);
                                            break;
                                        case 4:
                                            four.Replacement(new Vector2(340, 160));
                                            four.IdQuestion = 2;
                                            tmpRectangle = four.Rectangle.First();
                                            tmpRectangle.Width = 115;
                                            tmpRectangle.Height = 150;
                                            four.Rectangle.Clear();
                                            four.Rectangle.Add(tmpRectangle);
                                            break;
                                        case 5:
                                            four.Replacement(new Vector2(500, 160));
                                            four.IdQuestion = 2;
                                            tmpRectangle = four.Rectangle.First();
                                            tmpRectangle.Width = 115;
                                            tmpRectangle.Height = 150;
                                            four.Rectangle.Clear();
                                            four.Rectangle.Add(tmpRectangle);
                                            break;
                                    }
                                    _trouOccupe.RemoveAt(trou);
                                    _trouOccupe.Insert(trou, (short)cpt);
                                    break;
                                }
                                cpt++;

                            }

                        }
                        else
                        {
                            cpt = 0;
                            foreach (var four in _fourChaud)
                            {
                                if (four.Rectangle.First().X == 900)
                                {
                                    _fourChaudMonte.RemoveAt(cpt);
                                    _fourChaudMonte.Insert(cpt, 1);

                                    switch (trou)
                                    {
                                        case 0:
                                            four.Replacement(new Vector2(90, 230));
                                            four.IdQuestion = 1;
                                            tmpRectangle = four.Rectangle.First();
                                            tmpRectangle.Width = 200;
                                            tmpRectangle.Height = 200;
                                            four.Rectangle.Clear();
                                            four.Rectangle.Add(tmpRectangle);
                                            break;
                                        case 1:
                                            four.Replacement(new Vector2(300, 230));
                                            four.IdQuestion = 1;
                                            tmpRectangle = four.Rectangle.First();
                                            tmpRectangle.Width = 200;
                                            tmpRectangle.Height = 200;
                                            four.Rectangle.Clear();
                                            four.Rectangle.Add(tmpRectangle);
                                            break;
                                        case 2:
                                            four.Replacement(new Vector2(510, 230));
                                            four.IdQuestion = 1;
                                            tmpRectangle = four.Rectangle.First();
                                            tmpRectangle.Width = 200;
                                            tmpRectangle.Height = 200;
                                            four.Rectangle.Clear();
                                            four.Rectangle.Add(tmpRectangle);
                                            break;
                                        case 3:
                                            four.Replacement(new Vector2(165, 100));
                                            four.IdQuestion = 2;
                                            tmpRectangle = four.Rectangle.First();
                                            tmpRectangle.Width = 150;
                                            tmpRectangle.Height = 150;
                                            four.Rectangle.Clear();
                                            four.Rectangle.Add(tmpRectangle);
                                            break;
                                        case 4:
                                            four.Replacement(new Vector2(325, 100));
                                            four.IdQuestion = 2;
                                            tmpRectangle = four.Rectangle.First();
                                            tmpRectangle.Width = 150;
                                            tmpRectangle.Height = 150;
                                            four.Rectangle.Clear();
                                            four.Rectangle.Add(tmpRectangle);
                                            break;
                                        case 5:
                                            four.Replacement(new Vector2(485, 100));
                                            four.IdQuestion = 2;
                                            tmpRectangle = four.Rectangle.First();
                                            tmpRectangle.Width = 150;
                                            tmpRectangle.Height = 150;
                                            four.Rectangle.Clear();
                                            four.Rectangle.Add(tmpRectangle);
                                            break;
                                    }
                                    _trouOccupe.RemoveAt(trou);
                                    _trouOccupe.Insert(trou, (short)cpt);
                                    break;

                                }
                                cpt++;

                            }
                        }
                    }
                }

                cpt = 0;
                foreach (var four in _fourFroid)
                {
                    if (_fourFroidMonte.ElementAt(cpt) < 0)
                    {

                        int tmp = _fourFroidMonte.ElementAt(cpt);
                        _fourFroidMonte.RemoveAt(cpt);
                        _fourFroidMonte.Insert(cpt, tmp + 1);
                        four.Deplacement(new Vector2(0, 10));


                    }
                    else
                        if (_fourFroidMonte.ElementAt(cpt) > 0)
                        {
                            if (_fourFroidMonte.ElementAt(cpt) == 14)
                            {
                                _fourFroidMonte.RemoveAt(cpt);
                                _fourFroidMonte.Insert(cpt, -15);

                            }
                            else
                            {
                                int tmp = _fourFroidMonte.ElementAt(cpt);
                                _fourFroidMonte.RemoveAt(cpt);
                                _fourFroidMonte.Insert(cpt, tmp + 1);

                            }
                            four.Deplacement(new Vector2(0, -10));
                        }
                        else
                        {
                            if (four.IdQuestion != 0)
                            {
                                four.Replacement(new Vector2(900, 500));
                                four.IdQuestion = 0;
                                int trou = _trouOccupe.IndexOf((short)cpt);
                                _trouOccupe.RemoveAt(trou);
                                _trouOccupe.Insert(trou, -1);
                            }

                        }
                    cpt++;
                }
                cpt = 0;
                foreach (var four in _fourChaud)
                {
                    if (_fourChaudMonte.ElementAt(cpt) < 0)
                    {

                        int tmp = _fourChaudMonte.ElementAt(cpt);
                        _fourChaudMonte.RemoveAt(cpt);
                        _fourChaudMonte.Insert(cpt, tmp + 1);
                        four.Deplacement(new Vector2(0, 10));


                    }
                    else
                        if (_fourChaudMonte.ElementAt(cpt) > 0)
                        {
                            if (_fourChaudMonte.ElementAt(cpt) == 14)
                            {
                                _fourChaudMonte.RemoveAt(cpt);
                                _fourChaudMonte.Insert(cpt, -15);

                            }
                            else
                            {
                                int tmp = _fourChaudMonte.ElementAt(cpt);
                                _fourChaudMonte.RemoveAt(cpt);
                                _fourChaudMonte.Insert(cpt, tmp + 1);
                            }
                            four.Deplacement(new Vector2(0, -10));


                        }
                        else
                        {
                            if (four.IdQuestion != 0)
                            {
                                four.Replacement(new Vector2(900, 500));
                                four.IdQuestion = 0;
                                int trou = _trouOccupe.IndexOf((short)cpt);
                                _trouOccupe.RemoveAt(trou);
                                _trouOccupe.Insert(trou, -1);
                            }
                        }

                    cpt++;
                }

                _tempsRestant += new GameTime(e.TotalTime, e.ElapsedTime).ElapsedGameTime;

                if (_tempsRestant.Seconds + _tempsRestant.Minutes * 60 == 100)
                {
                    MessageBox.Show("You did not tap on 10 Victor's heads !", "Time up !", MessageBoxButton.OK);

                    if (recreation == false)
                    {
                        if (niveau == "Explorer")
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

                    if (state.GestureType == GestureType.Tap)
                    {
                        cpt = 0;
                        foreach (var four in _fourChaud)
                        {
                            if (four.Rectangle.First().X <= state.Position.X && four.Rectangle.First().X + four.Rectangle.First().Width >= state.Position.X && four.Rectangle.First().Y <= state.Position.Y && four.Rectangle.First().Y + four.Rectangle.First().Height >= state.Position.Y)
                            {
                                four.Replacement(new Vector2(900, 500));
                                
                                _fourChaudMonte.RemoveAt(cpt);
                                _fourChaudMonte.Insert(cpt, 0);
                                if (_fourTape > 0)
                                {
                                    _fourTape--;
                                    _jaugeInterieur.Width -= _jauge.Rectangle.First().Height / 10 - 1;
                                }
                                _soundEffect.Play();
                            }
                            cpt++;
                        }
                        cpt = 0;
                        foreach (var four in _fourFroid)
                        {
                            if (four.Rectangle.First().X <= state.Position.X && four.Rectangle.First().X + four.Rectangle.First().Width >= state.Position.X && four.Rectangle.First().Y <= state.Position.Y && four.Rectangle.First().Y + four.Rectangle.First().Height >= state.Position.Y)
                            {
                                four.Replacement(new Vector2(900, 500));
                                _jaugeInterieur.Width += _jauge.Rectangle.First().Height / 10 - 1;
                                _fourFroidMonte.RemoveAt(cpt);
                                _fourFroidMonte.Insert(cpt, 0);
                                _fourTape++;
                                _soundEffect.Play();
                            }
                            cpt++;

                        }
                    }
                }
                if (_fourTape == 10)
                {
                    
                    MessageBox.Show("You tapped on 10 Victor's heads !", "Congratulation !", MessageBoxButton.OK);

                    if (recreation == false)
                    {
                        if (niveau == "Explorer")
                            NavigationService.Navigate(new Uri("/Explorer/GameExplorer.xaml?id=" + scene, UriKind.Relative));

                        else
                        {
                            Session.cuisine_four = true;
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

        /// <summary>
        /// Permet à la page de se dessiner.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.Blue);

            // TODO: ajoutez ici votre code de dessin
            spriteBatch.Begin();
            //spriteBatch.Draw(background, Vector2.Zero, Color.FromNonPremultiplied(255,154,0, 255));
            spriteBatch.Draw(_textureBackground, _rectangle1Background, _rectangle1Background, Color.White);
            _fond3.Draw(spriteBatch);

            foreach (var texture in _fourFroid)
            {
                if (texture.IdQuestion == 2)
                texture.Draw(spriteBatch);
            }
            foreach (var texture in _fourChaud)
            {
                if (texture.IdQuestion == 2)
                texture.Draw(spriteBatch);
            }
            spriteBatch.Draw(_textureBackground, _rectangle2Background, _rectangle2Background, Color.White);

            _fond2.Draw(spriteBatch);

            foreach (var texture in _fourFroid)
            {
                if (texture.IdQuestion == 1)
                texture.Draw(spriteBatch);
            }
            foreach (var texture in _fourChaud)
            {
                if (texture.IdQuestion == 1)
                texture.Draw(spriteBatch);
            }

            _fond1.Draw(spriteBatch);
            spriteBatch.Draw(_textureBackground, _rectangle3Background, _rectangle3Background, Color.White);

            spriteBatch.Draw(contentManager.Load<Texture2D>("rouge"), _jaugeInterieur, Color.White);
            _jauge.DrawRotation(spriteBatch, (float) Math.PI/2, new Vector2(0,0));

            if (_tempsRestant.Seconds + _tempsRestant.Minutes * 60 > 80)

                spriteBatch.DrawString(_affichageTempsRestant, "Time: " + (99 - _tempsRestant.Seconds - _tempsRestant.Minutes * 60) + "." + (1000 - _tempsRestant.Milliseconds), new Vector2(0, 20), Color.Red);
            else
                spriteBatch.DrawString(_affichageTempsRestant, "Time: " + (99 - _tempsRestant.Seconds - _tempsRestant.Minutes * 60) + "." + (1000 - _tempsRestant.Milliseconds), new Vector2(0, 20), Color.Black);

            spriteBatch.End();
            if (debut)
            {
                MessageBox.Show("Tap on 10 Victor's heads but be careful and avoid the hot ovens!", "How to", MessageBoxButton.OK);
                debut = false;
            }
        }
    }
}