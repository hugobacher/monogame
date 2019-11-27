﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Template
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //Random generator
        Random random = new Random();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Declare variables for window size
        int windowWidth;
        int windowHeight;

        //Load the xwing texture and declare its position
        Texture2D xwing;
        Vector2 xwingPos;

        //A list with all bullet positions
        Texture2D redLaser;
        List<Vector2> xwingBulletPos = new List<Vector2>();

        //Load the background
        Texture2D background;
        Vector2 backgroundPos = new Vector2(0,0);

        //Load the tie fighters
        Texture2D tieFighter;
        List<Vector2> tieFighterPos = new List<Vector2>();

        //Load the explotion texture
        Texture2D explosion;
        Vector2 explosionPos;

        //Keyboard, mouse and controller states
        KeyboardState kNewState;
        KeyboardState kOldState;
        MouseState mNewState;
        MouseState mOldState;
        GamePadState gPState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Game size & fullscreen mode
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.IsFullScreen = true;

            //Give the window size variables their value
            windowHeight = graphics.PreferredBackBufferHeight;
            windowWidth = graphics.PreferredBackBufferWidth;

            //Set xwing start position
            xwingPos = new Vector2(windowWidth/2,windowHeight-150);
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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load the xwing texture into the game
            xwing = Content.Load<Texture2D>("xwing");
            //Load the red laser texture into the game
            redLaser = Content.Load<Texture2D>("redLaser");
            //Load background image
            background = Content.Load<Texture2D>("stars1080p");
            //Load background image
            tieFighter = Content.Load<Texture2D>("tieFighter");
            //Load explosion image
            explosion = Content.Load<Texture2D>("explosion");

            // TODO: use this.Content to load your game content here 
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            
            //Look what is pressed on the keyboard, mouse and controller
            kNewState = Keyboard.GetState();
            mNewState = Mouse.GetState();
            gPState = GamePad.GetState(PlayerIndex.One);

            //Stop the program if esc is pressed or back (share on ps4) button on a controller
            if (gPState.Buttons.Back == ButtonState.Pressed || kNewState.IsKeyDown(Keys.Escape))
                Exit();

            //Move the xwing right and left if the buttons are pressed
            if (kNewState.IsKeyDown(Keys.Right) || kNewState.IsKeyDown(Keys.D))
                //Make sure to not move outside the window
                if (xwingPos.X < windowWidth-xwing.Width)
                    xwingPos.X += 8;
            if (kNewState.IsKeyDown(Keys.Left) || kNewState.IsKeyDown(Keys.A))
                //Make sure to not move outside the window
                if (xwingPos.X > 0)
                    xwingPos.X -= 8;

            //Check if space or left mouse button is clicked to shoot bullet 
            if(kNewState.IsKeyDown(Keys.Space) && kOldState.IsKeyUp(Keys.Space) || mNewState.LeftButton == ButtonState.Pressed && mOldState.LeftButton == ButtonState.Released) {
                //Add bullets
                xwingBulletPos.Add(xwingPos + new Vector2(7,27));
                xwingBulletPos.Add(xwingPos + new Vector2(xwing.Width-11,27));
            }

            //Move the xwing bullets upwards
            for (int i = 0; i < xwingBulletPos.Count; i++) {
                xwingBulletPos[i] = xwingBulletPos[i] - new Vector2(0, 15);
            }

            //Check if the game should spawn a tieFighter
            int tieFighterSpawn = random.Next(10);
            int tieFighterXPos = random.Next(windowWidth);
            if(tieFighterSpawn == 0) {
                //Add tieFighters
                tieFighterPos.Add(new Vector2(tieFighterXPos, -50));
            }

            //Move the tieFighters downwards
            for (int i = 0; i < tieFighterPos.Count; i++) {
                tieFighterPos[i] = tieFighterPos[i] + new Vector2(0,10);
            }

            //Removes the objects
            RemoveObjects();

            //Save the keyboard & mouse state as the last frame, needs to be last!
            kOldState = kNewState;
            mOldState = mNewState;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Background color for the game
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            
            //Background image
            Rectangle backgroundRec = new Rectangle();
            backgroundRec.Location = backgroundPos.ToPoint();
            backgroundRec.Size = new Point(windowWidth, windowHeight);
            spriteBatch.Draw(background, backgroundRec, Color.White);

            //Draws the xwing, Color.White does not add any extra color on the object
            Rectangle xwingRec = new Rectangle();
            xwingRec.Location = xwingPos.ToPoint();
            xwingRec.Size = new Point(110,110);
            spriteBatch.Draw(xwing, xwingRec, Color.White);

            //Draws the tie fighters
            foreach (Vector2 tieFighterPos in tieFighterPos) {
                //Rectangle to resize the tieFighters
                Rectangle tieFighterRec = new Rectangle();
                tieFighterRec.Location = tieFighterPos.ToPoint();
                tieFighterRec.Size = new Point(80, 80);

                //Draw the tieFighters
                spriteBatch.Draw(tieFighter, tieFighterRec, Color.White);

                //Explosion when xwing hits tieFighter
                explosionPos = xwingPos - new Vector2(15,15);
                Rectangle explosionRec = new Rectangle();
                explosionRec.Location = explosionPos.ToPoint();
                explosionRec.Size = new Point(200, 200);
                if(xwingRec.Intersects(tieFighterRec)) {
                    spriteBatch.Draw(explosion, explosionRec, Color.White);
                }

            }

            //Draws the xwing bullets
            foreach (Vector2 bulletPos in xwingBulletPos) {
                //Rectangle to resize the bullet size
                Rectangle bulletRec = new Rectangle();
                bulletRec.Location = bulletPos.ToPoint();
                bulletRec.Size = new Point(5, 12);

                //Draw the bullets
                spriteBatch.Draw(redLaser, bulletRec, Color.White);
            }

            spriteBatch.End();

            // TODO: Add your drawing code here.

            base.Draw(gameTime);
        }

        void RemoveObjects()
        {
            //Remove bullets - more secure version
            List<Vector2> xwingBulletTemp = new List<Vector2>();
            foreach (var bullet in xwingBulletPos) {
                if (bullet.Y >= 0)
                    xwingBulletTemp.Add(bullet);
            }

            //Remove tieFighters
            List<Vector2> tieFighterTemp = new List<Vector2>();
            foreach (var tieFighter in tieFighterPos) {
                if (tieFighter.Y <= windowHeight)
                    tieFighterTemp.Add(tieFighter);
            }

            tieFighterPos = tieFighterTemp;
            xwingBulletPos = xwingBulletTemp;
        }
    }
}