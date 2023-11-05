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
        protected double speedPixelPerSecond = 1;
        private Missile missile; 

        /// Tous les constructeurs de la classe SpaceShip  <summary>
        public SpaceShip(int lives, int viewWidth, int viewHeight)
        {
            this.position = new Vecteur2D(viewWidth, viewHeight);
            this.lives = lives;
            this.image = SpaceInvaders.Properties.Resources.ship3;
        }

        /// Toutes les méthodes et propriétés de la classe SpaceShip
        public override void Update(Game gameInstance, double deltaT){}
        public void Shoot(Game gameInstance)
        {
            if (missile == null || missile.IsAlive() == false)
            {
                missile = new Missile(this.image.Width/2+this.position.LaPositionX, this.position.LaPositionY- SpaceInvaders.Properties.Resources.shoot1.Height, -2);
                gameInstance.AddNewGameObject(missile);
            }
        }
    }
}
