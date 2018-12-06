using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Viper viper = new Viper();
        const int NumAsteroids = 10;
        Model asteroidModel;
        Matrix[] asteroidTransforms;
        Asteroid[] asteroidList = new Asteroid[NumAsteroids];
        const float PlayfieldSizeX = 8000f;
        const float PlayfieldSizeY = 8000f;

        Random random = new Random();
        #region Camera
        //camera
        public float aspectRatio = 0;
        public Vector3 cameraPos = new Vector3(0.0f, 50.0f, 5000.0f);
        #endregion
        Texture2D stars;
        SpriteFont kootenay;


        Vector2 scorePosition = new Vector2(100, 50);
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            audioEngine = new AudioEngine("Content\\Audio\\MyGameAudio.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Audio\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Audio\\Sound Bank.xsb");

       
            ResetAsteroids();
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
            viper.model = Content.Load<Model>("models\\p1_wedge");
            stars = Content.Load<Texture2D>("textures/B1_stars");
            kootenay = Content.Load<SpriteFont>("fonts/Kootenay");
            asteroidModel = Content.Load<Model>("models\\asteroid1");
            //asteroidTransforms = SetupEffectDefaults(asteroidModel);
            viper.loadModel();
           
           aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
          asteroidTransforms = new Matrix[asteroidModel.Bones.Count];
          asteroidModel.CopyAbsoluteBoneTransformsTo(asteroidTransforms);



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
            // TODO: Add your update logic here
            // Get some input.
            UpdateInput();
            audioEngine.Update();
            // Add velocity to the current position.
            viper.position += viper.velocity;
            viper.velocity *= 0.95f;
            
           pos.X = viper.position.X;
           pos.Y = viper.position.Y ;
           pos.Z = viper.position.Z - 10000;
        
           float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
           for (int i = 0; i < NumAsteroids; i++)
           {
               asteroidList[i].Update(timeDelta);
           } 
         
          
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate,
    SaveStateMode.SaveState);
            
            spriteBatch.Draw(stars, new Rectangle(0, 0, 800, 600), Color.White);
            spriteBatch.End();
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
      SpriteSortMode.Immediate, SaveStateMode.SaveState);
            spriteBatch.DrawString(kootenay, "SPEED " + viper.velocity.Z,
                                   scorePosition, Color.LightGreen);
            Vector2 speedtoScreen =  new Vector2(100,100);
         
            spriteBatch.DrawString(kootenay, viper.position.ToString(),
                                    speedtoScreen, Color.LightGreen);
            //for (int i = 0; i < NumAsteroids; i++)
            //{
            //     Vector2 asteroidtoScreen = new Vector2 (500,i*25);
            //    spriteBatch.DrawString(kootenay, asteroidList[i].position.ToString(),
            //                       asteroidtoScreen, Color.LightBlue);
            //}
            spriteBatch.End();
            // TODO: Add your drawing code here
            // Draw the model. A model can have multiple meshes, so loop.
            
            for (int i = 0; i < NumAsteroids; i++)
            {
               
                if (true /*asteroidList[i].isActive*/)
                {
                    foreach (ModelMesh mesh in asteroidModel.Meshes)
                    {
                        // This is where the mesh orientation is set, as well 
                        // as our camera and projection.
                        
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.World = asteroidTransforms[mesh.ParentBone.Index]

                                * Matrix.CreateTranslation(asteroidList[i].position);
                            effect.View = Matrix.CreateLookAt(cameraPos,
                            viper.position, Vector3.Up);
                            //   Vector3.Zero, Vector3.Up);
                            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                                MathHelper.ToRadians(45.0f), aspectRatio,
                                1.0f, 100000.0f);
                        }
                        // Draw the mesh, using the effects set above.
                        mesh.Draw();
                    }
                    foreach (ModelMesh mesh in viper.model.Meshes)
                    {
                        // This is where the mesh orientation is set, as well 
                        // as our camera and projection.
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.World = viper.transforms[mesh.ParentBone.Index] *
                                Matrix.CreateRotationY(viper.Rotation + MathHelper.Pi)
                                * Matrix.CreateRotationX(viper.Pitch)
                                * Matrix.CreateTranslation(viper.position);
                            effect.View = Matrix.CreateLookAt(cameraPos,
                            viper.position, Vector3.Up);
                            //   Vector3.Zero, Vector3.Up);
                            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                                MathHelper.ToRadians(45.0f), camera.aspectRatio,
                                1.0f, 100000.0f);
                        }
                        // Draw the mesh, using the effects set above.
                        mesh.Draw();
                    }
                }
            }
            base.Draw(gameTime);
        }
        // Cue so we can hang on to the sound of the engine.
        Cue engineSound = null;
        protected void UpdateInput()
        {
            KeyboardState key = Keyboard.GetState();

            if (key.IsKeyDown(Keys.Up))
            {
                if (engineSound == null)
                {
                    engineSound = soundBank.GetCue("engine_2");
                    engineSound.Play();
                }

                else if (engineSound.IsPaused)
                {
                    engineSound.Resume();
                }
            }
            viper.Update(key);
          
           // In case you get lost, press A to warp back to the center.
            if (key.IsKeyDown(Keys.Space))
            {
               viper.position = Vector3.Zero;
               viper.velocity = Vector3.Zero;
               viper.Rotation = 0.0f;
            }

        }
       
        public const float AsteroidMinSpeed = 100.0f;
        public const float AsteroidMaxSpeed = 300.0f;
        private void ResetAsteroids()
        {
            float xStart;
            float yStart;
            for (int i = 0; i < NumAsteroids; i++)
            {
                if (random.Next(2) == 0)
                {
                    xStart = (float)-PlayfieldSizeX;
                }
                else
                {
                    xStart = (float)PlayfieldSizeX;
                }
                yStart =
                    (float)random.NextDouble() * PlayfieldSizeY;
                asteroidList[i].position = asteroidList[i].position = new Vector3(xStart, yStart, 0.0f);
                double angle = random.NextDouble() * 2 * Math.PI;
                asteroidList[i].direction.X = -(float)Math.Sin(angle);
                asteroidList[i].direction.Y = (float)Math.Cos(angle);
                asteroidList[i].speed = AsteroidMinSpeed +
                   (float)random.NextDouble() * AsteroidMaxSpeed;
            }

        }

    }
}
