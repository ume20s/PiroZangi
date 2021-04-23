using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(PiroZangi.Droid.SoundEffect))]

namespace PiroZangi.Droid
{
    class SoundEffect : ISoundEffect
    {
        SoundPool soundPool;
        int soundPoolId;


        public SoundEffect()
        {
            int SOUND_POOL_MAX = 6;

            AudioAttributes attr = new AudioAttributes.Builder()
                .SetUsage(AudioUsageKind.Media)
                .SetContentType(AudioContentType.Music)
                .Build();
            soundPool = new SoundPool.Builder()
               .SetAudioAttributes(attr)
               .SetMaxStreams(SOUND_POOL_MAX)
               .Build();
            soundPoolId = soundPool.Load(Android.App.Application.Context, Resource.Raw.zangi, 1);
        }

        public void SoundPlay()
        {
            soundPool.Play(soundPoolId, 1.0F, 1.0F, 0, 0, 1.0F);
        }
    }
}