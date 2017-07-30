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

        public void addSfx(string name, SoundEffect effect)
        {
            sfx.Add(name, effect);
        }

        public void fireSfx(string name){
            sfx[name].Play();
        }

        public void playMusic(){
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
        }

    }
}
