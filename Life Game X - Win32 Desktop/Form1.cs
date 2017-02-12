using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LifeGameX;

namespace Life_Game_X
{
    public partial class Form1 : Form
    {
        public Dictionary<Substance, Color> SubstanceColors = new Dictionary<Substance, Color>();
        public Dictionary<string, Color> SpeciesColor = new Dictionary<string, Color>();
        public Bitmap Img;
        public Graphics GDI;
        World World;
        public Form1()
        {
            InitializeComponent();
            Img = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
            World = new World(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
            World.OnUpdate += World_OnUpdate;
            World.OnError += World_OnError;
            GDI = Graphics.FromImage(Img);
            pictureBox1.Image = Img;
            CheckForIllegalCrossThreadCalls = false;
            DoubleBuffered = true;
            var nutrition = new Substance(World, 0.96, 1);
            World.AddInitialResource(nutrition, 100);
            World.Start(16);
            var life = new Life(World.CreateSpeciesID(), World);
            World.Birth(life, World.Width / 2, World.Height / 2);
        }

        public Color RandomSpeciesColor()
        {
            return Color.FromArgb(255, World.Random.Next(256), World.Random.Next(256), World.Random.Next(256));
        }

        public Color RandomSubstanceColor()
        {
            return Color.FromArgb(100, World.Random.Next(256), World.Random.Next(256), World.Random.Next(256));
        }

        private void World_OnError(object sender, UnhandledExceptionEventArgs e)
        {
            var x = e.ExceptionObject;
        }

        private void World_OnUpdate(object sender, EventArgs e)
        {
            GDI.Clear(Color.Black);
            for (var y = 0; y < World.Height; y++)
            {
                for(var x = 0; x < World.Width; x++)
                {
                    var cell = World[x, y];
                    foreach (var obj in cell)
                    {
                        if(obj is Life)
                        {
                            var life = obj as Life;
                            if (!SpeciesColor.ContainsKey(life.Species))
                            {
                                SpeciesColor[life.Species] = RandomSpeciesColor();
                            }
                            var color = SpeciesColor[life.Species];
                            GDI.DrawRectangle(new Pen(new SolidBrush(color)), x, y, 1, 1);
                        }
                        else if (obj is SubstanceCell)
                        {
                            var substanceCell = obj as SubstanceCell;
                            if (!SubstanceColors.ContainsKey(substanceCell.Substance))
                                SubstanceColors[substanceCell.Substance] = RandomSubstanceColor();
                            var color = SubstanceColors[substanceCell.Substance];
                            int alpha = (int)((255 * substanceCell.Amount / 2000));
                            if (alpha > 255)
                                alpha = 255;
                            if (alpha < 0)
                                alpha = 0;
                            GDI.DrawRectangle(new Pen(new SolidBrush(Color.FromArgb(alpha, color))), x, y, 1, 1);
                        }
                    }
                }
            }
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var life = new Life(World.CreateSpeciesID(), World);
            World.Birth(life, e.X, e.Y);
        }
    }
}
