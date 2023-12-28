using NAudio.Gui;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
        protected ColorMatrix colorMatrix;

        public SimpleObject(Side side, ColorMatrix colorMatrix) :base(side)
        {
            this.colorMatrix = colorMatrix;
        }
        public SimpleObject(Side side) : base(side)
        {
            colorMatrix = null;
        }
        protected abstract void OnCollision(Missile m, int numberOfPixelsInCollision, Game gameInstance);
        public override void Draw(Game gameInstance, Graphics graphics)
        {      
            if (colorMatrix != null)
            {
                ImageAttributes imageAttributes = new ImageAttributes();
                imageAttributes.SetColorMatrix(colorMatrix);
                graphics.DrawImage(this.image,
                                   new Rectangle((int)this.position.LaPositionX, (int)this.position.LaPositionY, this.image.Width, this.image.Height),
                                   0, 0, this.image.Width, this.image.Height,
                                   GraphicsUnit.Pixel,
                                   imageAttributes);
            }
            else
            {
                graphics.DrawImage(image, (float)position.LaPositionX, (float)position.LaPositionY, image.Width, image.Height);
            }
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
        protected ColorMatrix TheColorObject(Color couleur)
        {
            float r = couleur.R / 255f;
            float g = couleur.G / 255f;
            float b = couleur.B / 255f;

            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            {
               new float[] {0, 0, 0, 0, 0},
               new float[] {0, 0, 0, 0, 0},
               new float[] {0, 0, 0, 0, 0},
               new float[] {0, 0, 0, 1, 0},
               new float[] {r, g, b, 0, 1}
            });
            return colorMatrix;
        }
    }
}
