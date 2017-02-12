using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX.Behaviours
{
    public class Multiply : Behaviour
    {
        public const long TypeID = 0x20;
        public int Count { get; set; }
        public override void Act(params object[] args)
        {
            if (this.Life.Energy < this.EnergyCost.Value)
            {
                this.Life.Stimulate(this.Life.EnergyLowStimulus);
                return;
            }
            this.Life.Energy -= this.EnergyCost;
            var count = 0;
            if (args.Length <= 0)
                count = 1;
            if (args[0] is double || args[0] is long || args[0] is int)
            {
                count = 1 + (int)args[0];
                count = (count + Count) % Count;
                for(var i = 0; i < count; i++)
                {
                    var x = Life.World.Random.Next(3) - 1;
                    var y = Life.World.Random.Next(3) - 1;
                    var child = Life.Clone();

                    child.Energy = this.EnergyCost / (count + 1);

                    foreach(var item in Life.Resources)
                    {
                        var substance = item.Key;
                        var res = item.Value;
                        child.Resources[substance].SetAmount(res.Amount / (count + 1));
                    }

                    child.World.Birth(child, x, y);
                }
                //cut down resource
                foreach (var item in Life.Resources)
                {
                    var substance = item.Key;
                    var res = item.Value;
                    res.SetAmount(res.Amount / (count + 1));
                }
            }

        }

        public override Behaviour Clone(Life life)
        {
            return new Multiply(life, this.EnergyCost.Value);
        }

        public Multiply(Life life, double energyCost) : base(TypeID, life, energyCost)
        {
            this.Name = "Multiply";
            this.Description = "Multiply itself.";
        }
    }
}
