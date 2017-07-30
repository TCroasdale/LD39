using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;



namespace LD39
{
    class AudioManager
    {
        #region SINGLETON IMPL
        private static AudioManager instance;

        private AudioManager() {
            sfx = new Dictionary<string, SoundEffect>();
        }

        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AudioManager();
                }
                return instance;
            }
        }
        #endregion

        private Dictionary<string, SoundEffect> sfx;
        private Song backgroundMusic;
        
        public void setBackgroundMusic(Song song){
            backgroundMusic = song;
        }

        public void update(float deltaTime){
            if (!canFire)
            {
                timer -= deltaTime;
                if (timer <= 0)
                {
                    canFire = true;
                }
            }
        }

        public void addSfx(string name, SoundEffect effect)
        {
            sfx.Add(name, effect);
        }

        public void fireSfx(string name){
            if (canFire)
            {
                sfx[name].Play();
                canFire = false;
                timer = audioTimeout;
            }
        }

        public void playMusic(){
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Stop();
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 1.0f;
        }

        private float audioTimeout = 0.1f;
        private float timer;
        private bool canFire = true;

    }
}
