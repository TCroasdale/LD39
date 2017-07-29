using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LD39{
    abstract class Actor{


        /// <summary>
        /// Called when the actor is created.
        /// </summary>
        abstract public void initialise();

        /// <summary>
        /// Caleld once per frame on every actor
        /// </summary>
        /// <returns>if a non zero value is return the entity is deleted asap</returns>
        abstract public int Update(float deltaTime);

        /// <summary>
        /// Called once per frame on every actor can be used to draw extra things on the screen
        /// </summary>
        abstract public void Draw(SpriteBatch sb);

        public void setSprite(Texture2D spr){
            sprite = spr;
        }

        public Texture2D getSprite() { return sprite; }

        public void setPosition(Vector2 pos){
            position = pos;
        }
        public Vector2 getPosition() { return position; }

        public void setSize(Vector2 s) {
            size = s;
        }

        private Texture2D sprite;
        private Vector2 position;
        private Vector2 size;
    }
}
