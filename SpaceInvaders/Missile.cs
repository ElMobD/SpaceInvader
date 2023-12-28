using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace SpaceInvaders
{
    class Missile : SimpleObject
    {
        private double vitesse;
        private bool sfxPlayed = false;

        public Missile(double posX, double posY, double vitesse, Side side, Bitmap image) : base(side)
        {
            this.lives = image.Width * image.Height / 2;
            this.image = image;
            this.position = new Vecteur2D(posX, posY);
            this.vitesse = vitesse;
            Side = side;
        }
        public Missile(double posX, double posY, double vitesse, Side side, Bitmap image, ColorMatrix colorMatrix) : this(posX, posY, vitesse, side, image)
        {
            this.colorMatrix = colorMatrix;
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
        public void PlayEffect(Game g, double x, double y)
        {
            if (!sfxPlayed)
            {
                Vecteur2D pos = new Vecteur2D(x, y);
                Explosion hit = new Explosion(Side.Decor, pos, SpaceInvaders.Properties.Resources.hit2, 20);
                AudioSfx.PlaySound(SpaceInvaders.Properties.Resources.sfx_hit_1);
                g.AddNewGameObject(hit);
                sfxPlayed = true;
            }
        }
        public override Side Side
        {
            get { return side; }
            set { side = value; }
        }
    }
}

