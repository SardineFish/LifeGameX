using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX.Behaviours
{
    public class NoResponse : Behaviour
    {
        public const long TypeID= 0x00;
        public NoResponse(Life life) : base(TypeID, life, 0)
        {
            this.Name = "NoResponse";
            this.Description = "Nothing could be better.";
        }

        public override void Act(params object[] args)
        {
            return;
        }

        public override Behaviour Clone(Life life)
        {
            return new NoResponse(life);
        }
    }
}
