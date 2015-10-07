using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Windows.Media.Imaging;

namespace VictorNamespace
{
    public class Victor
    {
        private Texture2D _texture;
        

        //retournement
        private Rectangle _sourceRectangleRetournement;
        private Rectangle _destinationRectangleRetournement;
        private int _retournement;
        //Habit

        private static double _ratio = 0.51;
        private Texture2D _head;
        private Texture2D _pants;
        private Texture2D _shirt;


        private List<Rectangle> _destinationRectangleDisguise;
        private List<Rectangle> _positionDisguise;
        private DataBaseContext dbc;




        private string _assetName;

        //sprite
        private int _currentIndex = 0;
        private int _maxIndex = 6;
        private SpriteEffects _effect = SpriteEffects.FlipHorizontally;

        private bool fixe;
        private bool flip = false;

        private Vector2 _position = Vector2.One;
        private Vector2 _direction = Vector2.One;
        private List<int> _widthRectangle;
        private List<Rectangle> _destinationRectangle;
        private Vector2 _positionDestinationRectangle;
        private List<Vector2> _positionRectangle;
        private List<Rectangle> _sourceRectangle;

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

        public List<Rectangle> Rectangle
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

        public float Rectangle_X
        {
            get
            {
                return _positionDestinationRectangle.X;
            }
            set
            {
                _positionDestinationRectangle.X = value;
            }
        }

        public float Rectangle_Y
        {
            get
            {
                return _positionDestinationRectangle.Y;
            }
            set
            {
                _positionDestinationRectangle.Y = value;
            }
        }

        public Vector2 PositionRectangle
        {
            get
            {
                return _positionDestinationRectangle;
            }
        }

        public Vector2 Position
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

        #endregion


        public Victor(string assetName, Vector2 position)
        {
            _assetName = assetName;
            _position = position;
            fixe = false;
            _pants = null;
            _shirt = null;
            _head = null;
        }

