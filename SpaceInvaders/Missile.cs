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
            this.lives = 1;
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
        }
    }
}
