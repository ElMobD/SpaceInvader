using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class PlayerSpaceShip : SpaceShip
    {
        public PlayerSpaceShip(int lives, int viewWidth, int viewHeight) : base(lives, viewWidth, viewHeight){}
        public override void Update(Game gameInstance, double deltaT)
        {
            if (gameInstance.keyPressed.Contains(Keys.Right))
            {
                if (this.position.LaPositionX + image.Width < gameInstance.gameSize.Width)
                    this.position.LaPositionX += speedPixelPerSecond;
            }
            else if (gameInstance.keyPressed.Contains(Keys.Left))
            {
                if (0 < this.position.LaPositionX)
                    this.position.LaPositionX -= speedPixelPerSecond;
            }

            if (gameInstance.keyPressed.Contains(Keys.Space))
            {
                this.Shoot(gameInstance);
            }
        }
    }
}
