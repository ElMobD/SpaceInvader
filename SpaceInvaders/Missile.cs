using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SpaceInvaders
{
    class Missile : SimpleObject
    {
        private double vitesse;

        public Missile(double posX, double posY, double vitesse, Side side, Bitmap image) : base(side)
        {
            this.lives = image.Width*image.Height/2;
            this.image = image;
            this.position = new Vecteur2D(posX, posY);
            this.vitesse = vitesse;
            Side = side;
        }
        public override void Update(Game gameInstance, double deltaT)
        {
            this.position.LaPositionY += vitesse* deltaT;
            if (this.position.LaPositionY > gameInstance.gameSize.Height || this.position.LaPositionY <= -image.Height)
                this.lives = 0;
            
            foreach (GameObject gameObject in gameInstance.gameObjects)
            {
                if(gameObject != this)          
                    gameObject.Collision(this, gameInstance);      
            }
        }
        protected override void OnCollision(Missile m, int numberOfPixelsInCollision, Game gameInstance)
        {
            this.lives = 0;
            m.lives = 0;
        }
        public override Side Side
        {
            get { return side; }
            set { side = value; }
        }
    }
}
