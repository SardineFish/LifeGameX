using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    public class World
    {
        int substanceCount = 0;
        public string CreateSubstanceID()
        {
            substanceCount++;
            globalCount++;
            return substanceCount.ToString();
        }

        int livesCount = 0;
        public string CreateLifeID()
        {
            livesCount++;
            globalCount++;
            return livesCount.ToString();
        }

        int globalCount = 0;
        public string CreateID()
        {
            globalCount++;
            return globalCount.ToString();
        }

        long speciesCount = 0;
        public string CreateSpeciesID()
        {
            speciesCount++;
            globalCount++;
            return speciesCount.ToString();
        }

        Random random = new Random();
        public Random Random
        {
            get
            {
                return random;
            }
        }

        public MapCell[,] Map { get; set; }

        public MapCell this[long x, long y]
        {
            get
            {
                return Map[x, y];
            }
        }

        public List<Life> Lives { get; set; }

        public void Birth(Life life,long x,long y)
        {
            if (life.Alive)
                throw new ArgumentException("This life is alive.");
            life.Alive = true;
            this[x, y].Push(life);
            this.Lives.Add(life);
        }

        public World(long width, long height)
        {
            this.Map = new MapCell[width, height];
            for(var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    this.Map[x, y] = new MapCell(this, x, y);
                }
            }
            this.Lives = new List<Life>();
        }


    }
}
