using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Vest
{
    static class SoundHelper
    {
        static Dictionary<string, SoundEffect> sounds = new Dictionary<string,SoundEffect>();
        static SoundEffectInstance music1;
        static SoundEffectInstance music2;
        static float MusicVolume1;
        static float MusicVolume2;

        public static void Preload(string sound)
        {
            SoundEffect effect = null;
            if (!sounds.TryGetValue(sound, out effect))
            {
                effect = G.Content.Load<SoundEffect>(sound);
                sounds.Add(sound, effect);
            }
        }

        public static void SetMusicChannel1(string sound, float volume)
        {
            SoundEffect effect = null;
            if (!sounds.TryGetValue(sound, out effect))
            { return; }
            if (music1 != null)
                music1.Dispose();
            music1 = effect.CreateInstance();
            music1.IsLooped = true;
            music1.Volume = volume;
            music1.Play();
        }
        public static void SetMusicChannel2(string sound, float volume)
        {
            SoundEffect effect = null;
            if (!sounds.TryGetValue(sound, out effect))
            { return; }
            if (music2 != null)
                music2.Dispose();
            music2 = effect.CreateInstance();
            music2.IsLooped = true;
            music2.Volume = volume;
            music2.Play();
        }
        public static void SetMusicVolume1(float value)
        {
            if (music1 != null)
                music1.Volume = value;
        }
        public static void SetMusicVolume2(float value)
        {
            if (music2 != null)
                music2.Volume = value;
        }

        public static void PlaySound(string sound, float volume=1, float pitch=0, float pan=0)
        {
            SoundEffect effect=null;
            if (!sounds.TryGetValue(sound, out effect))
            {
                effect = G.Content.Load<SoundEffect>(sound);
                sounds.Add(sound, effect);
            }
            effect.Play(volume, pitch, pan);
        }
    }
}
