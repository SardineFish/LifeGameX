using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    public class Stimulus : IIdentification, ILifeComponent
    {
        public string Description { get; set; }

        public string ID { get; set; }

        public Life Life { get; set; }

        public string Name { get; set; }

        public Stimulus NoReaction { get; set; }

        RandomList<Reaction> reactions = new RandomList<Reaction>();
        public RandomList<Reaction> Reactions { get; set; }


        public void Handle(params object[] args)
        {

        }

        public Stimulus(Life life, Stimulus noReaction)
        {
            if (life == null)
                throw new ArgumentNullException("The life cannot be null.");
            this.Life = life;
            this.NoReaction = noReaction;
            this.ID = this.Life.World.CreateID();
            this.Name = "Stimulus-" + this.ID;
            this.Description = "A stimulus.";
        }
    }
}
