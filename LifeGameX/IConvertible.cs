using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    public interface IConvertible
    {
        double ToEnergy { get; set; }
        double FromEnergy { get; set; }
    }
}
