using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    public class Life : IIdentification, IPositionable
    {
        public World World { get; set; }
        public string Description { get; set; }

        public string ID { get; set; }

        public string Name { get; set; }

        public string Species { get; set; }

        public double Energy { get; set; }

        public bool Alive { get; set; }

        public Stimulus NoReactionStimulus { get; private set; }
        public Stimulus EnergyLowStimulus { get; private set; }

        public Dictionary<Substance,SubstanceCapsule> Resources { get; private set; }

        public RandomList<SubstanceCapsule> SubstanceCapsuleList { get; private set; }

        public List<Stimulus> StimulusList { get; private set; }

        public List<Reaction> ReactionList { get; private set; }

        public RandomList<Behaviour> BehaviourList { get; private set; }

        public Queue<PendingStimulus> PendingStimulus { get; private set; }

        public Stack<ReactionRecord> PrevioursReactions { get; private set; }

        public RandomList<Property> Properties { get; private set; }

        public RandomList<IRandomObject> RandomWeightList { get; private set; }

        public long X { get; set; }

        public long Y { get; set; }

        public void Stimulate (Stimulus stimulus,params object[] args)
        {
            this.PendingStimulus.Enqueue(new PendingStimulus(stimulus, args));
        }

        public Life(string species, World world)
        {
            if (world == null)
                throw new ArgumentNullException("World required.");
            this.World = world;
            this.Alive = false;
            this.ID = World.CreateLifeID();
            this.Species = species;
            this.Name = "Life-" + this.Species + "-" + this.ID;
            this.NoReactionStimulus = new Stimulus(this, this.NoReactionStimulus);
            this.EnergyLowStimulus = new Stimulus(this, this.NoReactionStimulus);
            this.Resources = new Dictionary<Substance, SubstanceCapsule>();
            this.SubstanceCapsuleList = new RandomList<SubstanceCapsule>();
            this.StimulusList = new List<Stimulus>();
            this.ReactionList = new List<Reaction>();
            this.BehaviourList = new RandomList<Behaviour>();
            this.PendingStimulus = new Queue<PendingStimulus>();
            this.PrevioursReactions = new Stack<ReactionRecord>();
            this.Properties = new RandomList<Property>();
            this.RandomWeightList = new RandomList<IRandomObject>();
            this.X = -1;
            this.Y = -1;
        }
    }
}
