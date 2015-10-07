using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;



namespace VictorNamespace
{
    public class Image_TexturePhoto
    {
        private Texture2D _couleur;
        //private List<string> _assetName;
        //private List<Vector2> _position;
        private Rectangle _rectangle;
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

        public Vector2 Position
        {
            get
            {
                return new Vector2(_rectangle.X, _rectangle.Y);
            }
            set
            {
                _rectangle.X = (int) value.X;
                _rectangle.Y = (int) value.Y;
            }
        }

        public Rectangle Rectangle
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
        /*public List<Texture2D> Texture
        {
            get
            {
                return _texture;
            }
            set
            {
                _texture = value;
            }
        }*/

        public string Name
        {
            get
            {
                return _name;
            }
        }

        /*public Color[] TextureData
        {
            get
            {

                Color[] textureData = new Color[_texture.ElementAt(0).Width * _texture.ElementAt(0).Height];
                _texture.ElementAt(0).GetData(textureData);
                return textureData;
            }
        }

        public Image_Texture()
        {
            _name = "Defaut";
            _assetName = new List<string>();
            _position = new List<Vector2>();
            _rectangle = new List<Rectangle>();

        }*/

        #endregion

        public Image_TexturePhoto(int idQuestion, string name, Vector2 position, int longueur, int hauteur)
        {
            _idQuestion = idQuestion;
            _name = name;
            _rectangle = new Rectangle((int)position.X, (int)position.Y, longueur, hauteur);
           

            
        }

        public bool Collision(Victor victor)
        {
            //if (!Session.verification_item(Name))
                foreach (var rectangleVictor in victor.Rectangle)
                {
                    if (_rectangle.Intersects(rectangleVictor))
                        return true;
                }
            return false;

        }


        public void LoadContent(ContentManager Content)
        {
            _couleur = Content.Load<Texture2D>("bleu");

        }

        public void Deplacement(Vector2 direction)
        {
            _rectangle.X += (int)direction.X;
            _rectangle.Y += (int)direction.Y;

        }

        public void Replacement(Vector2 endroit)
        {
            _rectangle.X = (int)endroit.X;
            _rectangle.Y = (int)endroit.Y;

        }

        public void Draw(SpriteBatch spriteBatch)
        {

                //if (!Session.verification_item(Name))
                    spriteBatch.Draw(_couleur, new Rectangle(_rectangle.X - 5, _rectangle.Y - 5, _rectangle.Width + 10, _rectangle.Height + 10), Color.White);


        }

        public void DrawRotation(SpriteBatch spriteBatch, float rotation, Vector2 pointRef)
        {

                //if (!Session.verification_item(Name))
                    spriteBatch.Draw(_couleur, new Rectangle(_rectangle.X - 1, _rectangle.Y - 1, _rectangle.Width + 2, _rectangle.Height + 2), _rectangle, Color.White, rotation, pointRef, SpriteEffects.None, 0);

        }

        /*public String testTransparence()
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
        }*/

    }
}
