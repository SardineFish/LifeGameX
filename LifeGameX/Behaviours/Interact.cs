using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX.Behaviours
{
    public class Interact : Behaviour
    {
        public const long TypeID = 0x80;
        public Interact( Life life, double energyCost) : base(TypeID, life, energyCost)
        {
            this.Name = "Interact.";
            this.Description = "Interact with other lives.";
        }

        public override void Act(params object[] args)
        {
            if (this.Life.Energy < this.EnergyCost.Value)
            {
                this.Life.Stimulate(this.Life.EnergyLowStimulus);
                return;
            }
            this.Life.Energy -= this.EnergyCost;
            Life target = null;
            long param = 0;
            if (args.Length > 0)
            {
                foreach (var obj in args)
                {
                    if (target == null && obj is Life)
                    {
                        target = obj as Life;
                    }
                    if(obj is double || obj is long || obj is int)
                    {
                        param += Convert.ToInt64(obj);
                    }
                }
            }
            if (target != null)
                goto Out;
            var lifeList = new List<Life>();
            for(var y = Life.Y - 1; y < Life.Y + 1; y++)
                for(var x=Life.X - 1; x < Life.X + 1; x++)
                    foreach (var obj in Life.World[x, y])
                    {
                        if (obj is Life)
                        {
                            lifeList.Add(obj as Life);
                        }
                    }


            if (lifeList.Count <= 0)
            {
                return;
            }
            target = lifeList[Life.World.Random.Next(lifeList.Count)];

            Out:
            if (target == null)
                return;

            target.Stimulate(target.InteractStimulus, Life, Life.Species, Life.ID, Life.Energy, Life.Resources.Count, param);
        }

        public override Behaviour Clone(Life life)
        {
            return new Interact(life, this.EnergyCost.Value);
        }
    }
}
