using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FLAPPER
{
    public partial class Form1 : Form
    {
        Random rm;
        bool botsing;
        int score;
        string gamestatus;
        int afstandhorizontaal = 0;
        int afstandverticaal = 0;
        Bitmap achtergroundpanel;
        Bitmap imgbuisboven;
        Bitmap imgbuisbeneden;
        Rectangle[] buizenboven;
        Rectangle[] buizenbeneden;
        Bitmap imgflapje;
        int yflappie;
        Graphics g;
        Rectangle rflappie;
        bool stop;


        public Form1()
        {
            InitializeComponent();
            gameitemconfig();
        }

        private void gameitemconfig()
        {
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty
                | System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, panel1, new object[] { true });

            botsing = false;
            rm = new Random();
            score = 0;
            gamestatus = "";
            afstandhorizontaal = 350;
            afstandverticaal = 150;
            achtergroundpanel = new Bitmap("back.png");

            //buizen
            imgbuisboven = new Bitmap("pipenaarboven.png");
            imgbuisbeneden = new Bitmap("pipenaarbeneden.png");
            buizenboven = new Rectangle[2];
            buizenbeneden = new Rectangle[2];

            maakbuizen(600);

            imgflapje = new Bitmap("xatu.png");
            imgflapje.MakeTransparent();
            yflappie = panel1.Height / 2;
            stop = false;
        }

        private void maakbuizen(int beginX)
        {
            int[] hoogtenieuwpaar = new int[] { 0, rm.Next(-150, 0) };
            for (int i = 0; i < buizenboven.Length; i++)
            {
                int ybegin = -250 + hoogtenieuwpaar[i];
                buizenbeneden[i] = new Rectangle(beginX + (afstandhorizontaal * i), ybegin,
                    imgbuisbeneden.Width, imgbuisbeneden.Height);

                buizenboven[i] = new Rectangle(beginX + (afstandhorizontaal * i), ybegin + afstandverticaal + imgbuisbeneden.Height,
                    imgbuisboven.Width, imgbuisboven.Height);

            }
            Console.WriteLine(buizenbeneden[0].Y + "" + buizenbeneden[1].Y);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            gameitemconfig(); 
            timer1.Start();
            timer2.Start();
            stop = true;

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            toonbuizen();
            panel1.Invalidate();
        }

        private void toonbuizen()
        {
            yflappie += 5;
            if (buizenboven[0].X <= 0 - imgbuisbeneden.Width-afstandhorizontaal)
            {
                score += 2;
                maakbuizen(panel1.Width - rm.Next(50, 120));
            }
            else
            {
                for (int i = 0; i < buizenboven.Length; i++)
                {
                    buizenboven[i].X -= 10;
                    buizenbeneden[i].X -= 10;
                }
            }
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < buizenbeneden.Length; i++)
            {
                
                botsing = buizenbeneden[i].IntersectsWith(rflappie) ||
                    buizenboven[i].IntersectsWith(rflappie) ||
                    (yflappie + 40) > panel1.Height;

                if (botsing) break;
            }

            if (botsing)
            {
                timer2.Stop();
                timer1.Stop();
                gamestatus = "game over";
            }
            else yflappie += rm.Next(-2, 6);
            panel1.Invalidate();
        }

        private void spelweergave(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            //achtergrond
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //
            g.DrawImage(achtergroundpanel, new Rectangle(0, 0, 5000, 1500), 0, 0, panel1.Width, panel1.Height, GraphicsUnit.Millimeter, null);
            //buizen
            for (int i = 0; i < buizenbeneden.Length; i++)
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //
                //
                g.DrawImage(imgbuisboven, buizenboven[i], 0, 0, imgbuisboven.Width, imgbuisboven.Height, GraphicsUnit.Pixel, null);
                //
                g.DrawRectangle(new Pen(Color.Red), buizenboven[i]);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //
                g.DrawImage(imgbuisbeneden, buizenbeneden[i], 0, 0, imgbuisbeneden.Width, imgbuisbeneden.Height, GraphicsUnit.Pixel, null);
                g.DrawRectangle(new Pen(Color.Red), buizenbeneden[i]);
            }

            //vogel
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            rflappie = new Rectangle(100, yflappie, 70, 40);
            //
            g.DrawImage(imgflapje, rflappie, 0, 0, 220, 140, GraphicsUnit.Millimeter, null);
            Font fn = new Font("Callibri", 24, FontStyle.Bold);
            g.DrawRectangle(new Pen(Color.Red), rflappie);
            g.DrawString(gamestatus, fn, new SolidBrush(Color.Yellow), rflappie.X + 70, rflappie.Y);
            g.DrawString(score.ToString(),fn,new SolidBrush(Color.Yellow),30,50);

        }

        private void stijgen(object sender, EventArgs e)
        {
            if (stop)
            {
                if (botsing)
                {
                    stop = false;
                }
                else
                {
                    yflappie -= 40;
                    panel1.Invalidate();
                }
            }
        }

        private void validatietoets(object sender, KeyEventArgs e)
        {
            if (stop)
            {
                if (botsing)
                {
                    stop = false;
                }
                else
                {
                    if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.S) yflappie += 40;
                    if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Z) yflappie -= 40;
                    panel1.Invalidate();
                }
            }
        }
    }
 }

