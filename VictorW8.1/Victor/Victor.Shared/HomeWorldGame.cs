using Victor.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;
using System.Diagnostics;

namespace Victor
{
    public class HomeWorldGame : Game
    {

        private Texture2D background;
        private Vector2 positionBackground;
        private Point realSizeBackground;
        private float ratioSizeScreen;

        //lampe
        private ImageTexture _lampe;
        private float _rotationLampe;
        private int _deltaRotationLampe;
        private bool _directionLampe;
        private TimeSpan _elapsedGameTimeLampe = TimeSpan.Zero;

        //Aiguilles
        private ImageTexture _petiteAiguille;
        private float _rotationPetiteAiguille;
        private TimeSpan _elapsedGameTimePetiteAiguille = TimeSpan.Zero;
        private ImageTexture _grandeAiguille;
        private float _rotationGrandeAiguille;


        //Flammes
        //private Image_Texture _flammes;
        private AnimatedImage _flammesSprite;



        private Vector2 touchPosition = new Vector2(0, 0);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Vector3 eye;
        Vector3 up;
        Vector3 at;

        float fov;
        //camera properties
        float zNear;
        float zFar;

        BasicEffect basicEffect;
        DynamicVertexBuffer vertexBuffer;
        DynamicIndexBuffer indexBuffer;
        VertexPositionColor[] cube;
        float cubeRotation = 0.0f;
        static ushort[] cubeIndices =
            {
                0,2,1, // -x
    			1,2,3,

                4,5,6, // +x
    			5,7,6,

                0,1,5, // -y
    			0,5,4,

                2,6,7, // +y
    			2,7,3,

                0,4,6, // -z
    			0,6,2,

                1,3,7, // +z
    			1,7,5,
            };
        public HomeWorldGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Resources";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //zNear = 0.001f;
            //zFar = 1000.0f;
            //fov = MathHelper.Pi * 70.0f / 180.0f;
            //eye = new Vector3(0.0f, 0.7f, 1.5f);
            //at = new Vector3(0.0f, 0.0f, 0.0f);
            //up = new Vector3(0.0f, 1.0f, 0.0f);

            //cube = new VertexPositionColor[8];
            //cube[0] = new VertexPositionColor(new Vector3(-0.5f, -0.5f, -0.5f), new Color(0.0f, 0.0f, 0.0f));
            //cube[1] = new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.5f), new Color(0.0f, 0.0f, 1.0f));
            //cube[2] = new VertexPositionColor(new Vector3(-0.5f, 0.5f, -0.5f), new Color(0.0f, 1.0f, 0.0f));
            //cube[3] = new VertexPositionColor(new Vector3(-0.5f, 0.5f, 0.5f), new Color(0.0f, 1.0f, 1.0f));
            //cube[4] = new VertexPositionColor(new Vector3(0.5f, -0.5f, -0.5f), new Color(1.0f, 0.0f, 0.0f));
            //cube[5] = new VertexPositionColor(new Vector3(0.5f, -0.5f, 0.5f), new Color(1.0f, 0.0f, 1.0f));
            //cube[6] = new VertexPositionColor(new Vector3(0.5f, 0.5f, -0.5f), new Color(1.0f, 1.0f, 0.0f));
            //cube[7] = new VertexPositionColor(new Vector3(0.5f, 0.5f, 0.5f), new Color(1.0f, 1.0f, 1.0f));

            //vertexBuffer = new DynamicVertexBuffer(graphics.GraphicsDevice, typeof(VertexPositionColor), 8, BufferUsage.WriteOnly);
            //indexBuffer = new DynamicIndexBuffer(graphics.GraphicsDevice, typeof(ushort), 36, BufferUsage.WriteOnly);

            //basicEffect = new BasicEffect(graphics.GraphicsDevice); //(device, null);
            //basicEffect.LightingEnabled = false;
            //basicEffect.VertexColorEnabled = true;
            //basicEffect.TextureEnabled = false;

            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            //Connexion à la BDD
            
            String niveau = "home";
            String lvl = "cuisine";
            /**
             * BACKGROUND
            **/
            //On récupère l'arrière plan de la scène
            
            //On affecte
            background = this.Content.Load<Texture2D>(@"Images\home_cuisine");



            ratioSizeScreen = (float)Window.ClientBounds.Height / (float)background.Height;
            
            realSizeBackground = new Point((int)(background.Width * ratioSizeScreen), Window.ClientBounds.Height);
            positionBackground = Vector2.Zero;

            //On s'occupe de la lampe
            _lampe = new ImageTexture(-1, "lampe_cuisine", @"Images\lampe_cuisine", new Vector2(1250 + positionBackground.X, 0), 130, 258);
            _lampe.LoadContent(Content);

            _rotationLampe = 0;
            _deltaRotationLampe = 10;
            _directionLampe = true;


            //On s'occupe des aiguilles
            _petiteAiguille = new ImageTexture(-1, "petite_aiguille", @"Images\aiguille-p", new Vector2(379 + positionBackground.X, 43), 3, 12);
            _petiteAiguille.LoadContent(Content);
            _rotationGrandeAiguille = 180;

