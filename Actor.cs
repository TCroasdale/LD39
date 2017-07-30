using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LD39{
    public abstract class Actor{

        public int id;

        /// <summary>
        /// Called when the actor is created.
        /// </summary>
        abstract public void initialise();

        /// <summary>
        /// Caleld once per frame on every actor
        /// </summary>
        /// <returns>if a non zero value is return the entity is deleted asap</returns>
        abstract public int Update(float deltaTime);

        abstract public bool OnCollision(collision info);
        abstract public void OnSeparation(collision info);

        abstract public void OnDestroy();

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
        public virtual Vector2 getPosition() {
            return position - size / 2;
        }

        public void setSize(Vector2 s)
        {
            size = s;
        }
        public Vector2 getSize()
        {
            return size;
        }

        public string tag = "Actor";

        private Texture2D sprite;
        private Vector2 position;
        private Vector2 size = Vector2.Zero;
    }
}
