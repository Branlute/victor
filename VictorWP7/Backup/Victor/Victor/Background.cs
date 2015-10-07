using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace VictorNamespace
{
    public class Background
    {
        private Texture2D _background;
        private String _image;
        private Vector2 _position;

        public Background(String image)
        {
            _image = image;
            _position = new Vector2(0, 0);


        }

        public void LoadContent(ContentManager Content)
        {
            _background = Content.Load<Texture2D>(@_image);


        }

        public void deplacement(Vector2 vitesse)
        {
            _position.X += vitesse.X;
            _position.Y += vitesse.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
           
             spriteBatch.Draw(_background, _position, Color.White);
   
        } 
    }
}
