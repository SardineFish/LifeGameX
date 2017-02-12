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
        
        public RandomList<Reaction> Reactions { get; set; }


        public void Handle(params object[] args)
        {
            if (this.Reactions.Count <= 0)
            {
                if (NoReaction == null)
                    return;
                Life.Stimulate(NoReaction, this);
                return;
            }
            var reacton = this.Reactions.GetRandom();
            reacton.Act(args);
            Life.PrevioursReactions.Add(new ReactionRecord(this, reacton));
        }

        public Stimulus(Life life, Stimulus noReaction)
        {
            if (life == null)
                throw new ArgumentNullException("The life cannot be null.");
            /*if (noReaction == null)
                throw new ArgumentNullException("NoReaction stimulus required.");*/
            this.Reactions = new RandomList<Reaction>();
            this.Life = life;
            this.NoReaction = noReaction;
            this.ID = life.World.CreateStimulusID();
            this.Name = "Stimulus-" + this.ID;
            this.Description = "A stimulus.";
            life.StimulusList.Add(this);
        }
    }
}