            _grandeAiguille = new ImageTexture(-1, "grande_aiguille", @"Images\aiguille-g", new Vector2(379 + positionBackground.X, 43), 3, 23);
            _grandeAiguille.LoadContent(Content);
            _rotationPetiteAiguille = 180;



            //On s'occupe de la flamme
            _flammesSprite = new AnimatedImage(@"Images\FlammesSprites", new Vector2(860 + positionBackground.X, 290), 4);
            _flammesSprite.LoadContent(Content);



            TouchPanel.EnabledGestures = GestureType.FreeDrag;


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //click souris

            if (TouchPanel.IsGestureAvailable)
            {
                //MouseState state = Mouse.GetState();
                GestureSample state = TouchPanel.ReadGesture();

                if (state.GestureType == GestureType.FreeDrag)
                {

                    float xDeplacement = state.Delta.X;
                    //On regarde si on est pas au bord de l'écran
                    if (positionBackground.X + xDeplacement > 0)
                    {
                        xDeplacement = -positionBackground.X;
                    }
                    else if (positionBackground.X + xDeplacement < 800 - background.Width )
                    {
                        xDeplacement = 800 - background.Width - positionBackground.X;
                    }
                        //On replace les éléments en fonction du background
                        positionBackground.X += xDeplacement;
                        _lampe.Deplacement(new Vector2(xDeplacement, 0));
                        _grandeAiguille.Deplacement(new Vector2(xDeplacement, 0));
                        _petiteAiguille.Deplacement(new Vector2(xDeplacement, 0));
                        _flammesSprite.deplacement(new Vector2(xDeplacement, 0));
                }
            }






                //On fait bouger la flamme
            _flammesSprite.UpdateFixe(gameTime);

            //On fait bouger la lampe
            _elapsedGameTimeLampe += gameTime.ElapsedGameTime;

            if (_elapsedGameTimeLampe.TotalMilliseconds >= 60)
            {

                _elapsedGameTimeLampe = TimeSpan.Zero;

                if (_rotationLampe >= 0)
                {
                    if (!_directionLampe)
                    {
                        _deltaRotationLampe = 10;
                        _directionLampe = true;
                    }

                    _deltaRotationLampe -= 1;
                }
                else
                {
                    if (_directionLampe)
                    {
                        _deltaRotationLampe = -10;
                        _directionLampe = false;
                    }

                    _deltaRotationLampe += 1;
                }
                _rotationLampe += _deltaRotationLampe;
            }
            _elapsedGameTimePetiteAiguille += gameTime.ElapsedGameTime;

            if (_elapsedGameTimePetiteAiguille.TotalMilliseconds >= 60)
            {

                _elapsedGameTimePetiteAiguille = TimeSpan.Zero;
                _rotationPetiteAiguille += 1;
                if (_rotationPetiteAiguille >= 360)
                {
                    _rotationPetiteAiguille -= 360;
                }
                    
            }


            _rotationGrandeAiguille += 6;
            if (_rotationGrandeAiguille >= 360)
            {
                _rotationGrandeAiguille -= 360;
            }


        



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            spriteBatch.Begin();
            spriteBatch.Draw(background, Window.ClientBounds, new Rectangle(positionBackground.ToPoint(), new Point((int)(background.Width - (realSizeBackground.X-Window.ClientBounds.Width)/ratioSizeScreen), background.Height)), Color.White);


            _grandeAiguille.DrawRotation(spriteBatch, MathHelper.ToRadians(_rotationGrandeAiguille), new Vector2(_grandeAiguille.Rectangle.ElementAt(0).Width / 2, 0));
            _petiteAiguille.DrawRotation(spriteBatch, MathHelper.ToRadians(_rotationPetiteAiguille), new Vector2(_petiteAiguille.Rectangle.ElementAt(0).Width / 2, 0));
            _flammesSprite.Draw(spriteBatch);
            _lampe.DrawRotation(spriteBatch, MathHelper.ToRadians(_rotationLampe), new Vector2(_lampe.Rectangle.ElementAt(0).Width / 2, 0));


            //GraphicsDevice.Clear(Color.CornflowerBlue);

            //// Compute camera matrices.
            //Matrix View = Matrix.CreateLookAt(eye, at, up);

            //Matrix Projection = Matrix.CreatePerspectiveFieldOfView(fov, GraphicsDevice.Viewport.AspectRatio, zNear, zFar);
            //cubeRotation += (0.0025f) * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //Matrix World = Matrix.CreateRotationY(cubeRotation);
            //// TODO: Add your drawing code here

            //vertexBuffer.SetData(cube, 0, 8, SetDataOptions.Discard);
            //indexBuffer.SetData(cubeIndices, 0, 36, SetDataOptions.Discard);

            //GraphicsDevice device = basicEffect.GraphicsDevice;
            //device.SetVertexBuffer(vertexBuffer);
            //device.Indices = indexBuffer;

            //basicEffect.View = View;
            //basicEffect.Projection = Projection;
            //basicEffect.World = World;
            //foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            //{
            //    pass.Apply();
            //    device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 8, 0, 36);
            //}
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
