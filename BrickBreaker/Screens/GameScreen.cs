/*  Created by: Steven HL
 *  Project: Brick Breaker
 *  Date: Tuesday, April 4th
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Xml;
using System.Threading;

namespace BrickBreaker
{
    public partial class GameScreen : UserControl
    {
        #region global values

        //player1 button control keys - DO NOT CHANGE
        Boolean leftArrowDown, rightArrowDown, spaceKeyDown, escDown, gamePaused, holding;

        // Game values

        int currentLevel = 1;
        string level, levelName;
        public static int lives, score, scoreMult;
        public static int powerupSpeed = 2;
        public static double lastPower = 0;
        public static int bSpeedMult = 1;
        public static int pSpeedMult = 1;
        Font scoreFont = new Font("OCR A std", 14, FontStyle.Regular);
        SolidBrush scoreBrush = new SolidBrush(Color.Cyan);

        // Paddle and Ball objects
        Paddle paddle;
        Ball ball;

        // list of all blocks for current level
        List<Block> blocks = new List<Block>();
        List<Ball> ballList = new List<Ball>();
        List<PowerUp> powers = new List<PowerUp>();

        // Brushes
        SolidBrush paddleBrush = new SolidBrush(Color.White);
        SolidBrush ballBrush = new SolidBrush(Color.White);

        //Random number gen
        Random randGen = new Random();
        int powerValue;

        #endregion

        public GameScreen()
        {
            InitializeComponent();
            OnStart();
        }


        public void OnStart()
        {
            //reset score
            lives = 5;
            score = 0;
            scoreMult = 1;

            //set all button presses to false.
            leftArrowDown = rightArrowDown = escDown = gamePaused = false;
            holding = true;

            // setup starting paddle values and create paddle object
            int paddleWidth = 80;
            int paddleHeight = 20;
            int paddleX = ((this.Width / 2) - (paddleWidth / 2));
            int paddleY = (this.Height - paddleHeight) - 60;
            int paddleSpeed = 8;
            paddle = new Paddle(paddleX, paddleY, paddleWidth, paddleHeight, paddleSpeed, Color.White);

            // setup starting ball values
            int ballX = this.Width / 2 - 10;
            int ballY = this.Height - paddle.height - 80;

            // Creates a new ball           
            int xSpeed = 6;
            int ySpeed = 6;
            int ballSize = 20;
            ball = new Ball(ballX, ballY, xSpeed, ySpeed, ballSize);
            ballList.Add(ball);


            NewLevel();

            // start the game engine loop
            gameTimer.Enabled = true;
        }

        public void NewLevel()
        {
            LevelLoad(Convert.ToString(currentLevel));
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player 1 button presses
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                case Keys.Escape:
                    if (gamePaused == true)
                    {
                        //restart the game
                        gamePaused = false;
                        gameTimer.Enabled = true;
                    }
                    else
                    {
                        gamePaused = true;
                    }

                    //TODO: change screen
                    break;
                case Keys.Space:
                    spaceKeyDown = true;
                    break;
                default:
                    break;
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player 1 button release
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                case Keys.Space:
                    spaceKeyDown = false;
                    break;
                default:
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //pause the game
            if (gamePaused == true)
            {
                gameTimer.Enabled = false;
            }

            // Move the paddle
            if (leftArrowDown && paddle.x > 0)
            {
                paddle.Move("left");
            }
            if (rightArrowDown && paddle.x < (this.Width - paddle.width))
            {
                paddle.Move("right");
            }

            if (escDown == true)
            {
                gamePaused = !gamePaused;
            }

            if (holding)
            {
                ballList[0].x = (paddle.x + (paddle.width / 2)) - ballList[0].size / 2;
                ballList[0].y = (this.Height - paddle.height) - 85;

                //If space bar pressed, release ball
                if (spaceKeyDown)
                {
                    holding = false;
                }
            }
            else if (!holding)
            {
                // Move ball
                ball.Move();
            }
            //Move powerups
            foreach (PowerUp p in powers)
            {
                p.Move();
            }

            // Check for collision with top and side walls
            ball.WallCollision(this);

            // Check for ball hitting bottom of screen
            foreach (Ball b in ballList)
            {
                if (ballList.Count() < 1)
                {
                    if (b.BottomCollision(this))
                    {
                        ballList.Remove(b);
                    }
                }

                if (ballList.Count() == 1)
                {
                    if (b.BottomCollision(this))
                    {
                        lives--;

                        // Moves the ball back to origin
                        b.x = ((paddle.x - (ball.size / 2)) + (paddle.width / 2));
                        b.y = (this.Height - paddle.height) - 85;
                        b.xSpeed = 6;
                        b.ySpeed = 6;
                        b.size = 20;

                        Refresh();
                        holding = true;

                        if (lives == 0)
                        {
                            gameTimer.Enabled = false;
                            OnEnd();
                        }
                    }
                }
            }


            if (ballList.Count() == 0)
            {
                lives--;

                // Moves the ball back to origin
                int ballX = ((paddle.x - (ball.size / 2)) + (paddle.width / 2));
                int ballY = (this.Height - paddle.height) - 85;
                int xSpeed = 6;
                int ySpeed = 6;
                int ballSize = 20;

                ball = new Ball(ballX, ballY, xSpeed, ySpeed, ballSize);
                ballList.Add(ball);
                holding = true;

                if (lives == 0)
                {
                    gameTimer.Enabled = false;
                    OnEnd();
                }
            }

            // Check for collision of ball with paddle, (incl. paddle movement)
            ball.PaddleCollision(paddle, leftArrowDown, rightArrowDown);

            // Check if ball has collided with any blocks
            foreach (Block b in blocks)
            {
                if (ball.BlockCollision(b))
                {
                    --b.hp;
                    //blocks.Remove(b);
                    int blockX = b.x;
                    int blockY = b.y + b.height;
                    int blockSize = b.width;

                    if (b.hp == 0)
                    {
                        blocks.Remove(b);

                        score = score + 100*scoreMult;
                        double d = score / 500;
                        double scoreint = Math.Round(d);
                        
                        if (scoreint > lastPower)
                        {
                            lastPower = scoreint;
                            NumberGen();
                            int powertype = powerValue;

                            PowerUp power = new PowerUp(blockSize / 2 + blockX, blockY, powerupSpeed, 15, powertype);
                            powers.Add(power);
                        }                        

                    }

                    if (blocks.Count == 0)
                    {
                        currentLevel++;
                        NewLevel();
                        holding = true;
                    }

                    break;
                }
            }

            //redraw the screen
            Refresh();
        }


        private void LevelLoad(string levelNo)
        {
            XmlReader brickReader = XmlReader.Create("Resources/Level"+levelNo+".xml");   

            while (brickReader.Read())
            {
                Block b = new Block(0, 0, 0);
                string x, y, hp;

                brickReader.ReadToFollowing("brick");
                x = brickReader.GetAttribute("x");
                y = brickReader.GetAttribute("y");
                hp = brickReader.GetAttribute("hp");

                b.x = Convert.ToInt16(x);
                b.y = Convert.ToInt16(y);
                b.hp = Convert.ToInt16(hp);

                if (b.hp != 0)
                {
                    blocks.Add(b);
                }
                
            }
            brickReader.Close();
        }
        
        public void OnEnd()

        {    
            //MenuScreen ps = new MenuScreen();
            Form1.ChangeScreen(this, "HighScreen");

        }

        public void NumberGen()
        {          
            powerValue = randGen.Next(1, 5);
        }

        public void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //Draw Paddle
            e.Graphics.DrawImage(Properties.Resources.Player1, paddle.x, paddle.y);

            //Draw Ball
            e.Graphics.DrawImage(Properties.Resources.ball, ball.x, ball.y);

            // Draws blocks
            foreach (Block b in blocks)
            {
                if (b.hp == 1)
                {
                    e.Graphics.DrawImage(Properties.Resources.greenBrick2, b.x, b.y);
                }
                else if (b.hp == 2)
                {
                    e.Graphics.DrawImage(Properties.Resources.blueBrick2, b.x, b.y);
                }
                else if (b.hp == 3)
                {
                    e.Graphics.DrawImage(Properties.Resources.purpBrick2, b.x, b.y);
                }
                else if (b.hp == 4)
                {
                    e.Graphics.DrawImage(Properties.Resources.redBrick2, b.x, b.y);
                }
            }

            // Draws powerups
            foreach (PowerUp p in powers)
            {
                SolidBrush powerBrush = new SolidBrush(p.UpdateColour());
                e.Graphics.FillRectangle(powerBrush, p.x, p.y, p.size, p.size);
            }

            //draws score
            e.Graphics.DrawString("Score: " + score, scoreFont, scoreBrush, 0, 25);

            //draw lives


            e.Graphics.DrawString("Lives: " + lives, scoreFont, scoreBrush, this.Width - 140, 25);

        }

    }


}
