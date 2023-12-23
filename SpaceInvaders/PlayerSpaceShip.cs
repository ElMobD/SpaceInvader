using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class PlayerSpaceShip : SpaceShip
    {
        public PlayerSpaceShip(int lives, int viewWidth, int viewHeight, Bitmap image, Side side) : base(lives, viewWidth, viewHeight, image, side){}
        public PlayerSpaceShip(int lives, int viewWidth, int viewHeight, Bitmap image, Side side, ColorMatrix colorMatrix) : base(lives, viewWidth, viewHeight, image, side, colorMatrix) { }
        public override void Update(Game gameInstance, double deltaT)
        {
            if (gameInstance.keyPressed.Contains(Keys.Right))
            {
                if (this.position.LaPositionX + image.Width < gameInstance.gameSize.Width)
                    this.position.LaPositionX += speedPixelPerSecond*deltaT;
            }
            else if (gameInstance.keyPressed.Contains(Keys.Left))
            {
                if (0 < this.position.LaPositionX)
                    this.position.LaPositionX -= speedPixelPerSecond*deltaT;
            }
            if (gameInstance.keyPressed.Contains(Keys.Space))
                this.Shoot(gameInstance, -600,Side.Ally);
        }
        public override void Draw(Game gameInstance, Graphics graphics)
        {
            base.Draw(gameInstance, graphics);
            Bitmap heart;
            int spacing = 0;
            for(int i = 0; i<this.lives; i++)
            {
                heart = SpaceInvaders.Properties.Resources.heart2;
                graphics.DrawImage(heart, 20+spacing, gameInstance.gameSize.Height - 30, heart.Width, heart.Height);
                spacing += 25;
            }
        }
    }
}
