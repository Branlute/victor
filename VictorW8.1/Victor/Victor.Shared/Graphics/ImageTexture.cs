using Victor.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victor.Graphics
{
    public class ImageTexture
    {
        private List<Texture2D> _texture;
        private Texture2D _couleur;
        private List<string> _assetName;
        private List<Vector2> _position;
        private List<Rectangle> _rectangle;
        private string _name;
        private int _idQuestion;




        #region Get/Set
        public int IdQuestion
        {
            get
            {
                return _idQuestion;
            }
            set
            {
                _idQuestion = value;
            }
        }

        public List<Vector2> Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public List<Rectangle> Rectangle
        {
            get
            {
                return _rectangle;
            }
            set
            {
                _rectangle = value;
            }
        }
        public List<Texture2D> Texture
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

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public Color[] TextureData
        {
            get
            {

                Color[] textureData = new Color[_texture.ElementAt(0).Width * _texture.ElementAt(0).Height];
                _texture.ElementAt(0).GetData(textureData);
                return textureData;
            }
        }

        #endregion

        public ImageTexture()
        {
            _name = "Defaut";
            _assetName = new List<string>();
            _position = new List<Vector2>();
            _rectangle = new List<Rectangle>();

        }

        public ImageTexture(int idQuestion, string name, List<string> assetName, List<Vector2> position, List<int> longueur, List<int> hauteur)
        {
            _idQuestion = idQuestion;
            _name = name;
            _assetName = assetName;
            _position = position;
            _rectangle = new List<Rectangle>();
            int i = 0;
            foreach (var positionTmp in _position)
            {

                _rectangle.Add(new Rectangle((int)positionTmp.X, (int)positionTmp.Y, longueur.ElementAt(i), hauteur.ElementAt(i)));
                i++;
            }
        }

        public ImageTexture(int idQuestion, string name, string assetName, Vector2 position, int longueur, int hauteur)
        {
            _idQuestion = idQuestion;
            _name = name;
            _assetName = new List<string>();
            _assetName.Add(assetName);
            _position = new List<Vector2>();
            _position.Add(position);
            _rectangle = new List<Rectangle>();
            _rectangle.Add(new Rectangle((int)position.X, (int)position.Y, longueur, hauteur));


        }


        //public bool Collision(Victor victor)
        //{
        //    if (!Session.verification_item(Name))
        //        foreach (var rectangle in _rectangle)
        //        {
        //            foreach (var rectangleVictor in victor.Rectangle)
        //            {
        //                if (rectangle.Intersects(rectangleVictor))
        //                    return true;
        //            }

        //        }
        //    return false;

        //}

        //public bool CollisionParPixel(Victor victor)
        //{
        //    int top = Math.Max(Rectangle.ElementAt(0).Top, victor.Rectangle.Top);
        //    int bottom = Math.Min(Rectangle.ElementAt(0).Bottom, victor.Rectangle.Bottom);
        //    int left = Math.Max(Rectangle.ElementAt(0).Left, victor.Rectangle.Left);
        //    int right = Math.Min(Rectangle.ElementAt(0).Right, victor.Rectangle.Right);

        //    float imageRatioHauteur = (float)(_texture.ElementAt(0).Height / Rectangle.ElementAt(0).Height) * _texture.ElementAt(0).Width;
        //    float imageRatioLargeur = (float)(_texture.ElementAt(0).Width / Rectangle.ElementAt(0).Width);

        //    int debutSprite = (victor.Texture.Width / (victor.MaxIndex + 1)) * victor.CurrentIndex;
        //    float victorRatioHauteur = (float)(victor.Texture.Height / victor.Rectangle.Height) * victor.Texture.Width;
        //    float victorRatioLargeur = (float)((float)(victor.Texture.Width / (victor.MaxIndex + 1)) / victor.Rectangle.Width);

        //    for (int y = bottom - 1; y >= top; y -= 1)
        //    {
        //        for (int x = left; x < right; x += 1)
        //        {
        //            Color currentColor = TextureData[(int)((x - Rectangle.ElementAt(0).X) * imageRatioLargeur) + (int)((y - Rectangle.ElementAt(0).Y) * imageRatioHauteur)];
        //            Color imageColor = victor.TextureData[debutSprite + (int)((x - victor.Rectangle.X) * victorRatioLargeur) + (int)((y - victor.Rectangle.Y) * victorRatioHauteur)];

        //            if (currentColor.A != 0 && imageColor.A != 0)
        //                return true;

        //        }

        //    }
        //    return false;
        //}


        public void AjoutTexture(string texture, Vector2 position, int longueur, int hauteur, ContentManager Content)
        {
            _assetName.Add(texture);
            //_texture.Add(Content.Load<Texture2D>(texture));
            _position.Add(position);
            _rectangle.Add(new Rectangle((int)position.X, (int)position.Y, longueur, hauteur));
        }


        public void LoadContent(ContentManager Content)
        {
            _texture = new List<Texture2D>();
            foreach (var textureTmp in _assetName)
            {
                _texture.Add(Content.Load<Texture2D>(@textureTmp));

            }
            if (!Session.verification_item(Name))
                _couleur = Content.Load<Texture2D>(_assetName.ElementAt(0) + "2");

        }

        public void Deplacement(Vector2 direction)
        {
            List<Rectangle> tmpRectangle = new List<Rectangle>();
            foreach (var image in _rectangle)
            {
                tmpRectangle.Add(new Rectangle(image.X + (int)direction.X, image.Y + (int)direction.Y, image.Width, image.Height));

            }
            _rectangle = tmpRectangle;

            List<Vector2> tmpRectangle2 = new List<Vector2>();
            foreach (var position in _position)
            {
                tmpRectangle2.Add(new Vector2(position.X + (int)direction.X, position.Y + (int)direction.Y));

            }
            _position = tmpRectangle2;



        }

        public void Replacement(Vector2 endroit)
        {
            List<Rectangle> tmpRectangle = new List<Rectangle>();
            foreach (var image in _rectangle)
            {
                tmpRectangle.Add(new Rectangle((int)endroit.X, (int)endroit.Y, image.Width, image.Height));

            }
            _rectangle = tmpRectangle;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int i = 0;
            foreach (var textureTmp in _texture)
            {
                if (!Session.verification_item(Name))
                    spriteBatch.Draw(_couleur, _rectangle.ElementAt(i), Color.White);
                else
                    spriteBatch.Draw(textureTmp, _rectangle.ElementAt(i), Color.White);
                i++;
            }
        }

        public void DrawRotation(SpriteBatch spriteBatch, float rotation, Vector2 pointRef)
        {
            int i = 0;
            foreach (var textureTmp in _texture)
            {
                if (!Session.verification_item(Name))
                    spriteBatch.Draw(_couleur, _rectangle.ElementAt(i), null, Color.White, rotation, pointRef, SpriteEffects.None, 0);
                else
                    spriteBatch.Draw(textureTmp, _rectangle.ElementAt(i), null, Color.White, rotation, pointRef, SpriteEffects.None, 0);

                i++;
            }
        }

        public String testTransparence()
        {
            int top = Rectangle.ElementAt(0).Top;
            int bottom = Rectangle.ElementAt(0).Bottom;
            int left = Rectangle.ElementAt(0).Left;
            int right = Rectangle.ElementAt(0).Right;
            String result = "";
            String a;
            for (int y = top; y <= bottom; y += 4)
            {
                for (int x = left; x <= right; x += 2)
                {
                    Color currentColor = TextureData[(int)((x - Rectangle.ElementAt(0).X) * (float)(_texture.ElementAt(0).Width / Rectangle.ElementAt(0).Width)) + (int)((y - Rectangle.ElementAt(0).Y) * (float)(_texture.ElementAt(0).Height / Rectangle.ElementAt(0).Height)) * _texture.ElementAt(0).Width];
                    if (currentColor.A != 0)
                        a = "0";
                    else
                        a = ".";


                    result = result + a;
                }
                result = result + "\n ";

            }
            return result;
        }
    }
}
