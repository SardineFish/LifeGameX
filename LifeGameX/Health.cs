using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    public class Health : ILifeComponent, IIdentification
    {
        public string Description { get; set; }

        public string ID { get; set; }

        public Life Life { get; set; }

        public string Name { get; set; }

        double value = 0;
        public double Value
        {
            get
            {
                return value;
            }
            set
            {
                if (value < 0)
                    value = 0;
                this.value = value;
                if (value < 0)
                    this.Empty.Handle(Life, this, 0);
            }
        }

        public Stimulus Empty { get; private set; }

        public Stimulus Low { get; private set; }

        public Health(Life life,double value)
        {
            this.Name = "Health";
            this.Description = "The health rate of the life, and it will die when the healty is empty.";
            this.Life = life;
            this.value = value;
            Empty = new Stimulus(life, life.NoReactionStimulus);
            life.BuildReaction(Empty, life.Behaviours[Behaviours.Action.Die.TypeID]);
            Low = new Stimulus(life, life.NoReactionStimulus);
            life.StimulusList.Add(Low);
        }
    }
}
