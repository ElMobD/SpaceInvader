using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class Missile : SimpleObject
    {
        private double vitesse;

        public Missile(double posX, double posY, double vitesse)
        {
            //this.lives = SpaceInvaders.Properties.Resources.shoot1.Height * SpaceInvaders.Properties.Resources.shoot1.Width;
            this.lives = 10;
            this.image = SpaceInvaders.Properties.Resources.shoot1;
            this.position = new Vecteur2D(posX, posY);
            this.vitesse = vitesse;
        }
        public override void Update(Game gameInstance, double deltaT)
        {
            this.position.LaPositionY += vitesse;
            if (this.position.LaPositionY > gameInstance.gameSize.Height || this.position.LaPositionY <= -image.Height)
            {
                lives = 0;
            }
            foreach (GameObject gameObject in gameInstance.gameObjects)
            {
                if(gameObject != this)
                {
                    gameObject.Collision(this);
                }
            }
        }
        protected override void OnCollision(Missile m, int numberOfPixelsInCollision)
        {
            Console.WriteLine("2 missiles sont entrés en collision.");
            this.lives = 0;
            m.lives = 0;
        }
    }
}
