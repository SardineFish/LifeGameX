using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    public class Reaction : IReaction, IIdentification, ILifeComponent, IRandomObject
    {
        string description;
        public string Description { get; set; }

        string id;
        public string ID { get; set; }

        string name;
        public string Name { get; set; }

        public Stimulus Source { get; set; }

        RandomList<Behaviour> behaviours = new RandomList<Behaviour>();
        public RandomList<Behaviour> Behaviours { get; set; }

        public Life Life { get; set; }

        public int Weight { get; set; }

        public Reaction (Life life,Stimulus source)
        {
            if (life == null)
                throw new ArgumentNullException("The life cannot be null.");
            if (source == null)
                throw new ArgumentNullException("The source cannot be null.");
            this.Life = life;
            this.Source = source;
            this.behaviours = new LifeGameX.RandomList<LifeGameX.Behaviour>();
            this.Description = "A reaction when " + this.Life + " is stimulated by " + this.Source.Name + " . ";
            life.RandomWeightList.Add(this);
        }
    }
}
