using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LD39
{
    class ArtManager
    {
        #region SINGLETON IMPL
        private static ArtManager instance;

        private ArtManager() {
            textures = new Dictionary<string, Texture2D>();
        }

        public static ArtManager Instance{
            get{
                if (instance == null){
                    instance = new ArtManager();
                }
                return instance;
            }
        }
        #endregion

        private Dictionary<string, Texture2D> textures;
        public void addTexture(string name, Texture2D tex){

            textures.Add(name, tex);
        }

        public Texture2D getTexture(string name){
            return textures[name];
        }
    }
}
