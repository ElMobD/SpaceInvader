using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    abstract class SimpleObject : GameObject
    {
        protected Bitmap image;
        protected Vecteur2D position;
        protected int lives;

        public SimpleObject() { }
        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawImage(this.image, (float)this.position.LaPositionX, (float)this.position.LaPositionY, this.image.Width, this.image.Height);
        }
        public override bool IsAlive()
        {
            if (lives > 0) return true;
            return false;
        }
    }
}