        public void repositionnement()
        {
            _currentIndex = 0;
            _retournement = 0;

            List<Rectangle> tmpRectangle = new List<Rectangle>();
            List<Rectangle> tmp2Rectangle = new List<Rectangle>();
            int valeurtmp = (int)( _destinationRectangle.ElementAt(0).Y - Position.Y);
            if (_effect == SpriteEffects.FlipHorizontally && flip == false)
            {
                flip = true;

                foreach (var destination in _destinationRectangle)
                {
                    tmpRectangle.Add(new Rectangle(_texture.Width / (_maxIndex + 1) - destination.Width - destination.X + (int)_position.X + (int)_position.X - (_texture.Width / (_maxIndex + 1)), destination.Y - valeurtmp, destination.Width, destination.Height));
                }
                _destinationRectangle = tmpRectangle;

                foreach (Rectangle destination in _destinationRectangleDisguise)
                {
                    tmp2Rectangle.Add(new Rectangle(_texture.Width / (_maxIndex + 1) - destination.Width - destination.X + (int)_position.X + (int)_position.X - (_texture.Width / (_maxIndex + 1)), destination.Y - valeurtmp, destination.Width, destination.Height));

                }
                _destinationRectangleDisguise = tmp2Rectangle;
            }
            else
            {
                if (_effect == SpriteEffects.None && flip == true)
                {
                    flip = false;
                    foreach (var destination in _destinationRectangle)
                    {
                        tmpRectangle.Add(new Rectangle(_texture.Width / (_maxIndex + 1) - destination.Width - destination.X + (int)_position.X, destination.Y - valeurtmp, destination.Width, destination.Height));
                    }
                    _destinationRectangle = tmpRectangle;

                    foreach (Rectangle destination in _destinationRectangleDisguise)
                    {
                        tmp2Rectangle.Add(new Rectangle(_texture.Width / (_maxIndex + 1) - destination.Width - destination.X + (int)_position.X, destination.Y - valeurtmp, destination.Width, destination.Height));

                    }
                    _destinationRectangleDisguise = tmp2Rectangle;
                }

            }            

        }
        public void LoadContent(ContentManager Content)
        {
            _positionDisguise = new List<Rectangle>();
            _destinationRectangleDisguise = new List<Rectangle>();
            
            //On récupère les informations
            dbc = new DataBaseContext(DataBaseContext.DBConnectionString);

                
                //La tete
                var heads = from accessoire in dbc.Accessoires
                            where accessoire.Modele.Nom == "head"
                            && accessoire.Porte == true
                            select accessoire;

            if(heads.Count()!=0)
            {
                _head = Content.Load<Texture2D>(heads.First().Url);
                _destinationRectangleDisguise.Add(new Rectangle((int)(_position.X + (float)72 * _ratio), (int)(_position.Y - (float)22 * _ratio), (int)((float)71 * _ratio), (int)((float)47 * _ratio)));

                _positionDisguise.Add(new Rectangle((int)((float)72 * _ratio), (int)((float)-22 * _ratio), (int)((float)71 * _ratio), (int)((float)47 * _ratio)));
            }

                
                //Les Pantalons
                var pants = from accessoire in dbc.Accessoires
                            where accessoire.Modele.Nom == "pants"
                            && accessoire.Porte == true
                            select accessoire;
                
            if (pants.Count() != 0)
            {
                _pants = Content.Load<Texture2D>(pants.First().Url);
                _destinationRectangleDisguise.Add(new Rectangle((int)(_position.X + (float)248 * _ratio), (int)(_position.Y + (float)135 * _ratio), (int)((float)95 * _ratio), (int)((float)151 * _ratio)));


                _positionDisguise.Add(new Rectangle((int)((float)248 * _ratio), (int)((float)135 * _ratio), (int)((float)95 * _ratio), (int)((float)151 * _ratio)));
            }

            
                
                //Les T-shirts
                var shirts = from accessoire in dbc.Accessoires
                             where accessoire.Modele.Nom == "shirt"
                             && accessoire.Porte == true
                             select accessoire;


                if (shirts.Count() != 0)
                {
                    _shirt = Content.Load<Texture2D>(shirts.First().Url);
                    _destinationRectangleDisguise.Add(new Rectangle((int)(_position.X + (float)115 * _ratio), (int)(_position.Y + (float)135 * _ratio), (int)((float)130 * _ratio), (int)((float)151 * _ratio)));
                    _positionDisguise.Add(new Rectangle((int)((float)115 * _ratio), (int)((float)135 * _ratio), (int)((float)130 * _ratio), (int)((float)151 * _ratio)));
                }

            dbc.Dispose();
            
            _texture = Content.Load<Texture2D>(@_assetName);

            _sourceRectangleRetournement = new Rectangle(0, 0, (_texture.Width / (_maxIndex + 1)), _texture.Height);

            _destinationRectangleRetournement = new Rectangle(0, 0, (_texture.Width / (_maxIndex + 1)), _texture.Height);

            _sourceRectangle = new List<Rectangle>();
            _sourceRectangle.Add(new Rectangle(0, 0, 66, 51));
            _sourceRectangle.Add(new Rectangle(41, 51, 27, 19));
            _sourceRectangle.Add(new Rectangle(47, 70, 128, 83));
            _sourceRectangle.Add(new Rectangle(173, 96, 27, 37));

            _positionRectangle = new List<Vector2>();
            _positionRectangle.Add(new Vector2(0, 0));
            _positionRectangle.Add(new Vector2(41, 51));
            _positionRectangle.Add(new Vector2(47, 70));
            _positionRectangle.Add(new Vector2(173, 96));

            //_sourceRectangle = new Rectangle(0, 0, _texture.Width / (_maxIndex + 1), _texture.Height);

            _destinationRectangle = new List<Rectangle>();
            _widthRectangle = new List<int>();

            foreach (var position in _sourceRectangle)
            {
                _destinationRectangle.Add(new Rectangle((int)_position.X + position.X, (int)_position.Y + position.Y, position.Width, position.Height));
                _widthRectangle.Add(position.Width);

            }

            List<Rectangle> tmpRectangle = new List<Rectangle>();
            List<Rectangle> tmp2Rectangle = new List<Rectangle>();
            if (_effect == SpriteEffects.FlipHorizontally && flip == false)
            {
                flip = true;

                foreach (var destination in _destinationRectangle)
                {
                    tmpRectangle.Add(new Rectangle(_texture.Width / (_maxIndex + 1) - destination.Width - destination.X + (int)_position.X + (int)_position.X - (_texture.Width / (_maxIndex + 1)), destination.Y, destination.Width, destination.Height));
                }
                _destinationRectangle = tmpRectangle;
                
                foreach (Rectangle destination in _destinationRectangleDisguise)
                {
                    tmp2Rectangle.Add(new Rectangle(_texture.Width / (_maxIndex + 1) - destination.Width - destination.X + (int)_position.X + (int)_position.X - (_texture.Width / (_maxIndex + 1)), destination.Y, destination.Width, destination.Height));

                }
                _destinationRectangleDisguise = tmp2Rectangle;
            }
            else
            {
                if (_effect == SpriteEffects.None && flip == true)
                {
                    flip = false;
                    foreach (var destination in _destinationRectangle)
                    {
                        tmpRectangle.Add(new Rectangle(_texture.Width / (_maxIndex + 1) - destination.Width - destination.X + (int)_position.X, destination.Y, destination.Width, destination.Height));
                    }
                    _destinationRectangle = tmpRectangle;
                    
                    foreach (Rectangle destination in _destinationRectangleDisguise)
                    {
                        tmp2Rectangle.Add(new Rectangle(_texture.Width / (_maxIndex + 1) - destination.Width - destination.X + (int)_position.X, destination.Y, destination.Width, destination.Height));

                    }
                    _destinationRectangleDisguise = tmp2Rectangle;
                }

            }

        }

