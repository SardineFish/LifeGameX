using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    /// <summary>
    /// A list of IRandomObject which can randomly get an item by the weight of each item. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RandomList<T> : List<T> where T : IRandomObject
    {
        /// <summary>
        /// 
        /// </summary>
        Random random = new Random();
        /// <summary>
        /// Randomly get an item. 
        /// </summary>
        /// <returns>An item.</returns>
        public T GetRandom()
        {
            
            long weightSum = 0;
            foreach(var item in this)
            {
                weightSum += item.Weight;
            }
            foreach (var item in this)
            {
                double p = (double)item.Weight / (double)weightSum;
                if (random.NextDouble() < p)
                    return item;
                weightSum -= item.Weight;
            }
            return this[this.Count - 1];
        }
    }
}
