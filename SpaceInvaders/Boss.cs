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
        public Boss(int lives, double viewWidth, double viewHeight, Bitmap image, Side side, ColorMatrix colorMatrix) : base(lives, viewWidth, viewHeight, image, side, colorMatrix)
        {
            
        }
        public override void Update(Game gameInstance, double deltaT){
            
        }
        public void DrawBossLives(Game gameInstance, Graphics graphics)
        {
            Console.WriteLine("Draw du boss");
        }
        public new void Shoot(Game gameInstance, int vitesse, Side side)
        {
            missile = new Missile(image.Width / 2 + position.LaPositionX, position.LaPositionY, vitesse, 
                side, SpaceInvaders.Properties.Resources.shoot1, colorMatrix != null ? colorMatrix:TheColorObject(Color.Red));
            gameInstance.AddNewGameObject(missile);
        }
    }
}
