using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class Vecteur2D
    {
        private double x, y;
        public Vecteur2D(double x = 0, double y = 0) 
        {
            this.x = x;
            this.y = y;
        }
        public double Norme /// Propiété Norme
        {
            get
            {
                return Math.Sqrt((this.x*this.x)+(this.y*this.y));
            }
        }
        public static Vecteur2D operator+(Vecteur2D v1, Vecteur2D v2)
        {
            return new Vecteur2D(v1.x + v2.x, v1.y + v2.y);
        }
        public static Vecteur2D operator -(Vecteur2D v1, Vecteur2D v2)
        {
            return new Vecteur2D(v1.x - v2.x, v1.y - v2.y);
        }
    }
}
