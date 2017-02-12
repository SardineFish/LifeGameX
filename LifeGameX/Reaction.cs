using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    public class Reaction : IReaction, IIdentification, ILifeComponent, IRandomObject
    {
        public string Description { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }

        public Stimulus Source { get; set; }
        public RandomList<Behaviour> Behaviours { get; set; }

        public Life Life { get; set; }

        public double Weight { get; set; }

        public void Act(params object[] args)
        {
            for(var i=0;i<this.Behaviours.Count; i++)
            {
                this.Behaviours[i].Act(args);
            }
        }

        public Reaction(Life life, Stimulus source, params Behaviour[] behaviours)
        {
            if (life == null)
                throw new ArgumentNullException("The life cannot be null.");
            if (source == null)
                throw new ArgumentNullException("The source cannot be null.");
            this.Life = life;
            this.Source = source;
            this.Behaviours = new LifeGameX.RandomList<LifeGameX.Behaviour>();
            this.Behaviours.AddRange(behaviours);
            this.Description = "A reaction when " + this.Life + " is stimulated by " + this.Source.Name + " . ";
            this.Weight = 100;
            life.RandomWeightList.Add(this);
        }
    }
}
