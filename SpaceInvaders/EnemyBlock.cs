using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SpaceInvaders
{
    class EnemyBlock : GameObject
    {
        //Champs
        private HashSet<SpaceShip> enemyShips;
        private int baseWidth;
        public Vecteur2D position;
        private int lives;
        private Size size;



        public EnemyBlock(Vecteur2D position, int baseWidth)
        {
            this.position = position;
            this.baseWidth = baseWidth;
            size = new Size (baseWidth, 0);
            enemyShips = new HashSet<SpaceShip>();
        }
        public Size Size
        {
            get { return size; } set { size = value; }
        }
        public Vecteur2D Position 
        { 
            get { return position; } set {  position = value; } 
        }
        public void AddLine(int nbShips, int nbLives, Bitmap shipImage)
        {

            double intervale = (Size.Width - shipImage.Width) / (nbShips - 1);
            for (int i = 0; i<nbShips; i++)
            {
                //Calcul de la position du vaisseau dans la ligne
                double x = Position.LaPositionX + i * intervale;
                double y = Position.LaPositionY + size.Height * 1.2;

                //Ajout du vaisseau dans l'ensemble du bloc
                SpaceShip newShip = new SpaceShip(nbLives, x, y, shipImage);
                enemyShips.Add(newShip);
            }
            UpdateSize();
            //Console.WriteLine(shipImage.Height);
        }
        public void UpdateSize()
        {
            if (enemyShips.Count > 0)
            {
                double minY = enemyShips.Min(ship => ship.position.LaPositionY);
                double maxY = enemyShips.Max(ship => ship.position.LaPositionY + ship.image.Height);
                double minX = enemyShips.Min(ship => ship.position.LaPositionX);
                double maxX = enemyShips.Max(ship => ship.position.LaPositionX + ship.image.Width);
                size = new Size((int)(maxX - minX), (int)(maxY - minY));
            }
            else
            {
                size = new Size(size.Width, 0);
            }
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            //Console.WriteLine(size);
            this.position.LaPositionX += 1;
            Console.WriteLine(this.Position.LaPositionX);
        }
        public override void Draw(Game gameInstance, Graphics graphics)
        {
            foreach (SpaceShip enemyShip in enemyShips)
            {
                enemyShip.Draw(gameInstance, graphics);
            }
        }
        public override bool IsAlive()
        {
            if (enemyShips.Count >= 0) return true;
            else return false;
        }
        public override void Collision(Missile m)
        {

        }
    }
}
