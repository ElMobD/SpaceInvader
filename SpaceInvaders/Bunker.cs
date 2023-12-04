using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class Bunker : SimpleObject
    {
        public Bunker(Vecteur2D position, Side side) :base(side) { 
            this.position = position;
            this.image = SpaceInvaders.Properties.Resources.bunker;
            this.lives = 1;
            this.side = side;
        }
        public override void Update(Game gameInstance, double deltaT){}
        protected override void OnCollision(Missile m, int numberOfPixelsInCollision, Game gameInstance)
        {
            m.lives--;
        }
        public override Side Side { 
            get { return side; }
            set { side = value; }
        }
    }
}
