using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX.Behaviours
{
    public static class Metabolize 
    {
        public class ResourceToEnergy : Behaviour
        {
            public override void Act(params object[] args)
            {
                if (this.Life.Energy < this.EnergyCost.Value)
                {
                    this.Life.Stimulate(this.Life.EnergyLowStimulus);
                    return;
                }
                SubstanceCapsule res = null;
                if (args[0] is Substance)
                {
                    res = this.Life.Resources[args[0] as Substance];
                }
                else if (args[0] is SubstanceCapsule)
                {
                    res = args[0] as SubstanceCapsule;
                }
                else
                {
                    if (this.Life.SubstanceCapsuleList.Count <= 0)
                        return;
                    res = this.Life.SubstanceCapsuleList.GetRandom();
                }
                double amount = this.Life.World.Random.NextDouble() * res.TransferLimit.Value;
                var energy = res.TransferToEnergy(amount);
                this.Life.Energy += energy;
            }

            public ResourceToEnergy(Life life, double energyCost)
            {
                if (life == null)
                    throw new ArgumentNullException("The life cannot be null.");
                this.Life = life;
                this.Name = "SubstanceToEnergy";
                this.ID = this.Life.World.CreateID();
                this.Description = "Transfer substance into energy.";
                this.Weight = 1;
                this.EnergyCost = new Property(life, energyCost);
                life.BehaviourList.Add(this);
                life.RandomWeightList.Add(this);
            }
        }

        public class ResourceToResource : Behaviour
        {

            public override void Act(params object[] args)
            {
                if (this.Life.Energy < this.EnergyCost.Value)
                {
                    this.Life.Stimulate(this.Life.EnergyLowStimulus);
                    return;
                }
                if (this.Life.SubstanceCapsuleList.Count <= 0)
                    return;
                SubstanceCapsule res1 = null;
                SubstanceCapsule res2 = null;
                for(var i = 0; i < args.Length; i++)
                {
                    if (res1 == null)
                    {
                        if (args[i] is SubstanceCapsule)
                        {
                            res1 = this.Life.Resources[args[i] as Substance];
                        }
                        else if (args[i] is SubstanceCapsule)
                        {
                            res1 = args[i] as SubstanceCapsule;
                        }
                    }
                    else if (res2 == null)
                    {
                        if (args[i] is SubstanceCapsule)
                        {
                            res2 = this.Life.Resources[args[i] as Substance];
                        }
                        else if (args[i] is SubstanceCapsule)
                        {
                            res2 = args[i] as SubstanceCapsule;
                        }
                    }
                }
                if (res1 == null)
                {
                    res1 = this.Life.SubstanceCapsuleList.GetRandom();
                }
                if (res2 == null)
                {
                    res2 = this.Life.SubstanceCapsuleList.GetRandom();
                }
                double amount = this.Life.World.Random.NextDouble() * res1.TransferLimit.Value;
                amount = res1.TransferTo(res2, amount);
            }

            public ResourceToResource(Life life, double energyCost)
            {
                if (life == null)
                    throw new ArgumentNullException("The life cannot be null.");
                this.Life = life;
                this.Name = "SubstanceToSubstance";
                this.ID = this.Life.World.CreateID();
                this.Description = "Transfer substance into another substance.";
                this.Weight = 1;
                this.EnergyCost = new Property(life, energyCost);
                life.BehaviourList.Add(this);
                life.RandomWeightList.Add(this);
            }
        }

        public class EnergyToResource : Behaviour
        {
            public override void Act(params object[] args)
            {
                if (this.Life.Energy < this.EnergyCost.Value)
                {
                    this.Life.Stimulate(this.Life.EnergyLowStimulus);
                    return;
                }
                SubstanceCapsule res = null;
                if (args[0] is Substance)
                {
                    res = this.Life.Resources[args[0] as Substance];
                }
                else if (args[0] is SubstanceCapsule)
                {
                    res = args[0] as SubstanceCapsule;
                }
                else
                {
                    if (this.Life.SubstanceCapsuleList.Count <= 0)
                        return;
                    res = this.Life.SubstanceCapsuleList.GetRandom();
                }
                double energy = this.Life.World.Random.NextDouble() * (res.TransferLimit.Value / res.Substance.FromEnergy);
                res.TransferFromEnergy(energy);
            }

            public EnergyToResource(Life life, double energyCost)
            {
                if (life == null)
                    throw new ArgumentNullException("The life cannot be null.");
                this.Life = life;
                this.Name = "EnergyToSubstance";
                this.ID = this.Life.World.CreateID();
                this.Description = "Transfer energy into substance.";
                this.Weight = 1;
                this.EnergyCost = new Property(life, energyCost);
                life.BehaviourList.Add(this);
                life.RandomWeightList.Add(this);
            }
        }

    }
}
