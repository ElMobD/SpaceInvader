using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class Missile : GameObject
    {
        private Vecteur2D position;
        private double vitesse;
        private int lives;
        private Bitmap image;

        public Missile(double posX, double posY, double vitesse)
        {
            this.lives = 1;
            this.image = SpaceInvaders.Properties.Resources.shoot1;
            this.position = new Vecteur2D(posX, posY);
            this.vitesse = vitesse;
        }

        public Vecteur2D Position
        {
            get 
            {  
                return this.position; 
            }
        }
        public double Vitesse
        {
            get
            {
                return this.vitesse;
            }
        }
        public int Lives
        {
            get
            {
                return this.lives;
            }
        }
        public Bitmap Image
        {
            get
            {
                return this.image;
            }
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            this.position.LaPositionY += vitesse;
            if (this.position.LaPositionY > gameInstance.gameSize.Height || this.position.LaPositionY <= -image.Height)
            {
                lives = 0;
            }
        }
        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawImage(this.image, (float)this.Position.LaPositionX, (float)this.Position.LaPositionY, this.image.Width, this.image.Height);
        }
        public override bool IsAlive()
        {
            if (lives > 0) return true;
            return false;
        }
    }
}
