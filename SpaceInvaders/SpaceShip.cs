using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace SpaceInvaders
{
    class SpaceShip : GameObject
    {
        /// Tous les champs d'instances de la classe SpaceShip 
        private double speedPixelPerSecond = 1.5;
        private Vecteur2D position;
        private int lives;
        private Bitmap image;


        /// Tous les constructeurs de la classe SpaceShip  <summary>
        public SpaceShip(int lives, int viewWidth, int viewHeight)
        {
            this.position = new Vecteur2D(viewWidth, viewHeight);
            this.lives = lives;
            this.image = SpaceInvaders.Properties.Resources.ship3;
        }

        /// Toutes les méthodes et propriétés de la classe SpaceShip
        public override void Update(Game gameInstance, double deltaT)
        {
            Console.WriteLine(deltaT);
            if (gameInstance.keyPressed.Contains(Keys.Right))
            {
                if(this.position.LaPositionX + SpaceInvaders.Properties.Resources.ship3.Width < gameInstance.gameSize.Width)
                    this.position.LaPositionX += speedPixelPerSecond; 
            }
            else if(gameInstance.keyPressed.Contains(Keys.Left))
            {
                if (0 < this.position.LaPositionX)
                    this.position.LaPositionX -= speedPixelPerSecond;
            }
        }
        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawImage(this.image, (float)this.Position.LaPositionX, (float)this.Position.LaPositionY, this.image.Width, this.image.Height);
        }
        public override bool IsAlive()
        {
            if(lives > 0) return true;
            return false;
        }
        public Vecteur2D Position
        {
            get
            {
                return this.position;
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
    }
}
