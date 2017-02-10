using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    abstract public class Behaviour : IBehaviour, IIdentification, ILifeComponent, IRandomObject
    {
        public string Description { get; set; }

        public string ID { get; set; }

        public Life Life { get; set; }

        public string Name { get; set; }

        public abstract void Act(params object[] args);

        public int Weight { get; set; }

        public Property EnergyCost { get; set; }
    }
}
