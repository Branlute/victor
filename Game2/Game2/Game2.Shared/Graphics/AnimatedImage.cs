using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game2.Graphics
{
    public class AnimatedImage
    {
        private Texture2D _texture;






        private string _assetName;

        //sprite
        private int _currentIndex = 0;
        private int _maxIndex;
        private SpriteEffects _effect = SpriteEffects.None;


        private Vector2 _direction = Vector2.One;
        private Rectangle _destinationRectangle;
        private Rectangle _sourceRectangle;

        //timer

        private TimeSpan _elapsedGameTime = TimeSpan.Zero;



        #region Get/Set
        public Color[] TextureData
        {
            get
            {
                Color[] textureData = new Color[_texture.Width * _texture.Height];
                _texture.GetData(textureData);
                return textureData;
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                return _destinationRectangle;
            }
            set
            {
                _destinationRectangle = value;
            }
        }

        public Texture2D Texture
        {
            get
            {
                return _texture;
            }
            set
            {
                _texture = value;
            }
        }

        public int MaxIndex
        {
            get
            {
                return _maxIndex;
            }
            set
            {
                _maxIndex = value;
            }
        }

        public int CurrentIndex
        {
            get
            {
                return _currentIndex;
            }
            set
            {
                _currentIndex = value;
            }
        }


        public Vector2 Position
        {
            get
            {
                return new Vector2(_destinationRectangle.X, _destinationRectangle.Y);
            }
            set
            {
                _destinationRectangle.X = (int)value.X;
                _destinationRectangle.Y = (int)value.Y;
            }
        }

        #endregion


        public AnimatedImage(string assetName, Vector2 position, int nbSprite)
        {
            _assetName = assetName;
            _destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 0, 0);
            _maxIndex = nbSprite - 1;

        }

        public void repositionnement(Vector2 position)
        {
            _currentIndex = 0;

            _destinationRectangle.X = (int)position.X;
            _destinationRectangle.Y = (int)position.Y;


        }
        public void LoadContent(ContentManager Content)
        {


            _texture = Content.Load<Texture2D>(@_assetName);

            _sourceRectangle = new Rectangle(0, 0, (_texture.Width / (_maxIndex + 1)), _texture.Height);
            _destinationRectangle.Width = (_texture.Width / (_maxIndex + 1));
            _destinationRectangle.Height = _texture.Height;

        }

        public void deplacement(Vector2 vitesse)
        {

            _destinationRectangle.X += (int)vitesse.X;
            _destinationRectangle.Y += (int)vitesse.Y;
        }

        public void UpdateFixe(GameTime gameTime)
        {

            _elapsedGameTime += gameTime.ElapsedGameTime;

            if (_elapsedGameTime.TotalMilliseconds >= 80)
            {

                _elapsedGameTime = TimeSpan.Zero;
                _currentIndex++;

                if (_currentIndex > _maxIndex)
                    _currentIndex = 0;

                _sourceRectangle.X = _currentIndex * _texture.Width / (_maxIndex + 1);
            }

        }


        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(_texture, _destinationRectangle, _sourceRectangle, Color.White, 0, Vector2.One, _effect, 0);


        }

    }
}
