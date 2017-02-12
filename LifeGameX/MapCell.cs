using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    public class MapCell : List<IPositionable>, IPositionable
    {
        public World World { get; set; }

        public long X { get; set; }

        public long Y { get; set; }

        public Dictionary<Substance, SubstanceCell> Substances { get; set; }

        public IPositionable Peak()
        {
            return this.Last();
        }

        public int Push(IPositionable obj)
        {
            if (this.Contains(obj))
                throw new ArgumentException("Existed.");
            base.Add(obj);
            return this.Count - 1;
        }

        public IPositionable Pop()
        {
            var obj = this.Last();
            this.RemoveAt(this.Count - 1);
            return obj;
        }

        internal MapCell(World world, long x, long y) : base()
        {
            this.Substances = new Dictionary<Substance, SubstanceCell>();
            this.World = world;
            this.X = x;
            this.Y = y;
        }

        public new void Add(IPositionable obj)
        {
            if (this.Contains(obj))
                throw new ArgumentException("Existed.");
            if(obj is SubstanceCell)
            {
                var substanceCell = obj as SubstanceCell;
                if (!this.Substances.Keys.Contains(substanceCell.Substance))
                    this.Substances.Add(substanceCell.Substance, substanceCell);
                else
                {
                    this.Substances[substanceCell.Substance].Amount += substanceCell.Amount;
                }
            }
            if (obj.X > 0 && obj.Y > 0)
            {
                this.World[obj.X, obj.Y].Remove(obj);
            }
            base.Add(obj);
            obj.X = this.X;
            obj.Y = this.Y;
        }

        public SubstanceCell Add(Substance substance, double amount)
        {
            if (!this.Substances.ContainsKey(substance))
            {
                var substanceCell = new SubstanceCell(substance, amount);
                this.Substances[substance] = substanceCell;
                Add(substanceCell);
                return substanceCell;
            }
            else
            {
                this.Substances[substance].Amount += amount;
                return this.Substances[substance];
            }
        }

        public double GetSubstance(Substance substance, double amount)
        {
            if (!this.Substances.ContainsKey(substance))
            {
                return 0;
            }
            if (this.Substances[substance].Amount - amount < 0)
                amount = this.Substances[substance].Amount;
            this.Substances[substance].Amount -= amount;
            return amount;
        }

        public new void Remove(IPositionable obj)
        {
            if (!this.Contains(obj))
                return;
            obj.X = -1;
            obj.Y = -1;
            base.Remove(obj);
        }

        public bool ContainsSubstance(Substance substance)
        {
            return this.Substances.Keys.Contains(substance);
        }
    }
}
