using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace LD39
{


    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.ApplyChanges();
            this.Window.Title = "Robo Climb?";
            Content.RootDirectory = "Content";
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

            base.Initialize();

            PhysicsManager.Instance.initialise();
            LevelManager.Instance.initialise();
            LevelManager.Instance.setTileset("Tiles");

            //Starting the game.
            StartGame();
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
            ArtManager.Instance.addTexture("Player", Content.Load<Texture2D>("Robo"));
            ArtManager.Instance.addTexture("Player_right", Content.Load<Texture2D>("Robo_right"));
            ArtManager.Instance.addTexture("Player_left", Content.Load<Texture2D>("Robo_left"));
            ArtManager.Instance.addTexture("Player_Fist", Content.Load<Texture2D>("RoboFist"));
            ArtManager.Instance.addTexture("Player_Arm", Content.Load<Texture2D>("RoboArm"));
            ArtManager.Instance.addTexture("Player_Fist_Vert", Content.Load<Texture2D>("RoboFist_Vert"));
            ArtManager.Instance.addTexture("Player_Arm_Vert", Content.Load<Texture2D>("RoboArm_Vert"));
            ArtManager.Instance.addTexture("Tiles", Content.Load<Texture2D>("Tiles"));
            ArtManager.Instance.addTexture("Battery", Content.Load<Texture2D>("Battery"));
            ArtManager.Instance.addTexture("SilverCoin", Content.Load<Texture2D>("SilverCoin"));
            ArtManager.Instance.addTexture("GoldCoin", Content.Load<Texture2D>("GoldCoin"));

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            PhysicsManager.Instance.update((float)gameTime.ElapsedGameTime.TotalSeconds);
            ActorManager.Instance.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            ActorManager.Instance.Draw(spriteBatch);
            LevelManager.Instance.drawLevel(spriteBatch);
            PhysicsManager.Instance.debugDraw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }


        void StartGame(){
            LevelManager.Instance.loadFile("BaseLevel.oel");
        }
    }
}
