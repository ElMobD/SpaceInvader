using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SpaceInvaders
{
    class Explosion : GameObject
    {
        private Vecteur2D position;
        private Bitmap image;
        private int lives;
        public Explosion(Side side, Vecteur2D position, Bitmap image, int lives) : base(side)
        {
            this.side = side;
            this.position = position;
            this.image = image;
            this.lives = lives;
        }
        public override void Update(Game gameInstance, double deltaT)
        {
            this.lives--;
        }
        public override void Draw(Game gameInstance, Graphics graphics) {
            graphics.DrawImage(this.image, (float)this.position.LaPositionX, (float)this.position.LaPositionY, this.image.Width, this.image.Height);
        }
        public override bool IsAlive()
        {
            if (lives > 0) return true;
            return false;
        }
        public override void Collision(Missile m, Game gameInstance) { }
        public override Side Side { get; set; }
        public int Live
        {
            get { return lives; }
            set {}
        }
    }
}
