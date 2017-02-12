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
            public const long TypeID = 0x11;
            public override void Act(params object[] args)
            {
                if (this.Life.Energy < this.EnergyCost.Value)
                {
                    this.Life.Stimulate(this.Life.EnergyLowStimulus);
                    return;
                }
                this.Life.Energy -= this.EnergyCost;
                SubstanceCapsule res = null;
                if (args.Length > 0 && args[0] is Substance)
                {
                    res = this.Life.Resources[args[0] as Substance];
                }
                else if (args.Length > 0 && args[0] is SubstanceCapsule)
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

            public override Behaviour Clone(Life life)
            {
                return new ResourceToEnergy(life, this.EnergyCost.Value);
            }

            public ResourceToEnergy(Life life, double energyCost) : base(TypeID, life, energyCost)
            {
                if (life == null)
                    throw new ArgumentNullException("The life cannot be null.");
                this.Name = "SubstanceToEnergy";
                this.Description = "Transfer substance into energy.";
            }
        }

        public class ResourceToResource : Behaviour
        {
            public const long TypeID = 0x12;
            public override void Act(params object[] args)
            {
                if (this.Life.Energy < this.EnergyCost.Value)
                {
                    this.Life.Stimulate(this.Life.EnergyLowStimulus);
                    return;
                }
                this.Life.Energy -= this.EnergyCost;
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
                            res1 = args[i] as SubstanceCapsule;
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

            public override Behaviour Clone(Life life)
            {
                return new ResourceToResource(life, this.EnergyCost.Value);
            }

            public ResourceToResource(Life life, double energyCost):base(TypeID, life, energyCost)
            {
                if (life == null)
                    throw new ArgumentNullException("The life cannot be null.");
                this.Name = "SubstanceToSubstance";
                this.Description = "Transfer substance into another substance.";
            }
        }

        public class EnergyToResource : Behaviour
        {
            public const long TypeID = 0x13;
            public override void Act(params object[] args)
            {
                if (this.Life.Energy < this.EnergyCost.Value)
                {
                    this.Life.Stimulate(this.Life.EnergyLowStimulus);
                    return;
                }
                this.Life.Energy -= this.EnergyCost;
                SubstanceCapsule res = null;
                if (args.Length>0&&args[0] is Substance)
                {
                    res = this.Life.Resources[args[0] as Substance];
                }
                else if (args.Length > 0 && args[0] is SubstanceCapsule)
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

            public override Behaviour Clone(Life life)
            {
                return new EnergyToResource(life, this.EnergyCost.Value);
            }

            public EnergyToResource(Life life, double energyCost):base(TypeID, life, energyCost)
            {
                this.Name = "EnergyToSubstance";
                this.Description = "Transfer energy into substance.";
            }
        }

        public class Cure : Behaviour
        {
            public const long TypeID = 0x14;

            public Cure(Life life, double energyCost) : base(TypeID, life, energyCost)
            {
                this.Name = "Cure";
                this.Description = "Increase Health which will cost energy.";
            }

            public override void Act(params object[] args)
            {
                if (this.Life.Energy < this.EnergyCost.Value)
                {
                    this.Life.Stimulate(this.Life.EnergyLowStimulus);
                    return;
                }
                this.Life.Energy -= this.EnergyCost;
                SubstanceCapsule res = null;
                double rate = 0;
                foreach (var obj in args)
                {
                    if (obj is SubstanceCapsule)
                        res = obj as SubstanceCapsule;
                    if (obj is double || obj is long || obj is int)
                        rate += Convert.ToDouble(obj);
                }
                if (rate == 0)
                    rate = 1;
                rate = (rate * Life.World.Random.NextDouble()) % 1.0;
                // cost resource
                if (res != null)
                {
                    var energy = res.TransferToEnergy(res.Amount * rate);
                    Life.Health.Value += energy;
                }
                //cost energy
                else
                {
                    var energy = Life.Energy * rate;
                    Life.Energy -= energy;
                    Life.Health.Value += energy;
                }
            }

            public override Behaviour Clone(Life life)
            {
                return new Cure(life, this.EnergyCost.Value);
            }
        }

        public class Autotomy : Behaviour
        {
            public const long TypeID = 0x15;
            public Autotomy(Life life, double energyCost) : base(TypeID, life, energyCost)
            {
                this.Name = "Autotomy";
                this.Description = "Damage itself to gain Energy.";
                
            }

            public override void Act(params object[] args)
            {
                if (this.Life.Energy < this.EnergyCost.Value)
                {
                    this.Life.Stimulate(this.Life.EnergyLowStimulus);
                    return;
                }
                this.Life.Energy -= this.EnergyCost;
                SubstanceCapsule res = null;
                double rate = 0;
                foreach (var obj in args)
                {
                    if (obj is SubstanceCapsule)
                        res = obj as SubstanceCapsule;
                    if (obj is double || obj is long || obj is int)
                        rate += Convert.ToDouble(obj);
                }
                if (rate == 0)
                    rate = 1;
                rate = (rate * Life.World.Random.NextDouble()) % 1.0;
                //gain resource
                if (res != null)
                {
                    var energy = rate * Life.Health.Value;
                    res.TransferFromEnergy(energy);
                }
                //gain energy
                else
                {
                    var energy = rate * Life.Health.Value;
                    Life.Energy += energy;
                }
            }

            public override Behaviour Clone(Life life)
            {
                return new Autotomy(life, this.EnergyCost.Value);
            }
        }

        public class CreateSubstance : Behaviour
        {
            public const long TypeID = 0x16;
            public CreateSubstance(Life life, double energyCost) : base(TypeID, life, energyCost)
            {
                this.Name = "CreateSubstance";
                this.Description = "Create a new type of substance.";
            }

            public override void Act(params object[] args)
            {
                if (this.Life.Energy < this.EnergyCost.Value)
                {
                    this.Life.Stimulate(this.Life.EnergyLowStimulus);
                    return;
                }
                this.Life.Energy -= this.EnergyCost;
                double param = 0;
                foreach(var obj in args)
                {
                    if (obj is double || obj is long || obj is int)
                        param += Convert.ToDouble(obj);
                }
                var fromEnergy = param * Life.World.Random.NextDouble();
                var substance = new Substance(
                    Life.World,
      /*to Energy*/ fromEnergy * (param * (0.95 + 0.06 / (Life.World.Random.NextDouble() - 1.065))),
                    fromEnergy
                    );
                var energy = Life.Energy * (-0.06 / (Life.World.Random.NextDouble() - 1.065));
                var v = Math.Tan(0.89 * Math.PI * (Life.World.Random.NextDouble() - 0.49)) * 7 + 50;
                var min = (Math.Tan(0.85 * Math.PI * (Life.World.Random.NextDouble() - 0.466)) / 10 + 0.3) * 0.5 + 0.05;
                var amount = substance.FromEnergy * energy;
                var limit = v * (Math.Tan(0.85 * Math.PI * (Life.World.Random.NextDouble() - 0.466)) / 10 + 0.3);
                var res = new SubstanceCapsule(Life, substance, amount, min + v, min, limit);
                Life.AddResource(res);
            }

            public override Behaviour Clone(Life life)
            {
                return new CreateSubstance(life, this.EnergyCost.Value);
            }
        }
    }
}
