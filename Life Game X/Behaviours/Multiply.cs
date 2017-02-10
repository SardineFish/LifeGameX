using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX.Behaviours
{
    public class Multiply : Behaviour
    {
        public int Count { get; set; }
        public override void Act(params object[] args)
        {
            if (this.Life.Energy < this.EnergyCost.Value)
            {
                this.Life.Stimulate(this.Life.EnergyLowStimulus);
                return;
            }
            if (args.Length > 0 && (args[0] is double || args[0] is long || args[0] is int))
            {
                var x = 1 + (int)args[0];
                x = (x + Count) % Count;

            }
        }
    }
}