        public void Retournement()
        {
            List<Rectangle> tmp2Rectangle = new List<Rectangle>();
            int cpt;
            if (_retournement > 0)
            {
                if (_retournement > 100)
                {
                    _destinationRectangleRetournement.Y = (int)_position.Y;
                    _destinationRectangleRetournement.X = (int)(_position.X - (_texture.Width / (_maxIndex + 1)) + (float)((float)(200 - _retournement) / 100) * (_texture.Width / (2*(_maxIndex + 1))));
                    _destinationRectangleRetournement.Width = (int)(((float)(_retournement - 100) / 100) * (_texture.Width / (_maxIndex + 1)));

                    cpt = 0;
                    foreach (Rectangle destination in _destinationRectangleDisguise)
                    {
                        
                            tmp2Rectangle.Add(new Rectangle((int)(_position.X - _positionDisguise.ElementAt(cpt).X - _positionDisguise.ElementAt(cpt).Width - (float)((float)(200 - _retournement) / 100) * ((_texture.Width / (_maxIndex + 1)) / 2 - _positionDisguise.ElementAt(cpt).X - _positionDisguise.ElementAt(cpt).Width)), destination.Y, (int)(((float)(_retournement - 100) / 100) * _positionDisguise.ElementAt(cpt).Width), destination.Height));
                        //else
                            //tmp2Rectangle.Add(new Rectangle((int)(_position.X - (_texture.Width / (_maxIndex + 1))+ _positionDisguise.ElementAt(cpt).X + _positionDisguise.ElementAt(cpt).Width + (float)((float)(200 - _retournement) / 100) * ((_texture.Width / (_maxIndex + 1)) / 2 - _positionDisguise.ElementAt(cpt).X - _positionDisguise.ElementAt(cpt).Width)), destination.Y, (int)(((float)(_retournement - 100) / 100) * _positionDisguise.ElementAt(cpt).Width), destination.Height));
                            
                        cpt++;
                    }
                    _destinationRectangleDisguise = tmp2Rectangle;

                }
                else
                {
                    _effect = SpriteEffects.None;
                    _destinationRectangleRetournement.Y = (int)_position.Y;
                    _destinationRectangleRetournement.X = (int)(_position.X - (_texture.Width / (_maxIndex + 1)) + (float)((float)(_retournement) / 100) * (_texture.Width / (2*(_maxIndex + 1))));
                    _destinationRectangleRetournement.Width = (int)(((float)(100 - _retournement) / 100) * (_texture.Width / (_maxIndex + 1)));
                    cpt = 0;
                    foreach (Rectangle destination in _destinationRectangleDisguise)
                    {
                        
                            tmp2Rectangle.Add(new Rectangle((int)(_position.X - (_texture.Width / (_maxIndex + 1)) + _positionDisguise.ElementAt(cpt).X + (float)((float)( _retournement) / 100) * ((_texture.Width / (_maxIndex + 1)) / 2 - _positionDisguise.ElementAt(cpt).X)), destination.Y, (int)(((float)(100 - _retournement) / 100) * _positionDisguise.ElementAt(cpt).Width), destination.Height));
                        //else
                            //tmp2Rectangle.Add(new Rectangle((int)(_position.X - (_texture.Width / (_maxIndex + 1)) + _positionDisguise.ElementAt(cpt).X + _positionDisguise.ElementAt(cpt).Width + (float)((float)(200 - _retournement) / 100) * ((_texture.Width / (_maxIndex + 1)) / 2 - _positionDisguise.ElementAt(cpt).X - _positionDisguise.ElementAt(cpt).Width)), destination.Y, (int)(((float)(_retournement - 100) / 100) * _positionDisguise.ElementAt(cpt).Width), destination.Height));

                        cpt++;
                    }
                    _destinationRectangleDisguise = tmp2Rectangle;

                }
                _retournement-=10;
            }
            else if (_retournement < 0)
            {
                if (_retournement < -100)
                {
                    _destinationRectangleRetournement.Y = (int)_position.Y;
                    _destinationRectangleRetournement.X = (int)(_position.X + (float)((float)(200 + _retournement) / 100) * (_texture.Width / (2*(_maxIndex + 1))));
                    _destinationRectangleRetournement.Width = (int)(((float)(-_retournement - 100) / 100) * (_texture.Width / (_maxIndex + 1)));

                    cpt = 0;
                    foreach (Rectangle destination in _destinationRectangleDisguise)
                    {
                        tmp2Rectangle.Add(new Rectangle((int)(_position.X + _positionDisguise.ElementAt(cpt).X + (float)((float)(200 +_retournement) / 100) * ((_texture.Width / (_maxIndex + 1)) / 2 - _positionDisguise.ElementAt(cpt).X)), destination.Y, (int)(((float)(-_retournement - 100) / 100) * _positionDisguise.ElementAt(cpt).Width), destination.Height));
                        cpt++;
                    }
                    _destinationRectangleDisguise = tmp2Rectangle;

                }
                else
                {
                    _effect = SpriteEffects.FlipHorizontally;
                    _destinationRectangleRetournement.Y = (int)_position.Y;
                    _destinationRectangleRetournement.X = (int)(_position.X + (float)((float)(-_retournement) / 100) * (_texture.Width / (2*(_maxIndex + 1))));
                    _destinationRectangleRetournement.Width = (int)(((float)(100 + _retournement) / 100) * (_texture.Width / (_maxIndex + 1)));

                    cpt = 0;
                    foreach (Rectangle destination in _destinationRectangleDisguise)
                    {
                        tmp2Rectangle.Add(new Rectangle((int)(_position.X + (_texture.Width / (2*(_maxIndex + 1))) + (float)((float)(100 +_retournement) / 100) * ((_texture.Width / (_maxIndex + 1)) / 2 - _positionDisguise.ElementAt(cpt).Width - _positionDisguise.ElementAt(cpt).X)), destination.Y, (int)(((float)(100 + _retournement) / 100) * _positionDisguise.ElementAt(cpt).Width), destination.Height));
                        cpt++;
                    }
                    _destinationRectangleDisguise = tmp2Rectangle;
                }

                _retournement+=10;
            }

        }

