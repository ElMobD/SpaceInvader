using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
        private Size size;
        private bool goingRight = true;
        private double speedCoef = 100;
        private double randomShootProbability = 1/3;
        private int descente = 5;
        private int speedCoefAdd = 5;

        public override Side Side
        {
            get { return side; }
            set { side = value; }
        }
        public Size Size
        {
            get { return size; }
            set { size = value; }
        }
        public Vecteur2D Position
        {
            get { return position; }
            set { position = value; }
        }

        public EnemyBlock(Vecteur2D position, int baseWidth, Side side) : base(side)
        {
            this.position = position;
            this.baseWidth = baseWidth;
            size = new Size (baseWidth, 0);
            enemyShips = new HashSet<SpaceShip>();
        }
        private Color GetRandomColor()
        {
            Random random = new Random();

            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);

            Color randomColor = Color.FromArgb(red, green, blue);
            return randomColor;
        }
        private ColorMatrix TheColorObject(Color couleur)
        {
            float r = couleur.R / 255f;
            float g = couleur.G / 255f;
            float b = couleur.B / 255f;

            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            {
               new float[] {0, 0, 0, 0, 0},
               new float[] {0, 0, 0, 0, 0},
               new float[] {0, 0, 0, 0, 0},
               new float[] {0, 0, 0, 1, 0},
               new float[] {r, g, b, 0, 1}
            });
            return colorMatrix;
        }
        public void AddBoss()
        {
            Bitmap imgBoss = SpaceInvaders.Properties.Resources.finalBoss;
            Boss boss = new Boss(100, 0, 50, imgBoss, Side.Boss);
            enemyShips.Add(boss);
        }
        public void AddLine(int nbShips, int nbLives, Bitmap shipImage)
        {
            Color newColor = GetRandomColor();
            double intervale = (Size.Width - shipImage.Width) / (nbShips - 1);
            for (int i = 0; i<nbShips; i++)
            {
                double x = Position.LaPositionX + i * intervale;
                double y = Position.LaPositionY + size.Height *  1.2;
                SpaceShip newShip = new SpaceShip(nbLives, x, y, shipImage, Side.Enemy, TheColorObject(newColor));
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
            CheckPlayerCollision(gameInstance);
            List<SpaceShip> shipsToRemove = new List<SpaceShip>();

            if (goingRight)
            {
                MoveShips(deltaT, 1, gameInstance);
                if (CheckEdgeCollision(gameInstance))
                {
                    HandleDirectionChange();
                    MoveDown();
                }
            }
            else if (!goingRight)
            {
                MoveShips(deltaT, -1, gameInstance);
                if (CheckEdgeCollision(gameInstance))
                {
                    HandleDirectionChange();
                    MoveDown();
                }
            }

            RemoveDeadShips(shipsToRemove);
            UpdateSize();
        }
        private void CheckPlayerCollision(Game gameInstance)
        {
            if (this.position.LaPositionY + this.Size.Height >= gameInstance.Player.position.LaPositionY)
                gameInstance.Player.Lives = 0;
        }
        private void MoveShips(double deltaT, double direction, Game gameInstance)
        {
            this.Position.LaPositionX += direction * speedCoef * deltaT;
            Random random = new Random();
            for (int i = 0; i < enemyShips.Count; i++)
            {
                SpaceShip enemyShip = enemyShips.ElementAt(i);
                double randomValue = random.NextDouble();
                enemyShip.position.LaPositionX += direction * speedCoef * deltaT;
                if (enemyShips.Count == 1) randomShootProbability = 5;
                if (randomValue < randomShootProbability * deltaT){
                    if(enemyShip is Boss boss)
                        boss.Shoot(gameInstance, 500, enemyShip.Side);
                    else
                        enemyShip.Shoot(gameInstance, 100, enemyShip.Side);
                }
                enemyShip.Update(gameInstance, deltaT);
            }
        }
        private bool CheckEdgeCollision(Game gameInstance)
        {
            if (goingRight)
                return this.Position.LaPositionX + this.Size.Width >= gameInstance.gameSize.Width;
            else
                return this.Position.LaPositionX <= 0;
        }
        private void HandleDirectionChange()
        {
            goingRight = !goingRight;
            speedCoef += speedCoefAdd;
            randomShootProbability += 0.1;

            foreach (SpaceShip enemyShip in enemyShips)
            {
                enemyShip.position.LaPositionY += descente;
            }
        }
        private void MoveDown()
        {
            this.Position.LaPositionY += descente;
            foreach (SpaceShip enemyShip in enemyShips)
            {
                enemyShip.position.LaPositionY += descente;
            }
        }
        private void RemoveDeadShips(List<SpaceShip> shipsToRemove)
        {
            shipsToRemove.AddRange(enemyShips.Where(ship => !ship.IsAlive()));
            foreach (SpaceShip shipToRemove in shipsToRemove)
            {
                enemyShips.Remove(shipToRemove);
            }
        }
        public override void Draw(Game gameInstance, Graphics graphics)
        {
            foreach (SpaceShip enemyShip in enemyShips)
            {
                if(enemyShip is Boss boss)
                    boss.Draw(gameInstance, graphics);
                else
                    enemyShip.Draw(gameInstance, graphics);
            }
        }
        public override bool IsAlive() => enemyShips.Count > 0;
        public override void Collision(Missile m, Game gameInstance)
        {
            foreach (SpaceShip enemyShip in enemyShips)
            {
                enemyShip.Collision(m, gameInstance);
            }
        }
    }
}
