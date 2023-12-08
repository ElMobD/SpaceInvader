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
            get{return Math.Sqrt((this.x*this.x)+(this.y*this.y));}
        }
        public double LaPositionX
        {
            get{return this.x;}
            set {this.x = value;}
        }
        public double LaPositionY
        {
            get{return this.y;}
            set{this.y = value;}
        }
        public static Vecteur2D operator+(Vecteur2D v1, Vecteur2D v2) /// Addition Vectorielle
        {
            return new Vecteur2D(v1.x + v2.x, v1.y + v2.y);
        }
        public static Vecteur2D operator-(Vecteur2D v1, Vecteur2D v2) /// Soustraction Vectorielle
        {
            return new Vecteur2D(v1.x - v2.x, v1.y - v2.y);
        }
        public static Vecteur2D operator-(Vecteur2D v1) /// Moins unaire
        {
            return new Vecteur2D(-v1.x, -v1.y);
        }
        public static Vecteur2D operator*(Vecteur2D v1, double mul) /// Multiplication par un scalaire à droite
        {
            return new Vecteur2D(mul * v1.x, mul * v1.y);
        }
        public static Vecteur2D operator *(double mul, Vecteur2D v1 ) /// Multiplication par un scalaire à droite
        {
            return new Vecteur2D(mul * v1.x, mul * v1.y);
        }
        public static Vecteur2D operator /(Vecteur2D v1, double div) /// Multiplication par un scalaire à droite
        {
            return new Vecteur2D(v1.x/div, v1.y/div);
        }
    }
}
