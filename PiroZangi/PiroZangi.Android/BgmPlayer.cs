using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Android.Media;

[assembly: Dependency(typeof(PiroZangi.Droid.BgmPlayer))]

namespace PiroZangi.Droid
{
    class BgmPlayer : IMediaPlayer
    {
        MediaPlayer player = null;
        private async Task StartPlayerAsync(string title)
        {
            var resourceId = (int)typeof(Resource.Raw).GetField(title).GetValue(null);

            await Task.Run(() => {
                if (player == null) {
                    player = new MediaPlayer();
                    player.SetAudioStreamType(Stream.Music);
                    player = MediaPlayer.Create(global::Android.App.Application.Context, resourceId);
                    player.Looping = false;
                    player.Start();
                } else {
                    if (player.IsPlaying == true) {
                        player.Pause();
                    } else {
                        player.Start();
                    }
                }
            });
        }

        private void StopPlayer()
        {
            if ((player != null)) {
                if (player.IsPlaying) {
                    player.Stop();
                }
                player.Release();
                player = null;
            }
        }

        public void Restart()
        {
            if ((player != null)) {
                player.Start();
            }
        }

        public void Pause()
        {
            if ((player != null)) {
                if (player.IsPlaying) {
                    player.Pause();
                }
            }
        }

        public async Task PlayAsync(string title)
        {
            await StartPlayerAsync(title);
        }

        public void Stop()
        {
            StopPlayer();
        }
    }
}