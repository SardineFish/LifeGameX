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
        public double MagicFunction(double based, double range)
        {
            return (Math.Tan(0.85 * Math.PI * (Random.NextDouble() - 0.466)) / 10) * range + based;
        }
        public string RegisterSubstance(Substance substance)
        {
            Substances.Add(substance);
            globalCount++;
            return Substances.Count.ToString();
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

        public Thread Thread { get; private set; }

        public WorldStatus Status { get; private set; }

        public long DeltaTime { get; private set; }

        public long Width { get; private set; }
        public long Height { get; private set; }

        public long Time { get; private set; }

        public MapCell[,] Map { get; set; }

        public List<Substance> Substances { get; private set; }

        public event EventHandler OnUpdate;

        public event UnhandledExceptionEventHandler OnError;

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
            this[x, y].Add(life);
            this.Lives.Add(life);
        }

        public void Kill(Life life)
        {
            if (!life.Alive)
                throw new ArgumentException("This life is not alive.");
            this[life.X, life.Y].Remove(life);
            life.Alive = false;
            this.Lives.Remove(life);
        }

        public World(long width, long height)
        {
            this.Width = width;
            this.Height = height;
            this.Map = new MapCell[width, height];
            this.Substances = new List<LifeGameX.Substance>();
            for(var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    this.Map[x, y] = new MapCell(this, x, y);
                }
            }
            this.Lives = new List<Life>();
        }

        public void AddInitialResource(Substance substance,double amount)
        {
            for(var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    this.Map[x, y].Add(substance, amount);
                }
            }
        }

        internal void Update(long dt)
        {
            this.Time += dt;
#if !DEBUG
            try
            {
#endif
                var count = this.Lives.Count;
                for(var i = 0; i < count && i<this.Lives.Count; i++)
                {
                    var life = this.Lives[i];
                    life.Update(dt);
                }
                OnUpdate?.Invoke(this, new EventArgs());
#if !DEBUG
            }
            catch (ThreadAbortException)
            {
                return;
            }
            catch (Exception ex)
            {
                this.OnError?.Invoke(this, new UnhandledExceptionEventArgs(ex, false));
            }
#endif
        }

        long lastTime = 0;
        void timerCallback(object state)
        {
#if !DEBUG
            try
            {
#endif
                long t = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
                var dt = t - lastTime;
                lastTime = t;
                Update(dt);
#if !DEBUG
            }
            catch (ThreadAbortException)
            {
                return;
            }
            catch (Exception ex)
            {
                this.OnError?.Invoke(this, new UnhandledExceptionEventArgs(ex, false));
                
            }
#endif
        }

        void start()
        {
#if DEBUG
            while(true)
            {
                Thread.Sleep((int)DeltaTime);
                timerCallback(null);
            }
#endif
#if !DEBUG
            Timer timer;
            try
            {
                lastTime = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
                timer = new Timer(timerCallback, null, 0, DeltaTime);
            }
            catch (ThreadAbortException)
            {
                return;
            }
            catch (Exception ex)
            {
                this.OnError?.Invoke(this, new UnhandledExceptionEventArgs(ex, false));
            }
#endif
        }

        public void Start(long dt)
        {
            if (Status == WorldStatus.Running)
                return;
            this.DeltaTime = dt;
            Thread = new Thread(start);
            Status = WorldStatus.Running;
            Thread.Start();
        }

        public void Stop()
        {
            this.Status = WorldStatus.Stopping;
            Thread.Abort();
            this.Status = WorldStatus.NotRun;
        }
    }
}
