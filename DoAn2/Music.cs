using System;
using System.Windows.Media;

namespace DoAn2
{
    public static class Music
    {
        private static MediaPlayer player;

        public static void Play()
        {
            if(player == null)
            {
                player = new MediaPlayer();
                player.Open(new Uri("Sound/background.mp3", UriKind.Relative));
                player.MediaEnded += Player_MediaEnded;          
            }
            player.Play();
        }

        private static void Player_MediaEnded(object sender, EventArgs e)
        {
            player.Position = TimeSpan.Zero;
            player.Play();
        }

        public static void Stop()
        {
            try
            {
                player.Stop();
            }
            catch 
            {

            }
        }

    }
}
