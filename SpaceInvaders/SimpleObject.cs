using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    abstract class SimpleObject : GameObject
    {
        public Bitmap image;
        public Vecteur2D position;
        public int lives;

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
        public bool IsRectangleDisjoint(SimpleObject g, Missile m)
        {
            double x1 = g.position.LaPositionX;
            double y1 = g.position.LaPositionY;
            double lx1 = g.image.Width;
            double ly1 = g.image.Height;

            double x2 = m.position.LaPositionX;
            double y2 = m.position.LaPositionY;
            double lx2 = m.image.Width;
            double ly2 = m.image.Height;

            bool sontDisjoints = (x1 + lx1 < x2) || (x2 + lx2 < x1) || (y1 + ly1 < y2) || (y2 + ly2 < y1);

            if (sontDisjoints)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void Collision(Missile m)
        {
            
        }
        public int Lives
        {
            get { return this.lives; }
        }
    }
}
