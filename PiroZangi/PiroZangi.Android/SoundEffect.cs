using Android.Media;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(PiroZangi.Droid.SoundEffect))]

namespace PiroZangi.Droid
{
    class SoundEffect : ISoundEffect
    {
        SoundPool soundPool;
        int[] soundPoolId = new int[8];

        // もろもろの初期化とファイルの読み込み
        public SoundEffect()
        {
            int SOUND_POOL_MAX = 8;

            AudioAttributes attr = new AudioAttributes.Builder()
                .SetUsage(AudioUsageKind.Media)
                .SetContentType(AudioContentType.Music)
                .Build();
            soundPool = new SoundPool.Builder()
               .SetAudioAttributes(attr)
               .SetMaxStreams(SOUND_POOL_MAX)
               .Build();
            soundPoolId[0] = soundPool.Load(Android.App.Application.Context, Resource.Raw.pirozangi, 1);
            soundPoolId[1] = soundPool.Load(Android.App.Application.Context, Resource.Raw.zangi, 1);
            soundPoolId[2] = soundPool.Load(Android.App.Application.Context, Resource.Raw.aonori, 1);
            soundPoolId[3] = soundPool.Load(Android.App.Application.Context, Resource.Raw.piro, 1);
            soundPoolId[4] = soundPool.Load(Android.App.Application.Context, Resource.Raw.ten, 1);
            soundPoolId[5] = soundPool.Load(Android.App.Application.Context, Resource.Raw.yui, 1);
            soundPoolId[6] = soundPool.Load(Android.App.Application.Context, Resource.Raw.cap, 1);
            soundPoolId[7] = soundPool.Load(Android.App.Application.Context, Resource.Raw.gameover, 1);
        }

        // 効果音の再生
        public void SoundPlay(int c)
        {
            soundPool.Play(soundPoolId[c], 1.0F, 1.0F, 0, 0, 1.0F);
        }
    }
}