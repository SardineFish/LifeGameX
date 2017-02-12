using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    public class Substance : IIdentification, IConvertible
    {
        public World World { get; private set; }
        public string Description { get; set; }

        public Substance Create(World world)
        {
            if (world == null)
                throw new ArgumentNullException("World required.");
            Substance substance = new Substance(world);
            substance.ID = world.RegisterSubstance(this);
            substance.Name = "Substance-" + this.ID;
            substance.Description = substance.Name;
            substance.toEnergy = 1;
            substance.fromEnergy = 1;
            return substance;
        }

        double fromEnergy;
        public double FromEnergy
        {
            get
            {
                return fromEnergy;
            }

            set
            {
                if (value < toEnergy)
                    return;
                fromEnergy = value;
            }
        }

        double toEnergy;
        public double ToEnergy
        {
            get
            {
                return toEnergy;
            }

            set
            {
                if (fromEnergy < value)
                    return;
                toEnergy = value;
            }
        }

        public string ID { get; set; }

        public string Name { get; set; }

        private Substance(World world)
        {
            if (world == null)
                throw new ArgumentNullException("World required.");
            this.World = world;
            
        }

        public Substance(World world, double toEnergy,double fromEnergy)
        {
            this.World = world;
            this.ID = world.RegisterSubstance(this);
            this.Name = "Substance-" + this.ID;
            this.Description = "A substance.";
            this.FromEnergy = fromEnergy;
            this.ToEnergy = toEnergy;
        }
    }
}
