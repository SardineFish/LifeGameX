using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LifeGameX
{
    public enum WorldStatus
    {
        NotRun=0,
        Running=1,
        Stopping=2,
    }
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

        long stimulusCount = 0;
        public string CreateStimulusID()
        {
            globalCount++;
            stimulusCount++;
            return stimulusCount.ToString();
        }

        Random random = new Random();
        public Random Random
        {
            get
            {
                return random;
            }
        }

        public Task Task { get; private set; }

        public WorldStatus Status { get; private set; }

        public long Width { get; private set; }
        public long Height { get; private set; }

        public long Time { get; private set; }

        public MapCell[,] Map { get; set; }

        public MapCell this[long x, long y]
        {
            get
            {
                if (x < 0 || x > this.Width || y < 0 || y > this.Height)
                {
                    return new MapCell(this, x, y);
                }
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

        public void Kill(Life life)
        {
            if (life.Alive)
                throw new ArgumentException("This life is alive.");
            this[life.X, life.Y].Remove(life);
            life.Alive = false;
            this.Lives.Remove(life);
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

        internal void Update(long dt)
        {
            this.Time += dt;
            foreach (var life in Lives)
            {
                life.Update(dt);
            }
        }

        void start()
        {
            while (true)
            {
                
            }
        }

        public void Start()
        {
            if (this.Status == WorldStatus.Running)
                return;
            Task = new Task(start);
            this.Status = WorldStatus.Running;
            Task.Start();
        }

        public void Stop()
        {
            this.Status = WorldStatus.Stopping;
        }
    }
}
