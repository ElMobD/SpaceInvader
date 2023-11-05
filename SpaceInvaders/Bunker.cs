using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class Bunker : SimpleObject
    {
        public Bunker(Vecteur2D position) { 
            this.position = position;
            this.image = SpaceInvaders.Properties.Resources.bunker;
            this.lives = 1;
        }
        public override void Update(Game gameInstance, double deltaT){}
        public override void Collision(Missile m)
        {
            if (!IsRectangleDisjoint(this, m))
            {
                double bunkerX = this.position.LaPositionX;
                double bunkerY = this.position.LaPositionY;

                double missileX = m.position.LaPositionX;
                double missileY = m.position.LaPositionY;

                for(int i = 0; i< m.image.Width; i++)
                {
                    for(int j = 0; j< m.image.Height; j++)
                    {
                        int missilePixelScreenX = (int)(missileX + i);
                        int missilePixelScreenY = (int)(missileY + j);

                        int missilePixelOtherX = (int)(missilePixelScreenX - bunkerX);
                        int missilePixelOtherY = (int)(missilePixelScreenY - bunkerY);

                        //Console.WriteLine(missilePixelOtherX + " , " + missilePixelOtherY);

                        if(missilePixelOtherX >= 0 && 
                            missilePixelOtherX < this.image.Width && 
                            missilePixelOtherY >= 0 && missilePixelOtherY < this.image.Height)
                        {
                            //Console.WriteLine(missilePixelOtherX + " , " + missilePixelOtherY);
                            Color pixelColor = image.GetPixel(missilePixelOtherX, missilePixelOtherY);
                            if (pixelColor.R == 0 && pixelColor.G == 0 && pixelColor.B == 0)
                            {
                                Color newColor = Color.FromArgb(0, 255, 255, 255);
                                image.SetPixel(missilePixelOtherX, missilePixelOtherY, newColor);
                                m.lives--;
                            }
                        }
                    }
                }
            }
        }
    }
}
