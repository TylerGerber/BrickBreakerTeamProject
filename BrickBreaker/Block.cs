using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BrickBreaker
{
    public class Block
    {
        public int width = 50;
        public int height = 25;

        public int x;
        public int y; 
        public int hp;
        public Color colour;
        public Rectangle rightRec, leftRec, topRec, bottomRec;

        public static Random rand = new Random();

        public Block(int _x, int _y, int _hp)
        {
            x = _x;
            y = _y;
            hp = _hp;
            leftRec = new Rectangle(x, y, 1, height);
            rightRec = new Rectangle(x + width, y, 1, height);
            topRec = new Rectangle(x, y, width, 1);
            bottomRec = new Rectangle(x, y + height, width, 1);
        }
        public Color UpdateColour()
        {
            switch (hp)
            {
                case 1:
                     return Color.Red;
                    
                case 2:
                    return Color.Yellow;
                    
                case 3:
                    return Color.Green;
                   
                case 4:
                    return Color.Cyan;
                    
            }
            return Color.Pink;
        }
    }
} 