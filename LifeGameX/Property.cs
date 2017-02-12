using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    public class Property : ILifeComponent, IIdentification, IRandomObject
    {
        public string Description { get; set; }

        public string ID { get; set; }

        public Life Life { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }

        public Stimulus Changed { get; set; }

        public double Weight { get; set; }

        public Property(Life life,double value)
        {
            if (life == null)
                throw new ArgumentNullException("The life cannot be null.");
            this.Life = life;
            this.Value = value;
            this.ID = this.Life.World.CreateID();
            this.Name = "Prop-" + this.ID;
            this.Description = "A property owned by " + (this.Life?.Name);
            this.Weight = 100;
            life.RandomWeightList.Add(this);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //   +
        public static double operator +(Property p,double n)
        {
            return p.Value + n;
        }
        public static double operator +(double n,Property p)
        {
            return n + p.Value;
        }
        public static double operator +(Property p1,Property p2)
        {
            return p1.Value + p2.Value;
        }
        //   -
        public static double operator -(Property p, double n)
        {
            return p.Value - n;
        }
        public static double operator -(double n, Property p)
        {
            return n - p.Value;
        }
        public static double operator -(Property p1, Property p2)
        {
            return p1.Value - p2.Value;
        }

        //   *
        public static double operator *(Property p, double n)
        {
            return p.Value * n;
        }
        public static double operator *(double n, Property p)
        {
            return n * p.Value;
        }
        public static double operator *(Property p1, Property p2)
        {
            return p1.Value * p2.Value;
        }

        //   /
        public static double operator /(Property p, double n)
        {
            return p.Value / n;
        }
        public static double operator /(double n, Property p)
        {
            return n / p.Value;
        }
        public static double operator /(Property p1, Property p2)
        {
            return p1.Value / p2.Value;
        }

        //   %
        public static double operator %(Property p, double n)
        {
            return p.Value % n;
        }
        public static double operator %(double n, Property p)
        {
            return n % p.Value;
        }
        public static double operator %(Property p1, Property p2)
        {
            return p1.Value % p2.Value;
        }

        //   ==
        public static bool operator ==(Property p, double n)
        {
            if (Object.Equals(p, null))
                return false;
            return p.Value == n;
        }
        public static bool operator ==(double n, Property p)
        {
            return p == n;
        }
        public static bool operator ==(Property p1, Property p2)
        {
            return p2 == p1.Value;
        }

        //   !=
        public static bool operator !=(Property p, double n)
        {
            return p.Value != n;
        }
        public static bool operator !=(double n, Property p)
        {
            return p.Value != n;
        }
        public static bool operator !=(Property p1, Property p2)
        {
            return p1.Value != p2.Value;
        }

        //   <
        public static bool operator <(Property p, double n)
        {
            return p.Value < n;
        }
        public static bool operator <(double n, Property p)
        {
            return n < p.Value;
        }
        public static bool operator <(Property p1, Property p2)
        {
            return p1.Value < p2.Value;
        }

        //   >
        public static bool operator >(Property p, double n)
        {
            return p.Value < n;
        }
        public static bool operator >(double n, Property p)
        {
            return n > p.Value;
        }
        public static bool operator >(Property p1, Property p2)
        {
            return p1.Value > p2.Value;
        }

        public static explicit operator double (Property p)
        {
            return p.Value;
        }
    }
}
