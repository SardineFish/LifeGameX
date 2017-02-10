using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    public interface IReaction
    {
        Stimulus Source { get; set; }
        RandomList<Behaviour> Behaviours { get; set; }

    }
}
