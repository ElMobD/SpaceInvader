using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace SpaceInvaders
{
    class Boss : SpaceShip
    {
        public Boss(int lives, double viewWidth, double viewHeight, Bitmap image, Side side) : base(lives, viewWidth, viewHeight, image, side)
        {

        }
        public override void Update(Game gameInstance, double deltaT) 
        {
            
        }
        public new void Shoot(Game gameInstance, int vitesse, Side side)
        {
            if (missile == null || missile.IsAlive() == false)
            {
                
            }
        }
    }
}
