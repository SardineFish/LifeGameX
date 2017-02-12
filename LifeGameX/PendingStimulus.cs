using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    public class PendingStimulus
    {
        public Stimulus Stimulus { get; set; }
        public object[] Args { get; set; }

        public PendingStimulus(Stimulus stimulus,object[] args)
        {
            this.Stimulus = stimulus;
            this.Args = args;
        }
    }
}
