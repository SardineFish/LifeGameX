using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    public interface IPositionable
    {
        long X { get; set; }
        long Y { get; set; }
        World World { get; set; }
    }
}
