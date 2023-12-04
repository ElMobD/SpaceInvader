using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace SpaceInvaders
{
    class SpaceShip : SimpleObject
    {
        /// Tous les champs d'instances de la classe SpaceShip 
        protected double speedPixelPerSecond = 200;
        private Missile missile;

        /// Tous les constructeurs de la classe SpaceShip  <summary>
        public SpaceShip(int lives, double viewWidth, double viewHeight, Bitmap image, Side side) : base(side)
        {
            this.position = new Vecteur2D(viewWidth, viewHeight);
            this.lives = lives;
            this.image = image;
            Side = side;
        }

        /// Toutes les méthodes et propriétés de la classe SpaceShip
        public override void Update(Game gameInstance, double deltaT){
            
        }
        public void Shoot(Game gameInstance,int vitesse, Side side)
        {
            if (missile == null || missile.IsAlive() == false)
            {
                if(side == Side.Ally)
                {
                    missile = new Missile(this.image.Width / 2 + this.position.LaPositionX, this.position.LaPositionY - SpaceInvaders.Properties.Resources.shoot1.Height, vitesse, side);
                    gameInstance.AddNewGameObject(missile);
                }else if(side == Side.Enemy)
                {
                    missile = new Missile(this.image.Width / 2 + this.position.LaPositionX, this.position.LaPositionY, vitesse, side);
                    gameInstance.AddNewGameObject(missile);
                }
            }
        }
        protected override void OnCollision(Missile m, int numberOfPixelsInCollision, Game gameInstance)
        {
            Vecteur2D pos = new Vecteur2D(m.position.LaPositionX - SpaceInvaders.Properties.Resources.hit1.Width, m.position.LaPositionY - SpaceInvaders.Properties.Resources.hit1.Height);
            Explosion hit = new Explosion(Side.Decor, pos, SpaceInvaders.Properties.Resources.hit1, 10);
            gameInstance.AddNewGameObject(hit);
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