        public void deplacement(Vector2 vitesse)
        {
  
                List<Rectangle> tmpRectangle = new List<Rectangle>();
                List<Rectangle> tmp2Rectangle = new List<Rectangle>();
                foreach (Rectangle position in _destinationRectangle)
                {
                    tmpRectangle.Add(new Rectangle((int)(position.X + vitesse.X), (int)(position.Y + vitesse.Y), position.Width, position.Height));

                }
                _destinationRectangle = tmpRectangle;
                foreach (Rectangle position in _destinationRectangleDisguise)
                {
                    tmp2Rectangle.Add(new Rectangle((int)(position.X + vitesse.X), (int)(position.Y + vitesse.Y), position.Width, position.Height));

                }
                _destinationRectangleDisguise = tmp2Rectangle;

                _position.X += vitesse.X;
                _position.Y += vitesse.Y;
        }

        public void UpdateFixe(GameTime gameTime)
        {

            _elapsedGameTime += gameTime.ElapsedGameTime;

            if (_elapsedGameTime.TotalMilliseconds >= 30)
            {
                List<Rectangle> tmpRectangle;
                _elapsedGameTime = TimeSpan.Zero;
                _currentIndex++;

                if (_currentIndex > _maxIndex)
                    _currentIndex = 0;
                tmpRectangle = new List<Rectangle>();
                int cpt = 0;
                foreach (var source in _sourceRectangle)
                {
                    tmpRectangle.Add(new Rectangle(_currentIndex * _texture.Width / (_maxIndex + 1) + (int)_positionRectangle.ElementAt(cpt).X, source.Y, source.Width, source.Height));
                    cpt++;
                }
                _sourceRectangle = tmpRectangle;
            }
        }
        
