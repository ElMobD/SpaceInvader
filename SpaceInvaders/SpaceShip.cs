using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace SpaceInvaders
{
    class SpaceShip : SimpleObject
    {
        /// Tous les champs d'instances de la classe SpaceShip 
        protected double speedPixelPerSecond = 200;
        protected Missile missile;

        /// Tous les constructeurs de la classe SpaceShip  <summary>
        public SpaceShip(int lives, double viewWidth, double viewHeight, Bitmap image, Side side) : base(side) 
        {
            this.position = new Vecteur2D(viewWidth, viewHeight);
            this.lives = lives;
            this.image = image;
            Side = side;
        } 
        public SpaceShip(int lives, double viewWidth, double viewHeight, Bitmap image, Side side, ColorMatrix colorMatrix) : this(lives, viewWidth, viewHeight, image, side)
        {
            this.colorMatrix = colorMatrix;
        }

        /// Toutes les méthodes et propriétés de la classe SpaceShip
        public override void Update(Game gameInstance, double deltaT){}
        public void Shoot(Game gameInstance,int vitesse, Side side)
        {
            if (missile == null || missile.IsAlive() == false)
            {
                // Pour le décalage du missile par rapport au spaceShip
                if (side == Side.Ally)
                {
                    missile = new Missile(image.Width / 2 + position.LaPositionX, position.LaPositionY - SpaceInvaders.Properties.Resources.shoot1.Height, vitesse, side, SpaceInvaders.Properties.Resources.shoot1, colorMatrix!=null ? colorMatrix : TheColorObject(Color.White));;
                    gameInstance.AddNewGameObject(missile);
                    //AudioSfx.PlaySound(SpaceInvaders.Properties.Resources.sfx_shoot_1);
                }
                else if(side == Side.Enemy || side == Side.Boss)
                {
                    missile = new Missile(image.Width / 2 + position.LaPositionX, position.LaPositionY+image.Height, vitesse, side, SpaceInvaders.Properties.Resources.shoot1, colorMatrix != null ? colorMatrix : TheColorObject(Color.Red));
                    gameInstance.AddNewGameObject(missile);
                    //AudioSfx.PlaySound(SpaceInvaders.Properties.Resources.sfx_shoot_2);
                }
                
            }
        }
        protected override void OnCollision(Missile m, int numberOfPixelsInCollision, Game gameInstance)
        {
            this.Lives--;
            m.Lives = 0;
        }
        public override Side Side
        {
            get { return side; }
            set { side = value; }
        }
    }
}
