using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;


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
                }else if(side == Side.Enemy || side == Side.Boss)
                {
                    missile = new Missile(image.Width / 2 + position.LaPositionX, position.LaPositionY+image.Height, vitesse, side, SpaceInvaders.Properties.Resources.shoot1, colorMatrix);
                    gameInstance.AddNewGameObject(missile);
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