        public void Update(GameTime gameTime, SharedGraphicsDeviceManager graphics)
        {
            

            if (_retournement < -10 || _retournement > 10)
            {
                Retournement();


            }
            else
            {
                List<Rectangle> tmpRectangle;
                List<Rectangle> tmp2Rectangle;
                if (_retournement != 0)
                    Retournement();
                else
                {
                    
                    if (_destinationRectangle.First().X <= 0 || _destinationRectangle.First().X + _destinationRectangle.First().Width >= graphics.PreferredBackBufferHeight)
                    {
                        _direction.X *= -1;

                        if (_effect == SpriteEffects.FlipHorizontally)
                        {
                            _retournement = 200;
                            Retournement();
                            //_effect = SpriteEffects.None;
                        }
                        else
                        {
                            if (_effect == SpriteEffects.None)
                            {
                                _retournement = -200;
                                Retournement();
                                //_effect = SpriteEffects.FlipHorizontally;
                            }

                        }

                    }
                }


                    if (_effect == SpriteEffects.FlipHorizontally && flip == false)
                    {
                        flip = true;
                        tmpRectangle = new List<Rectangle>();
                        foreach (var destination in _destinationRectangle)
                        {
                            tmpRectangle.Add(new Rectangle(_texture.Width / (_maxIndex + 1) - destination.Width - destination.X + (int)_position.X + (int)_position.X , destination.Y, destination.Width, destination.Height));
                        }
                        _destinationRectangle = tmpRectangle;
                        int cpt = 0;
                        tmp2Rectangle = new List<Rectangle>();
                        foreach (var destination in _destinationRectangleDisguise)
                        {
                            tmp2Rectangle.Add(new Rectangle((int)_position.X + _texture.Width / (_maxIndex + 1) - (int)_positionDisguise.ElementAt(cpt).Width - (int)_positionDisguise.ElementAt(cpt).X, destination.Y, destination.Width, destination.Height));
                            cpt++;
                        }
                        _destinationRectangleDisguise = tmp2Rectangle;
                    }
                    else
                    {
                        if (_effect == SpriteEffects.None && flip == true)
                        {
                            flip = false;
                            tmpRectangle = new List<Rectangle>();
                            int cpt = 0;
                            foreach (var destination in _destinationRectangle)
                            {
                                tmpRectangle.Add(new Rectangle(destination.X - _texture.Width / (_maxIndex + 1) + 2 * (int)_positionRectangle.ElementAt(cpt).X + destination.Width, destination.Y, destination.Width, destination.Height));
                                cpt++;
                            }
                            _destinationRectangle = tmpRectangle;

                            tmp2Rectangle = new List<Rectangle>();
                            cpt = 0;
                            foreach (Rectangle destination in _destinationRectangleDisguise)
                            {
                                tmp2Rectangle.Add(new Rectangle((int)_position.X + (int)_positionDisguise.ElementAt(cpt).X - _texture.Width / (_maxIndex + 1) + 2, destination.Y, destination.Width, destination.Height));
                                cpt++;
                            }
                            _destinationRectangleDisguise = tmp2Rectangle;
                        }

                    }


                    tmpRectangle = new List<Rectangle>();
                    foreach (var position in _destinationRectangle)
                    {
                        tmpRectangle.Add(new Rectangle((int)position.X + 3 * (int)_direction.X, position.Y, position.Width, position.Height));
                    }
                    _destinationRectangle = tmpRectangle;

                    tmp2Rectangle = new List<Rectangle>();
                    foreach (var position in _destinationRectangleDisguise)
                    {
                        tmp2Rectangle.Add(new Rectangle((int)position.X + 3 * (int)_direction.X, position.Y, position.Width, position.Height));
                    }
                    _destinationRectangleDisguise = tmp2Rectangle;

                    _elapsedGameTime += gameTime.ElapsedGameTime;

                    if (_elapsedGameTime.TotalMilliseconds >= 50)
                    {
                        _elapsedGameTime = TimeSpan.Zero;
                        _currentIndex++;

                        if (_currentIndex > _maxIndex)
                            _currentIndex = 0;
                        tmpRectangle = new List<Rectangle>();
                        int cpt = 0;
                        foreach (var source in _sourceRectangle)
                        {
                            tmpRectangle.Add(new Rectangle(_currentIndex * _texture.Width / (_maxIndex + 1) + (int)_positionRectangle.ElementAt(cpt).X, source.Y, source.Width, source.Height));
                            cpt++;
                        }
                        _sourceRectangle = tmpRectangle;
                    }

                    if (flip)
                        _position.X = _destinationRectangle.First().X + _destinationRectangle.First().Width;
                    else
                        _position.X = _destinationRectangle.First().X;
                    _position.Y = _destinationRectangle.First().Y;
                
                 
            }
        }

