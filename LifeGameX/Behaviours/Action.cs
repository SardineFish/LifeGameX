using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX.Behaviours
{
    public static class Action
    {

        public class Die : Behaviour
        {
            public const long TypeID = 0x41;
            public Die(Life life) : base(TypeID, life, 0)
            {
                this.Name = "Die";
                this.Description = "Die.";
                this.Weight = 1;
            }

            public override void Act(params object[] args)
            {
                var distance = Math.Log(Life.Energy);
                if (distance < 1)
                    distance = 1;
                foreach(var res in Life.SubstanceCapsuleList)
                {
                    Life.ReleaseSubstance(res.Substance, res.Amount, distance);
                }
                Life.World.Kill(this.Life);
            }

            public override Behaviour Clone(Life life)
            {
                return new Die(life);
            }
        }
        public class Move : Behaviour
        {
            public const long TypeID = 0x42;
            public Move(Life life, double energyCost) : base(TypeID, life, energyCost)
            {
                this.Name = "Move";
                this.Description = "Move to somewhere.";
            }

            public override void Act(params object[] args)
            {
                if (Life.Energy < this.EnergyCost)
                {
                    this.Life.Stimulate(this.Life.EnergyLowStimulus);
                    return;
                }
                Life.Energy -= this.EnergyCost;
                long x = 0, y = 0;
                if (args.Length <= 0)
                {
                    x = Life.World.Random.Next(3) - 1;
                    y = Life.World.Random.Next(3) - 1;
                }
                else if (args[0] is double || args[0] is long || args[0] is int)
                {
                    var n = Convert.ToDouble(args[0]);
                    x = (long)((n * Life.World.Random.Next(3)) % 3) - 1;
                    y = (long)((n * Life.World.Random.Next(3)) % 3) - 1;
                }
                x += Life.X;
                y += Life.Y;
                if (x < 0)
                    x = 0;
                else if (x > Life.World.Width)
                    x = Life.World.Width - 1;
                if (y < 0)
                    y = 0;
                else if (y > Life.World.Height)
                    y = Life.World.Height - 1;
                this.Life.Move(x, y);
            }


            public override Behaviour Clone(Life life)
            {
                return new Move(life, this.EnergyCost.Value);
            }
        }
        public class Absorb : Behaviour
        {
            public const long TypeID = 0x43;

            public Absorb(Life life, double energyCost) : base(TypeID, life, energyCost)
            {
                this.Name = "Absorb";
                this.Description = "Absorb and storage substance from World.";
            }

            public override void Act(params object[] args)
            {
                if (Life.Energy < this.EnergyCost)
                {
                    this.Life.Stimulate(this.Life.EnergyLowStimulus);
                    return;
                }
                
                Life.Energy = Life.Energy - this.EnergyCost;
                double param = 0;
                Substance substance = null;
                SubstanceCell resCell = null;
                foreach (object obj in args)
                {
                    if (obj is double || obj is long || obj is int)
                    {
                        param += Convert.ToDouble(obj);
                    }
                    else if (substance == null)
                    {
                        if (obj is Substance)
                            substance = obj as Substance;
                        else if (obj is SubstanceCapsule)
                            substance = (obj as SubstanceCapsule).Substance;
                        else if (obj is SubstanceCell)
                            substance = (obj as SubstanceCell).Substance;
                    }
                }
                var cell = Life.World[Life.X, Life.Y];
                if ((substance == null || !cell.Substances.ContainsKey(substance)) && cell.Substances.Count > 0)
                {
                    substance = cell.Substances.Keys.ToArray()[(int)((param * Life.World.Random.NextDouble()) % 1) * cell.Substances.Count];
                }

                if (substance == null || !cell.Substances.ContainsKey(substance))
                {
                    return;
                }

                resCell = cell.Substances[substance];
                double amount = 0;

                // create SubstanceCapsule
                if(!Life.Resources.Keys.Contains(substance))
                {
                    var v = ((Math.Tan(0.85 * Math.PI * (Life.World.Random.NextDouble() - 0.466)) / 10) * 500 + 200);
                    var min = ((Math.Tan(0.85 * Math.PI * (Life.World.Random.NextDouble() - 0.466)) / 10 + 0.3) * 0.5 + 0.05);
                    var limit = v * (Math.Tan(0.85 * Math.PI * (Life.World.Random.NextDouble() - 0.49)) / 10 + 0.3);
                    amount = limit * (Math.Tan(0.85 * Math.PI * (Life.World.Random.NextDouble() - 0.49)) / 10 + 0.5);
                    amount = cell.GetSubstance(substance, amount);
                    var res = new SubstanceCapsule(Life, substance, amount, min + v, min, limit);
                    Life.AddResource(res);
                }
                else
                {
                    var res = Life.Resources[substance];
                    amount = res.TransferLimit * (Math.Tan(0.85 * Math.PI * (Life.World.Random.NextDouble() - 0.49)) / 10 + 0.5);
                    amount = cell.GetSubstance(substance, amount);
                    res.Add(amount);
                }
            }

            public override Behaviour Clone(Life life)
            {
                return new Absorb(life, this.EnergyCost.Value);
            }
        }

        public class Release : Behaviour
        {
            public const long TypeID = 0x44;

            public Release(Life life, double energyCost) : base(TypeID, life, energyCost)
            {
                this.Name = "Release";
                this.Description = "Release substance to World.";
            }

            public override void Act(params object[] args)
            {
                if (Life.Energy < this.EnergyCost)
                {
                    this.Life.Stimulate(this.Life.EnergyLowStimulus);
                    return;
                }
                Life.Energy -= this.EnergyCost;
                double param = 0;
                Substance substance = null;
                SubstanceCapsule res = null;
                foreach (object obj in args)
                {
                    if (obj is double || obj is long || obj is int)
                    {
                        param += Convert.ToDouble(obj);
                    }
                    else if (substance == null)
                    {
                        if (obj is Substance)
                            substance = obj as Substance;
                        else if (obj is SubstanceCapsule)
                        {
                            substance = (obj as SubstanceCapsule).Substance;
                        }
                    }
                }
                if (substance == null || !Life.Resources.ContainsKey(substance))
                {
                    if (Life.Resources.Count <= 0)
                        return;
                    substance = Life.SubstanceCapsuleList.GetRandom().Substance;
                }

                res = Life.Resources[substance];
                var amount = res.TransferLimit * Math.Tan(0.85 * Math.PI * (Life.World.Random.NextDouble() - 0.49)) / 10 + 0.5;
                amount = res.Take(amount);
                Life.World[Life.X, Life.Y].Add(substance, amount);
            }

            public override Behaviour Clone(Life life)
            {
                return new Release(life, this.EnergyCost.Value);
            }
        }
    }
}
