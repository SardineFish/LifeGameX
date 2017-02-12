using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    public class SubstanceCapsule : ILifeComponent, IIdentification, IRandomObject
    {
        public const double FewRate = 0.2;
        public const double MuchRate = 0.8;
        public string Description { get; set; }

        public string ID { get; set; }

        public Life Life { get; set; }

        public string Name { get; set; }

        public Substance Substance { get; set; }

        double amount;
        public double Amount
        {
            get
            {
                return amount;
            }
            set
            {
                var lim = this.TransferLimit == null ? 0 : this.TransferLimit.Value;
                if (value - this.amount > lim)
                {
                    value = this.amount + lim;
                }
                if (value > Max.Value)
                    value = Max.Value;
                if (value < Min.Value)
                    value = Min.Value;
                this.amount = value;
                if ((double)this.amount / (double)this.Max.Value >= MuchRate)
                {
                    Life.Stimulate(Much, this, amount);
                }
                else if ((double)this.amount / (double)this.Max.Value <= FewRate)
                {
                    Life.Stimulate(Few, this, amount);
                }
                if (this.amount == this.Min)
                {
                    Life.Stimulate(Empty, this, amount);
                }
            }
        }

        public Property Max { get; set; }

        public Property Min { get; set; }

        public Property TransferLimit { get; set; }

        public Stimulus Few { get; private set; }

        public Stimulus Much { get; private set; }

        public Stimulus Empty { get; private set; }

        public double Weight { get; set; }

        public double TransferToEnergy(double amount)
        {
            if (amount > TransferLimit.Value)
                amount = TransferLimit.Value;
            if (amount < 0)
                amount = 0;
            if (this.Amount - amount < this.Min.Value)
                amount = this.Amount - this.Min.Value;
            this.Amount -= amount;
            return this.Substance.ToEnergy * amount;
        }

        public double TransferFromEnergy(double energy)
        {
            double amount = this.Substance.FromEnergy * energy;
            if (amount > TransferLimit.Value)
                amount = TransferLimit.Value;
            if (this.Amount + amount > this.Max.Value)
                amount = this.Max.Value - this.Amount;
            this.amount += amount;
            return amount;
        }

        public double TransferTo(SubstanceCapsule res,double amount)
        {
            if (res == null)
                throw new ArgumentNullException("The resource cannot be null.");
            var energy = this.TransferToEnergy(amount);
            amount = res.Substance.FromEnergy * energy;
            res.Amount += amount;
            return amount;
        }

        public double Take(double amount)
        {
            if (amount > TransferLimit)
                amount = TransferLimit.Value;
            if (this.Amount - amount < Min)
                amount = this.Amount - Min;
            return amount;
        }

        public double Add(double amount)
        {
            if (amount > TransferLimit)
                amount = TransferLimit.Value;
            if (amount + this.Amount > Max)
                amount = Max - this.Amount;
            this.Amount += amount;
            return amount;
        }

        internal void SetAmount(double amount)
        {
            this.amount = amount;
        }

        public SubstanceCapsule(Life life, Substance substance, double amount = 0, double max = 100, double min = 0, double transferLimit = 100)
        {
            if (life == null)
                throw new ArgumentNullException("The life cannot be null.");
            if (substance == null)
                throw new ArgumentNullException("The substance cannot be null.");
            if (life.Resources.ContainsKey(substance))
                throw new ArgumentException("This life has already had the capsule of this substance.");
            this.Life = life;
            this.Substance = substance;
            this.amount = amount;
            this.Max = new Property(life, max);
            this.Min = new Property(life, min);
            this.TransferLimit = new Property(life, transferLimit);
            this.Few = new Stimulus(life, life.NoReactionStimulus);
            this.Few.Description = "The " + this.Name + " of " + life.Name + " is too less.";
            this.Much = new Stimulus(life, life.NoReactionStimulus);
            this.Much.Description = "The " + this.Name + " of " + life.Name + " is too much.";
            this.Empty = new Stimulus(life, life.NoReactionStimulus);
            this.Much.Description = "The " + this.Name + " of " + life.Name + " is empty!";
            this.Weight = 100;
        }
    }
}
