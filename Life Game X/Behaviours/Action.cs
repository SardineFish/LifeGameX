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
                var distance = Life.Energy / 10;
                foreach(var res in Life.SubstanceCapsuleList)
                {
                    Life.ReleaseSubstance(res.Substance, res.Amount, distance);
                }
                Life.Energy = 0;
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
    }
}