        public void Update(GameTime gameTime, SharedGraphicsDeviceManager graphics, Vector2 souris)
        {
            if (_retournement < -10 || _retournement > 10)
            {
                Retournement();


            }
            else
            {
                List<Rectangle> tmpRectangle;
                List<Rectangle> tmp2Rectangle;
                

                if ((_position.X > souris.X + 2 || _position.X < souris.X - 2) || (_position.Y > souris.Y + 4 || _position.Y <= souris.Y - 4))
                {
                    if (fixe)
                        fixe = false;
                    if (_retournement == 0)
                    {
                        if (_effect == SpriteEffects.FlipHorizontally)
                        {
                            if (souris.X > _destinationRectangle.First().X - _texture.Width / (_maxIndex + 1) + _destinationRectangle.First().Width - 1 && souris.X < (_destinationRectangle.First().X + _destinationRectangle.First().Width + 1))
                            {
                                if (flip)
                                    souris.X += _texture.Width / (_maxIndex + 1) + 1;
                                else
                                    souris.X -= (_texture.Width / (_maxIndex + 1) + 1);
                            }
                            else
                                if (souris.X < _position.X)
                                {
                                    _retournement = 200;
                                    Retournement();
                                    //_effect = SpriteEffects.None;
                                }
                                
                        }
                        else
                        {
                            if (souris.X > _destinationRectangle.First().X - 1 && souris.X < (_destinationRectangle.First().X + _texture.Width / (_maxIndex + 1) + 1))
                            {
                                if (flip)
                                    souris.X += _texture.Width / (_maxIndex + 1) + 1;
                                else
                                    souris.X -= (_texture.Width / (_maxIndex + 1) + 1);
                            }
                            else
                                if (souris.X > _position.X)
                                {
                                    _retournement = -200;
                                    Retournement();
                                    //_effect = SpriteEffects.None;
                                }

                        }
                    }
                    else
                    {
                        Retournement();


                        if (_effect == SpriteEffects.FlipHorizontally && flip == false)
                        {
                            flip = true;
                            tmpRectangle = new List<Rectangle>();
                            foreach (var destination in _destinationRectangle)
                            {
                                tmpRectangle.Add(new Rectangle(_texture.Width / (_maxIndex + 1) - destination.Width - destination.X + (int)_position.X + (int)_position.X, destination.Y, destination.Width, destination.Height));
                            }
                            _destinationRectangle = tmpRectangle;

                            int cpt = 0;
                            tmp2Rectangle = new List<Rectangle>();
                            foreach (var destination in _destinationRectangleDisguise)
                            {
                                tmp2Rectangle.Add(new Rectangle((int)_position.X + _texture.Width / (_maxIndex + 1) - (int)_positionDisguise.ElementAt(cpt).Width - (int)_positionDisguise.ElementAt(cpt).X, destination.Y, destination.Width, destination.Height));
                                cpt++;
                            }
                            _destinationRectangleDisguise = tmp2Rectangle;
                        }
                        else
                        {
                            if (_effect == SpriteEffects.None && flip == true)
                            {
                                flip = false;
                                tmpRectangle = new List<Rectangle>();
                                int cpt = 0;
                                foreach (var destination in _destinationRectangle)
                                {
                                    tmpRectangle.Add(new Rectangle(destination.X - _texture.Width / (_maxIndex + 1) + 2 * (int)_positionRectangle.ElementAt(cpt).X + destination.Width, destination.Y, destination.Width, destination.Height));
                                    cpt++;
                                }
                                _destinationRectangle = tmpRectangle;

                                tmp2Rectangle = new List<Rectangle>();
                                cpt = 0;
                                foreach (Rectangle destination in _destinationRectangleDisguise)
                                {
                                    tmp2Rectangle.Add(new Rectangle((int)_position.X + (int)_positionDisguise.ElementAt(cpt).X - _texture.Width / (_maxIndex + 1) + 2, destination.Y, destination.Width, destination.Height));
                                    cpt++;
                                }
                                _destinationRectangleDisguise = tmp2Rectangle;
                            }

                        }
                    }


                    if (_retournement == 0)
                    {
                        _direction.X = souris.X - _position.X;
                        _direction.Y = souris.Y - _position.Y;

                        if (_direction.X == _direction.Y)
                        {
                            if (_direction.X < 0)
                            {
                                _direction.X = -1;
                                _direction.Y = -1;
                            }
                            else
                            {
                                _direction.X = 1;
                                _direction.Y = 1;
                            }
                        }
                        else
                        {
                            if (_direction.X * _direction.X > _direction.Y * _direction.Y)
                            {
                                if (_direction.X < 0)
                                {
                                    _direction.Y = _direction.Y / _direction.X * -1;
                                    _direction.X = -1;
                                }
                                else
                                {
                                    _direction.Y = _direction.Y / _direction.X;
                                    _direction.X = 1;
                                }

                            }
                            else
                            {
                                if (_direction.Y < 0)
                                {
                                    _direction.X = _direction.X / _direction.Y * -1;
                                    _direction.Y = -1;
                                }
                                else
                                {
                                    _direction.X = _direction.X / _direction.Y;
                                    _direction.Y = 1;
                                }
                            }
                        }


                        tmpRectangle = new List<Rectangle>();
                        foreach (var position in _destinationRectangle)
                        {
                            tmpRectangle.Add(new Rectangle((int)position.X + 3 * (int)_direction.X, position.Y + (int)(3 * _direction.Y), position.Width, position.Height));
                        }
                        _destinationRectangle = tmpRectangle;

                        tmp2Rectangle = new List<Rectangle>();
                        foreach (var position in _destinationRectangleDisguise)
                        {
                            tmp2Rectangle.Add(new Rectangle((int)position.X + 3 * (int)_direction.X, position.Y + (int)(3 * _direction.Y), position.Width, position.Height));
                        }
                        _destinationRectangleDisguise = tmp2Rectangle;


                        _elapsedGameTime += gameTime.ElapsedGameTime;

                        if (_elapsedGameTime.TotalMilliseconds >= 50)
                        {
                            _elapsedGameTime = TimeSpan.Zero;
                            _currentIndex++;

                            if (_currentIndex > _maxIndex)
                                _currentIndex = 0;

                            tmpRectangle = new List<Rectangle>();
                            int cpt = 0;
                            foreach (var source in _sourceRectangle)
                            {
                                tmpRectangle.Add(new Rectangle(_currentIndex * _texture.Width / (_maxIndex + 1) + (int)_positionRectangle.ElementAt(cpt).X, source.Y, source.Width, source.Height));
                                cpt++;
                            }
                            _sourceRectangle = tmpRectangle;

                        }
                    }
                    
                }
                else
                {
                    if (!fixe)
                    {
                        _currentIndex = 0;
                        int cpt = 0;
                        tmpRectangle = new List<Rectangle>();
                        foreach (var source in _sourceRectangle)
                        {
                            tmpRectangle.Add(new Rectangle(_currentIndex * _texture.Width / (_maxIndex + 1) + (int)_positionRectangle.ElementAt(cpt).X, source.Y, source.Width, source.Height));
                            cpt++;
                        }
                        _sourceRectangle = tmpRectangle;
                        fixe = true;
                    }
                }

                if (flip)
                    _position.X = _destinationRectangle.First().X + _destinationRectangle.First().Width;
                else
                    _position.X = _destinationRectangle.First().X;
                _position.Y = _destinationRectangle.First().Y;
            }
        }

