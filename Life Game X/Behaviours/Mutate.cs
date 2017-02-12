using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX.Behaviours
{
    public class Mutate : Behaviour
    {
        public const long TypeID = 0x200;
        public Mutate(Life life, double energyCost) : base(TypeID, life, energyCost)
        {
            this.Name = "Mutate";
            this.Description = "Change the species, and rebuild some reactions.";
            this.Weight = 10;
        }

        public override void Act(params object[] args)
        {
            if (this.Life.Energy < this.EnergyCost.Value)
            {
                this.Life.Stimulate(this.Life.EnergyLowStimulus);
                return;
            }
            this.Life.Energy -= this.EnergyCost;
            Life.Species = Life.World.CreateSpeciesID();
        }

        public override Behaviour Clone(Life life)
        {
            return new Mutate(life, this.EnergyCost.Value);
        }
    }
}
