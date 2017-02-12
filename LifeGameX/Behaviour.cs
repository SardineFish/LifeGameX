using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    abstract public class Behaviour : IBehaviour, IIdentification, ILifeComponent, IRandomObject
    {
        public string Description { get; set; }

        public string ID { get; set; }

        public Life Life { get; set; }

        public string Name { get; set; }

        public abstract void Act(params object[] args);

        double w = 0;
        public double Weight
        {
            get
            {
                return w;
            }
            set
            {
                w = value;
            }
        }

        public Property EnergyCost { get; set; }

        public abstract Behaviour Clone(Life life);

        protected Behaviour(long typeID, Life life, double energyCost = 0)
        {
            if (life == null)
                throw new ArgumentNullException("The life cannot be empty");
            if (life.Behaviours.ContainsKey(typeID))
                throw new ArgumentException("Behaviour existed.");
            this.Life = life;
            this.Weight = 100;
            this.ID = typeID.ToString();
            this.EnergyCost = new Property(life, energyCost);
            life.Behaviours.Add(typeID, this);
            life.BehaviourList.Add(this);
            life.RandomWeightList.Add(this);
        }
    }
}
