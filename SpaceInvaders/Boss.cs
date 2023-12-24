using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace SpaceInvaders
{
    class Boss : SpaceShip
    {
        private int maxHealth;
        public Boss(int lives, double viewWidth, double viewHeight, Bitmap image, Side side) : base(lives, viewWidth, viewHeight, image, side)
        {
            maxHealth = lives;
        }
        public Boss(int lives, double viewWidth, double viewHeight, Bitmap image, Side side, ColorMatrix colorMatrix) : base(lives, viewWidth, viewHeight, image, side, colorMatrix)
        {
            maxHealth = lives;
        }
        public override void Update(Game gameInstance, double deltaT){}
        public new void Draw(Game gameInstance, Graphics graphics)
        {
            base.Draw(gameInstance, graphics);
            double largeur = 10;
            double longueur = image.Width;
            float pourcentage = (float)lives / (float)maxHealth;
            Color healthColor = GetHealthColor(pourcentage);
            DrawRec(largeur, longueur, graphics, healthColor, pourcentage);
        }
        private Color GetHealthColor(float pourcentage)
        {
            if (pourcentage > 0.6) return Color.Green;
            else if (pourcentage > 0.3) return Color.Yellow;
            else return Color.Red;
        }
        private void DrawRec(double largeur, double longueur, Graphics g, Color color, float pourcentage)
        {
            Pen pen = new Pen(Color.White, 2);
            g.DrawRectangle(pen, (float)position.LaPositionX, (float)position.LaPositionY-20, (float)longueur, (float)largeur);
            pen.Dispose();
            DrawLife(g, color, (float)position.LaPositionX, (float)position.LaPositionY - 20, pourcentage, (float)longueur, (float)largeur);
        }
        private void DrawLife(Graphics g, Color color, float posX, float posY, float pourcentage, float longueurMax, float largeur)
        {
            float newLongueur = longueurMax;
            newLongueur *= pourcentage; 
            Brush healthBrush = new SolidBrush(color);
            g.FillRectangle(healthBrush, posX, posY, newLongueur, largeur);
        }
        public new void Shoot(Game gameInstance, int vitesse, Side side)
        {
            if (missile == null || missile.IsAlive() == false)
            {
                missile = new Missile(image.Width / 2 + position.LaPositionX, position.LaPositionY + image.Height, vitesse,
                side, SpaceInvaders.Properties.Resources.shoot1, colorMatrix != null ? colorMatrix : TheColorObject(Color.Red));
                gameInstance.AddNewGameObject(missile);

            }
        }
    }
}