        public void retour()
        {
            Rectangle tmp;
            _currentIndex = 0;

            int cpt = 0;
            foreach (var source in _sourceRectangle)
            {
                tmp = new Rectangle(_currentIndex * _texture.Width / (_maxIndex + 1) + source.Width, 0, source.Width, _texture.Height);
                _destinationRectangle.RemoveAt(cpt);
                _destinationRectangle.Insert(cpt, tmp);
                cpt++;
            }
            fixe = true;
            _direction.X *= -1;
            _direction.Y *= -1;

            tmp = _destinationRectangle.First();
            tmp.X += (int)(3 * _direction.X);
            tmp.Y += (int)(3 * _direction.Y);
            _destinationRectangle.RemoveAt(0);
            _destinationRectangle.Insert(0, tmp);

            if (flip)
                _position.X = _destinationRectangle.First().X + _texture.Width / (_maxIndex + 1);
            else
                _position.X = _destinationRectangle.First().X;
            _position.Y = _destinationRectangle.First().Y;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int cpt = 0;
            if (_retournement != 0)
            {
                spriteBatch.Draw(_texture, _destinationRectangleRetournement, _sourceRectangleRetournement, Color.White, 0, Vector2.One, _effect, 0);
                if (_head!=null)
                {
                    spriteBatch.Draw(_head, _destinationRectangleDisguise.ElementAt(cpt), null, Color.White, 0, Vector2.One, _effect, 0);
                    cpt++;
                }
                if (_pants!=null)
                {
                    spriteBatch.Draw(_pants, _destinationRectangleDisguise.ElementAt(cpt), null, Color.White, 0, Vector2.One, _effect, 0);
                    cpt++;
                }
                if (_shirt!=null)
                {
                    spriteBatch.Draw(_shirt, _destinationRectangleDisguise.ElementAt(cpt), null, Color.White, 0, Vector2.One, _effect, 0);
                    cpt++;
                }
            }
            else
            {
                

                foreach (var source in _sourceRectangle)
                {
                    spriteBatch.Draw(_texture, _destinationRectangle.ElementAt(cpt), source, Color.White, 0, Vector2.One, _effect, 0);
                    cpt++;
                }

                cpt = 0;
                if (_head != null)
                {
                    spriteBatch.Draw(_head, _destinationRectangleDisguise.ElementAt(cpt), null, Color.White, 0, Vector2.One, _effect, 0);
                    cpt++;
                }
                if (_pants != null)
                {
                    spriteBatch.Draw(_pants, _destinationRectangleDisguise.ElementAt(cpt), null, Color.White, 0, Vector2.One, _effect, 0);
                    cpt++;
                }
                if (_shirt != null)
                {
                    spriteBatch.Draw(_shirt, _destinationRectangleDisguise.ElementAt(cpt), null, Color.White, 0, Vector2.One, _effect, 0);
                    cpt++;
                }
                

            }

        }

        
    }
}

