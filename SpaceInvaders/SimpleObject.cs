using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;

namespace SpaceInvaders
{
    abstract class SimpleObject : GameObject
    {
        public Bitmap image;
        public Vecteur2D position;
        public int lives;
        private Vecteur2D pixelOnObject;

        public SimpleObject(Side side) :base(side){}
        protected abstract void OnCollision(Missile m, int numberOfPixelsInCollision, Game gameInstance);
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

            if (sontDisjoints) return true;
            else return false;
        }
        public override void Collision(Missile m, Game gameInstance)
        {
            if (!IsRectangleDisjoint(this, m))
            {
                double objectX = this.position.LaPositionX;
                double objectY = this.position.LaPositionY;
                double missileX = m.position.LaPositionX;
                double missileY = m.position.LaPositionY;

                for (int i = 0; i < m.image.Width; i++)
                {
                    for (int j = 0; j < m.image.Height; j++)
                    {
                        if (ChangementRepere(i, j, missileX, missileY, objectX, objectY))
                        {
                            Color pixelColor = image.GetPixel((int)pixelOnObject.LaPositionX, (int)pixelOnObject.LaPositionY);
                            if (pixelColor.R == 0 && pixelColor.G == 0 && pixelColor.B == 0)
                                CollisionOnPixel(m, gameInstance);                                          
                        }  
                    }
                }
            }
        }
        private void CollisionOnPixel(Missile m, Game gameInstance)
        {
            if (m.Side != this.Side)
            {
                Vecteur2D pos = new Vecteur2D(m.position.LaPositionX - SpaceInvaders.Properties.Resources.hit2.Width / 2, m.position.LaPositionY - SpaceInvaders.Properties.Resources.hit2.Height / 2);
                Explosion hit = new Explosion(Side.Decor, pos, SpaceInvaders.Properties.Resources.hit2, 20);
                gameInstance.AddNewGameObject(hit);
                if (this.GetType() == typeof(Bunker))
                {
                    Color newColor = Color.FromArgb(0, 255, 255, 255);
                    image.SetPixel((int)pixelOnObject.LaPositionX, (int)pixelOnObject.LaPositionY, newColor);
                }
                if (m.IsAlive())
                {
                    OnCollision(m, 1, gameInstance);
                }
            }
        }
        private bool ChangementRepere(int i, int j, double missileX, double missileY, double objectX, double objectY)
        {
            int missilePixelScreenX = (int)(missileX + i);
            int missilePixelScreenY = (int)(missileY + j);
            int missilePixelOtherX = (int)(missilePixelScreenX - objectX);
            int missilePixelOtherY = (int)(missilePixelScreenY - objectY);
            this.pixelOnObject = new Vecteur2D(missilePixelOtherX, missilePixelOtherY);
            if (missilePixelOtherX >= 0 && missilePixelOtherX < this.image.Width && missilePixelOtherY >= 0 && missilePixelOtherY < this.image.Height)
                return true;
            return false;
        }
        public int Lives
        {
            get { return this.lives; }
            set { this.lives = value; }
        }
    }
}
