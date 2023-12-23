using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Media;
using System.Runtime.CompilerServices;
using System.Drawing.Imaging;
using System.Threading;
using System.IO;

namespace SpaceInvaders
{
    /// Test djfhsdkjfhdjsif
    /// <summary>
    /// This class represents the entire game, it implements the singleton pattern
    /// </summary>
    enum GameState { Play, Pause, Win, Lost };
    class Game
    {
        /// Champs d'instance ajouté 
        private PlayerSpaceShip playerShip;
        private EnemyBlock enemies;
        private GameState state = GameState.Play;
        private Image backgroundImage;
        private bool bossAlleadyAdded;
        private Audio backgroundMusic;
        private int currentMusic;

        public PlayerSpaceShip Player
        {
            get
            {
                return this.playerShip;
            }
        }
        public GameState State
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
            }
        }

        #region GameObjects management
        /// <summary>
        /// Set of all game objects currently in the game
        /// </summary>
        public HashSet<GameObject> gameObjects = new HashSet<GameObject>();

        /// <summary>
        /// Set of new game objects scheduled for addition to the game
        /// </summary>
        private HashSet<GameObject> pendingNewGameObjects = new HashSet<GameObject>();

        /// <summary>
        /// Schedule a new object for addition in the game.
        /// The new object will be added at the beginning of the next update loop
        /// </summary>
        /// <param name="gameObject">object to add</param>
        public void AddNewGameObject(GameObject gameObject)
        {
            pendingNewGameObjects.Add(gameObject);
        }
        #endregion

        #region game technical elements
        /// <summary>
        /// Size of the game area
        /// </summary>
        public Size gameSize;

        /// <summary>
        /// State of the keyboard
        /// </summary>
        public HashSet<Keys> keyPressed = new HashSet<Keys>();

        #endregion

        #region static fields (helpers)

        /// <summary>
        /// Singleton for easy access
        /// </summary>
        public static Game game { get; private set; }

        /// <summary>
        /// A shared black brush
        /// </summary>
        private static Brush blackBrush = new SolidBrush(Color.Black);

        /// <summary>
        /// A shared simple font
        /// </summary>
        private static Font defaultFont = new Font("Times New Roman", 24, FontStyle.Bold, GraphicsUnit.Pixel);
        #endregion


        #region constructors
        /// <summary>
        /// Singleton constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        /// 
        /// <returns></returns>
        public static Game CreateGame(Size gameSize)
        {
            if (game == null)
                game = new Game(gameSize);  
            return game;
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        private Game(Size gameSize)
        {
            InitGame(gameSize);
        }

        #endregion
        
        #region methods

        /// <summary>
        /// Force a given key to be ignored in following updates until the user
        /// explicitily retype it or the system autofires it again.
        /// </summary>
        /// <param name="key">key to ignore</param>
        public void ReleaseKey(Keys key)
        {
            keyPressed.Remove(key);
        }


        /// <summary>
        /// Draw the whole game
        /// </summary>
        /// <param name="g">Graphics to draw in</param>
        public void Draw(Graphics g)
        {
            if (backgroundImage != null)
                g.DrawImage(backgroundImage, 0, 0, gameSize.Width, gameSize.Height);
            string texte;
            if (this.state == GameState.Play)
            {
                DrawGameStatus(g, "En cours", new Font("Arial", 12), Brushes.White, 10, 10);
                DrawGameObjects(g);
            }else if (this.state == GameState.Pause)
                DrawGameStatus(g, "Pause", new Font("Arial", 20), Brushes.White, this.gameSize.Height / 2, this.gameSize.Width / 2);
            else if (this.state == GameState.Win || this.state == GameState.Lost){
                texte = (this.state == GameState.Win) ? "Tu as gagné ! Appuie sur Espace pour recommencer." : "Tu as perdu ! Appuie sur Espace pour recommencer.";
                DrawGameStatus(g, texte, new Font("Arial", 15), Brushes.White, this.gameSize.Height / 2, this.gameSize.Width / 2);
            }
        }
        private void DrawGameStatus(Graphics g, string text, Font font, Brush brush, float x, float y)
        {
            double tailleTexteX = g.MeasureString(text, font).Width;
            double tailleTexteY = g.MeasureString(text, font).Height;
            g.DrawString(text, font, brush, x - (text == "En cours" ?  x : (float)tailleTexteX / 2), y - (float)tailleTexteY / 2);
        }

        private void DrawGameObjects(Graphics g)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(this, g);
            }
        }
        /// <summary>
        /// Update game
        /// </summary>
        public void Update(double deltaT)
        {
            // add new game objects
            gameObjects.UnionWith(pendingNewGameObjects);
            pendingNewGameObjects.Clear();

            // if space is pressed
            if (keyPressed.Contains(Keys.P) && this.state == GameState.Play || keyPressed.Contains(Keys.P) && this.state == GameState.Pause)
            {
                if(this.state == GameState.Play)
                    this.state = GameState.Pause;
                else
                    this.state = GameState.Play;

                ReleaseKey(Keys.P);
            }

            // update each game object
            if (this.state == GameState.Play)
            {
                if (this.enemies.IsAlive() && bossAlleadyAdded)
                    PlayMusic(SpaceInvaders.Properties.Resources.bossMusic, 4);
                else PlayMusic(SpaceInvaders.Properties.Resources.playSong1, 1);
                foreach (GameObject gameObject in gameObjects) {
                    gameObject.Update(this, deltaT);
                }
            }
            else if(this.state == GameState.Pause){
                PlayMusic(SpaceInvaders.Properties.Resources.music1, 2);
            }
            else if (this.state == GameState.Win || this.state == GameState.Lost){
                Restart();
                PlayMusic(SpaceInvaders.Properties.Resources.menuSong, 3);
            }

            // verify if the enemies are dead
            if (!this.enemies.IsAlive() && !bossAlleadyAdded)
            {
                enemies.AddBoss();
                bossAlleadyAdded = true;
            }else if (!this.enemies.IsAlive() && bossAlleadyAdded) state = GameState.Win;
            // verify if the player is deads
            if (playerShip.Lives <= 0) state = GameState.Lost;
            // remove dead objects
            gameObjects.RemoveWhere(gameObject => !gameObject.IsAlive());
        }
        private void PlayMusic(UnmanagedMemoryStream theMusic, int a)
        {
            if (currentMusic != a)
            {
                backgroundMusic = new Audio(theMusic);
                backgroundMusic.Play();
                currentMusic = a;
            }
        }
        private void Restart()
        {
            game.gameObjects.Clear();
            if (keyPressed.Contains(Keys.Space))
            {
                InitGame(game.gameSize);
                this.state = GameState.Play;
                ReleaseKey(Keys.Space);
            }
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

        private void InitGame(Size gameSize)
        {
            this.gameSize = gameSize;
            this.playerShip = new PlayerSpaceShip(5, this.gameSize.Height/2-SpaceInvaders.Properties.Resources.ship3.Width, this.gameSize.Height -50, SpaceInvaders.Properties.Resources.ship3, Side.Ally, TheColorObject(Color.White));
            this.enemies = new EnemyBlock(new Vecteur2D(0, 50), 300, Side.Enemy);
            this.backgroundImage = SpaceInvaders.Properties.Resources.background2;
            this.bossAlleadyAdded = false;

            AddNewGameObject(playerShip);

            // AJOUT des 3 Bunkers
            AddNewGameObject(new Bunker(new Vecteur2D(100 - SpaceInvaders.Properties.Resources.bunker.Width / 2, this.gameSize.Height - 150), Side.Neutral, TheColorObject(Color.White)));
            AddNewGameObject(new Bunker(new Vecteur2D(300 - SpaceInvaders.Properties.Resources.bunker.Width / 2, this.gameSize.Height - 150), Side.Neutral, TheColorObject(Color.White)));
            AddNewGameObject(new Bunker(new Vecteur2D(500 - SpaceInvaders.Properties.Resources.bunker.Width / 2, this.gameSize.Height - 150), Side.Neutral, TheColorObject(Color.White)));


            //AJOUT DE LIGNES
            enemies.AddLine(2, 1, SpaceInvaders.Properties.Resources.ship6);
            Thread.Sleep(20);
            enemies.AddLine(3, 1, SpaceInvaders.Properties.Resources.ship7);
            Thread.Sleep(20);
            enemies.AddLine(4, 1, SpaceInvaders.Properties.Resources.ship8);
            Thread.Sleep(20);
            enemies.AddLine(5, 1, SpaceInvaders.Properties.Resources.ship3);
            Thread.Sleep(20);
            enemies.AddLine(6, 1, SpaceInvaders.Properties.Resources.ship4);
            Thread.Sleep(20);
            enemies.AddLine(7, 1, SpaceInvaders.Properties.Resources.ship5);
            //AJOUT du bloc d'enemy
            AddNewGameObject(enemies);
        }
        #endregion
    }
}
