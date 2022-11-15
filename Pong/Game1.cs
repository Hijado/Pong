using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
    /// <summary>
    /// Declares variables needed for program
    /// </summary>
    public class Game1 : Game
    {   
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        SpriteFont _font;
        Texture2D back, padle1, padle2, bal, GameOverScreen, MainMenuScreen;
        Vector2 direction, position, padle1V, padle2V, balV, bal0, bal1, bal2, bal3, bal4, bal5, balOrg;
        Rectangle padle1Rec, padle2Rec, balRec, padle1top, padle1bot, padle2top, padle2bot;
        KeyboardState cKBS;
        Random random = new Random();

        // State of the game 0 is main menu, 1 is gameplay, 2 is gameover
        int state = 0;

        int padleSpeed, C, D;
        float balSpeed, degrees;
        bool bal00, bal01, bal02, bal03, bal04, bal05;
        bool player1;
        bool player2;
        float angle;

        /// <summary>
        /// Height and width of screen, also creates graphics manager
        /// </summary>
        public Game1()
        {
            // Reconfigures the height and width
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        
        /// <summary>
        /// Does nothing atm
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Loads all sprites and asigns values to variables
        /// </summary>
        protected override void LoadContent()
        {

            // Asigns value to counters (switch statement), loads all sprites
            C = 0;
            D = 0;

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            back = Content.Load<Texture2D>("Back");
            padle1 = Content.Load<Texture2D>("Padle");
            padle2 = Content.Load<Texture2D>("Padle");
            bal = Content.Load<Texture2D>("Ball");
            GameOverScreen = Content.Load<Texture2D>("GameOverScreenV1");
            MainMenuScreen = Content.Load<Texture2D>("WelcomePong");
            _font = Content.Load<SpriteFont>("Welcome");

            // Gives initial vector to all sprites
            padle1V = new Vector2(50, 310);
            padle2V = new Vector2(1210, 310);
            balV = new Vector2(630, 330);
            balOrg = new Vector2(630, 330);

            bal0 = new Vector2(75, 15);
            bal1 = new Vector2(45, 15);
            bal2 = new Vector2(15, 15);
            bal3 = new Vector2(1190, 15);
            bal4 = new Vector2(1220, 15);
            bal5 = new Vector2(1250, 15);
            padleSpeed = 10;

            // Alive = true, death = false, score
            bal00 = bal01 = bal02 = bal03 = bal04 = bal05 = true;

            // Calls balUpdate to calculate a random direction
            balUpdate();

        }

        /// <summary>
        /// Main method for updating the game
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                state = 1;
            }

            gamePlay();

            base.Update(gameTime);
 
        }

        /// <summary>
        /// Calculates rectangle collision, main method for gameplay
        /// </summary>
        private void gamePlay()
        {
            if (state == 1)
            {
                // Retrieves input
                input();
                player1 = true;
                player2 = true;

                // Configures rectangles and bal velocity
                balV += position;

                padle1Rec = new Rectangle((int)padle1V.X, (int)padle1V.Y , width: 26, height: 100);
                padle1top = new Rectangle((int)padle1V.X, (int)padle1V.Y - 20, width: 26, height: 20);
                padle1bot = new Rectangle((int)padle1V.X, (int)padle1V.Y + 100, width: 26, height: 20);

                padle2top = new Rectangle((int)padle2V.X - 10, (int)padle2V.Y - 20, width: 26, height: 20);
                padle2bot = new Rectangle((int)padle2V.X - 10, (int)padle2V.Y + 100, width: 26, height: 20);
                padle2Rec = new Rectangle((int)padle2V.X - 10, (int)padle2V.Y, width: 26, height: 100);
                balRec = new Rectangle((int)balV.X, (int)balV.Y, width: 16, height: 16);

                // Prevents Ball sprite from leaving screen Y-axe
                if (balV.Y < 50 || balV.Y > 650)
                {
                    position.Y = -position.Y;
                }

                // Detects if the balRec and pedalxRecs collide, and if so adds speed
                // If no collision then x player loses life, balV resets with new angle and normal speed
                if (balRec.Intersects(padle1top))
                {
                    position.Y = - 5;
                    position.X = - 15;
                }
                else if (balRec.Intersects(padle1bot))
                {
                    position.Y = 5;
                    position.X = - 15;
                }
                else if (balRec.Intersects(padle2top))
                {
                    position.Y = - 5;
                    position.X = 15;
                }
                else if (balRec.Intersects(padle2bot))
                {
                    position.Y = 5;
                    position.X = 15;
                }


                if (balRec.Intersects(padle1Rec))
                {   
                    position.X = -position.X + 1;
                }
                else if (balV.X < 0)
                {
                    balV = balOrg;
                    balUpdate();
                    C++;
                    isAlive(C);
                }

                if (balRec.Intersects(padle2Rec))
                {    
                    position.X = -position.X - 1;
                }
                else if (balV.X > 1280)
                {
                    balV = balOrg;
                    balUpdate();
                    D++;
                    isAlive1(D);

                }
            }
        }

        /// <summary>
        /// Gets the random direction for the ball sprite
        /// </summary>
        private void balUpdate()
        {   
            // Calculates random angle and gives it the standard balSpeed, position gets added to the balV
            balSpeed = 5f;
            degrees = random.Next(30, 330);
            int X = 120; 

            for (int i = 60; i <= 100000; i += 90)
            {
                if (i < degrees && degrees < X)
                {
                    degrees = random.Next(30, 330);
                    X += 90;
                }
                else
                    break;
            }

            angle = MathHelper.ToRadians(degrees);
            direction = new Vector2((float)Math.Cos(angle), -(float)Math.Sin(angle));
            position = direction * balSpeed;

        }

        /// <summary>
        /// Sets sprite lives player1 to false if live is lost
        /// </summary>
        /// <param name="c1">Counter</param>
        private void isAlive(int c1)
        {
            // Sets the Ball sprites to false (Dead) for player1 and if third life is gone -> endscreen
            // -> player1 life is false
            switch (c1)
            {
                case 1: 
                    bal02 = false; 
                    break;
                case 2: 
                    bal01 = false; 
                    break;
                case 3: 
                    bal00 = false;
                    state = 2;
                    player1 = false;
                    break;
            }
        }

        /// <summary>
        /// Sets sprite lives player2 to false if live is lost
        /// </summary>
        /// <param name="d1">Counter</param>
        private void isAlive1(int d1)
        {   
            // Sets the Ball sprites to false (Dead) for player2 and if third life is gone -> endscreen
            // -> player2 life is false
            switch (d1)
            {
                case 1: 
                    bal03 = false;
                    break;
                case 2 : 
                    bal04 = false;
                    break;
                case 3 :
                    bal05 = false;
                    state = 2;
                    player2 = false;
                    break;        
            }
        }

        /// <summary>
        /// Gets your keyboard input
        /// </summary>
        private void input()
        {   
            // InputHelper
            cKBS = Keyboard.GetState();

            // Prevents padles from leaving screen
            padle2V.Y = MathHelper.Clamp(padle2V.Y, 50, _graphics.PreferredBackBufferHeight - padle2Rec.Height - 50);
            padle1V.Y = MathHelper.Clamp(padle1V.Y, 50, _graphics.PreferredBackBufferHeight - padle1Rec.Height - 50);

            if (cKBS.IsKeyDown(Keys.Down))
            {
                padle2V = new Vector2(1210, padle2V.Y + padleSpeed);
            }
            else if (cKBS.IsKeyDown(Keys.Up))
            {
                padle2V = new Vector2(1210, padle2V.Y - padleSpeed);
            }

            if (cKBS.IsKeyDown(Keys.S))
            {
                padle1V = new Vector2(50, padle1V.Y + padleSpeed);
            }
            else if (cKBS.IsKeyDown(Keys.W))
            {
                padle1V = new Vector2(50, padle1V.Y - padleSpeed);
            }
        }

        /// <summary>
        /// Main draw method for welcome, gameplay and gameover screen
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            // Drawing sprites
            // Main menu
            if (state == 0)
            {
                _spriteBatch.Draw(MainMenuScreen, Vector2.Zero, Color.White);
            }

            gamePlay_draw();

            gameOver_draw();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws the gameplay screen
        /// </summary>
        private void gamePlay_draw()
        {
            // Gameplay
            if (state == 1)
            {
                _spriteBatch.Draw(back, Vector2.Zero, Color.White);
                _spriteBatch.Draw(padle1, padle1V, Color.White);
                _spriteBatch.Draw(padle2, padle2V, Color.White);
                _spriteBatch.Draw(bal, balV, Color.White);

                // Rectangle draws, not needed for game
                /*_spriteBatch.Draw(padle1, padle1top, Color.Yellow);
                _spriteBatch.Draw(padle2, padle2top, Color.Yellow);
                _spriteBatch.Draw(padle1, padle1bot, Color.Yellow);
                _spriteBatch.Draw(padle2, padle2bot, Color.Yellow);

                _spriteBatch.Draw(padle1, padle1Rec, Color.Purple);
                _spriteBatch.Draw(padle2, padle2Rec, Color.Purple);

                _spriteBatch.Draw(bal, balRec, Color.Red);*/

                if (bal02)
                {
                    _spriteBatch.Draw(bal, bal0, Color.White);
                }
                if (bal01)
                {
                    _spriteBatch.Draw(bal, bal1, Color.White);
                }
                if (bal00)
                {
                    _spriteBatch.Draw(bal, bal2, Color.White);
                }
                if (bal03)
                {
                    _spriteBatch.Draw(bal, bal3, Color.White);
                }
                if (bal04)
                {
                    _spriteBatch.Draw(bal, bal4, Color.White);
                }
                if (bal05)
                {
                    _spriteBatch.Draw(bal, bal5, Color.White);
                }
            }
        }
        
        /// <summary>
        /// Draws the gameover screen
        /// </summary>
        private void gameOver_draw()
        {
            // Gameover
            if (state == 2)
            {
                _spriteBatch.Draw(GameOverScreen, Vector2.Zero, Color.White);
                if (player1)
                {
                    _spriteBatch.DrawString(_font, text: "Player 1 Won, Press Space to play again", new Vector2(25, 0), Color.White);
                }
                else if (player2)
                {
                    _spriteBatch.DrawString(_font, text: "Player 2 Won, Press Space to play again", new Vector2(25, 0), Color.White);
                }
            }
            _spriteBatch.End();

            // Resets all values for new game
            if (!player1 || !player2)
            {
                LoadContent();
            }
        }
    }
}