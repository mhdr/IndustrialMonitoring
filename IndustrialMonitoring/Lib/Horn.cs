using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Application = System.Windows.Forms.Application;

namespace IndustrialMonitoring.Lib
{
    public class Horn
    {
        private string hornFile;
        private MediaPlayer Player;
        private bool isPlaying = false;

        public static Horn hornInstance;

        public static Horn GetInstance()
        {
            if (hornInstance == null)
            {
                hornInstance=new Horn();
                return hornInstance;
            }
            else
            {
                return hornInstance;
            }
        }

        private Horn()
        {
            hornFile = Path.Combine(Application.StartupPath, "Resources/horn.mp3");
            Player=new MediaPlayer();
            Player.Open(new Uri(hornFile));
            Player.MediaEnded += Player_MediaEnded;
        }

        void Player_MediaEnded(object sender, EventArgs e)
        {
            isPlaying = false;
        }

        public void Start()
        {
            if (!isPlaying)
            {
                
                Player.Play();
                Player.Position=new TimeSpan(0,0,0,0,0);
                isPlaying = true;
            }

        }

        public void Stop()
        {
            Player.Stop();
            isPlaying = false;
        }
    }
}
