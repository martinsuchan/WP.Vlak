using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Vlak.Model.Enums;

namespace Vlak
{
    public class SoundEffectManager
    {
        private readonly SoundEffect step;
        private readonly SoundEffect cargo;
        private readonly SoundEffect crash;
        private readonly SoundEffect win;

        public SoundEffectManager(ContentManager contentManager)
        {
            step = contentManager.Load<SoundEffect>("sound/step");
            cargo = contentManager.Load<SoundEffect>("sound/cargo");
            crash = contentManager.Load<SoundEffect>("sound/crash");
            win = contentManager.Load<SoundEffect>("sound/win");
        }

        public void PlaySound(GameSound sound)
        {
            if (!Settings.SoundEnabled.Value) return;

            switch (sound)
            {
                case GameSound.None:
                    break;
                case GameSound.Step:
                    step.Play(0.5f, 0, 0);
                    break;
                case GameSound.Cargo:
                    cargo.Play();
                    break;
                case GameSound.Crash:
                    crash.Play();
                    break;
                case GameSound.Win:
                    win.Play();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}