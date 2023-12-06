using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SpaceInvaders
{
    class EnemyBlock : GameObject
    {
        //Champs
        private HashSet<SpaceShip> enemyShips;
        private int baseWidth;
        public Vecteur2D position;
        private Size size;
        private bool goingRight = true;
        private double speedCoef = 100;
        private double randomShootProbability = 1/3;
        private int descente = 10;
        private int speedCoefAdd = 5;


        public EnemyBlock(Vecteur2D position, int baseWidth, Side side) : base(side)
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
                double y = Position.LaPositionY + size.Height *  1.2;

                //Ajout du vaisseau dans l'ensemble du bloc
                SpaceShip newShip = new SpaceShip(nbLives, x, y, shipImage, Side.Enemy);
                enemyShips.Add(newShip);
            }
            UpdateSize();
        }
        public void UpdateSize()
        {
            if (enemyShips.Count > 0)
            {
                double minY = enemyShips.Min(ship => ship.position.LaPositionY);
                double maxY = enemyShips.Max(ship => ship.position.LaPositionY + ship.image.Height);
                double minX = enemyShips.Min(ship => ship.position.LaPositionX);
                double maxX = enemyShips.Max(ship => ship.position.LaPositionX + ship.image.Width);
                this.Size = new Size((int)(maxX - minX), (int)(maxY - minY));
                this.Position.LaPositionX = minX;
                this.Position.LaPositionY = minY;
            }
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            if(this.position.LaPositionY+ this.Size.Height >= gameInstance.Player.position.LaPositionY)
            {
                gameInstance.Player.Lives = 0;
            }
                
            List<SpaceShip> shipsToRemove = new List<SpaceShip>();
            Random random = new Random();
            if (goingRight)
            {
                if (this.Position.LaPositionX + this.Size.Width < gameInstance.gameSize.Width)
                {
                    this.Position.LaPositionX += speedCoef*deltaT;
                    foreach (SpaceShip enemyShip in enemyShips)
                    {
                        if (enemyShip.IsAlive())
                        {
                            enemyShip.position.LaPositionX += speedCoef * deltaT;
                            double randomValue = random.NextDouble();
                            if (randomValue < randomShootProbability * deltaT)
                            {
                                enemyShip.Shoot(gameInstance, 400, enemyShip.Side);
                            }
                        }
                        else
                            shipsToRemove.Add(enemyShip);
                    }
                }
                else
                {
                    goingRight = false;
                    speedCoef += speedCoefAdd; // Augmentez la vitesse du bloc
                    this.Position.LaPositionY += descente; // Faire descendre le bloc
                    randomShootProbability += 0.1;
                    foreach (SpaceShip enemyShip in enemyShips)
                    {
                        enemyShip.position.LaPositionY += descente;
                    }
                }
            }
            else if(!goingRight)
            {
                if (this.position.LaPositionX > 0)
                {
                    this.Position.LaPositionX += -speedCoef*deltaT;
                    foreach (SpaceShip enemyShip in enemyShips)
                    {
                        if (enemyShip.IsAlive())
                        {
                            enemyShip.position.LaPositionX += -speedCoef * deltaT;
                            double randomValue = random.NextDouble();
                            if (randomValue < randomShootProbability * deltaT)
                            {
                                enemyShip.Shoot(gameInstance, 400, enemyShip.Side);
                            }
                        }
                        else
                            shipsToRemove.Add(enemyShip);
                    }
                }
                else
                {
                    goingRight = true;
                    speedCoef += speedCoefAdd; // Augmentez la vitesse du bloc
                    this.Position.LaPositionY += descente; // Faire descendre le bloc
                    randomShootProbability += 0.1;
                    foreach (SpaceShip enemyShip in enemyShips)
                    {
                        enemyShip.position.LaPositionY += descente;
                    }
                }
            }
            foreach (SpaceShip shipToRemove in shipsToRemove)
            {
                enemyShips.Remove(shipToRemove);
            }
            UpdateSize();
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
            if (enemyShips.Count <= 0) 
                return false;
            else 
                return true;
        }
        public bool IsRectangleDisjoint(SpaceShip s, Missile m)
        {
            double x1 = s.position.LaPositionX;
            double y1 = s.position.LaPositionY;
            double lx1 = s.image.Width;
            double ly1 = s.image.Height;

            double x2 = m.position.LaPositionX;
            double y2 = m.position.LaPositionY;
            double lx2 = m.image.Width;
            double ly2 = m.image.Height;

            bool sontDisjoints = (x1 + lx1 < x2) || (x2 + lx2 < x1) || (y1 + ly1 < y2) || (y2 + ly2 < y1);

            if (sontDisjoints)
                return true;
            else
                return false;
        }
        public override void Collision(Missile m, Game gameInstance)
        {
            foreach (SpaceShip enemyShip in enemyShips)
            {
                enemyShip.Collision(m, gameInstance);
            }
        }
        public override Side Side
        {
            get { return side; }
            set { side = value; }
        }
    }
}
