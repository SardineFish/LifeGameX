using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    public class ReactionRecord
    {
        public Stimulus Stimulus { get; set; }
        public Reaction Reaction { get; set; }
        public ReactionRecord(Stimulus stimulus,Reaction reaction)
        {
            if (stimulus == null)
                throw new ArgumentNullException("The Stimulus cannot be null.");
            if (reaction == null)
                throw new ArgumentNullException("The reaction cannot be null.");
            this.Stimulus = stimulus;
            this.Reaction = reaction;
        }
    }
}
