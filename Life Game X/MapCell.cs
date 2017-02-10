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
            this.World = world;
            this.X = x;
            this.Y = y;
            
        }

        public new void Add(IPositionable obj)
        {
            if (this.Contains(obj))
                throw new ArgumentException("Existed.");
            if (obj.X > 0 && obj.Y > 0)
            {
                this.World[obj.X, obj.Y].Remove(obj);
            }
            base.Add(obj);
            obj.X = this.X;
            obj.Y = this.Y;
        }

        public new void Remove(IPositionable obj)
        {
            if (!this.Contains(obj))
                return;
            obj.X = -1;
            obj.Y = -1;
            base.Remove(obj);
        }
    }
}
