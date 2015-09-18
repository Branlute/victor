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
    public partial class Couteau : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;


        //Background
        private Texture2D _textureBackground;

        //private Texture2D background;
        private Image_Texture _couteau;
        private int[] _tableau;
        private int _solution;
        private List<Image_Texture> next;
        private Image_Texture _etui;
        private Texture2D _labyrinthe;
        private Rectangle _labyrintheRectangleSource;
        private TimeSpan _tempsRestant = TimeSpan.Zero;
        private SpriteFont _position;
        private SpriteEffects effet1 = SpriteEffects.None;
        private SpriteEffects effet2 = SpriteEffects.None;
        System.Random generator = new System.Random();
        private String souris="";
        private bool recreation = false;
        private string scene="";
        private string niveau="";
        private bool _fini;

        private bool debut = true;

        public Couteau()
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
            _couteau = new Image_Texture(0, "couteau_MiniJeu", "couteau", new Vector2(7, 100), 10, 60);
            _couteau.LoadContent(contentManager);
            


            _labyrinthe = contentManager.Load<Texture2D>("labyrinthe");
            _labyrintheRectangleSource = new Rectangle(125, 100, _labyrinthe.Width, _labyrinthe.Height);
            next = new List<Image_Texture>();
            next.Add(new Image_Texture(0, "next_MiniJeu", "next", new Vector2(84, 108-40/2), 40, 40));
            next.Add(new Image_Texture(0, "next_MiniJeu", "next", new Vector2(84, 188 - 40 / 2), 40, 40));
            next.Add(new Image_Texture(0, "next_MiniJeu", "next", new Vector2(84, 267 - 40 / 2), 40, 40));
            next.Add(new Image_Texture(0, "next_MiniJeu", "next", new Vector2(84, 347 - 40 / 2), 40, 40));

            foreach (var image in next)
                image.LoadContent(contentManager);

            TouchPanel.EnabledGestures = GestureType.Tap;
            _tableau = new int[4];
            _tableau[0] = 2;
            _tableau[1] = 3;
            _tableau[2] = 1;
            _tableau[3] = 0;
            _solution = generator.Next(4);



            _position = contentManager.Load<SpriteFont>(@"SpriteFont1");
            if (generator.Next(2) == 1)
            {
                effet1 = SpriteEffects.FlipHorizontally;
                int[] tmpTableau = (int[])_tableau.Clone();
                for(int i = 0; i < 4; i++)
                    _tableau[tmpTableau[i]] = i;

            }
            else
            {
                effet1 = SpriteEffects.None;
            }
            if (generator.Next(2) == 1)
            {
                effet2 = SpriteEffects.FlipVertically;
                int[] tmpTableau = (int[])_tableau.Clone();
                for (int i = 0; i < 4; i++)
                    _tableau[3-i] = 3 - tmpTableau[i];
            }
            else
            {
                effet2 = SpriteEffects.None;
            }
            
            
            switch (_tableau[_solution])
            {
                case 0:
                    _etui = new Image_Texture(0, "etui_MiniJeu", "etui", new Vector2(645, 108-100/2), 66, 100);
                    break;
                case 1:
                    _etui = new Image_Texture(0, "etui_MiniJeu", "etui", new Vector2(645, 188-100/2), 66, 100);
                    break;
                case 2:
                    _etui = new Image_Texture(0, "etui_MiniJeu", "etui", new Vector2(645, 267-100/2), 66, 100);
                    break;
                case 3:
                    _etui = new Image_Texture(0, "etui_MiniJeu", "etui", new Vector2(645, 347-100/2), 66, 100);
                    break;
            }
            _etui.LoadContent(contentManager);

            _fini = false;

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
                if (TouchPanel.IsGestureAvailable)
                {
                    GestureSample state = TouchPanel.ReadGesture();

                    if (state.GestureType == GestureType.Tap)
                    {
                        int cpt = 0;
                        foreach (var image in next)
                        {
                            if (image.Rectangle.First().X <= state.Position.X && image.Rectangle.First().X + image.Rectangle.First().Width >= state.Position.X && image.Rectangle.First().Y <= state.Position.Y && image.Rectangle.First().Y + image.Rectangle.First().Height >= state.Position.Y)
                            {
                                if (cpt == _solution)
                                {
                                    
                                    MessageBox.Show("You found the knife case !", "Congratulation !", MessageBoxButton.OK);

                                    if (recreation == false)
                                    {
                                        if (niveau == "Explorer")
                                            NavigationService.Navigate(new Uri("/Explorer/GameExplorer.xaml?id=" + scene, UriKind.Relative));

                                        else
                                        {
                                            Session.cuisine_couteau = true;
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
                                    MessageBox.Show("You did not find the knife case !", "Wrong way !", MessageBoxButton.OK);

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
                                    break;
                                }

                            }

                            cpt++;

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
            spriteBatch.Draw(_textureBackground, new Rectangle(0, 0, _textureBackground.Width, _textureBackground.Height), Color.White);
            spriteBatch.Draw(_labyrinthe, _labyrintheRectangleSource, null, Color.White, 0, new Vector2(0, 0), effet1 | effet2 , 0);
            spriteBatch.DrawString(_position, souris , new Vector2(0,0), Color.Red);
            _etui.Draw(spriteBatch);
            foreach (var image in next)
                image.Draw(spriteBatch);


            spriteBatch.End();
            if (debut)
            {
                MessageBox.Show("Find the good way to reach knife’s case!", "How to", MessageBoxButton.OK);
                debut = false;
            }
        }
    }
}