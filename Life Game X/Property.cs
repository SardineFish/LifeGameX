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

        public int Weight { get; set; }

        public Property(Life life,double value)
        {
            if (life == null)
                throw new ArgumentNullException("The life cannot be null.");
            this.Life = life;
            this.Value = value;
            this.ID = this.Life.World.CreateID();
            this.Name = "Prop-" + this.ID;
            this.Description = "A property owned by " + (this.Life?.Name);
            this.Weight = 1;
            life.RandomWeightList.Add(this);
        }
    }
}
