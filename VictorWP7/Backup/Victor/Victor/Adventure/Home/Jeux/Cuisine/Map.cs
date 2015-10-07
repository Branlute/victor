using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace VictorNamespace.Adventure.Home.Jeux.Cuisine
{
    public class Map
    {
        private Texture2D _texture;
        private Rectangle _destinationRectangle;
        private Rectangle _sourceRectangle;
        private string _nameTexture;
        public Map(string assetName)
        {
            _nameTexture = assetName;
            _sourceRectangle = new Rectangle(0, 0, 800, 480);
            _destinationRectangle = new Rectangle(0, 0, 800, 480);


            // Créez une minuterie pour cette page

        }


        public void LoadContent(ContentManager Content)
        {
            _texture = Content.Load<Texture2D>(_nameTexture);

        }


        public void deplacement(Vector2 direction)
        {

            _sourceRectangle.X += (int)direction.X;
            if (_sourceRectangle.X + _sourceRectangle.Width > _texture.Width)
            {
                _sourceRectangle.X = _sourceRectangle.X + _sourceRectangle.Width - _texture.Width;

            }
            _sourceRectangle.Y += (int)direction.Y;
            if (_sourceRectangle.Y + _sourceRectangle.Height > _texture.Height)
            {

                _sourceRectangle.Y = _sourceRectangle.Y + _sourceRectangle.Height - _texture.Height;
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

            spriteBatch.Draw(_texture, _destinationRectangle, _sourceRectangle, Color.White);
            // TODO: ajoutez ici votre code de dessin
        }
    }
}
