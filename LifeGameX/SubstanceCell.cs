using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    public class SubstanceCell: IPositionable
    {
        public Substance Substance { get; set; }
        public double Amount { get; set; }
        public World World { get; set; }

        public long X { get; set; }

        public long Y { get; set; }

        public SubstanceCell(Substance substance, double amount = 0)
        {
            if (substance == null)
                throw new ArgumentNullException("Substance required.");
            this.Substance = substance;
            this.Amount = amount;
        }
    }
}
