using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace VictorNamespace.Adventure.Home.Jeux.Cuisine
{
    public class Obstacle
    {
        
        private Texture2D _texture;
        private Rectangle _destinationRectangle;
        private string _nameTexture;



        #region Get/Set
        public Rectangle DestinationRectangle
        {
            get
            {
                return _destinationRectangle;
            }
        }

        #endregion



        public Obstacle(string assetName, Rectangle position)
        {
            _nameTexture = assetName;
            _destinationRectangle = position;


        }

        public void LoadContent(ContentManager Content)
        {
            _texture = Content.Load<Texture2D>(_nameTexture);

        }

        public void deplacement(Vector2 direction)
        {
            _destinationRectangle.X -= (int)direction.X;
            _destinationRectangle.Y += (int)direction.Y;
        }


        public void replacement(Vector2 emplacement)
        {
            _destinationRectangle.X = (int)emplacement.X;
            _destinationRectangle.Y = (int)emplacement.Y;

        }

        public bool Collision(Victor victor)
        {
                foreach (var rectangleVictor in victor.Rectangle)
                {
                    if (_destinationRectangle.Intersects(rectangleVictor))
                        return true;
                }
            return false;

        }

        public bool AuDessus(Victor victor)
        {
            if (victor.Position.Y + victor.Texture.Height - 20 < this.DestinationRectangle.Y)
            {
                if ((victor.Position.X - victor.Texture.Width / 8 + 60 > this.DestinationRectangle.X && victor.Position.X - victor.Texture.Width / 8 + 20 < this.DestinationRectangle.X + this.DestinationRectangle.Width) || (victor.Position.X < this.DestinationRectangle.X + this.DestinationRectangle.Width && victor.Position.X > this.DestinationRectangle.X))
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Permet à la page d'exécuter la logique, comme par exemple mettre à jour le monde,
        /// contrôler les collisions, collecter les données d'entrée, et lire le son.
        /// </summary>
        public void OnUpdate(object sender, GameTimerEventArgs e)
        {
            // TODO: ajoutez ici votre logique de mise à jour
        }

        /// <summary>
        /// Permet à la page de se dessiner.
        /// </summary>
        public void OnDraw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(_texture, _destinationRectangle, Color.White);
               

            // TODO: ajoutez ici votre code de dessin
        }
    }
}