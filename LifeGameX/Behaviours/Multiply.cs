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
            long count = 0;
            double param = 0;
            foreach (var obj in args)
            {
                if (obj is double || obj is long || obj is int)
                {
                    param += Convert.ToDouble(obj);
                }
            }
            if (param <= 0)
                param = 1;
            param = ((Math.Tan(0.85 * Math.PI * ((param * Life.World.Random.NextDouble()) % 1 - 0.466)) / 10) * 8 + 3);
            count = (long)param;
            if (args.Length <= 0)
                count = 1;
            for (var i = 0; i < count; i++)
            {
                long x = Life.World.Random.Next(3) - 1;
                long y = Life.World.Random.Next(3) - 1;
                x += Life.X;
                y += Life.Y;
                var child = Life.Clone();

                child.Energy = Life.Energy / (count + 1);

                foreach (var item in Life.Resources)
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
            Life.Energy = Life.Energy / (count + 1);
        }

        public override Behaviour Clone(Life life)
        {
            return new Multiply(life, this.EnergyCost.Value);
        }

        public Multiply(Life life, double energyCost) : base(TypeID, life, energyCost)
        {
            this.Name = "Multiply";
            this.Description = "Multiply itself.";
            this.Weight = 50;
        }
    }
}
