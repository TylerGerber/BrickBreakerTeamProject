using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BrickBreaker
{
    class PowerUp
    {
        public int powerValue;
        public int x, y, powerSpeed, size;
        public Color colour;

        // create a random number generator 
       

        public PowerUp(int _x, int _y, int _powerSpeed, int _size, int _powerValue)
        {
            x = _x;
            y = _y;
            powerSpeed = _powerSpeed;
            size = _size;
            powerValue = _powerValue;
        }

        public void PowerUpCollision(Paddle p)
        {
            Rectangle powerUpRec = new Rectangle(x, y, size, size);
            Rectangle paddleRec = new Rectangle(p.x, p.y, p.width, p.height);

            if (powerUpRec.IntersectsWith(paddleRec))
            {
                switch (powerValue)
                {
                    case 1:
                        GameScreen.bSpeedMult = GameScreen.bSpeedMult + 5;
                        break;
                    case 2:
                        GameScreen.pSpeedMult = GameScreen.pSpeedMult + 5;
                        break;
                    case 3:
                        GameScreen.scoreMult = GameScreen.scoreMult + 5;
                        break;
                    case 4:
                        GameScreen.score = GameScreen.score + 2;
                        break;
                }
            }
        }

        public Color UpdateColour()
        {
           return Color.Blue;
        }

        //move the power up down the screen
        public void Move()
        {
            y = y + powerSpeed;
        }
    }
}
