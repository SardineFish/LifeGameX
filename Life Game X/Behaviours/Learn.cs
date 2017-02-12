using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX.Behaviours
{
    public static class Learn
    {
        public class EstablishReaction : Behaviour
        {
            public const long TypeID = 0x101;
            public EstablishReaction(Life life, double energyCost) : base(TypeID, life, energyCost)
            {
                this.Name = "EstablishReaction";
                this.Description = "Establish a reatcion to handle a stimulus.";
            }

            public override void Act(params object[] args)
            {
                if (this.Life.Energy < this.EnergyCost.Value)
                {
                    this.Life.Stimulate(this.Life.EnergyLowStimulus);
                    return;
                }
                this.Life.Energy -= this.EnergyCost;
                Stimulus stimulus = null;
                long param = 0;
                if (args.Length > 0)
                {
                    foreach(var obj in args)
                    {
                        if(obj is Stimulus)
                        {
                            stimulus = obj as Stimulus;
                        }
                        if(obj is double || obj is long || obj is int)
                        {
                            param += Convert.ToInt64(obj);
                        }
                    }
                }
                if (stimulus == null || args.Length <= 0)
                {
                    for (var i = 0; i < Life.PrevioursReactions.Count; i++)
                    {
                        if (Life.World.Random.NextDouble() < ((double)(i + 1) / (double)Life.PrevioursReactions.Count) * 0.5 + 0.5)
                        {
                            stimulus = Life.PrevioursReactions[Life.PrevioursReactions.Count - i - 1].Stimulus;
                        }
                    }
                }
                if (stimulus == null)
                    return;

                int count = (int)(1 - 0.3 / (Life.World.Random.NextDouble() - 1.05));
                List<Behaviour> behaviors = new List<Behaviour>();
                for(var i = 0; i < count; i++)
                {
                    var behav = Life.BehaviourList.GetRandom();
                    if (behaviors.Contains(behav))
                        continue;
                    behaviors.Add(behav);
                }
                Life.BuildReaction(stimulus, behaviors.ToArray());
            }

            public override Behaviour Clone(Life life)
            {
                return new EstablishReaction(life, this.EnergyCost.Value);
            }
        }

        public class AbolishReaction : Behaviour
        {
            public const long TypeID = 0x102;
            public AbolishReaction(Life life, double energyCost) : base(TypeID, life, energyCost)
            {
                this.Name = "AbolishReaction.";
                this.Description = "Abolish a reaction.";
            }

            public override void Act(params object[] args)
            {
                if (this.Life.Energy < this.EnergyCost.Value)
                {
                    this.Life.Stimulate(this.Life.EnergyLowStimulus);
                    return;
                }
                this.Life.Energy -= this.EnergyCost;
                Reaction reaction = null;
                long param = 0;
                if (args.Length > 0)
                {
                    foreach (var obj in args)
                    {
                        if (obj is Reaction)
                        {
                            reaction = obj as Reaction;
                        }
                        if (obj is double || obj is long || obj is int)
                        {
                            param += Convert.ToInt64(obj);
                        }
                    }
                }
                if (reaction == null || args.Length <= 0)
                {
                    //from all
                    if (Life.World.Random.NextDouble() < 0.3)
                    {
                        reaction = Life.ReactionList[Life.World.Random.Next(Life.ReactionList.Count)];
                    }
                    //from previours
                    else
                    {
                        for (var i = 0; i < Life.PrevioursReactions.Count; i++)
                        {
                            if (Life.World.Random.NextDouble() < ((double)(i + 1) / (double)Life.PrevioursReactions.Count) * 0.5 + 0.5)
                            {
                                reaction = Life.PrevioursReactions[Life.PrevioursReactions.Count - i - 1].Reaction;
                            }
                        }
                    }
                }
                if (reaction == null)
                    return;
                Life.RemoveReaction(reaction);
            }

            public override Behaviour Clone(Life life)
            {
                return new AbolishReaction(life, this.EnergyCost.Value);
            }
        }

        public class IncreaseWeight : Behaviour
        {
            public const long TypeID = 0x104;
            public IncreaseWeight(Life life, double energyCost) : base(TypeID, life, energyCost)
            {
                this.Name = "IncreaseWeight";
                this.Description = "Increase the weight of a RandomObject.";
            }

            public override void Act(params object[] args)
            {
                if (this.Life.Energy < this.EnergyCost.Value)
                {
                    this.Life.Stimulate(this.Life.EnergyLowStimulus);
                    return;
                }
                this.Life.Energy -= this.EnergyCost;
                IRandomObject randomObj = null;
                long param = 0;
                if (args.Length > 0)
                {
                    foreach (var obj in args)
                    {
                        if (obj is IRandomObject)
                        {
                            randomObj = obj as IRandomObject;
                        }
                        if (obj is double || obj is long || obj is int)
                        {
                            param += Convert.ToInt64(obj);
                        }
                    }
                }
                if (randomObj == null || args.Length <= 0)
                {
                    //from all
                    if (Life.World.Random.NextDouble() < 0.3)
                    {
                        randomObj = Life.RandomWeightList[Life.World.Random.Next(Life.RandomWeightList.Count)];
                    }
                    //from previours
                    else
                    {
                        for (var i = 0; i < Life.PrevioursReactions.Count; i++)
                        {
                            if (Life.World.Random.NextDouble() < ((double)(i + 1) / (double)Life.PrevioursReactions.Count) * 0.5 + 0.5)
                            {
                                randomObj = Life.PrevioursReactions[Life.PrevioursReactions.Count - i - 1].Reaction;
                            }
                        }
                    }
                }
                if (randomObj == null)
                    return;

                var r = -0.06 / ((Life.World.Random.NextDouble() * param % 1) - 1.08);
                randomObj.Weight = (int)(randomObj.Weight * (1 + r));
            }

            public override Behaviour Clone(Life life)
            {
                return new IncreaseWeight(life, this.EnergyCost.Value);
            }
        }

        public class DecreaseWeight : Behaviour
        {
            public const long TypeID = 0x108;
            public DecreaseWeight( Life life, double energyCost) : base(TypeID, life, energyCost)
            {
                this.Name = "DecreaseWeight";
                this.Description = "Decrease the weight of a RandomObject";
            }

            public override void Act(params object[] args)
            {
                if (this.Life.Energy < this.EnergyCost.Value)
                {
                    this.Life.Stimulate(this.Life.EnergyLowStimulus);
                    return;
                }
                this.Life.Energy -= this.EnergyCost;
                IRandomObject randomObj = null;
                long param = 0;
                if (args.Length > 0)
                {
                    foreach (var obj in args)
                    {
                        if (obj is IRandomObject)
                        {
                            randomObj = obj as IRandomObject;
                        }
                        if (obj is double || obj is long || obj is int)
                        {
                            param += Convert.ToInt64(obj);
                        }
                    }
                }
                if (randomObj == null || args.Length <= 0)
                {
                    //from all
                    if (Life.World.Random.NextDouble() < 0.3)
                    {
                        randomObj = Life.RandomWeightList[Life.World.Random.Next(Life.RandomWeightList.Count)];
                    }
                    //from previours
                    else
                    {
                        for (var i = 0; i < Life.PrevioursReactions.Count; i++)
                        {
                            if (Life.World.Random.NextDouble() < ((double)(i + 1) / (double)Life.PrevioursReactions.Count) * 0.5 + 0.5)
                            {
                                randomObj = Life.PrevioursReactions[Life.PrevioursReactions.Count - i - 1].Reaction;
                            }
                        }
                    }
                }
                if (randomObj == null)
                    return;

                var r = -0.06 / ((Life.World.Random.NextDouble() * param % 1) - 1.08);
                randomObj.Weight = (int)(randomObj.Weight * (1 - r));
            }

            public override Behaviour Clone(Life life)
            {
                return new DecreaseWeight(life, this.EnergyCost.Value);
            }
        }
    }
}
