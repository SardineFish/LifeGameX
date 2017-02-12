﻿using System;
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

        double energy = 0;
        public double Energy
        {
            get
            {
                return energy;
            }
            set
            {
                energy = value;
                if (energy < 10)
                    EnergyLowStimulus.Handle(this, energy);
                if (energy <= 0)
                    EnergyEmptyStimulus.Handle(this, energy);
            }
        }

        public Health Health { get; private set; }

        public bool Alive { get; set; }

        public long Age { get; private set; }

        public long X { get; set; }

        public long Y { get; set; }

        #region Stimulus

        public Stimulus NoReactionStimulus { get; private set; }
        public Stimulus EnergyLowStimulus { get; private set; }
        public Stimulus InteractStimulus { get; private set; }
        public Stimulus EnvironmentStimulus { get; private set; }
        public Stimulus EnergyEmptyStimulus { get; private set; }
        public Stimulus TimeUpdateStimulus { get; private set; }

        #endregion

        #region Lists

        public Dictionary<Substance,SubstanceCapsule> Resources { get; private set; }
        public RandomList<SubstanceCapsule> SubstanceCapsuleList { get; private set; }
        public List<Stimulus> StimulusList { get; private set; }
        public List<Reaction> ReactionList { get; private set; }
        public Dictionary<long,Behaviour> Behaviours { get; private set; }
        public RandomList<Behaviour> BehaviourList { get; private set; }
        public Queue<PendingStimulus> PendingStimulus { get; private set; }
        public List<ReactionRecord> PrevioursReactions { get; private set; }
        public RandomList<Property> Properties { get; private set; }
        public RandomList<IRandomObject> RandomWeightList { get; private set; }

        #endregion


        public void Stimulate (Stimulus stimulus,params object[] args)
        {
            this.PendingStimulus.Enqueue(new PendingStimulus(stimulus, args));
        }

        public Reaction BuildReaction(Stimulus stimulusSource,params Behaviour[] behaviors)
        {
            if (stimulusSource == null || behaviors == null)
                throw new ArgumentNullException();
            if (!this.StimulusList.Contains(stimulusSource))
                throw new Exception("Stimulus no available.");
            var reaction = new Reaction(this, stimulusSource);
            stimulusSource.Reactions.Add(reaction);
            this.ReactionList.Add(reaction);
            return reaction;
        }

        public void RemoveReaction(Reaction reaction)
        {
            if (reaction == null)
                throw new ArgumentNullException();
            if (!this.ReactionList.Contains(reaction))
                throw new Exception("Reaction no available.");
            reaction.Source.Reactions.Remove(reaction);
            this.ReactionList.Remove(reaction);
            for(var i = 0; i < this.PrevioursReactions.Count; i++)
            {
                if (this.PrevioursReactions[i].Reaction == reaction)
                    this.PrevioursReactions.RemoveAt(i);
            }
            
        }

        public void AddResource(SubstanceCapsule res)
        {
            Resources.Add(res.Substance, res);
            SubstanceCapsuleList.Add(res);
            RandomWeightList.Add(res);
        }

        private void cloneReaction(Stimulus stimulusFrom, Stimulus stimulusTo, Life life)
        {
            var behaviorList = new List<Behaviour>();
            foreach (var reaction in stimulusFrom.Reactions)
            {
                foreach (var behaviors in reaction.Behaviours)
                {
                    behaviorList.Add(behaviors.Clone(life));
                }
            }
            life.BuildReaction(stimulusTo, behaviorList.ToArray());
        }

        public Life Clone()
        {
            var child = new Life(this.Species, this.World);

            child.Energy = 0;

            //clone reaction
            cloneReaction(this.NoReactionStimulus, child.NoReactionStimulus, child);
            cloneReaction(this.EnergyLowStimulus, child.EnergyLowStimulus, child);
            cloneReaction(this.InteractStimulus, child.InteractStimulus, child);
            cloneReaction(this.EnvironmentStimulus, child.EnvironmentStimulus, child);
            cloneReaction(this.TimeUpdateStimulus, child.TimeUpdateStimulus, child);

            //clone resource
            foreach (var res in this.Resources)
            {
                var resCapsule = new SubstanceCapsule(child, res.Key, 0, res.Value.Max.Value, res.Value.Min.Value, res.Value.TransferLimit.Value);
                cloneReaction(res.Value.Few, resCapsule.Few, child);
                cloneReaction(res.Value.Much, resCapsule.Much, child);
                child.AddResource(resCapsule);
            }

            return child;
        }

        public void Move(long x,long y)
        {
            World[X, Y].Remove(this);
            World[X, Y].Add(this);
        }

        internal void Update(long dt)
        { 
            if (!Alive)
                return;
            Age += dt;
            while (this.PendingStimulus.Count > 0)
            {
                if (!Alive)
                    return;
                var pending = PendingStimulus.Dequeue();
                pending.Stimulus.Handle(pending.Args);
            }
        }

        public void ReleaseSubstance(Substance substance, double amount, double distance)
        {
            if (substance == null)
                throw new ArgumentNullException("Substance cannot be null.");
            if (!this.Resources.ContainsKey(substance))
                return;
            if (amount > this.Resources[substance].Amount)
                amount = this.Resources[substance].Amount;
            Func<double, double, double> f = (double x, double y) =>
            {
                var t = x * x + y * y;
                return (1 / ((2 * Math.PI) * 3)) * Math.Exp(-t / (18));
            };
            List<long> xList = new List<long>();
            List<long> yList = new List<long>();
            List<double> rList = new List<double>();
            double sum = 0;
            for (long y = (long)(this.Y - distance); y < (long)(this.Y + distance); y++)
            {
                for (long x = (long)(this.X - distance); x < (long)(this.X + distance); x++)
                {
                    if (x * x + y * y > distance * distance)
                        continue;
                    if (x < 0)
                        x = -x;
                    if (x > World.Width)
                        x = World.Width - (x - World.Width);
                    if (y < 0)
                        y = -y;
                    if (y > World.Height)
                        y = World.Height - (y - World.Height);
                    var r = f(x, y);
                    sum += r;
                    xList.Add(x);
                    yList.Add(y);
                    rList.Add(r);
                }
            }
            for(var i = 0; i < yList.Count; i++)
            {
                var x = xList[i];
                var y = yList[i];
                var r = rList[i];
                var amt = amount * r / sum;
                World[x, y].Add(substance, amt);
            }
            this.Resources[substance].SetAmount(this.Resources[substance].Amount - amount);
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
            this.Resources = new Dictionary<Substance, SubstanceCapsule>();
            this.SubstanceCapsuleList = new RandomList<SubstanceCapsule>();
            this.ReactionList = new List<Reaction>();
            this.Behaviours = new Dictionary<long, Behaviour>();
            this.BehaviourList = new RandomList<Behaviour>();
            this.PendingStimulus = new Queue<PendingStimulus>();
            this.PrevioursReactions = new List<ReactionRecord>();
            this.Properties = new RandomList<Property>();
            this.RandomWeightList = new RandomList<IRandomObject>();
            this.X = -1;
            this.Y = -1;

            //init Behaviors
            #region Behaviors
            // NoResponse
            var noResponse = new Behaviours.NoResponse(this);
            // Metabolize
            var resToEnergy = new Behaviours.Metabolize.ResourceToEnergy(this, 10);
            var resToRes = new Behaviours.Metabolize.ResourceToResource(this, 10);
            var energyToRes = new Behaviours.Metabolize.EnergyToResource(this, 10);
            var cure = new Behaviours.Metabolize.Cure(this, 5);
            var autotomy = new Behaviours.Metabolize.Autotomy(this, 5);
            // Multiply
            var multiply = new Behaviours.Multiply(this, 20);
            // Action
            var die = new Behaviours.Action.Die(this);
            var move = new Behaviours.Action.Move(this, 20);
            // Interact
            var interact = new Behaviours.Interact(this, 10);
            // Learn
            var establishReaction = new Behaviours.Learn.EstablishReaction(this, 10);
            var abolishReaction = new Behaviours.Learn.AbolishReaction(this, 10);
            var incWeight = new Behaviours.Learn.IncreaseWeight(this, 5);
            var decWeight = new Behaviours.Learn.DecreaseWeight(this, 5);

            var mutate = new Behaviours.Mutate(this, 50);

            #endregion

            //init Energy
            this.energy = 200;
            this.BuildReaction(this.EnergyEmptyStimulus, this.Behaviours[LifeGameX.Behaviours.Action.Die.TypeID]);

            //init Health
            this.Health = new LifeGameX.Health(this, 100);

            //init stimulus
            this.StimulusList = new List<Stimulus>();
            this.NoReactionStimulus = new Stimulus(this, this.NoReactionStimulus);
            this.EnergyLowStimulus = new Stimulus(this, this.NoReactionStimulus);
            this.InteractStimulus = new Stimulus(this, this.NoReactionStimulus);
            this.EnvironmentStimulus = new Stimulus(this, this.NoReactionStimulus);
            this.TimeUpdateStimulus = new Stimulus(this, this.NoReactionStimulus);

            this.BuildReaction(TimeUpdateStimulus, noResponse).Weight = 500;
            this.BuildReaction(TimeUpdateStimulus, move);
            this.BuildReaction(TimeUpdateStimulus, energyToRes);
            this.BuildReaction(TimeUpdateStimulus, resToEnergy);
            this.BuildReaction(TimeUpdateStimulus, multiply).Weight = 50;
        }
    }
}
